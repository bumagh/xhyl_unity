using System;
using System.Collections;
using UnityEngine;

public class JSYS_LL_WinGameStart : MonoBehaviour
{
	private static JSYS_LL_WinGameStart G_WinGameStart;

	public static JSYS_LL_WinGameStart GetSingleton()
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
			JSYS_LL_DataIOWin.GetSingleton().WriteUserInfo(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, isHall: false, string.Empty);
		}
		else
		{
			JSYS_LL_DataIOWin.GetSingleton().WriteUserInfo("0", "0", isHall: false, string.Empty);
		}
		yield return new WaitForSeconds(0.5f);
		JSYS_LL_DataIOWin.GetSingleton().StartPCExe(gamePath);
		Application.Quit();
	}
}
