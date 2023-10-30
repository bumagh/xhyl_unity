using GameCommon;
using UnityEngine;

public class DK_DoubleClick : MonoBehaviour
{
	private int clickCount;

	private DK_ISwimObj sptISwimObj;

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
				for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
				{
					if (DK_GameInfo.getInstance().UserList[i].SeatIndex == DK_GameInfo.getInstance().User.SeatIndex && base.gameObject != DK_GameInfo.getInstance().UserList[i].LockFish)
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
		DK_GameInfo.getInstance().GameScene.ClickFish();
		if (All_TipCanvas.GetInstance().IsPayPanelActive() || All_TipCanvas.GetInstance().IsCheckPwdActive())
		{
			return;
		}
		clickCount++;
		sptISwimObj = DK_FishPoolMngr.GetSingleton().GetSwimObjByTag(base.transform.parent.gameObject);
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
		DK_ISwimObj swimObjByTag = DK_FishPoolMngr.GetSingleton().GetSwimObjByTag(gameObject);
		if (!swimObjByTag || swimObjByTag.mFishType < DK_FISH_TYPE.Fish_Turtle || DK_GameInfo.getInstance().User.ScoreCount <= 0 || !gameObject.activeSelf || !DK_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr || swimObjByTag.bFishDead || ((bool)base.transform.parent.Find("Fish").gameObject && !base.transform.parent.Find("Fish").gameObject.activeSelf))
		{
			return;
		}
		DK_Lock currentGun = DK_GameInfo.getInstance().GameScene.GetCurrentGun(DK_GameInfo.getInstance().User.SeatIndex);
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].SeatIndex != DK_GameInfo.getInstance().User.SeatIndex)
			{
				continue;
			}
			if (!DK_GameInfo.getInstance().UserList[i].Lock)
			{
				sptISwimObj.bLocked = true;
				DK_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.StartLocking(DK_GameInfo.getInstance().UserList[i].SeatIndex, self: true);
				bool flag = DK_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
				continue;
			}
			DK_GameInfo.getInstance().UserList[i].Lock = false;
			Vector3 zero = Vector3.zero;
			if ((bool)DK_GameInfo.getInstance().UserList[i].LockFish)
			{
				DK_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
				zero = DK_GameInfo.getInstance().UserList[i].LockFish.transform.Find("LockFlag").position;
				DK_FishPoolMngr.GetSingleton().GetSwimObjByTag(DK_GameInfo.getInstance().UserList[i].LockFish).HideLockedFlag();
				DK_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.ChangeLockFish(zero);
				bool flag2 = DK_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
			}
		}
	}

	public void Release()
	{
		DK_GameInfo.getInstance().GameScene.ReleaseFish();
	}
}
