using BestHTTP;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using BestHTTP.Examples;
using BestHTTP.Logger;
using BestHTTP.Statistics;
using System.Collections.Generic;
using UnityEngine;

public class SampleSelector : MonoBehaviour
{
	public const int statisticsHeight = 160;

	private List<SampleDescriptor> Samples = new List<SampleDescriptor>();

	public static SampleDescriptor SelectedSample;

	private Vector2 scrollPos;

	private void Awake()
	{
		HTTPManager.Logger.Level = Loglevels.All;
	}

	private void Start()
	{
		GUIHelper.ClientArea = new Rect(0f, 165f, Screen.width, Screen.height - 160 - 50);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (SelectedSample != null && SelectedSample.IsRunning)
			{
				SelectedSample.DestroyUnityObject();
			}
			else
			{
				Application.Quit();
			}
		}
		if ((UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter) || UnityEngine.Input.GetKeyDown(KeyCode.Return)) && SelectedSample != null && !SelectedSample.IsRunning)
		{
			SelectedSample.CreateUnityObject();
		}
	}

	private void OnGUI()
	{
		GeneralStatistics stats = HTTPManager.GetGeneralStatistics(StatisticsQueryFlags.All);
		GUIHelper.DrawArea(new Rect(0f, 0f, Screen.width / 3, 160f), drawHeader: false, delegate
		{
			GUIHelper.DrawCenteredText("Connections");
			GUILayout.Space(5f);
			GUIHelper.DrawRow("Sum:", stats.Connections.ToString());
			GUIHelper.DrawRow("Active:", stats.ActiveConnections.ToString());
			GUIHelper.DrawRow("Free:", stats.FreeConnections.ToString());
			GUIHelper.DrawRow("Recycled:", stats.RecycledConnections.ToString());
			GUIHelper.DrawRow("Requests in queue:", stats.RequestsInQueue.ToString());
		});
		GUIHelper.DrawArea(new Rect(Screen.width / 3, 0f, Screen.width / 3, 160f), drawHeader: false, delegate
		{
			GUIHelper.DrawCenteredText("Cache");
			if (!HTTPCacheService.IsSupported)
			{
				GUI.color = Color.yellow;
				GUIHelper.DrawCenteredText("Disabled in WebPlayer, WebGL & Samsung Smart TV Builds!");
				GUI.color = Color.white;
			}
			else
			{
				GUILayout.Space(5f);
				GUIHelper.DrawRow("Cached entities:", stats.CacheEntityCount.ToString());
				GUIHelper.DrawRow("Sum Size (bytes): ", stats.CacheSize.ToString("N0"));
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Clear Cache"))
				{
					HTTPCacheService.BeginClear();
				}
				GUILayout.EndVertical();
			}
		});
		GUIHelper.DrawArea(new Rect(Screen.width / 3 * 2, 0f, Screen.width / 3, 160f), drawHeader: false, delegate
		{
			GUIHelper.DrawCenteredText("Cookies");
			if (!CookieJar.IsSavingSupported)
			{
				GUI.color = Color.yellow;
				GUIHelper.DrawCenteredText("Saving and loading from disk is disabled in WebPlayer, WebGL & Samsung Smart TV Builds!");
				GUI.color = Color.white;
			}
			else
			{
				GUILayout.Space(5f);
				GUIHelper.DrawRow("Cookies:", stats.CookieCount.ToString());
				GUIHelper.DrawRow("Estimated size (bytes):", stats.CookieJarSize.ToString("N0"));
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Clear Cookies"))
				{
					HTTPManager.OnQuit();
				}
				GUILayout.EndVertical();
			}
		});
		if (SelectedSample == null || (SelectedSample != null && !SelectedSample.IsRunning))
		{
			GUIHelper.DrawArea(new Rect(0f, 165f, (SelectedSample != null) ? (Screen.width / 3) : Screen.width, Screen.height - 160 - 5), drawHeader: false, delegate
			{
				scrollPos = GUILayout.BeginScrollView(scrollPos);
				for (int i = 0; i < Samples.Count; i++)
				{
					DrawSample(Samples[i]);
				}
				GUILayout.EndScrollView();
			});
			if (SelectedSample != null)
			{
				DrawSampleDetails(SelectedSample);
			}
		}
		else if (SelectedSample != null && SelectedSample.IsRunning)
		{
			GUILayout.BeginArea(new Rect(0f, Screen.height - 50, Screen.width, 50f), string.Empty);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Back", GUILayout.MinWidth(100f)))
			{
				SelectedSample.DestroyUnityObject();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}

	private void DrawSample(SampleDescriptor sample)
	{
		if (sample.IsLabel)
		{
			GUILayout.Space(15f);
			GUIHelper.DrawCenteredText(sample.DisplayName);
			GUILayout.Space(5f);
		}
		else if (GUILayout.Button(sample.DisplayName))
		{
			sample.IsSelected = true;
			if (SelectedSample != null)
			{
				SelectedSample.IsSelected = false;
			}
			SelectedSample = sample;
		}
	}

	private void DrawSampleDetails(SampleDescriptor sample)
	{
		Rect rect = new Rect(Screen.width / 3, 165f, Screen.width / 3 * 2, Screen.height - 160 - 5);
		GUI.Box(rect, string.Empty);
		GUILayout.BeginArea(rect);
		GUILayout.BeginVertical();
		GUIHelper.DrawCenteredText(sample.DisplayName);
		GUILayout.Space(5f);
		GUILayout.Label(sample.Description);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Start Sample"))
		{
			sample.CreateUnityObject();
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
