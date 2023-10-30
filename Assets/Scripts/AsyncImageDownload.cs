using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AsyncImageDownload : MonoBehaviour
{
	public Sprite placeholder;

	private static AsyncImageDownload _instance;

	public static AsyncImageDownload Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("AsyncImageDownload");
				_instance = gameObject.AddComponent<AsyncImageDownload>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				_instance.Init();
			}
			return _instance;
		}
	}

	public string path => Application.persistentDataPath + "/ImageCache/";

	public static AsyncImageDownload GetInstance()
	{
		return Instance;
	}

	public bool Init()
	{
		if (!Directory.Exists(Application.persistentDataPath + "/ImageCache/"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/ImageCache/");
		}
		if (placeholder == null)
		{
			Texture2D texture2D = Resources.Load<Texture2D>("qrm");
			placeholder = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero);
		}
		return true;
	}

	public void SetAsyncImage(string url, Image image)
	{
		image.sprite = placeholder;
		try
		{
			if (!File.Exists(path + url.GetHashCode()))
			{
				StartCoroutine(DownloadImage(url, image));
			}
			else
			{
				StartCoroutine(DownloadImage(url, image));
			}
		}
		catch (Exception)
		{
		}
	}

	private IEnumerator DownloadImage(string url, Image image)
	{
		UnityEngine.Debug.Log("downloading new image:" + path + url.GetHashCode());
		WWW www = new WWW(url);
		yield return www;
		if (www.isDone && www.error == null)
		{
			Texture2D texture = www.texture;
			byte[] bytes = texture.EncodeToPNG();
			File.WriteAllBytes(path + url.GetHashCode(), bytes);
			Sprite sprite2 = image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0f, 0f));
		}
		www.Dispose();
	}

	private IEnumerator LoadLocalImage(string url, Image image)
	{
		string filePath = "file:///" + path + url.GetHashCode();
		WWW www = new WWW(filePath);
		yield return www;
		if (www.isDone && www.error == null)
		{
			Texture2D texture = www.texture;
			Sprite sprite2 = image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0f, 0f));
		}
		www.Dispose();
	}
}
