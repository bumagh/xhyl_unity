using UnityEngine;

public class STTF_DoubleClick : MonoBehaviour
{
	private int clickCount;

	private STTF_ISwimObj sptISwimObj;

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
				for (int i = 0; i < STTF_GameInfo.getInstance().UserList.Count; i++)
				{
					if (STTF_GameInfo.getInstance().UserList[i].SeatIndex == STTF_GameInfo.getInstance().User.SeatIndex && base.gameObject != STTF_GameInfo.getInstance().UserList[i].LockFish)
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
		STTF_GameInfo.getInstance().GameScene.ClickFish();
		if (All_TipCanvas.GetInstance().IsPayPanelActive() || All_TipCanvas.GetInstance().IsCheckPwdActive())
		{
			return;
		}
		clickCount++;
		sptISwimObj = STTF_FishPoolMngr.GetSingleton().GetSwimObjByTag(base.transform.parent.gameObject);
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
		STTF_ISwimObj swimObjByTag = STTF_FishPoolMngr.GetSingleton().GetSwimObjByTag(gameObject);
		if (!swimObjByTag || STTF_GameInfo.getInstance().User.ScoreCount <= 0 || !gameObject.activeSelf || !STTF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr || swimObjByTag.bFishDead || ((bool)base.transform.parent.Find("Fish").gameObject && !base.transform.parent.Find("Fish").gameObject.activeSelf))
		{
			return;
		}
		STTF_Lock currentGun = STTF_GameInfo.getInstance().GameScene.GetCurrentGun(STTF_GameInfo.getInstance().User.SeatIndex);
		for (int i = 0; i < STTF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STTF_GameInfo.getInstance().UserList[i].SeatIndex != STTF_GameInfo.getInstance().User.SeatIndex)
			{
				continue;
			}
			if (!STTF_GameInfo.getInstance().UserList[i].Lock)
			{
				sptISwimObj.bLocked = true;
				STTF_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.StartLocking(STTF_GameInfo.getInstance().UserList[i].SeatIndex, self: true);
				bool flag = STTF_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
				continue;
			}
			STTF_GameInfo.getInstance().UserList[i].Lock = false;
			Vector3 zero = Vector3.zero;
			if ((bool)STTF_GameInfo.getInstance().UserList[i].LockFish)
			{
				STTF_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
				zero = STTF_GameInfo.getInstance().UserList[i].LockFish.transform.Find("LockFlag").position;
				STTF_FishPoolMngr.GetSingleton().GetSwimObjByTag(STTF_GameInfo.getInstance().UserList[i].LockFish).HideLockedFlag();
				STTF_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.ChangeLockFish(zero);
				bool flag2 = STTF_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
			}
		}
	}

	public void Release()
	{
		STTF_GameInfo.getInstance().GameScene.ReleaseFish();
	}
}
