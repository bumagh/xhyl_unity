using System;
using UnityEngine;
using UnityEngine.UI;

public class JSYS_LL_RoomList : MonoBehaviour
{
	public Text userName;

	public Text coinCount;

	public Text testCoinCount;

	private JSYS_LL_GameInfo mGameInfo;

	private void Start()
	{
		JSYS_LL_NetMngr.isInLoading = false;
		mGameInfo = JSYS_LL_GameInfo.getInstance();
		updateUserInfo();
	}

	public void updateUserInfo()
	{
		try
		{
			userName.text = mGameInfo.UserInfo.strName;
			coinCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.CoinCount % 10000).ToString() : mGameInfo.UserInfo.CoinCount.ToString());
			testCoinCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.ExpCoinCount % 10000).ToString() : mGameInfo.UserInfo.ExpCoinCount.ToString());
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
	}

	public void ShowRoomList(int iMode)
	{
		JSYS_LL_GameTipManager.GetSingleton().EndNetTiming();
	}

	public void OnClickRoom(int iRoomId)
	{
		if (!JSYS_LL_MyTest.TEST)
		{
			UnityEngine.Debug.LogError(iRoomId);
			return;
		}
		GameObject.Find("TableListPanel").GetComponent<JSYS_LL_TableList>().ShowTableList();
		string[] array = new string[8];
		int[] array2 = new int[8];
		int[] array3 = new int[8];
		for (int i = 0; i < 6; i++)
		{
			array[i] = "person" + i;
			array2[i] = i + 1;
			array3[i] = i + 1;
		}
	}

	public void clickBack()
	{
		JSYS_LL_GameTipManager.GetSingleton().ClickBlack();
	}
}
