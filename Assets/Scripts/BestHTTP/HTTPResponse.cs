using BestHTTP.Caching;
using BestHTTP.Cookies;
using BestHTTP.Decompression.Zlib;
using BestHTTP.Extensions;
using BestHTTP.Logger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace BestHTTP
{
	public class HTTPResponse : IDisposable
	{
		internal const byte CR = 13;

		internal const byte LF = 10;

		public const int MinBufferSize = 4096;

		protected string dataAsText;

		protected Texture2D texture;

		internal HTTPRequest baseRequest;

		protected Stream Stream;

		protected List<byte[]> streamedFragments;

		protected object SyncRoot = new object();

		protected byte[] fragmentBuffer;

		protected int fragmentBufferDataLength;

		protected Stream cacheStream;

		protected int allFragmentSize;

		public int VersionMajor
		{
			get;
			protected set;
		}

		public int VersionMinor
		{
			get;
			protected set;
		}

		public int StatusCode
		{
			get;
			protected set;
		}

		public bool IsSuccess => (StatusCode >= 200 && StatusCode < 300) || StatusCode == 304;

		public string Message
		{
			get;
			protected set;
		}

		public bool IsStreamed
		{
			get;
			protected set;
		}

		public bool IsStreamingFinished
		{
			get;
			internal set;
		}

		public bool IsFromCache
		{
			get;
			internal set;
		}

		public Dictionary<string, List<string>> Headers
		{
			get;
			protected set;
		}

		public byte[] Data
		{
			get;
			internal set;
		}

		public bool IsUpgraded
		{
			get;
			protected set;
		}

		public List<Cookie> Cookies
		{
			get;
			internal set;
		}

		public string DataAsText
		{
			get
			{
				if (Data == null)
				{
					return string.Empty;
				}
				if (!string.IsNullOrEmpty(dataAsText))
				{
					return dataAsText;
				}
				return dataAsText = Encoding.UTF8.GetString(Data, 0, Data.Length);
			}
		}

		public Texture2D DataAsTexture2D
		{
			get
			{
				if (Data == null)
				{
					return null;
				}
				if (texture != null)
				{
					return texture;
				}
				texture = new Texture2D(0, 0, TextureFormat.ARGB32, mipChain: false);
				texture.LoadImage(Data);
				return texture;
			}
		}

		public bool IsClosedManually
		{
			get;
			protected set;
		}

		internal HTTPResponse(HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache)
		{
			baseRequest = request;
			Stream = stream;
			IsStreamed = isStreamed;
			IsFromCache = isFromCache;
			IsClosedManually = false;
		}

		internal virtual bool Receive(int forceReadRawContentLength = -1, bool readPayloadData = true)
		{
			string empty = string.Empty;
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				VerboseLogging($"Receive. forceReadRawContentLength: '{forceReadRawContentLength:N0}', readPayloadData: '{readPayloadData:N0}'");
			}
			try
			{
				empty = ReadTo(Stream, 32);
			}
			catch
			{
				if (baseRequest.DisableRetry)
				{
					HTTPManager.Logger.Warning("HTTPResponse", $"{baseRequest.CurrentUri.ToString()} - Failed to read Status Line! Retry is disabled, re-throwing exception.");
					throw;
				}
				HTTPManager.Logger.Warning("HTTPResponse", $"{baseRequest.CurrentUri.ToString()} - Failed to read Status Line! Retry is enabled, returning with false.");
				return false;
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				VerboseLogging($"Status Line: '{empty}'");
			}
			if (string.IsNullOrEmpty(empty))
			{
				if (!baseRequest.DisableRetry)
				{
					return false;
				}
				throw new Exception("Remote server closed the connection before sending response header!");
			}
			string[] array = empty.Split('/', '.');
			VersionMajor = int.Parse(array[1]);
			VersionMinor = int.Parse(array[2]);
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				VerboseLogging($"HTTP Version: '{VersionMajor.ToString()}.{VersionMinor.ToString()}'");
			}
			string text = NoTrimReadTo(Stream, 32, 10);
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				VerboseLogging($"Status Code: '{text}'");
			}
			int result;
			if (baseRequest.DisableRetry)
			{
				result = int.Parse(text);
			}
			else if (!int.TryParse(text, out result))
			{
				return false;
			}
			StatusCode = result;
			if (text.Length > 0 && (byte)text[text.Length - 1] != 10 && (byte)text[text.Length - 1] != 13)
			{
				Message = ReadTo(Stream, 10);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					VerboseLogging($"Status Message: '{Message}'");
				}
			}
			else
			{
				HTTPManager.Logger.Warning("HTTPResponse", $"{baseRequest.CurrentUri.ToString()} - Skipping Status Message reading!");
				Message = string.Empty;
			}
			ReadHeaders(Stream);
			IsUpgraded = (StatusCode == 101 && (HasHeaderWithValue("connection", "upgrade") || HasHeader("upgrade")));
			if (IsUpgraded && HTTPManager.Logger.Level == Loglevels.All)
			{
				VerboseLogging("Request Upgraded!");
			}
			if (!readPayloadData)
			{
				return true;
			}
			return ReadPayload(forceReadRawContentLength);
		}

		protected bool ReadPayload(int forceReadRawContentLength)
		{
			if (forceReadRawContentLength != -1)
			{
				IsFromCache = true;
				ReadRaw(Stream, forceReadRawContentLength);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					VerboseLogging("ReadPayload Finished!");
				}
				return true;
			}
			if ((StatusCode >= 100 && StatusCode < 200) || StatusCode == 204 || StatusCode == 304 || baseRequest.MethodType == HTTPMethods.Head)
			{
				return true;
			}
			if (HasHeaderWithValue("transfer-encoding", "chunked"))
			{
				ReadChunked(Stream);
			}
			else
			{
				List<string> headerValues = GetHeaderValues("content-length");
				List<string> headerValues2 = GetHeaderValues("content-range");
				if (headerValues != null && headerValues2 == null)
				{
					ReadRaw(Stream, int.Parse(headerValues[0]));
				}
				else if (headerValues2 != null)
				{
					if (headerValues != null)
					{
						ReadRaw(Stream, int.Parse(headerValues[0]));
					}
					else
					{
						HTTPRange range = GetRange();
						ReadRaw(Stream, range.LastBytePos - range.FirstBytePos + 1);
					}
				}
				else
				{
					ReadUnknownSize(Stream);
				}
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				VerboseLogging("ReadPayload Finished!");
			}
			return true;
		}

		protected void ReadHeaders(Stream stream)
		{
			string text = ReadTo(stream, 58, 10).Trim();
			while (text != string.Empty)
			{
				string text2 = ReadTo(stream, 10);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					VerboseLogging($"Header - '{text}': '{text2}'");
				}
				AddHeader(text, text2);
				text = ReadTo(stream, 58, 10);
			}
		}

		protected void AddHeader(string name, string value)
		{
			name = name.ToLower();
			if (Headers == null)
			{
				Headers = new Dictionary<string, List<string>>();
			}
			if (!Headers.TryGetValue(name, out List<string> value2))
			{
				Headers.Add(name, value2 = new List<string>(1));
			}
			value2.Add(value);
		}

		public List<string> GetHeaderValues(string name)
		{
			if (Headers == null)
			{
				return null;
			}
			name = name.ToLower();
			if (!Headers.TryGetValue(name, out List<string> value) || value.Count == 0)
			{
				return null;
			}
			return value;
		}

		public string GetFirstHeaderValue(string name)
		{
			if (Headers == null)
			{
				return null;
			}
			name = name.ToLower();
			if (!Headers.TryGetValue(name, out List<string> value) || value.Count == 0)
			{
				return null;
			}
			return value[0];
		}

		public bool HasHeaderWithValue(string headerName, string value)
		{
			List<string> headerValues = GetHeaderValues(headerName);
			if (headerValues == null)
			{
				return false;
			}
			for (int i = 0; i < headerValues.Count; i++)
			{
				if (string.Compare(headerValues[i], value, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasHeader(string headerName)
		{
			List<string> headerValues = GetHeaderValues(headerName);
			if (headerValues == null)
			{
				return false;
			}
			return true;
		}

		public HTTPRange GetRange()
		{
			List<string> headerValues = GetHeaderValues("content-range");
			if (headerValues == null)
			{
				return null;
			}
			string[] array = headerValues[0].Split(new char[3]
			{
				' ',
				'-',
				'/'
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array[1] == "*")
			{
				return new HTTPRange(int.Parse(array[2]));
			}
			return new HTTPRange(int.Parse(array[1]), int.Parse(array[2]), (!(array[3] != "*")) ? (-1) : int.Parse(array[3]));
		}

		public static string ReadTo(Stream stream, byte blocker)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = stream.ReadByte();
				while (num != blocker && num != -1)
				{
					memoryStream.WriteByte((byte)num);
					num = stream.ReadByte();
				}
				return memoryStream.ToArray().AsciiToString().Trim();
			}
		}

		public static string ReadTo(Stream stream, byte blocker1, byte blocker2)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = stream.ReadByte();
				while (num != blocker1 && num != blocker2 && num != -1)
				{
					memoryStream.WriteByte((byte)num);
					num = stream.ReadByte();
				}
				return memoryStream.ToArray().AsciiToString().Trim();
			}
		}

		public static string NoTrimReadTo(Stream stream, byte blocker1, byte blocker2)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = stream.ReadByte();
				while (num != blocker1 && num != blocker2 && num != -1)
				{
					memoryStream.WriteByte((byte)num);
					num = stream.ReadByte();
				}
				return memoryStream.ToArray().AsciiToString();
			}
		}

		protected int ReadChunkLength(Stream stream)
		{
			string text = ReadTo(stream, 10);
			string[] array = text.Split(';');
			string text2 = array[0];
			if (int.TryParse(text2, NumberStyles.AllowHexSpecifier, null, out int result))
			{
				return result;
			}
			throw new Exception($"Can't parse '{text2}' as a hex number!");
		}

		protected void ReadChunked(Stream stream)
		{
			BeginReceiveStreamFragments();
			string firstHeaderValue = GetFirstHeaderValue("Content-Length");
			bool flag = !string.IsNullOrEmpty(firstHeaderValue);
			int result = 0;
			if (flag)
			{
				flag = int.TryParse(firstHeaderValue, out result);
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				VerboseLogging($"ReadChunked - hasContentLengthHeader: {flag.ToString()}, contentLengthHeader: {firstHeaderValue} realLength: {result:N0}");
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = ReadChunkLength(stream);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					VerboseLogging($"chunkLength: {num:N0}");
				}
				byte[] array = new byte[num];
				int num2 = 0;
				baseRequest.DownloadLength = ((!flag) ? num : result);
				baseRequest.DownloadProgressChanged = (IsSuccess || IsFromCache);
				while (num != 0)
				{
					if (array.Length < num)
					{
						Array.Resize(ref array, num);
					}
					int num3 = 0;
					do
					{
						int num4 = stream.Read(array, num3, num - num3);
						if (num4 <= 0)
						{
							throw ExceptionHelper.ServerClosedTCPStream();
						}
						num3 += num4;
						baseRequest.Downloaded += num4;
						baseRequest.DownloadProgressChanged = (IsSuccess || IsFromCache);
					}
					while (num3 < num);
					if (baseRequest.UseStreaming)
					{
						WaitWhileHasFragments();
						FeedStreamFragment(array, 0, num3);
					}
					else
					{
						memoryStream.Write(array, 0, num3);
					}
					ReadTo(stream, 10);
					num2 += num3;
					num = ReadChunkLength(stream);
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						VerboseLogging($"chunkLength: {num:N0}");
					}
					if (!flag)
					{
						baseRequest.DownloadLength += num;
					}
					baseRequest.DownloadProgressChanged = (IsSuccess || IsFromCache);
				}
				if (baseRequest.UseStreaming)
				{
					FlushRemainingFragmentBuffer();
				}
				ReadHeaders(stream);
				if (!baseRequest.UseStreaming)
				{
					Data = DecodeStream(memoryStream);
				}
			}
		}

		internal void ReadRaw(Stream stream, int contentLength)
		{
			BeginReceiveStreamFragments();
			baseRequest.DownloadLength = contentLength;
			baseRequest.DownloadProgressChanged = (IsSuccess || IsFromCache);
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				VerboseLogging($"ReadRaw - contentLength: {contentLength:N0}");
			}
			using (MemoryStream memoryStream = new MemoryStream((!baseRequest.UseStreaming) ? contentLength : 0))
			{
				byte[] array = new byte[Math.Max(baseRequest.StreamFragmentSize, 4096)];
				int num = 0;
				while (contentLength > 0)
				{
					num = 0;
					do
					{
						int num2 = stream.Read(array, num, Math.Min(contentLength, array.Length - num));
						if (num2 <= 0)
						{
							throw ExceptionHelper.ServerClosedTCPStream();
						}
						num += num2;
						contentLength -= num2;
						baseRequest.Downloaded += num2;
						baseRequest.DownloadProgressChanged = (IsSuccess || IsFromCache);
					}
					while (num < array.Length && contentLength > 0);
					if (baseRequest.UseStreaming)
					{
						WaitWhileHasFragments();
						FeedStreamFragment(array, 0, num);
					}
					else
					{
						memoryStream.Write(array, 0, num);
					}
				}
				if (baseRequest.UseStreaming)
				{
					FlushRemainingFragmentBuffer();
				}
				if (!baseRequest.UseStreaming)
				{
					Data = DecodeStream(memoryStream);
				}
			}
		}

		protected void ReadUnknownSize(Stream stream)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] array = new byte[Math.Max(baseRequest.StreamFragmentSize, 4096)];
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					VerboseLogging($"ReadUnknownSize - buffer size: {array.Length:N0}");
				}
				int num = 0;
				int num2 = 0;
				do
				{
					num = 0;
					do
					{
						num2 = 0;
						NetworkStream networkStream = stream as NetworkStream;
						if (networkStream != null && baseRequest.EnableSafeReadOnUnknownContentLength)
						{
							for (int i = num; i < array.Length; i++)
							{
								if (!networkStream.DataAvailable)
								{
									break;
								}
								int num3 = stream.ReadByte();
								if (num3 < 0)
								{
									break;
								}
								array[i] = (byte)num3;
								num2++;
							}
						}
						else
						{
							num2 = stream.Read(array, num, array.Length - num);
						}
						num += num2;
						baseRequest.Downloaded += num2;
						baseRequest.DownloadLength = baseRequest.Downloaded;
						baseRequest.DownloadProgressChanged = (IsSuccess || IsFromCache);
					}
					while (num < array.Length && num2 > 0);
					if (baseRequest.UseStreaming)
					{
						WaitWhileHasFragments();
						FeedStreamFragment(array, 0, num);
					}
					else
					{
						memoryStream.Write(array, 0, num);
					}
				}
				while (num2 > 0);
				if (baseRequest.UseStreaming)
				{
					FlushRemainingFragmentBuffer();
				}
				if (!baseRequest.UseStreaming)
				{
					Data = DecodeStream(memoryStream);
				}
			}
		}

		protected byte[] DecodeStream(MemoryStream streamToDecode)
		{
			streamToDecode.Seek(0L, SeekOrigin.Begin);
			List<string> list = (!IsFromCache) ? GetHeaderValues("content-encoding") : null;
			Stream stream = null;
			if (list == null)
			{
				return streamToDecode.ToArray();
			}
			switch (list[0])
			{
			case "gzip":
				stream = new GZipStream(streamToDecode, CompressionMode.Decompress);
				break;
			case "deflate":
				stream = new DeflateStream(streamToDecode, CompressionMode.Decompress);
				break;
			default:
				return streamToDecode.ToArray();
			}
			using (MemoryStream memoryStream = new MemoryStream((int)streamToDecode.Length))
			{
				byte[] array = new byte[1024];
				int num = 0;
				while ((num = stream.Read(array, 0, array.Length)) > 0)
				{
					memoryStream.Write(array, 0, num);
				}
				return memoryStream.ToArray();
			}
		}

		protected void BeginReceiveStreamFragments()
		{
			if (!baseRequest.DisableCache && baseRequest.UseStreaming && !IsFromCache && HTTPCacheService.IsCacheble(baseRequest.CurrentUri, baseRequest.MethodType, this))
			{
				cacheStream = HTTPCacheService.PrepareStreamed(baseRequest.CurrentUri, this);
			}
			allFragmentSize = 0;
		}

		protected void FeedStreamFragment(byte[] buffer, int pos, int length)
		{
			if (fragmentBuffer == null)
			{
				fragmentBuffer = new byte[baseRequest.StreamFragmentSize];
				fragmentBufferDataLength = 0;
			}
			if (fragmentBufferDataLength + length <= baseRequest.StreamFragmentSize)
			{
				Array.Copy(buffer, pos, fragmentBuffer, fragmentBufferDataLength, length);
				fragmentBufferDataLength += length;
				if (fragmentBufferDataLength == baseRequest.StreamFragmentSize)
				{
					AddStreamedFragment(fragmentBuffer);
					fragmentBuffer = null;
					fragmentBufferDataLength = 0;
				}
			}
			else
			{
				int num = baseRequest.StreamFragmentSize - fragmentBufferDataLength;
				FeedStreamFragment(buffer, pos, num);
				FeedStreamFragment(buffer, pos + num, length - num);
			}
		}

		protected void FlushRemainingFragmentBuffer()
		{
			if (fragmentBuffer != null)
			{
				Array.Resize(ref fragmentBuffer, fragmentBufferDataLength);
				AddStreamedFragment(fragmentBuffer);
				fragmentBuffer = null;
				fragmentBufferDataLength = 0;
			}
			if (cacheStream != null)
			{
				cacheStream.Dispose();
				cacheStream = null;
				HTTPCacheService.SetBodyLength(baseRequest.CurrentUri, allFragmentSize);
			}
		}

		protected void AddStreamedFragment(byte[] buffer)
		{
			lock (SyncRoot)
			{
				if (streamedFragments == null)
				{
					streamedFragments = new List<byte[]>();
				}
				streamedFragments.Add(buffer);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					VerboseLogging($"AddStreamedFragment buffer length: {buffer.Length:N0} streamedFragments: {streamedFragments.Count:N0}");
				}
				if (cacheStream != null)
				{
					cacheStream.Write(buffer, 0, buffer.Length);
					allFragmentSize += buffer.Length;
				}
			}
		}

		protected void WaitWhileHasFragments()
		{
			while (baseRequest.UseStreaming && HasStreamedFragments())
			{
				Thread.Sleep(16);
			}
		}

		public List<byte[]> GetStreamedFragments()
		{
			lock (SyncRoot)
			{
				if (streamedFragments == null || streamedFragments.Count == 0)
				{
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						VerboseLogging("GetStreamedFragments - no fragments, returning with null");
					}
					return null;
				}
				List<byte[]> list = new List<byte[]>(streamedFragments);
				streamedFragments.Clear();
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					VerboseLogging($"GetStreamedFragments - returning with {list.Count.ToString():N0} fragments");
				}
				return list;
			}
		}

		internal bool HasStreamedFragments()
		{
			lock (SyncRoot)
			{
				return streamedFragments != null && streamedFragments.Count > 0;
			}
		}

		internal void FinishStreaming()
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				VerboseLogging("FinishStreaming");
			}
			IsStreamingFinished = true;
			Dispose();
		}

		private void VerboseLogging(string str)
		{
			HTTPManager.Logger.Verbose("HTTPResponse", "'" + baseRequest.CurrentUri.ToString() + "' - " + str);
		}

		public void Dispose()
		{
			if (cacheStream != null)
			{
				cacheStream.Dispose();
				cacheStream = null;
			}
		}
	}
}
