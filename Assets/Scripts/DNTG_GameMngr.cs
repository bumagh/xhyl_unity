using GameCommon;
using System;
using UnityEngine;

public class DNTG_GameMngr : MonoBehaviour
{
	public static DNTG_GameMngr G_GameMngr;

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
			DNTG_BulletPoolMngr.GetSingleton().SetPlayerIndex(_playerSeatID);
		}
	}

	public static DNTG_GameMngr GetSingleton()
	{
		return G_GameMngr;
	}

	private void Awake()
	{
		if (G_GameMngr == null)
		{
			G_GameMngr = this;
		}
		Application.runInBackground = false;
		Application.targetFrameRate = 60;
		Resources.UnloadUnusedAssets();
	}

	private void Start()
	{
		try
		{
			DNTG_NetMngr.GetSingleton().IsGameSceneLoadOk = true;
			mPlayerSeatID = DNTG_GameInfo.getInstance().User.SeatIndex;
		}
		catch (Exception)
		{
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (DNTG_GameParameter.G_bTest)
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
		}
		else if (DNTG_NetMngr.GetSingleton() != null)
		{
			DNTG_NetMngr.GetSingleton().MyPostMessageThread.ClearAllState();
		}
	}

	private void _setViewport()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		float num3 = 1280f;
		float num4 = 720f;
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
		DNTG_NetMngr.GetSingleton().IsGameSceneLoadOk = false;
		DNTG_NetMngr.GetSingleton().mSceneBg = -1;
		Resources.UnloadUnusedAssets();
	}
}
