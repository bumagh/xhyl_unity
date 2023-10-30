using System;
using UnityEngine;

namespace BestHTTP.Examples
{
	public sealed class TextureDownloadSample : MonoBehaviour
	{
		private const string BaseURL = "https://besthttp.azurewebsites.net/Content/";

		private string[] Images = new string[9]
		{
			"One.png",
			"Two.png",
			"Three.png",
			"Four.png",
			"Five.png",
			"Six.png",
			"Seven.png",
			"Eight.png",
			"Nine.png"
		};

		private Texture2D[] Textures = new Texture2D[9];

		private bool allDownloadedFromLocalCache;

		private int finishedCount;

		private Vector2 scrollPos;

		private void Awake()
		{
			HTTPManager.MaxConnectionPerServer = 1;
			for (int i = 0; i < Images.Length; i++)
			{
				Textures[i] = new Texture2D(100, 150);
			}
		}

		private void OnDestroy()
		{
			HTTPManager.MaxConnectionPerServer = 4;
		}

		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, drawHeader: true, delegate
			{
				scrollPos = GUILayout.BeginScrollView(scrollPos);
				GUILayout.SelectionGrid(0, Textures, 3);
				if (finishedCount == Images.Length && allDownloadedFromLocalCache)
				{
					GUIHelper.DrawCenteredText("All images loaded from the local cache!");
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				GUILayout.Label("Max Connection/Server: ", GUILayout.Width(150f));
				GUILayout.Label(HTTPManager.MaxConnectionPerServer.ToString(), GUILayout.Width(20f));
				HTTPManager.MaxConnectionPerServer = (byte)GUILayout.HorizontalSlider((int)HTTPManager.MaxConnectionPerServer, 1f, 10f);
				GUILayout.EndHorizontal();
				if (GUILayout.Button("Start Download"))
				{
					DownloadImages();
				}
				GUILayout.EndScrollView();
			});
		}

		private void DownloadImages()
		{
			allDownloadedFromLocalCache = true;
			finishedCount = 0;
			for (int i = 0; i < Images.Length; i++)
			{
				Textures[i] = new Texture2D(100, 150);
				HTTPRequest hTTPRequest = new HTTPRequest(new Uri("https://besthttp.azurewebsites.net/Content/" + Images[i]), ImageDownloaded);
				hTTPRequest.Tag = Textures[i];
				hTTPRequest.Send();
			}
		}

		private void ImageDownloaded(HTTPRequest req, HTTPResponse resp)
		{
			finishedCount++;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					Texture2D tex = req.Tag as Texture2D;
					tex.LoadImage(resp.Data);
					allDownloadedFromLocalCache = (allDownloadedFromLocalCache && resp.IsFromCache);
				}
				else
				{
					UnityEngine.Debug.LogWarning($"Request finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}");
				}
				break;
			case HTTPRequestStates.Error:
				UnityEngine.Debug.LogError("Request Finished with Error! " + ((req.Exception == null) ? "No Exception" : (req.Exception.Message + "\n" + req.Exception.StackTrace)));
				break;
			case HTTPRequestStates.Aborted:
				UnityEngine.Debug.LogWarning("Request Aborted!");
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				UnityEngine.Debug.LogError("Connection Timed Out!");
				break;
			case HTTPRequestStates.TimedOut:
				UnityEngine.Debug.LogError("Processing the request Timed Out!");
				break;
			}
		}
	}
}
