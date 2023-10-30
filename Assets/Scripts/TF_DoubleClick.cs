using UnityEngine;

public class TF_DoubleClick : MonoBehaviour
{
	private int clickCount;

	private TF_ISwimObj sptISwimObj;

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
				for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
				{
					if (TF_GameInfo.getInstance().UserList[i].SeatIndex == TF_GameInfo.getInstance().User.SeatIndex && base.gameObject != TF_GameInfo.getInstance().UserList[i].LockFish)
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
		TF_GameInfo.getInstance().GameScene.ClickFish();
		if (TF_GameInfo.getInstance().GameScene.sptDlgInOut.gameObject.activeSelf)
		{
			return;
		}
		clickCount++;
		sptISwimObj = TF_FishPoolMngr.GetSingleton().GetSwimObjByTag(base.transform.parent.gameObject);
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
		TF_ISwimObj swimObjByTag = TF_FishPoolMngr.GetSingleton().GetSwimObjByTag(gameObject);
		if (!swimObjByTag || TF_GameInfo.getInstance().User.ScoreCount <= 0 || !gameObject.activeSelf || !TF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr || swimObjByTag.bFishDead || ((bool)base.transform.parent.Find("Fish").gameObject && !base.transform.parent.Find("Fish").gameObject.activeSelf))
		{
			return;
		}
		TF_Lock currentGun = TF_GameInfo.getInstance().GameScene.GetCurrentGun(TF_GameInfo.getInstance().User.SeatIndex);
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].SeatIndex != TF_GameInfo.getInstance().User.SeatIndex)
			{
				continue;
			}
			if (!TF_GameInfo.getInstance().UserList[i].Lock)
			{
				sptISwimObj.bLocked = true;
				TF_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.StartLocking(TF_GameInfo.getInstance().UserList[i].SeatIndex, self: true);
				bool flag = TF_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
				continue;
			}
			TF_GameInfo.getInstance().UserList[i].Lock = false;
			Vector3 zero = Vector3.zero;
			if ((bool)TF_GameInfo.getInstance().UserList[i].LockFish)
			{
				TF_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
				zero = TF_GameInfo.getInstance().UserList[i].LockFish.transform.Find("LockFlag").position;
				TF_FishPoolMngr.GetSingleton().GetSwimObjByTag(TF_GameInfo.getInstance().UserList[i].LockFish).HideLockedFlag();
				TF_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.ChangeLockFish(zero);
				bool flag2 = TF_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
			}
		}
	}

	public void Release()
	{
		TF_GameInfo.getInstance().GameScene.ReleaseFish();
	}
}
