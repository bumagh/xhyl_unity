using GameCommon;
using System.Collections.Generic;
using UnityEngine;

public class TF_BulletPoolMngr : MonoBehaviour
{
	public static TF_BulletPoolMngr G_BulletPoolMngr;

	public Transform mBullet;

	public bool bTest;

	private int _bulletID;

	private Vector3[] _barBettePos = new Vector3[4];

	private float _fBarBetteR = 0.6f;

	private float _fBulletSpeed = 6f;

	private int _nPlayerIndex = 1;

	private float _fLiziTime;

	private int _barBettePower = 1;

	public List<GameObject> mCurrentPlayerBullet = new List<GameObject>();

	public List<GameObject> mOtherPlayerBullet = new List<GameObject>();

	private bool _isFireEnableBySvr = true;

	private bool bGameMaintained;

	private int _testIndex;

	public bool mIsFireEnableBySvr
	{
		get
		{
			return _isFireEnableBySvr;
		}
		set
		{
			_isFireEnableBySvr = value;
		}
	}

	public bool BGameMaintained
	{
		get
		{
			return bGameMaintained;
		}
		set
		{
			bGameMaintained = value;
		}
	}

	public static TF_BulletPoolMngr GetSingleton()
	{
		return G_BulletPoolMngr;
	}

	private void Awake()
	{
		if (G_BulletPoolMngr == null)
		{
			G_BulletPoolMngr = this;
		}
	}

	private void Start()
	{
		_barBettePos[0] = Vector3.left * 3.8f + Vector3.down * 3f + Vector3.forward;
		_barBettePos[1] = Vector3.right * 3.2f + Vector3.down * 3f + Vector3.forward;
		_barBettePos[2] = Vector3.right * 3.8f + Vector3.up * 3f + Vector3.forward;
		_barBettePos[3] = Vector3.left * 3.2f + Vector3.up * 3f + Vector3.forward;
		SetPlayerIndex(1);
	}

	public bool IsFireEnable()
	{
		bTest = (mIsFireEnableBySvr && mCurrentPlayerBullet.Count < 12);
		return mIsFireEnableBySvr && mCurrentPlayerBullet.Count < 12;
	}

	public bool IsOtherPlayerFireEnable()
	{
		return mOtherPlayerBullet.Count < 36;
	}

	public void SetPlayerIndex(int nIndex)
	{
		if (nIndex <= 4 && nIndex >= 1)
		{
			_nPlayerIndex = nIndex;
			Camera.main.transform.rotation = Quaternion.identity;
			if (nIndex == 3 || nIndex == 4)
			{
				Camera.main.transform.Rotate(Vector3.forward, 180f);
			}
		}
	}

	public void SetBarbettePower(int nPower)
	{
		_barBettePower = nPower;
	}

	public void SetLizi(float time = 20f)
	{
		_fLiziTime += time;
	}

	public void LanchBullet(int nPlayer, float fAgl, int nPower)
	{
		if (IsFireEnable())
		{
			Transform transform = LanchBullet(nPlayer, fAgl, nPower, _fLiziTime, isTellSvr: true);
			mCurrentPlayerBullet.Add(transform.gameObject);
		}
	}

	public void LanchBullet(int nPlayer, float fAgl, int nPower, bool isLizi)
	{
		if (IsOtherPlayerFireEnable())
		{
			if (isLizi)
			{
				Transform transform = LanchBullet(nPlayer, fAgl, nPower, 1f, isTellSvr: false);
				mOtherPlayerBullet.Add(transform.gameObject);
			}
			else
			{
				Transform transform2 = LanchBullet(nPlayer, fAgl, nPower, 0f, isTellSvr: false);
				mOtherPlayerBullet.Add(transform2.gameObject);
			}
		}
	}

	public Transform LanchBullet(int nPlayer, float fAgl, int nPower, float fLiziTime, bool isTellSvr)
	{
		Transform transform = TF_PoolManager.Pools["TFBulletPool"].Spawn(mBullet);
		TF_Bullet component = transform.GetComponent<TF_Bullet>();
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].SeatIndex == nPlayer)
			{
				TF_Lock currentGun = TF_GameInfo.getInstance().GameScene.GetCurrentGun(TF_GameInfo.getInstance().UserList[i].SeatIndex);
				if (currentGun.lockProcess != TF_ELOCK.MoveToOtherFish && TF_GameInfo.getInstance().UserList[i].LockFish != null && TF_GameInfo.getInstance().UserList[i].Lock)
				{
					component.IsLocking = true;
				}
				else
				{
					component.IsLocking = false;
				}
			}
		}
		component.mPlayerID = nPlayer;
		component.mBulletID = _bulletID;
		component.mPower = nPower;
		if (nPower <= 50 && nPower >= 1)
		{
			if (fLiziTime > 0f)
			{
				component.mIsLizi = true;
				component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_NORMAL_LIZI);
				TF_MusicMngr.GetSingleton().PlayGameSound(TF_MusicMngr.GAME_SOUND.SOUND_LIZI_SHOOT);
			}
			else
			{
				component.mIsLizi = false;
				TF_MusicMngr.GetSingleton().PlayGameSound(TF_MusicMngr.GAME_SOUND.SOUND_NORMAL_SHOOT);
				switch (nPlayer)
				{
				case 1:
					component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_NORMAL1);
					break;
				case 2:
					component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_NORMAL2);
					break;
				case 3:
					component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_NORMAL3);
					break;
				case 4:
					component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_NORMAL4);
					break;
				}
			}
		}
		else if (nPower < 1000 && nPower >= 51)
		{
			if (fLiziTime > 0f)
			{
				component.mIsLizi = true;
				component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_SUPER_LIZI);
				TF_MusicMngr.GetSingleton().PlayGameSound(TF_MusicMngr.GAME_SOUND.SOUND_LIZI_SHOOT);
			}
			else
			{
				component.mIsLizi = false;
				TF_MusicMngr.GetSingleton().PlayGameSound(TF_MusicMngr.GAME_SOUND.SOUND_NORMAL_SHOOT);
				switch (nPlayer)
				{
				case 1:
					component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_SUPER1);
					break;
				case 2:
					component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_SUPER2);
					break;
				case 3:
					component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_SUPER3);
					break;
				case 4:
					component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_SUPER4);
					break;
				}
			}
		}
		else if (fLiziTime > 0f)
		{
			component.mIsLizi = true;
			component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_SUPER_LIZI4);
			TF_MusicMngr.GetSingleton().PlayGameSound(TF_MusicMngr.GAME_SOUND.SOUND_LIZI_SHOOT);
		}
		else
		{
			component.mIsLizi = false;
			TF_MusicMngr.GetSingleton().PlayGameSound(TF_MusicMngr.GAME_SOUND.SOUND_NORMAL_SHOOT);
			switch (nPlayer)
			{
			case 1:
				component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_SUPER4_1);
				break;
			case 2:
				component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_SUPER4_2);
				break;
			case 3:
				component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_SUPER4_3);
				break;
			case 4:
				component.SetBulletType(TF_BULLET_TYPE.BULLET_TYP_SUPER4_4);
				break;
			}
		}
		Vector3 point = Vector3.up;
		if (nPlayer == 3 || nPlayer == 4)
		{
			point = Vector3.down;
		}
		component.SetRotation(fAgl);
		if (!isTellSvr && (nPlayer - 1) / 2 != (TF_GameMngr.GetSingleton().mPlayerSeatID - 1) / 2)
		{
			component.SetRotation(fAgl + 180f);
		}
		Quaternion rotation = default(Quaternion);
		rotation.eulerAngles = Vector3.forward * fAgl;
		point = rotation * point;
		transform.position = _barBettePos[nPlayer - 1] + _fBarBetteR * point.normalized;
		if (_fLiziTime > 0f)
		{
			component.mSpeed = point.normalized * _fBulletSpeed * 1.5f;
		}
		else
		{
			component.mSpeed = point.normalized * _fBulletSpeed;
		}
		_bulletID++;
		if (isTellSvr)
		{
			TF_NetMngr.GetSingleton().MyCreateSocket.SendFired(component.mBulletID, _nPlayerIndex, fAgl, nPower, component.mIsLizi);
		}
		return transform;
	}

	private void Update()
	{
		if (_fLiziTime > 0f)
		{
			if (mIsFireEnableBySvr)
			{
				_fLiziTime -= Time.deltaTime;
			}
		}
		else
		{
			_fLiziTime = 0f;
		}
		if (TF_GameParameter.G_bTest && Input.GetMouseButtonDown(0))
		{
			Vector3 a = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			Vector3 toDirection = a - _barBettePos[_nPlayerIndex - 1];
			toDirection.z = 0f;
			Quaternion quaternion = default(Quaternion);
			int nPlayerIndex = _nPlayerIndex;
			quaternion.SetFromToRotation(Vector3.up, toDirection);
			Vector3 eulerAngles = quaternion.eulerAngles;
			float z = eulerAngles.z;
			_barBettePower = 51;
			_fLiziTime = 0f;
			LanchBullet(_nPlayerIndex, z, _barBettePower, _fLiziTime, isTellSvr: false);
			int nPlayerIndex2 = _nPlayerIndex;
			LanchBullet(1, z, _barBettePower);
			LanchBullet(2, z, _barBettePower, isLizi: false);
			LanchBullet(3, z, _barBettePower, isLizi: false);
			LanchBullet(4, z, _barBettePower, isLizi: false);
		}
	}

	public void DestroyBullet(GameObject bullet)
	{
		mCurrentPlayerBullet.Remove(bullet);
		mOtherPlayerBullet.Remove(bullet);
		TF_PoolManager.Pools["TFBulletPool"].Despawn(bullet.transform);
	}

	public void RemoveAllBullet()
	{
		_bulletID = 0;
		UnityEngine.Debug.Log("RemoveAllBullet");
		TF_PoolManager.Pools["TFBulletPool"].DespawnAll();
	}

	public bool IsBulletLizi()
	{
		return _fLiziTime > 0f;
	}

	public void LockingBulletToNormal()
	{
		for (int i = 0; i < mCurrentPlayerBullet.Count; i++)
		{
			mCurrentPlayerBullet[i].GetComponent<TF_Bullet>().IsLocking = false;
		}
	}

	public void OtherLockingBulletToNormal(int player)
	{
		for (int i = 0; i < mOtherPlayerBullet.Count; i++)
		{
			TF_Bullet component = mOtherPlayerBullet[i].GetComponent<TF_Bullet>();
			if (component.mPlayerID == player)
			{
				component.IsLocking = false;
			}
		}
	}
}
