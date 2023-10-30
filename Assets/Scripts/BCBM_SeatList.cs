using BCBM_GameCommon;
using UnityEngine;

public class BCBM_SeatList : MonoBehaviour
{
	private BCBM_GameInfo mGameInfo;

	private void Start()
	{
		base.transform.gameObject.AddComponent<TweenAlpha>();
		base.transform.GetComponent<TweenAlpha>().alpha = 1f;
		HideSeatList(3);
		mGameInfo = BCBM_GameInfo.getInstance();
	}

	public void HideSeatList(int iMode = 0)
	{
		base.transform.GetComponent<TweenAlpha>().alpha = 1f;
		base.transform.GetComponent<TweenAlpha>().enabled = false;
		switch (iMode)
		{
		case 1:
			if (BCBM_AppUIMngr.GetSingleton().GetAppState == AppState.App_On_TableList_Panel)
			{
			}
			break;
		case 2:
			if (BCBM_AppUIMngr.GetSingleton().GetAppState == AppState.App_On_TableList_Panel)
			{
			}
			break;
		}
	}

	public void OnClickBackToTable()
	{
		UnityEngine.Debug.LogError(base.gameObject.name + " 点击返回到桌台");
	}
}
