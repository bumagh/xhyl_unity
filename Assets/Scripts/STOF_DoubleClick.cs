using GameCommon;
using UnityEngine;

public class STOF_DoubleClick : MonoBehaviour
{
	private int clickCount;

	private STOF_ISwimObj sptISwimObj;

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
				for (int i = 0; i < STOF_GameInfo.getInstance().UserList.Count; i++)
				{
					if (STOF_GameInfo.getInstance().UserList[i].SeatIndex == STOF_GameInfo.getInstance().User.SeatIndex && base.gameObject != STOF_GameInfo.getInstance().UserList[i].LockFish)
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
		if (STOF_GameInfo.getInstance() != null)
		{
			STOF_GameInfo.getInstance().GameScene.ClickFish();
		}
		if (All_TipCanvas.GetInstance().IsPayPanelActive() || All_TipCanvas.GetInstance().IsCheckPwdActive())
		{
			return;
		}
		clickCount++;
		sptISwimObj = STOF_FishPoolMngr.GetSingleton().GetSwimObjByTag(base.transform.parent.gameObject);
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
		STOF_ISwimObj swimObjByTag = STOF_FishPoolMngr.GetSingleton().GetSwimObjByTag(gameObject);
		if (!swimObjByTag || swimObjByTag.mFishType < STOF_FISH_TYPE.Fish_Turtle || STOF_GameInfo.getInstance().User.ScoreCount <= 0 || !gameObject.activeSelf || !STOF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr || swimObjByTag.bFishDead || ((bool)base.transform.parent.Find("Fish").gameObject && !base.transform.parent.Find("Fish").gameObject.activeSelf))
		{
			return;
		}
		STOF_Lock currentGun = STOF_GameInfo.getInstance().GameScene.GetCurrentGun(STOF_GameInfo.getInstance().User.SeatIndex);
		for (int i = 0; i < STOF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STOF_GameInfo.getInstance().UserList[i].SeatIndex != STOF_GameInfo.getInstance().User.SeatIndex)
			{
				continue;
			}
			if (!STOF_GameInfo.getInstance().UserList[i].Lock)
			{
				sptISwimObj.bLocked = true;
				STOF_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.StartLocking(STOF_GameInfo.getInstance().UserList[i].SeatIndex, self: true);
				bool flag = STOF_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
				continue;
			}
			STOF_GameInfo.getInstance().UserList[i].Lock = false;
			Vector3 zero = Vector3.zero;
			if ((bool)STOF_GameInfo.getInstance().UserList[i].LockFish)
			{
				STOF_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
				zero = STOF_GameInfo.getInstance().UserList[i].LockFish.transform.Find("LockFlag").position;
				STOF_FishPoolMngr.GetSingleton().GetSwimObjByTag(STOF_GameInfo.getInstance().UserList[i].LockFish).HideLockedFlag();
				STOF_GameInfo.getInstance().UserList[i].LockFish = gameObject;
				currentGun.ChangeLockFish(zero);
				bool flag2 = STOF_NetMngr.GetSingleton().MyCreateSocket.SendLockFish(swimObjByTag.mServerID);
			}
		}
	}

	public void Release()
	{
		STOF_GameInfo.getInstance().GameScene.ReleaseFish();
	}
}
