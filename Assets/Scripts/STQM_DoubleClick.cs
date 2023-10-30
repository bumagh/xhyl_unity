using GameCommon;
using UnityEngine;

public class STQM_DoubleClick : MonoBehaviour
{
	private int clickCount;

	private STQM_ISwimObj sptISwimObj;

	private float time;

	private void Update()
	{
		CheckClick();
	}

	private void CheckClick()
	{
		if (!sptISwimObj)
		{
			return;
		}
		if (clickCount == 1)
		{
			time += Time.deltaTime;
			if (time >= 0.8f)
			{
				time = 0f;
				clickCount = 0;
				sptISwimObj.mClickOneSID = 0;
			}
		}
		else
		{
			if (clickCount == 0)
			{
				return;
			}
			if (time < 0.8f && sptISwimObj.mServerID == sptISwimObj.mClickOneSID)
			{
				for (int i = 0; i < STQM_GameInfo.getInstance().UserList.Count; i++)
				{
					if (STQM_GameInfo.getInstance().UserList[i].SeatIndex == STQM_GameInfo.getInstance().User.SeatIndex && base.gameObject != STQM_GameInfo.getInstance().UserList[i].LockFish)
					{
						SendLocking();
					}
				}
			}
			time = 0f;
			sptISwimObj.mClickOneSID = 0;
			clickCount = 0;
		}
	}

	private void OnMouseDown()
	{
		STQM_GameInfo.getInstance().GameScene.ClickFish();
		if (All_TipCanvas.GetInstance().IsPayPanelActive() || All_TipCanvas.GetInstance().IsCheckPwdActive())
		{
			return;
		}
		clickCount++;
		sptISwimObj = STQM_FishPoolMngr.GetSingleton().GetSwimObjByTag(base.transform.parent.gameObject);
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
		}
	}

	private void SendLocking()
	{
		GameObject gameObject = base.transform.parent.gameObject;
		STQM_ISwimObj swimObjByTag = STQM_FishPoolMngr.GetSingleton().GetSwimObjByTag(gameObject);
		if (!swimObjByTag || swimObjByTag.mFishType < STQM_FISH_TYPE.Fish_Turtle || STQM_GameInfo.getInstance().User.ScoreCount <= 0 || !gameObject.activeSelf || !STQM_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr || swimObjByTag.bFishDead || ((bool)base.transform.parent.Find("Fish").gameObject && !base.transform.parent.Find("Fish").gameObject.activeSelf))
		{
			return;
		}
		STQM_Lock currentGun = STQM_GameInfo.getInstance().GameScene.GetCurrentGun(STQM_GameInfo.getInstance().User.SeatIndex);
		for (int i = 0; i < STQM_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STQM_GameInfo.getInstance().UserList[i].SeatIndex != STQM_GameInfo.getInstance().User.SeatIndex)
			{
				continue;
			}
			if (!STQM_GameInfo.getInstance().UserList[i].Lock)
			{
				sptISwimObj.bLocked = true;
				STQM_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.StartLocking(STQM_GameInfo.getInstance().UserList[i].SeatIndex, self: true);
				bool flag = STQM_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
				continue;
			}
			STQM_GameInfo.getInstance().UserList[i].Lock = false;
			Vector3 zero = Vector3.zero;
			if ((bool)STQM_GameInfo.getInstance().UserList[i].LockFish)
			{
				STQM_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
				zero = STQM_GameInfo.getInstance().UserList[i].LockFish.transform.Find("LockFlag").position;
				STQM_FishPoolMngr.GetSingleton().GetSwimObjByTag(STQM_GameInfo.getInstance().UserList[i].LockFish).HideLockedFlag();
				STQM_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.ChangeLockFish(zero);
				bool flag2 = STQM_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
			}
		}
	}

	public void Release()
	{
		STQM_GameInfo.getInstance().GameScene.ReleaseFish();
	}
}
