using System;
using System.Collections;
using UnityEngine;

public class BCBM_WinGameStart : MonoBehaviour
{
	private static BCBM_WinGameStart G_WinGameStart;

	public static BCBM_WinGameStart GetSingleton()
	{
		return G_WinGameStart;
	}

	private void Awake()
	{
		if (G_WinGameStart == null)
		{
			G_WinGameStart = this;
		}
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void StartGameHall(bool isCanLogin)
	{
		StopCoroutine("StartPCGame");
		StartCoroutine("StartPCGame", isCanLogin);
	}

	private IEnumerator StartPCGame(object typeValue)
	{
		bool isCanLogin = (bool)typeValue;
		string gamePath = Environment.CurrentDirectory + "\\GameHall.exe";
		if (isCanLogin)
		{
			BCBM_DataIOWin.GetSingleton().WriteUserInfo(BCBM_GameInfo.getInstance().UserId, BCBM_GameInfo.getInstance().Pwd, isHall: false, string.Empty);
		}
		else
		{
			BCBM_DataIOWin.GetSingleton().WriteUserInfo("0", "0", isHall: false, string.Empty);
		}
		yield return new WaitForSeconds(0.5f);
		BCBM_DataIOWin.GetSingleton().StartPCExe(gamePath);
		Application.Quit();
	}
}
