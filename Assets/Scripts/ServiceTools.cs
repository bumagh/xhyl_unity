using SA.CrossPlatform.App;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public static class ServiceTools
{
	public static string ToCoinString(this int coin)
	{
		string text = coin.ToString();
		if (text.Length > 3)
		{
			string text2 = string.Empty;
			for (int i = 0; i < text.Length; i++)
			{
				if (i % 3 == 0 && i > 0)
				{
					text2 = "," + text2;
				}
				text2 = text[text.Length - 1 - i].ToString() + text2;
			}
			return text2;
		}
		return text;
	}

	public static int ToInt(this string str)
	{
		try
		{
			return int.Parse(str);
		}
		catch
		{
			return -1;
		}
	}

	public static int ToLimitInt(this string str)
	{
		try
		{
			return int.Parse(str);
		}
		catch
		{
			return 0;
		}
	}

	public static long ToLong(this string str)
	{
		try
		{
			return long.Parse(str);
		}
		catch
		{
			return -1L;
		}
	}

	public static int HexToInt(this string str)
	{
		bool flag = true;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] != 'F' && str[i] != 'f')
			{
				flag = false;
			}
		}
		if (flag)
		{
			return -1;
		}
		try
		{
			return int.Parse(str, NumberStyles.HexNumber);
		}
		catch
		{
			return -1;
		}
	}

	public static string ToTimeString(this int second, int strCount)
	{
		string text = "00";
		float num = Mathf.Floor(second / 3600);
		if (num != 0f)
		{
			text = ((num.ToString().Length != 1) ? num.ToString() : ("0" + num.ToString()));
		}
		string text2 = "00";
		float num2 = Mathf.Floor(((float)second - num * 3600f) / 60f);
		if (num2 != 0f)
		{
			text2 = ((num2.ToString().Length != 1) ? num2.ToString() : ("0" + num2.ToString()));
		}
		string text3 = "00";
		float num3 = Mathf.Floor((float)second - num * 3600f - num2 * 60f);
		if (num3 != 0f)
		{
			text3 = ((num3.ToString().Length != 1) ? num3.ToString() : ("0" + num3.ToString()));
		}
		switch (strCount)
		{
		case 1:
			return text3;
		default:
			return ((!(text == "00")) ? (text + ":") : string.Empty) + ((!(text2 == "00")) ? (text2 + ":") : string.Empty) + text3;
		case 2:
			return ((!(text2 == "00")) ? (text2 + ":") : string.Empty) + text3;
		}
	}

	public static bool SaveTextureToPNG(RenderTexture rt, string contents, string pngName)
	{
		try
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = rt;
			Texture2D texture2D = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, mipChain: false);
			texture2D.ReadPixels(new Rect(0f, 0f, rt.width, rt.height), 0, 0);
			SaveTextureToPNG(texture2D, contents, pngName);
			UnityEngine.Object.DestroyImmediate(texture2D);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool SaveTextureToPNG(Texture2D texture, string contents, string pngName)
	{
		try
		{
			byte[] buffer = texture.EncodeToPNG();
			if (!Directory.Exists(contents))
			{
				Directory.CreateDirectory(contents);
			}
			FileStream fileStream = File.Open(contents + "/" + pngName, FileMode.Create);
			new BinaryWriter(fileStream).Write(buffer);
			fileStream.Close();
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static Texture2D ReadTexturFromPath(string path)
	{
		return ReadTexturFromPath(path, 256);
	}

	public static Texture2D ReadTexturFromPath(string path, int size)
	{
		try
		{
			FileStream fileStream = File.Open(path, FileMode.Open);
			if (fileStream != null)
			{
				byte[] array = new BinaryReader(fileStream).ReadBytes((int)fileStream.Length);
				if (array.Length != 0)
				{
					Texture2D texture2D = new Texture2D(size, size);
					texture2D.LoadImage(array);
					return texture2D;
				}
			}
		}
		catch
		{
			return null;
		}
		return null;
	}

	public static int VersionCompared(string mVersion, string cVersion)
	{
		if (mVersion == cVersion)
		{
			return 0;
		}
		for (int i = 0; i < 5; i++)
		{
			int num = GetStringFromChar(mVersion, ".", i).ToInt();
			int num2 = GetStringFromChar(cVersion, ".", i).ToInt();
			if (num > num2)
			{
				return 1;
			}
			if (num2 > num)
			{
				return 2;
			}
		}
		return 0;
	}

	public static string GetStringFromChar(string str, string charStr, int index)
	{
		if (str == null)
		{
			return string.Empty;
		}
		int num = 0;
		string text = string.Empty;
		for (int i = 0; i < str.Length; i++)
		{
			char c = str[i];
			if (c == '.')
			{
				num++;
			}
			else if (num == index)
			{
				text += c.ToString();
			}
		}
		return text;
	}

	public static bool HaveSpecialChar(this string str)
	{
		string text = "`,/;";
		foreach (char value in text)
		{
			if (str.IndexOf(value) >= 0)
			{
				return true;
			}
		}
		return false;
	}

	public static void ToClipboard(this string s)
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = s;
		textEditor.SelectAll();
		textEditor.Copy();
	}

	public static Color32[] ColorToColor32(this Color[] colors)
	{
		Color32[] array = new Color32[colors.Length];
		for (int i = 0; i < colors.Length; i++)
		{
			array[i] = colors[i].ColorToColor32();
		}
		return array;
	}

	public static Color32 ColorToColor32(this Color color)
	{
		return new Color32((byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), (byte)(color.a * 255f));
	}

	public static Texture2D ScaleTexture(this Texture2D source, int maxSize)
	{
		int num = Mathf.Max(source.width, source.height);
		int targetWidth = source.width;
		int targetHeight = source.height;
		if (num > maxSize)
		{
			if (source.width > source.height)
			{
				targetWidth = maxSize;
				targetHeight = (int)((float)(source.height * maxSize) * 1f / (float)source.width);
			}
			else
			{
				targetHeight = maxSize;
				targetWidth = (int)((float)(source.width * maxSize) * 1f / (float)source.height);
			}
		}
		return source.ScaleTexture(targetWidth, targetHeight);
	}

	public static Texture2D ScaleTexture(this Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D texture2D = new Texture2D(targetWidth, targetHeight, source.format, mipChain: false);
		float num = 1f / (float)targetWidth;
		float num2 = 1f / (float)targetHeight;
		for (int i = 0; i < texture2D.height; i++)
		{
			for (int j = 0; j < texture2D.width; j++)
			{
				Color pixelBilinear = source.GetPixelBilinear((float)j / (float)texture2D.width, (float)i / (float)texture2D.height);
				texture2D.SetPixel(j, i, pixelBilinear);
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	public static List<string> ToStrList(this string str, string key)
	{
		if (str.IndexOf(key) >= 0)
		{
			int num = str.IndexOf(key);
			List<string> list = new List<string>();
			while (num >= 0)
			{
				list.Add(str.Substring(0, num));
				str = str.Substring(num + key.Length, str.Length - num - key.Length);
				num = str.IndexOf(key);
			}
			if (!string.IsNullOrEmpty(str))
			{
				list.Add(str);
			}
			return list;
		}
		List<string> list2 = new List<string>();
		list2.Add(str);
		return list2;
	}

	public static void PickImage(Action<Texture2D> finisEvent)
	{
		if (Application.platform == RuntimePlatform.WindowsPlayer)
		{
			Texture2D image2 = WindowsTools.GetImage();
			if (finisEvent != null)
			{
				finisEvent(image2);
			}
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			MAndroidTools.inter.OpenAlbum(delegate(Texture2D image)
			{
				if (finisEvent != null)
				{
					finisEvent(image);
				}
			});
		}
		else
		{
			UM_iGalleryService galleryService = UM_Application.GalleryService;
			int thumbnailSize = 1024;
			galleryService.PickImage(thumbnailSize, delegate(UM_MediaResult result)
			{
				if (result.IsSucceeded)
				{
					Texture2D thumbnail = result.Media.Thumbnail;
					if (finisEvent != null)
					{
						finisEvent(thumbnail);
					}
				}
			});
		}
	}
}
