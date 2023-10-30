using System;
using UnityEngine;

public class MAndroidTools : MonoBehaviour
{
	private class AndroidMyCallback : AndroidJavaProxy
	{
		public AndroidMyCallback()
			: base("com.SCY.Video.MyInterface")
		{
		}

		public void poo(string base64)
		{
			imageBase64 = base64;
		}
	}

	private static MAndroidTools m_inter;

	private Action<Texture2D> imageCallbake;

	private static string imageBase64 = string.Empty;

	private const string UNITY_PLAYER_CLASS = "com.unity3d.player.UnityPlayer";

	private const string UNITY_ACTIVITY_FIELD = "currentActivity";

	private const string WRAPPER_CLASS = "com.SCY.Video.JunAlbum";

	private static AndroidJavaObject currentUnityActivityAJO;

	private static AndroidJavaObject wrapper;

	private static AndroidJavaObject packageManager;

	public static MAndroidTools inter
	{
		get
		{
			if (m_inter == null)
			{
				GameObject gameObject = new GameObject();
				m_inter = gameObject.AddComponent<MAndroidTools>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				currentUnityActivityAJO = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
				packageManager = currentUnityActivityAJO.Call<AndroidJavaObject>("getPackageManager", Array.Empty<object>());
				wrapper = new AndroidJavaObject("com.SCY.Video.JunAlbum", currentUnityActivityAJO);
			}
			return m_inter;
		}
	}

	private void Update()
	{
		if (!string.IsNullOrEmpty(imageBase64) && Application.platform == RuntimePlatform.Android)
		{
			string base64EncodedStrong = imageBase64;
			imageBase64 = string.Empty;
			try
			{
				Texture2D texture2D = new Texture2D(1, 1);
				texture2D.LoadImageFromBase64(base64EncodedStrong);
				if (imageCallbake != null)
				{
					imageCallbake(texture2D);
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				throw;
			}
		}
	}

	public void OpenAlbum(Action<Texture2D> callback)
	{
		imageCallbake = callback;
		wrapper.Call("openAlbum", new AndroidMyCallback());
	}

	public void KillProcess(string packageName)
	{
		wrapper.Call("killProcess", packageName);
	}

	public void PutExtra(string msg, string bundleId)
	{
		packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", new object[1]
		{
			bundleId
		}).Call<AndroidJavaObject>("putExtra", new object[2]
		{
			"arguments",
			msg
		});
	}

	public string GetExtra()
	{
		AndroidJavaObject androidJavaObject = currentUnityActivityAJO.Call<AndroidJavaObject>("getIntent", Array.Empty<object>());
		if (androidJavaObject.Call<bool>("hasExtra", new object[1]
		{
			"arguments"
		}))
		{
			string text = androidJavaObject.Call<AndroidJavaObject>("getExtras", Array.Empty<object>()).Call<string>("getString", new object[1]
			{
				"arguments"
			});
			UnityEngine.Debug.Log(text);
			return text;
		}
		UnityEngine.Debug.Log("No auguments");
		return string.Empty;
	}
}
