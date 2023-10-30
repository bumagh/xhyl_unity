using System;
using UnityEngine;

public class DP_GameMsgMngr : MonoBehaviour
{
	private long lCurTickTime;

	private bool bPause;

	private bool bReset;

	public static DP_GameMsgMngr G_GameMsgMngr;

	public static DP_GameMsgMngr GetSingleton()
	{
		return G_GameMsgMngr;
	}

	private void Awake()
	{
		if (G_GameMsgMngr == null)
		{
			G_GameMsgMngr = this;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		bPause = pauseStatus;
		if (bPause)
		{
			lCurTickTime = DateTime.Now.Ticks;
		}
		else
		{
			bReset = true;
		}
	}

	private void Update()
	{
		BackGroundCheck();
	}

	private void BackGroundCheck()
	{
		if (bReset)
		{
			bReset = false;
			long num = DateTime.Now.Ticks - lCurTickTime;
			if (num > 120000000)
			{
				DP_NetMngr.GetSingleton().MyPostMessageThread.ClearAllState();
			}
		}
	}
}
