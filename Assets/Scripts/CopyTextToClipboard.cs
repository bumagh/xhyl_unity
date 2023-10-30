using System;
using UnityEngine;

public class CopyTextToClipboard : MonoBehaviour
{
	public static CopyTextToClipboard instance;

	private AndroidJavaObject jo;

	private AndroidJavaClass jc;

	private void Start()
	{
		instance = this;
		try
		{
			jc = new AndroidJavaClass("com.example.androidtools.MainActivity");
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.ToString());
		}
	}

	public void OnCopy(string text)
	{
		jc.CallStatic("copyTextToClipboard", text);
	}

	public string OnPaste()
	{
		string empty = string.Empty;
		return jc.CallStatic<string>("getTextFromClipboard", Array.Empty<object>());
	}
}
