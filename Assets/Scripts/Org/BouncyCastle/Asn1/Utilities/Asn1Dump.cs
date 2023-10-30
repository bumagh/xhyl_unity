using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Org.BouncyCastle.Asn1.Utilities
{
	public sealed class Asn1Dump
	{
		private static readonly string NewLine = Platform.NewLine;

		private const string Tab = "    ";

		private const int SampleSize = 32;

		private Asn1Dump()
		{
		}

		private static void AsString(string indent, bool verbose, Asn1Object obj, StringBuilder buf)
		{
			if (obj is Asn1Sequence)
			{
				string text = indent + "    ";
				buf.Append(indent);
				if (obj is BerSequence)
				{
					buf.Append("BER Sequence");
				}
				else if (obj is DerSequence)
				{
					buf.Append("DER Sequence");
				}
				else
				{
					buf.Append("Sequence");
				}
				buf.Append(NewLine);
				IEnumerator enumerator = ((Asn1Sequence)obj).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Asn1Encodable asn1Encodable = (Asn1Encodable)enumerator.Current;
						if (asn1Encodable == null || asn1Encodable is Asn1Null)
						{
							buf.Append(text);
							buf.Append("NULL");
							buf.Append(NewLine);
						}
						else
						{
							AsString(text, verbose, asn1Encodable.ToAsn1Object(), buf);
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			else if (obj is DerTaggedObject)
			{
				string text2 = indent + "    ";
				buf.Append(indent);
				if (obj is BerTaggedObject)
				{
					buf.Append("BER Tagged [");
				}
				else
				{
					buf.Append("Tagged [");
				}
				DerTaggedObject derTaggedObject = (DerTaggedObject)obj;
				buf.Append(derTaggedObject.TagNo.ToString());
				buf.Append(']');
				if (!derTaggedObject.IsExplicit())
				{
					buf.Append(" IMPLICIT ");
				}
				buf.Append(NewLine);
				if (derTaggedObject.IsEmpty())
				{
					buf.Append(text2);
					buf.Append("EMPTY");
					buf.Append(NewLine);
				}
				else
				{
					AsString(text2, verbose, derTaggedObject.GetObject(), buf);
				}
			}
			else if (obj is BerSet)
			{
				string text3 = indent + "    ";
				buf.Append(indent);
				buf.Append("BER Set");
				buf.Append(NewLine);
				IEnumerator enumerator2 = ((Asn1Set)obj).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Asn1Encodable asn1Encodable2 = (Asn1Encodable)enumerator2.Current;
						if (asn1Encodable2 == null)
						{
							buf.Append(text3);
							buf.Append("NULL");
							buf.Append(NewLine);
						}
						else
						{
							AsString(text3, verbose, asn1Encodable2.ToAsn1Object(), buf);
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
			}
			else if (obj is DerSet)
			{
				string text4 = indent + "    ";
				buf.Append(indent);
				buf.Append("DER Set");
				buf.Append(NewLine);
				IEnumerator enumerator3 = ((Asn1Set)obj).GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						Asn1Encodable asn1Encodable3 = (Asn1Encodable)enumerator3.Current;
						if (asn1Encodable3 == null)
						{
							buf.Append(text4);
							buf.Append("NULL");
							buf.Append(NewLine);
						}
						else
						{
							AsString(text4, verbose, asn1Encodable3.ToAsn1Object(), buf);
						}
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = (enumerator3 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
			}
			else if (obj is DerObjectIdentifier)
			{
				buf.Append(indent + "ObjectIdentifier(" + ((DerObjectIdentifier)obj).Id + ")" + NewLine);
			}
			else if (obj is DerBoolean)
			{
				buf.Append(indent + "Boolean(" + ((DerBoolean)obj).IsTrue + ")" + NewLine);
			}
			else if (obj is DerInteger)
			{
				buf.Append(indent + "Integer(" + ((DerInteger)obj).Value + ")" + NewLine);
			}
			else if (obj is BerOctetString)
			{
				byte[] octets = ((Asn1OctetString)obj).GetOctets();
				string text5 = (!verbose) ? string.Empty : dumpBinaryDataAsString(indent, octets);
				buf.Append(indent + "BER Octet String[" + octets.Length + "] " + text5 + NewLine);
			}
			else if (obj is DerOctetString)
			{
				byte[] octets2 = ((Asn1OctetString)obj).GetOctets();
				string text6 = (!verbose) ? string.Empty : dumpBinaryDataAsString(indent, octets2);
				buf.Append(indent + "DER Octet String[" + octets2.Length + "] " + text6 + NewLine);
			}
			else if (obj is DerBitString)
			{
				DerBitString derBitString = (DerBitString)obj;
				byte[] bytes = derBitString.GetBytes();
				string text7 = (!verbose) ? string.Empty : dumpBinaryDataAsString(indent, bytes);
				buf.Append(indent + "DER Bit String[" + bytes.Length + ", " + derBitString.PadBits + "] " + text7 + NewLine);
			}
			else if (obj is DerIA5String)
			{
				buf.Append(indent + "IA5String(" + ((DerIA5String)obj).GetString() + ") " + NewLine);
			}
			else if (obj is DerUtf8String)
			{
				buf.Append(indent + "UTF8String(" + ((DerUtf8String)obj).GetString() + ") " + NewLine);
			}
			else if (obj is DerPrintableString)
			{
				buf.Append(indent + "PrintableString(" + ((DerPrintableString)obj).GetString() + ") " + NewLine);
			}
			else if (obj is DerVisibleString)
			{
				buf.Append(indent + "VisibleString(" + ((DerVisibleString)obj).GetString() + ") " + NewLine);
			}
			else if (obj is DerBmpString)
			{
				buf.Append(indent + "BMPString(" + ((DerBmpString)obj).GetString() + ") " + NewLine);
			}
			else if (obj is DerT61String)
			{
				buf.Append(indent + "T61String(" + ((DerT61String)obj).GetString() + ") " + NewLine);
			}
			else if (obj is DerGraphicString)
			{
				buf.Append(indent + "GraphicString(" + ((DerGraphicString)obj).GetString() + ") " + NewLine);
			}
			else if (obj is DerVideotexString)
			{
				buf.Append(indent + "VideotexString(" + ((DerVideotexString)obj).GetString() + ") " + NewLine);
			}
			else if (obj is DerUtcTime)
			{
				buf.Append(indent + "UTCTime(" + ((DerUtcTime)obj).TimeString + ") " + NewLine);
			}
			else if (obj is DerGeneralizedTime)
			{
				buf.Append(indent + "GeneralizedTime(" + ((DerGeneralizedTime)obj).GetTime() + ") " + NewLine);
			}
			else if (obj is BerApplicationSpecific)
			{
				buf.Append(outputApplicationSpecific("BER", indent, verbose, (BerApplicationSpecific)obj));
			}
			else if (obj is DerApplicationSpecific)
			{
				buf.Append(outputApplicationSpecific("DER", indent, verbose, (DerApplicationSpecific)obj));
			}
			else if (obj is DerEnumerated)
			{
				DerEnumerated derEnumerated = (DerEnumerated)obj;
				buf.Append(indent + "DER Enumerated(" + derEnumerated.Value + ")" + NewLine);
			}
			else if (obj is DerExternal)
			{
				DerExternal derExternal = (DerExternal)obj;
				buf.Append(indent + "External " + NewLine);
				string text8 = indent + "    ";
				if (derExternal.DirectReference != null)
				{
					buf.Append(text8 + "Direct Reference: " + derExternal.DirectReference.Id + NewLine);
				}
				if (derExternal.IndirectReference != null)
				{
					buf.Append(text8 + "Indirect Reference: " + derExternal.IndirectReference.ToString() + NewLine);
				}
				if (derExternal.DataValueDescriptor != null)
				{
					AsString(text8, verbose, derExternal.DataValueDescriptor, buf);
				}
				buf.Append(text8 + "Encoding: " + derExternal.Encoding + NewLine);
				AsString(text8, verbose, derExternal.ExternalContent, buf);
			}
			else
			{
				buf.Append(indent + obj.ToString() + NewLine);
			}
		}

		private static string outputApplicationSpecific(string type, string indent, bool verbose, DerApplicationSpecific app)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (app.IsConstructed())
			{
				try
				{
					Asn1Sequence instance = Asn1Sequence.GetInstance(app.GetObject(16));
					stringBuilder.Append(indent + type + " ApplicationSpecific[" + app.ApplicationTag + "]" + NewLine);
					IEnumerator enumerator = instance.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Asn1Encodable asn1Encodable = (Asn1Encodable)enumerator.Current;
							AsString(indent + "    ", verbose, asn1Encodable.ToAsn1Object(), stringBuilder);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				catch (IOException value)
				{
					stringBuilder.Append(value);
				}
				return stringBuilder.ToString();
			}
			return indent + type + " ApplicationSpecific[" + app.ApplicationTag + "] (" + Hex.ToHexString(app.GetContents()) + ")" + NewLine;
		}

		[Obsolete("Use version accepting Asn1Encodable")]
		public static string DumpAsString(object obj)
		{
			if (obj is Asn1Encodable)
			{
				StringBuilder stringBuilder = new StringBuilder();
				AsString(string.Empty, verbose: false, ((Asn1Encodable)obj).ToAsn1Object(), stringBuilder);
				return stringBuilder.ToString();
			}
			return "unknown object type " + obj.ToString();
		}

		public static string DumpAsString(Asn1Encodable obj)
		{
			return DumpAsString(obj, verbose: false);
		}

		public static string DumpAsString(Asn1Encodable obj, bool verbose)
		{
			StringBuilder stringBuilder = new StringBuilder();
			AsString(string.Empty, verbose, obj.ToAsn1Object(), stringBuilder);
			return stringBuilder.ToString();
		}

		private static string dumpBinaryDataAsString(string indent, byte[] bytes)
		{
			indent += "    ";
			StringBuilder stringBuilder = new StringBuilder(NewLine);
			for (int i = 0; i < bytes.Length; i += 32)
			{
				if (bytes.Length - i > 32)
				{
					stringBuilder.Append(indent);
					stringBuilder.Append(Hex.ToHexString(bytes, i, 32));
					stringBuilder.Append("    ");
					stringBuilder.Append(calculateAscString(bytes, i, 32));
					stringBuilder.Append(NewLine);
					continue;
				}
				stringBuilder.Append(indent);
				stringBuilder.Append(Hex.ToHexString(bytes, i, bytes.Length - i));
				for (int j = bytes.Length - i; j != 32; j++)
				{
					stringBuilder.Append("  ");
				}
				stringBuilder.Append("    ");
				stringBuilder.Append(calculateAscString(bytes, i, bytes.Length - i));
				stringBuilder.Append(NewLine);
			}
			return stringBuilder.ToString();
		}

		private static string calculateAscString(byte[] bytes, int off, int len)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = off; i != off + len; i++)
			{
				char c = (char)bytes[i];
				if (c >= ' ' && c <= '~')
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
