using GameCommon;
using UnityEngine;

public class BZJX_DoubleClick : MonoBehaviour
{
	private int clickCount;

	private BZJX_ISwimObj sptISwimObj;

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
				for (int i = 0; i < BZJX_GameInfo.getInstance().UserList.Count; i++)
				{
					if (BZJX_GameInfo.getInstance().UserList[i].SeatIndex == BZJX_GameInfo.getInstance().User.SeatIndex && base.gameObject != BZJX_GameInfo.getInstance().UserList[i].LockFish)
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
		if (BZJX_GameInfo.getInstance() != null)
		{
			BZJX_GameInfo.getInstance().GameScene.ClickFish();
		}
		if (All_TipCanvas.GetInstance().IsPayPanelActive() || All_TipCanvas.GetInstance().IsCheckPwdActive())
		{
			return;
		}
		clickCount++;
		sptISwimObj = BZJX_FishPoolMngr.GetSingleton().GetSwimObjByTag(base.transform.parent.gameObject);
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
		BZJX_ISwimObj swimObjByTag = BZJX_FishPoolMngr.GetSingleton().GetSwimObjByTag(gameObject);
		if (!swimObjByTag || swimObjByTag.mFishType < BZJX_FISH_TYPE.Fish_Turtle || BZJX_GameInfo.getInstance().User.ScoreCount <= 0 || !gameObject.activeSelf || !BZJX_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr || swimObjByTag.bFishDead || ((bool)base.transform.parent.Find("Fish").gameObject && !base.transform.parent.Find("Fish").gameObject.activeSelf))
		{
			return;
		}
		BZJX_Lock currentGun = BZJX_GameInfo.getInstance().GameScene.GetCurrentGun(BZJX_GameInfo.getInstance().User.SeatIndex);
		for (int i = 0; i < BZJX_GameInfo.getInstance().UserList.Count; i++)
		{
			if (BZJX_GameInfo.getInstance().UserList[i].SeatIndex != BZJX_GameInfo.getInstance().User.SeatIndex)
			{
				continue;
			}
			if (!BZJX_GameInfo.getInstance().UserList[i].Lock)
			{
				sptISwimObj.bLocked = true;
				BZJX_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.StartLocking(BZJX_GameInfo.getInstance().UserList[i].SeatIndex, self: true);
				bool flag = BZJX_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
				continue;
			}
			BZJX_GameInfo.getInstance().UserList[i].Lock = false;
			Vector3 zero = Vector3.zero;
			if ((bool)BZJX_GameInfo.getInstance().UserList[i].LockFish)
			{
				BZJX_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
				zero = BZJX_GameInfo.getInstance().UserList[i].LockFish.transform.Find("LockFlag").position;
				BZJX_FishPoolMngr.GetSingleton().GetSwimObjByTag(BZJX_GameInfo.getInstance().UserList[i].LockFish).HideLockedFlag();
				BZJX_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.ChangeLockFish(zero);
				bool flag2 = BZJX_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
			}
		}
	}

	public void Release()
	{
		BZJX_GameInfo.getInstance().GameScene.ReleaseFish();
	}
}
