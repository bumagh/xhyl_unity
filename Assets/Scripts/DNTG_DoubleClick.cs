using GameCommon;
using UnityEngine;

public class DNTG_DoubleClick : MonoBehaviour
{
	private int clickCount;

	private DNTG_ISwimObj sptISwimObj;

	private float time;

	private void Update()
	{
		if (base.gameObject.activeInHierarchy && sptISwimObj != null)
		{
			CheckClick2();
		}
	}

	private void CheckClick2()
	{
		if (!sptISwimObj || clickCount == 0)
		{
			return;
		}
		if (sptISwimObj.mServerID == sptISwimObj.mClickOneSID && DNTG_GameInfo.getInstance().GameScene.bLock)
		{
			for (int i = 0; i < DNTG_GameInfo.getInstance().UserList.Count; i++)
			{
				if (DNTG_GameInfo.getInstance().UserList[i].SeatIndex == DNTG_GameInfo.getInstance().User.SeatIndex && base.gameObject != DNTG_GameInfo.getInstance().UserList[i].LockFish)
				{
					SendLocking();
				}
			}
		}
		time = 0f;
		sptISwimObj.mClickOneSID = 0;
		clickCount = 0;
	}

	private void OnMouseDown()
	{
		if (DNTG_GameInfo.getInstance() != null)
		{
			DNTG_GameInfo.getInstance().GameScene.ClickFish();
		}
		if (All_TipCanvas.GetInstance().IsPayPanelActive() || All_TipCanvas.GetInstance().IsCheckPwdActive())
		{
			return;
		}
		clickCount++;
		sptISwimObj = DNTG_FishPoolMngr.GetSingleton().GetSwimObjByTag(base.transform.parent.gameObject);
		if (!sptISwimObj.bFishDead)
		{
			if (clickCount == 1)
			{
				sptISwimObj.mClickOneSID = sptISwimObj.mServerID;
			}
		}
		else
		{
			sptISwimObj = null;
			clickCount = 0;
		}
	}

	private void SendLocking()
	{
		GameObject gameObject = base.transform.parent.gameObject;
		DNTG_ISwimObj swimObjByTag = DNTG_FishPoolMngr.GetSingleton().GetSwimObjByTag(gameObject);
		if ((!swimObjByTag || swimObjByTag.mFishType < DNTG_FISH_TYPE.Fish_Turtle || DNTG_GameInfo.getInstance().User.ScoreCount <= 0 || !gameObject.activeSelf || !DNTG_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr || swimObjByTag.bFishDead) && swimObjByTag.specialFishType <= SpecialFishType.CommonFish)
		{
			return;
		}
		if (swimObjByTag.mFishType == DNTG_FISH_TYPE.Fish_GoldFull)
		{
			Transform transform = base.transform.parent.Find("MiddelFish/Fish");
			if (transform == null || (transform != null && !transform.gameObject.activeInHierarchy))
			{
				UnityEngine.Debug.LogError("=====金玉满堂不存在或者已经死了=====");
				return;
			}
		}
		else if ((bool)base.transform.parent.Find("Fish").gameObject && !base.transform.parent.Find("Fish").gameObject.activeSelf)
		{
			UnityEngine.Debug.LogError("=====杂鱼已经死了=====");
			return;
		}
		DNTG_Lock currentGun = DNTG_GameInfo.getInstance().GameScene.GetCurrentGun(DNTG_GameInfo.getInstance().User.SeatIndex);
		for (int i = 0; i < DNTG_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DNTG_GameInfo.getInstance().UserList[i].SeatIndex != DNTG_GameInfo.getInstance().User.SeatIndex)
			{
				continue;
			}
			if (!DNTG_GameInfo.getInstance().UserList[i].Lock)
			{
				sptISwimObj.bLocked = true;
				DNTG_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.StartLocking(DNTG_GameInfo.getInstance().UserList[i].SeatIndex, self: true);
				bool flag = DNTG_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
				continue;
			}
			DNTG_GameInfo.getInstance().UserList[i].Lock = false;
			Vector3 zero = Vector3.zero;
			if ((bool)DNTG_GameInfo.getInstance().UserList[i].LockFish)
			{
				DNTG_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
				zero = DNTG_GameInfo.getInstance().UserList[i].LockFish.transform.Find("LockFlag").position;
				DNTG_FishPoolMngr.GetSingleton().GetSwimObjByTag(DNTG_GameInfo.getInstance().UserList[i].LockFish).HideLockedFlag();
				DNTG_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.ChangeLockFish(zero);
				bool flag2 = DNTG_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
			}
		}
	}

	public void Release()
	{
		DNTG_GameInfo.getInstance().GameScene.ReleaseFish();
	}
}
