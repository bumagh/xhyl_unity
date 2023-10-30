using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class DK_Lock : MonoBehaviour
{
	public DK_ELOCK lockProcess;

	[SerializeField]
	private Image imgLine;

	private float distance;

	private int seatId;

	private float lineLength;

	private float per;

	private Transform tfGun;

	private GameObject objLockFish;

	private DK_ISwimObj sptSwimObj;

	private Vector3 vecLockFlag;

	private Vector3 vecGun;

	private bool bStartLock;

	public DK_LockCard sptLockCard;

	private bool bUp;

	private Color[] colLock = new Color[4]
	{
		new Color(1f, 146f / 255f, 0f, 0.8f),
		new Color(0.8235294f, 0f, 1f, 0.8f),
		new Color(0f, 32f / 255f, 248f / 255f, 0.8f),
		new Color(0f, 1f, 7f / 85f, 0.8f)
	};

	private void Start()
	{
		imgLine.fillAmount = 0f;
		per = Mathf.Sqrt(2156800f) / Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
	}

	private void SetImgLinesSpr()
	{
	}

	private bool IsOneself()
	{
		return seatId == DK_GameInfo.getInstance().User.SeatIndex;
	}

	public void StartLocking(int seatid, bool self = false)
	{
		base.gameObject.SetActive(value: true);
		if (tfGun == null)
		{
			tfGun = base.transform.parent;
		}
		imgLine.color = colLock[seatid - 1];
		bUp = DK_GameInfo.getInstance().GameScene.GetIsUp(base.transform.parent.parent.parent.gameObject);
		vecGun = Camera.main.WorldToScreenPoint(tfGun.position);
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].SeatIndex == seatid)
			{
				objLockFish = DK_GameInfo.getInstance().UserList[i].LockFish;
			}
			if (DK_GameInfo.getInstance().User.SeatIndex == seatid)
			{
				self = true;
			}
		}
		seatId = seatid;
		SetImgLinesSpr();
		sptSwimObj = DK_FishPoolMngr.GetSingleton().GetSwimObjByTag(objLockFish);
		if (self)
		{
			imgLine.fillAmount = 0f;
			distance = 0f;
			lockProcess = DK_ELOCK.Locking;
			CaculateDirection();
			vecLockFlag = Camera.main.WorldToScreenPoint(sptSwimObj.GetLockFishPos());
		}
		else
		{
			lockProcess = DK_ELOCK.WillLocked;
			CaculateDirection();
			imgLine.fillAmount = lineLength;
		}
		bStartLock = true;
	}

	public void EndLock()
	{
		if (bStartLock)
		{
			lockProcess = DK_ELOCK.EndMoving;
			sptSwimObj.HideLockedFlag();
			vecLockFlag = Camera.main.WorldToScreenPoint(sptSwimObj.GetLockFishPos());
		}
	}

	private void Update()
	{
		if (bStartLock && DK_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
		{
			if (lockProcess == DK_ELOCK.Locking)
			{
				distance += 0.5f * (Time.deltaTime / 0.3f);
				if (distance <= 0f)
				{
					distance = 0f;
				}
				else if (distance >= lineLength)
				{
					distance = lineLength;
					lockProcess = DK_ELOCK.WillLocked;
				}
			}
			else if (lockProcess == DK_ELOCK.WillLocked)
			{
				distance = lineLength;
				sptSwimObj.InitLock(seatId);
				sptLockCard.ShowLockCard(seatId, bUp);
				if (DK_GameInfo.getInstance().User.SeatIndex == seatId)
				{
					DK_GameInfo.getInstance().GameScene.LockFishAutoFire();
				}
				for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
				{
					if (DK_GameInfo.getInstance().UserList[i].SeatIndex == seatId)
					{
						DK_GameInfo.getInstance().UserList[i].Lock = true;
					}
				}
				lockProcess = DK_ELOCK.Locked;
			}
			else if (lockProcess == DK_ELOCK.Locked)
			{
				distance = lineLength;
			}
			else if (lockProcess == DK_ELOCK.MoveToOtherFish)
			{
				distance = lineLength;
			}
			else if (lockProcess == DK_ELOCK.LookForALockFish)
			{
				distance -= 0.5f * (Time.deltaTime / 0.3f);
				if (distance <= 0f)
				{
					distance = 0f;
				}
				int num = DK_FishPoolMngr.GetSingleton().LookForACanBeLockFish(seatId);
				if (num != -1)
				{
					StartLocking(seatId, self: true);
				}
			}
			imgLine.fillAmount = distance;
			if (lockProcess != DK_ELOCK.LookForALockFish)
			{
				CaculateDirection();
			}
		}
		if (lockProcess == DK_ELOCK.EndMoving)
		{
			distance -= 0.5f * (Time.deltaTime / 0.3f);
			if (distance <= 0f)
			{
				distance = 0f;
			}
			if (distance == 0f)
			{
				lockProcess = DK_ELOCK.LockEnd;
			}
			imgLine.fillAmount = distance;
		}
		else if (lockProcess == DK_ELOCK.LockEnd)
		{
			lockProcess = DK_ELOCK.None;
			if (sptLockCard != null)
			{
				sptLockCard.HideLockCard();
			}
			imgLine.fillAmount = 0f;
			for (int j = 0; j < DK_GameInfo.getInstance().UserList.Count; j++)
			{
				if (DK_GameInfo.getInstance().UserList[j].SeatIndex == seatId)
				{
					DK_GameInfo.getInstance().UserList[j].Lock = false;
					DK_GameInfo.getInstance().UserList[j].LockFish = null;
				}
			}
			sptSwimObj.HideLockedFlag();
			base.gameObject.SetActive(value: false);
			bStartLock = false;
		}
		if (DK_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
		{
			return;
		}
		if (seatId == DK_GameInfo.getInstance().User.SeatIndex)
		{
			DK_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
			if (!DK_GameInfo.getInstance().GameScene.bAuto)
			{
				if (lockProcess < DK_ELOCK.EndMoving)
				{
					EndLock();
				}
			}
			else if (lockProcess == DK_ELOCK.LookForALockFish)
			{
				distance -= 0.5f * (Time.deltaTime / 0.3f);
				if (distance <= 0f)
				{
					distance = 0f;
				}
				imgLine.fillAmount = distance;
			}
			else
			{
				StartLockForLockFish();
			}
		}
		else if (lockProcess < DK_ELOCK.EndMoving)
		{
			EndLock();
		}
	}

	private bool IsLockFishNull()
	{
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].SeatIndex == seatId && DK_GameInfo.getInstance().UserList[i].LockFish == null && DK_GameInfo.getInstance().UserList[i].Lock)
			{
				return true;
			}
		}
		return false;
	}

	public void ChangeLockFish(Vector3 initpos)
	{
		lockProcess = DK_ELOCK.MoveToOtherFish;
		if (DK_GameInfo.getInstance().User.SeatIndex == seatId)
		{
			DK_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
		}
		sptSwimObj.HideLockedFlag();
		sptSwimObj.bLocked = false;
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].SeatIndex == seatId)
			{
				objLockFish = DK_GameInfo.getInstance().UserList[i].LockFish;
			}
		}
		if ((bool)objLockFish)
		{
			sptSwimObj = DK_FishPoolMngr.GetSingleton().GetSwimObjByTag(objLockFish);
			sptSwimObj.bLocked = true;
			vecLockFlag = Camera.main.WorldToScreenPoint(sptSwimObj.GetLockFishPos());
		}
	}

	public void StartLockForLockFish()
	{
		sptSwimObj.HideLockedFlag();
		sptLockCard.HideLockCard();
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].SeatIndex == seatId)
			{
				vecLockFlag = Camera.main.WorldToScreenPoint(sptSwimObj.GetLockFishPos());
				DK_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
		lockProcess = DK_ELOCK.LookForALockFish;
	}

	private void CaculateDirection()
	{
		Vector3 initpos = Vector3.zero;
		initpos = ((lockProcess != DK_ELOCK.MoveToOtherFish) ? Camera.main.WorldToScreenPoint(sptSwimObj.GetLockFishPos()) : vecLockFlag);
		float f = (initpos.x - vecGun.x) / (initpos.y - vecGun.y);
		float num = Mathf.Atan(f);
		float num2 = num * 180f / 3.14159f;
		if (initpos.y < vecGun.y)
		{
			num2 = ((!(initpos.x < vecGun.x)) ? (num2 + 180f) : (num2 - 180f));
		}
		if (!bUp)
		{
			tfGun.localEulerAngles = Vector3.forward * (0f - num2);
		}
		else
		{
			tfGun.localEulerAngles = Vector3.forward * (0f - num2 + 180f);
		}
		lineLength = Mathf.Sqrt((initpos.x - vecGun.x) * (initpos.x - vecGun.x) + (initpos.y - vecGun.y) * (initpos.y - vecGun.y)) * per;
		lineLength /= 1200f;
		if (lockProcess == DK_ELOCK.Locking)
		{
			initpos.z = 0f;
		}
		else if (lockProcess == DK_ELOCK.Locked)
		{
			if (!sptSwimObj.IsLockFishOutsideWindow() && !IsLockFishNull() && !sptSwimObj.bFishDead && sptSwimObj.gameObject.activeSelf)
			{
				return;
			}
			if (seatId == DK_GameInfo.getInstance().User.SeatIndex)
			{
				if (DK_GameInfo.getInstance().GameScene.bAuto)
				{
					sptSwimObj.HideLockedFlag();
					sptLockCard.HideLockCard();
					int num3 = DK_FishPoolMngr.GetSingleton().LookForACanBeLockFish(seatId);
					if (num3 != -1)
					{
						if (seatId == DK_GameInfo.getInstance().User.SeatIndex)
						{
							for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
							{
								if (DK_GameInfo.getInstance().UserList[i].SeatIndex == DK_GameInfo.getInstance().User.SeatIndex)
								{
									DK_GameInfo.getInstance().UserList[i].Lock = false;
								}
							}
						}
						ChangeLockFish(initpos);
					}
					else
					{
						StartLockForLockFish();
						DK_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
					}
				}
				else
				{
					EndLock();
					DK_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
					DK_NetMngr.GetSingleton().MyCreateSocket.SendUnLockFish();
				}
			}
			else
			{
				EndLock();
			}
		}
		else
		{
			if (lockProcess != DK_ELOCK.MoveToOtherFish)
			{
				return;
			}
			DK_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
			float num4 = Mathf.Sqrt((vecLockFlag.x - initpos.x) * (vecLockFlag.x - initpos.x) + (vecLockFlag.y - initpos.y) * (vecLockFlag.y - initpos.y));
			if (num4 < 1E-06f)
			{
				lockProcess = DK_ELOCK.Locked;
				if (DK_GameInfo.getInstance().User.SeatIndex == seatId)
				{
					for (int j = 0; j < DK_GameInfo.getInstance().UserList.Count; j++)
					{
						if (DK_GameInfo.getInstance().UserList[j].SeatIndex == seatId)
						{
							DK_GameInfo.getInstance().UserList[j].Lock = true;
						}
					}
					DK_GameInfo.getInstance().GameScene.LockFishAutoFire();
				}
				vecLockFlag = initpos;
				sptSwimObj.InitLock(seatId);
				sptLockCard.ShowLockCard(seatId, bUp);
			}
			initpos.z = 0f;
		}
	}
}
