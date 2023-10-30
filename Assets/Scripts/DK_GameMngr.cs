using GameCommon;
using System;
using UnityEngine;

public class DK_GameMngr : MonoBehaviour
{
	public static DK_GameMngr G_GameMngr;

	private int _playerSeatID = 1;

	private long lCurTickTime;

	private bool _IsPause;

	private bool _IsReset;

	public int mPlayerSeatID
	{
		get
		{
			return _playerSeatID;
		}
		set
		{
			_playerSeatID = value;
			DK_BulletPoolMngr.GetSingleton().SetPlayerIndex(_playerSeatID);
		}
	}

	public static DK_GameMngr GetSingleton()
	{
		return G_GameMngr;
	}

	private void Awake()
	{
		if (G_GameMngr == null)
		{
			G_GameMngr = this;
		}
		Application.runInBackground = true;
		Application.targetFrameRate = 60;
		Resources.UnloadUnusedAssets();
	}

	private void Start()
	{
		DK_NetMngr.GetSingleton().IsGameSceneLoadOk = true;
		_setViewport();
		mPlayerSeatID = DK_GameInfo.getInstance().User.SeatIndex;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (DK_GameParameter.G_bTest)
		{
			if (pauseStatus)
			{
				UnityEngine.Debug.Log("OnApplicationPause");
			}
			else
			{
				UnityEngine.Debug.Log("OnApplicationBack");
			}
		}
		if (pauseStatus)
		{
			lCurTickTime = DateTime.Now.Ticks;
			return;
		}
		long num = DateTime.Now.Ticks - lCurTickTime;
		DK_NetMngr.GetSingleton().MyPostMessageThread.ClearAllState();
	}

	private void _setViewport()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		float num3 = Screen.width;
		float num4 = Screen.height;
		float x = 0f;
		float y = 0f;
		float num5 = 1f;
		float num6 = 1f;
		if (num3 / num4 > num / num2)
		{
			num5 = 1f;
			num6 = num4 * num / num3 / num2;
			y = (1f - num6) / 2f;
		}
		else if (num3 / num4 < num / num2)
		{
			num6 = 1f;
			num5 = num3 * num2 / num4 / num;
			x = (1f - num5) / 2f;
		}
		for (int i = 0; i < Camera.allCameras.Length; i++)
		{
			Camera camera = Camera.allCameras[i];
			camera.rect = new Rect(x, y, num5, num6);
		}
	}

	private void OnDestroy()
	{
		DK_NetMngr.GetSingleton().IsGameSceneLoadOk = false;
		DK_NetMngr.GetSingleton().mSceneBg = -1;
		Resources.UnloadUnusedAssets();
	}
}
