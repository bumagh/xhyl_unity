using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SafeTools
{
	public static DataConfig m_config;

	public static char[] m_mapA;

	public static char[] m_mapB;

	public static bool m_isInit;

	private static string m_ip;

	private static string m_url;

	public static string RSA_Encrypt(string key, string content)
	{
		UnityEngine.Debug.Log("key1:" + key);
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.FromXmlString(key);
		return Convert.ToBase64String(rSACryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(content), fOAEP: false));
	}

	public static string RSA_Decrypt(string key, string source)
	{
		UnityEngine.Debug.Log("key2:" + key);
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.FromXmlString(key);
		byte[] rgb = Convert.FromBase64String(source);
		byte[] bytes = rSACryptoServiceProvider.Decrypt(rgb, fOAEP: false);
		return Encoding.UTF8.GetString(bytes);
	}

	public static string RSA_Decrypt(RSAParameters publicParas, string source)
	{
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.ImportParameters(publicParas);
		byte[] rgb = Convert.FromBase64String(source);
		byte[] bytes = rSACryptoServiceProvider.Decrypt(rgb, fOAEP: false);
		return Encoding.UTF8.GetString(bytes);
	}

	public static string RSA_Decrypt(string strExponent, string strModulus, string source)
	{
		RSAParameters publicParas = default(RSAParameters);
		publicParas.Exponent = Convert.FromBase64String(strExponent);
		publicParas.Modulus = Convert.FromBase64String(strModulus);
		return RSA_Decrypt(publicParas, source);
	}

	public static string RSA_Decrypt(string[] paras, string source)
	{
		RSAParameters publicParas = default(RSAParameters);
		publicParas.Exponent = Convert.FromBase64String(paras[0]);
		publicParas.Modulus = Convert.FromBase64String(paras[1]);
		return RSA_Decrypt(publicParas, source);
	}

	public static string[] RSA_Resolve(RSACryptoServiceProvider rsa)
	{
		string[] array = new string[2];
		RSAParameters rSAParameters = rsa.ExportParameters(includePrivateParameters: false);
		array[0] = Convert.ToBase64String(rSAParameters.Exponent);
		array[1] = Convert.ToBase64String(rSAParameters.Modulus);
		return array;
	}

	public static string AES_Transform(byte[] key, byte[] IV, string content, bool isEncrypt = false)
	{
		string empty = string.Empty;
		byte[] array = (!isEncrypt) ? Convert.FromBase64String(content) : Encoding.UTF8.GetBytes(content);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Key = key;
		rijndaelManaged.IV = IV;
		rijndaelManaged.Mode = CipherMode.CBC;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		ICryptoTransform cryptoTransform2;
		if (isEncrypt)
		{
			ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
			cryptoTransform2 = cryptoTransform;
		}
		else
		{
			cryptoTransform2 = rijndaelManaged.CreateDecryptor();
		}
		ICryptoTransform cryptoTransform3 = cryptoTransform2;
		byte[] array2 = cryptoTransform3.TransformFinalBlock(array, 0, array.Length);
		if (isEncrypt)
		{
			return Convert.ToBase64String(array2);
		}
		return Encoding.UTF8.GetString(array2);
	}

	public static string AES_Transform(string keyStr, string IVStr, string content, bool isEncrypt = false)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(keyStr);
		byte[] bytes2 = Encoding.UTF8.GetBytes(IVStr);
		return AES_Transform(bytes, bytes2, content, isEncrypt);
	}

	public static string _Adjust_String_1(string source)
	{
		char[] array = source.ToCharArray();
		int i = 1;
		int num;
		for (num = array.Length - 2; i < num; i += 2)
		{
			char c = array[i];
			array[i] = array[i + 1];
			array[i + 1] = c;
		}
		i = 1;
		num = array.Length - 2;
		while (i < num)
		{
			char c2 = array[i];
			array[i] = array[num];
			array[num] = c2;
			i++;
			num--;
		}
		return new string(array);
	}

	public static string Adjust_String_1(string source)
	{
		return (string)GetValue(IE_Adjust_String_1(source), toEnd: true);
	}

	private static IEnumerator IE_Adjust_String_1(string source)
	{
		char[] chars = source.ToCharArray();
		int begin2 = 1;
		int end2;
		for (end2 = chars.Length - 2; begin2 < end2; begin2 += 2)
		{
			char c = chars[begin2];
			chars[begin2] = chars[begin2 + 1];
			chars[begin2 + 1] = c;
		}
		begin2 = 1;
		end2 = chars.Length - 2;
		while (begin2 < end2)
		{
			char c2 = chars[begin2];
			chars[begin2] = chars[end2];
			chars[end2] = c2;
			begin2++;
			end2--;
		}
		yield return new string(chars);
	}

	public static IEnumerator IE_Adjust_String_3(string source)
	{
		char[] chars = source.ToCharArray();
		char[] map3 = m_mapA;
		HashSet<char> set3 = new HashSet<char>();
		set3.UnionWith(map3);
		char[] map2 = m_mapB;
		HashSet<char> set2 = new HashSet<char>(set3);
		set2.IntersectWith(map2);
		Dictionary<char, int> dic = new Dictionary<char, int>();
		int j = 0;
		for (int len3 = map2.Length; j < len3; j++)
		{
			dic.Add(map3[j], j);
		}
		char[] retChars = new char[chars.Length];
		int i = 0;
		for (int len2 = chars.Length; i < len2; i++)
		{
			if (dic.ContainsKey(chars[i]))
			{
				retChars[i] = map2[dic[chars[i]]];
			}
			else
			{
				retChars[i] = chars[i];
			}
		}
		yield return new string(retChars);
	}

	public static string Adjust_String_3(string source)
	{
		return (string)GetValue(IE_Adjust_String_3(source), toEnd: true);
	}

	public static bool CompareStr(string strA, string strB)
	{
		int length = strA.Length;
		int length2 = strB.Length;
		UnityEngine.Debug.Log($"lenA:{length}, lenB:{length2}");
		if (length != length2)
		{
			return false;
		}
		bool result = true;
		for (int i = 0; i < length; i++)
		{
			if (strA[i] != strB[i])
			{
				UnityEngine.Debug.Log($"pos:{i}, A:[{strA[i]}], B:[{strB[i]}]");
				result = false;
			}
		}
		return result;
	}

	public static IEnumerator IE_Demo(string strA, string strB)
	{
		string ret = strA + strB;
		yield return 123;
		yield return ret;
	}

	public static T GetValue<T>(IEnumerator ie, bool toEnd = false)
	{
		while (ie.MoveNext() && (toEnd || ie.Current == null))
		{
		}
		return (T)ie.Current;
	}

	public static object GetValue(IEnumerator ie, bool toEnd = false)
	{
		while (ie.MoveNext() && (toEnd || ie.Current == null))
		{
		}
		return ie.Current;
	}

	public static void Init(DataConfig config)
	{
		UnityEngine.Debug.Log("safetools.init");
		m_config = config;
		m_mapA = config.Key_a.ToCharArray();
		m_mapB = config.Key_b.ToCharArray();
		m_ip = (string)GetValue(IE_ReadIp(), toEnd: true);
		m_url = (string)GetValue(IE_ReadUrl(), toEnd: true);
		UnityEngine.Debug.Log("ip: " + m_ip);
		m_isInit = true;
	}

	public static string GetIp()
	{
		if (!m_isInit)
		{
			throw new Exception("config not init");
		}
		return GetValue<string>(IE_GetIp(), toEnd: true);
	}

	public static IPAddress GetAddress()
	{
		string ip = GetIp();
		IPAddress address = null;
		try
		{
			if (IPAddress.TryParse(ip, out address))
			{
				return address;
			}
			IPAddress[] addressList = Dns.GetHostEntry(ip).AddressList;
			IPAddress[] array = addressList;
			foreach (IPAddress iPAddress in array)
			{
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					address = iPAddress;
					break;
				}
			}
			return address;
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
			return address;
		}
		finally
		{
		}
	}

	private static IEnumerator IE_GetIp()
	{
		yield return m_ip;
	}

	public static string GetUrl()
	{
		if (!m_isInit)
		{
			throw new Exception("config not init");
		}
		return GetValue<string>(IE_GetUrl(), toEnd: true);
	}

	private static IEnumerator IE_GetUrl()
	{
		yield return m_url;
	}

	public static string ParseVersion(string content)
	{
		return GetValue<string>(IE_ParseVersion(content), toEnd: true);
	}

	private static IEnumerator IE_ParseVersion(string content)
	{
		string private_key = Adjust_String_3(m_config.Key_3);
		string aes_key = RSA_Decrypt(private_key, m_config.Key_4);
		string aes_iv = RSA_Decrypt(private_key, m_config.Key_5);
		yield return AES_Transform(aes_key, aes_iv, content);
	}

	private static IEnumerator IE_ReadIp()
	{
		string private_key2 = m_config.Key_2;
		if (private_key2 == null || private_key2.Length == 0)
		{
			throw new Exception("config.Key_2 is empty");
		}
		private_key2 = Adjust_String_3(private_key2);
		string strIp = Adjust_String_3(m_config.IP);
		yield return RSA_Decrypt(private_key2, strIp);
	}

	private static IEnumerator IE_ReadUrl()
	{
		string private_key2 = m_config.Key_2;
		if (private_key2 == null || private_key2.Length == 0)
		{
			throw new Exception("config.Key_2 is empty");
		}
		private_key2 = Adjust_String_3(private_key2);
		if (string.IsNullOrEmpty(m_config.Key_8))
		{
			yield return string.Empty;
			yield break;
		}
		string strUrl = Adjust_String_3(m_config.Key_8);
		yield return RSA_Decrypt(private_key2, strUrl);
	}

	public static int GetPort()
	{
		return m_config.Port;
	}

	public static string GetVersionKey()
	{
		return string.Empty;
	}

	private static IEnumerator IE_GetVersionKey()
	{
		string empty = string.Empty;
		string private_key2 = m_config.Key_3;
		if (private_key2 == null || private_key2.Length == 0)
		{
			throw new Exception("config.Key_3 is empty");
		}
		private_key2 = Adjust_String_3(private_key2);
		yield return RSA_Decrypt(private_key2, m_config.Key_4);
	}

	private static IEnumerator Post(string url, string data)
	{
		WWW www = new WWW(url, Encoding.UTF8.GetBytes(data));
		yield return www;
		if (www.error != null)
		{
			UnityEngine.Debug.Log(www.error);
		}
		else
		{
			UnityEngine.Debug.Log(www.text);
		}
	}
}
