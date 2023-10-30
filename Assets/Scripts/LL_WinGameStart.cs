using System;
using System.Collections;
using UnityEngine;

public class LL_WinGameStart : MonoBehaviour
{
	private static LL_WinGameStart G_WinGameStart;

	public static LL_WinGameStart GetSingleton()
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
		//TODO 启动PC的可执行文件，此逻辑在移动端无用
		// StopCoroutine("StartPCGame");
		// StartCoroutine("StartPCGame", isCanLogin);
	}

	// private IEnumerator StartPCGame(object typeValue)
	// {
	// 	bool isCanLogin = (bool)typeValue;
	// 	string gamePath = Environment.CurrentDirectory + "\\GameHall.exe";
	// 	if (isCanLogin)
	// 	{
	// 		LL_DataIOWin.GetSingleton().WriteUserInfo(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, isHall: false, string.Empty);
	// 	}
	// 	else
	// 	{
	// 		LL_DataIOWin.GetSingleton().WriteUserInfo("0", "0", isHall: false, string.Empty);
	// 	}
	// 	yield return new WaitForSeconds(0.5f);
	// 	LL_DataIOWin.GetSingleton().StartPCExe(gamePath);
	// 	Application.Quit();
	// }
}
