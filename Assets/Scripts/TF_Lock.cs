using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class TF_Lock : MonoBehaviour
{
	public TF_ELOCK lockProcess;

	[SerializeField]
	private Image imgLine;

	private float distance;

	private int seatId;

	private float lineLength;

	private float per;

	[SerializeField]
	private Transform tfGun;

	private GameObject objLockFish;

	private TF_ISwimObj sptSwimObj;

	public TF_LockingFlag sptLockFlag;

	public Image imgLockFlag;

	private Vector3 vecLockFlag;

	private Vector3 vecGun;

	private bool bStartLock;

	public TF_LockCard sptLockCard;

	private bool bUp;

	private Color[] colLock = new Color[4]
	{
		new Color(1f, 146f / 255f, 0f, 1f),
		new Color(0.8235294f, 0f, 1f, 1f),
		new Color(0f, 32f / 255f, 248f / 255f, 1f),
		new Color(0f, 1f, 7f / 85f, 1f)
	};

	[SerializeField]
	private Sprite[] spiLockFlag;

	private void Start()
	{
		imgLine.fillAmount = 0f;
		per = Mathf.Sqrt(2156800f) / Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
	}

	public void StartLocking(int seatid, bool self = false)
	{
		base.gameObject.SetActive(value: true);
		imgLine.color = colLock[seatid - 1];
		imgLockFlag.color = colLock[seatid - 1];
		bUp = TF_GameInfo.getInstance().GameScene.GetIsUp(base.transform.parent.parent.parent.gameObject);
		vecGun = Camera.main.WorldToScreenPoint(tfGun.position);
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].SeatIndex == seatid)
			{
				objLockFish = TF_GameInfo.getInstance().UserList[i].LockFish;
			}
			if (TF_GameInfo.getInstance().User.SeatIndex == seatid)
			{
				self = true;
			}
		}
		seatId = seatid;
		sptSwimObj = TF_FishPoolMngr.GetSingleton().GetSwimObjByTag(objLockFish);
		imgLockFlag.sprite = spiLockFlag[seatId - 1];
		if (self)
		{
			imgLine.fillAmount = 0f;
			distance = 0f;
			lockProcess = TF_ELOCK.Locking;
			CaculateDirection();
			vecLockFlag = Camera.main.WorldToScreenPoint(sptSwimObj.GetLockFishPos());
		}
		else
		{
			lockProcess = TF_ELOCK.WillLocked;
			CaculateDirection();
			imgLine.fillAmount = lineLength;
		}
		bStartLock = true;
	}

	public void EndLock()
	{
		if (bStartLock)
		{
			lockProcess = TF_ELOCK.EndMoving;
			sptSwimObj.HideLockedFlag();
			vecLockFlag = Camera.main.WorldToScreenPoint(sptSwimObj.GetLockFishPos());
			InitLockingFlagObj();
		}
	}

	private void InitLockingFlagObj()
	{
		imgLockFlag.gameObject.SetActive(value: true);
	}

	private void Update()
	{
		if (bStartLock && TF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
		{
			if (lockProcess == TF_ELOCK.Locking)
			{
				distance += 0.5f * (Time.deltaTime / 0.3f);
				if (distance <= 0f)
				{
					distance = 0f;
				}
				else if (distance >= lineLength)
				{
					distance = lineLength;
					lockProcess = TF_ELOCK.WillLocked;
				}
			}
			else if (lockProcess == TF_ELOCK.WillLocked)
			{
				distance = lineLength;
				sptSwimObj.InitLock(seatId);
				sptLockCard.ShowLockCard(seatId, bUp);
				imgLockFlag.gameObject.SetActive(value: false);
				if (TF_GameInfo.getInstance().User.SeatIndex == seatId)
				{
					TF_GameInfo.getInstance().GameScene.LockFishAutoFire();
				}
				for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
				{
					if (TF_GameInfo.getInstance().UserList[i].SeatIndex == seatId)
					{
						TF_GameInfo.getInstance().UserList[i].Lock = true;
					}
				}
				lockProcess = TF_ELOCK.Locked;
			}
			else if (lockProcess == TF_ELOCK.Locked)
			{
				distance = lineLength;
			}
			else if (lockProcess == TF_ELOCK.MoveToOtherFish)
			{
				distance = lineLength;
			}
			else if (lockProcess == TF_ELOCK.LookForALockFish)
			{
				distance -= 0.5f * (Time.deltaTime / 0.3f);
				if (distance <= 0f)
				{
					distance = 0f;
					sptLockFlag.HideLockingFlag();
				}
				int num = TF_FishPoolMngr.GetSingleton().LookForACanBeLockFish(seatId);
				if (num != -1)
				{
					StartLocking(seatId, self: true);
				}
			}
			imgLine.fillAmount = distance;
			if (lockProcess != TF_ELOCK.LookForALockFish)
			{
				CaculateDirection();
			}
		}
		if (lockProcess == TF_ELOCK.EndMoving)
		{
			distance -= 0.5f * (Time.deltaTime / 0.3f);
			if (distance <= 0f)
			{
				distance = 0f;
			}
			if (distance == 0f)
			{
				lockProcess = TF_ELOCK.LockEnd;
			}
			imgLine.fillAmount = distance;
		}
		else if (lockProcess == TF_ELOCK.LockEnd)
		{
			lockProcess = TF_ELOCK.None;
			if (sptLockCard != null)
			{
				sptLockCard.HideLockCard();
			}
			imgLine.fillAmount = 0f;
			sptLockFlag.HideLockingFlag();
			for (int j = 0; j < TF_GameInfo.getInstance().UserList.Count; j++)
			{
				if (TF_GameInfo.getInstance().UserList[j].SeatIndex == seatId)
				{
					TF_GameInfo.getInstance().UserList[j].Lock = false;
					TF_GameInfo.getInstance().UserList[j].LockFish = null;
				}
			}
			sptSwimObj.HideLockedFlag();
			base.gameObject.SetActive(value: false);
			bStartLock = false;
		}
		if (TF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr)
		{
			return;
		}
		if (seatId == TF_GameInfo.getInstance().User.SeatIndex)
		{
			TF_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
			if (!TF_GameInfo.getInstance().GameScene.bAuto)
			{
				if (lockProcess < TF_ELOCK.EndMoving)
				{
					EndLock();
				}
			}
			else if (lockProcess == TF_ELOCK.LookForALockFish)
			{
				distance -= 0.5f * (Time.deltaTime / 0.3f);
				if (distance <= 0f)
				{
					distance = 0f;
					sptLockFlag.HideLockingFlag();
				}
				imgLine.fillAmount = distance;
			}
			else
			{
				StartLockForLockFish();
			}
		}
		else if (lockProcess < TF_ELOCK.EndMoving)
		{
			EndLock();
		}
	}

	private bool IsLockFishNull()
	{
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].SeatIndex == seatId && TF_GameInfo.getInstance().UserList[i].LockFish == null && TF_GameInfo.getInstance().UserList[i].Lock)
			{
				return true;
			}
		}
		return false;
	}

	public void ChangeLockFish(Vector3 initpos)
	{
		lockProcess = TF_ELOCK.MoveToOtherFish;
		if (TF_GameInfo.getInstance().User.SeatIndex == seatId)
		{
			TF_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
		}
		sptSwimObj.HideLockedFlag();
		sptSwimObj.bLocked = false;
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].SeatIndex == seatId)
			{
				objLockFish = TF_GameInfo.getInstance().UserList[i].LockFish;
			}
		}
		if ((bool)objLockFish)
		{
			sptSwimObj = TF_FishPoolMngr.GetSingleton().GetSwimObjByTag(objLockFish);
			sptSwimObj.bLocked = true;
			vecLockFlag = Camera.main.WorldToScreenPoint(sptSwimObj.GetLockFishPos());
		}
	}

	public void StartLockForLockFish()
	{
		sptSwimObj.HideLockedFlag();
		sptLockCard.HideLockCard();
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].SeatIndex == seatId)
			{
				vecLockFlag = Camera.main.WorldToScreenPoint(sptSwimObj.GetLockFishPos());
				TF_GameInfo.getInstance().UserList[i].LockFish = null;
			}
		}
		lockProcess = TF_ELOCK.LookForALockFish;
	}

	private void CaculateDirection()
	{
		Vector3 initpos = Vector3.zero;
		initpos = ((lockProcess != TF_ELOCK.MoveToOtherFish) ? Camera.main.WorldToScreenPoint(sptSwimObj.GetLockFishPos()) : vecLockFlag);
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
		if (lockProcess == TF_ELOCK.Locking)
		{
			initpos.z = 0f;
		}
		else if (lockProcess == TF_ELOCK.Locked)
		{
			if (!sptSwimObj.IsLockFishOutsideWindow() && !IsLockFishNull() && !sptSwimObj.bFishDead && sptSwimObj.gameObject.activeSelf)
			{
				return;
			}
			if (seatId == TF_GameInfo.getInstance().User.SeatIndex)
			{
				if (TF_GameInfo.getInstance().GameScene.bAuto)
				{
					sptSwimObj.HideLockedFlag();
					sptLockCard.HideLockCard();
					int num3 = TF_FishPoolMngr.GetSingleton().LookForACanBeLockFish(seatId);
					if (num3 != -1)
					{
						if (seatId == TF_GameInfo.getInstance().User.SeatIndex)
						{
							for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
							{
								if (TF_GameInfo.getInstance().UserList[i].SeatIndex == TF_GameInfo.getInstance().User.SeatIndex)
								{
									TF_GameInfo.getInstance().UserList[i].Lock = false;
								}
							}
						}
						ChangeLockFish(initpos);
					}
					else
					{
						StartLockForLockFish();
						TF_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
					}
				}
				else
				{
					EndLock();
					TF_GameInfo.getInstance().GameScene.CancelLockFishAutoFire();
					TF_NetMngr.GetSingleton().MyCreateSocket.SendUnLockFish();
				}
			}
			else
			{
				EndLock();
			}
		}
		else
		{
			if (lockProcess != TF_ELOCK.MoveToOtherFish)
			{
				return;
			}
			TF_BulletPoolMngr.GetSingleton().LockingBulletToNormal();
			float num4 = Mathf.Sqrt((vecLockFlag.x - initpos.x) * (vecLockFlag.x - initpos.x) + (vecLockFlag.y - initpos.y) * (vecLockFlag.y - initpos.y));
			if (num4 < 1E-06f)
			{
				lockProcess = TF_ELOCK.Locked;
				if (TF_GameInfo.getInstance().User.SeatIndex == seatId)
				{
					for (int j = 0; j < TF_GameInfo.getInstance().UserList.Count; j++)
					{
						if (TF_GameInfo.getInstance().UserList[j].SeatIndex == seatId)
						{
							TF_GameInfo.getInstance().UserList[j].Lock = true;
						}
					}
					TF_GameInfo.getInstance().GameScene.LockFishAutoFire();
				}
				imgLockFlag.gameObject.SetActive(value: false);
				vecLockFlag = initpos;
				sptSwimObj.InitLock(seatId);
				sptLockCard.ShowLockCard(seatId, bUp);
			}
			initpos.z = 0f;
		}
	}
}
