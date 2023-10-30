using LL_GameCommon;
using LL_UICommon;
using UnityEngine;

public class LL_ProcessEscapeButton : MonoBehaviour
{
	protected LL_HudManager mHudMngr;

	protected LL_TableList mTableList;

	protected LL_SeatList mSeatList;

	private void Start()
	{
		mHudMngr = GameObject.Find("HudPanel").GetComponent<LL_HudManager>();
		mTableList = GameObject.Find("TableListPanel").GetComponent<LL_TableList>();
		mSeatList = GameObject.Find("SeatPanel").GetComponent<LL_SeatList>();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_TableList_Panel && LL_GameTipManager.GetSingleton().GetTipType() == EGameTipType.NoneTip)
			{
				mTableList.OnClickBackToRoom();
			}
			else if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_Table && LL_GameTipManager.GetSingleton().GetTipType() == EGameTipType.NoneTip)
			{
				mSeatList.OnClickBackToTable();
			}
			else if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_Game && LL_GameTipManager.GetSingleton().GetTipType() == EGameTipType.NoneTip)
			{
				mHudMngr.OnClickExitGame();
			}
		}
	}
}
