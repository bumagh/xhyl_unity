using BCBM_GameCommon;
using System.Collections;
using UnityEngine;

public class BCBM_TableList : MonoBehaviour
{
	protected BCBM_PersonInfo mUserInfo;

	protected ArrayList mTableInfoList = new ArrayList();

	protected int mCurrentRoomId = -1;

	protected int mCurrentTableId;

	protected float mAnimationTime = -1f;

	protected int mAnimationStep = -1;

	protected float mStepTime;

	protected bool mIsDraging;

	private BCBM_GameInfo mGameInfo;

	public static BCBM_Desk[] deskListstatic;

	private void Awake()
	{
	}

	public void InitTableList(BCBM_Desk[] deskList, int iRoomId = 1)
	{
		mCurrentRoomId = iRoomId;
		mTableInfoList.Clear();
		mCurrentTableId = 0;
		if (deskList != null)
		{
			for (int i = 0; i < deskList.Length; i++)
			{
				BCBM_TableInfo bCBM_TableInfo = new BCBM_TableInfo();
				bCBM_TableInfo.RoomId = deskList[i].roomId;
				bCBM_TableInfo.TableServerID = deskList[i].id;
				bCBM_TableInfo.TableName = deskList[i].name;
				bCBM_TableInfo.MinBet = deskList[i].minBet;
				bCBM_TableInfo.MaxBet = deskList[i].maxBet;
				bCBM_TableInfo.Ristrict = deskList[i].minGold;
				bCBM_TableInfo.CreditPerCoin = deskList[i].exchange;
				bCBM_TableInfo.MinZHXBet = deskList[i].min_zxh;
				bCBM_TableInfo.MaxCD = deskList[i].betTime;
				bCBM_TableInfo.MaxZXBet = deskList[i].max_zx;
				bCBM_TableInfo.MaxHBet = deskList[i].max_h;
				bCBM_TableInfo.PersonCount = deskList[i].onlineNumber;
				bCBM_TableInfo.CoinInSetting = deskList[i].onceExchangeValue;
				bCBM_TableInfo.IsAutoKick = deskList[i].autoKick;
				mTableInfoList.Add(bCBM_TableInfo);
			}
		}
	}

	public void _clickTable(int num)
	{
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendDeskInfo(BCBM_GameInfo.getInstance().UserInfo.RoomId, num);
		BCBM_GameInfo.getInstance().UserInfo.TableId = num;
	}

	public void OnClickBackToRoom()
	{
		int roomId = BCBM_GameInfo.getInstance().UserInfo.RoomId;
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(roomId);
		BCBM_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
		BCBM_GameInfo.getInstance().UserInfo.RoomId = -1;
		BCBM_GameInfo.getInstance().UserInfo.TableId = -1;
	}

	protected void _releaseFromBg(GameObject go, bool isPress)
	{
	}

	protected void _dragTable(GameObject go, Vector2 delt)
	{
	}
}
