using GameCommon;
using System.Collections.Generic;
using UnityEngine;

public class DK_BulletPoolMngr : MonoBehaviour
{
	public static DK_BulletPoolMngr G_BulletPoolMngr;

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

	public DK_GameScene _gameScene;

	private bool _isFireEnableBySvr = true;

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

	public static DK_BulletPoolMngr GetSingleton()
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
		Transform transform = DK_PoolManager.Pools["DKBulletPool"].Spawn(mBullet);
		DK_Bullet component = transform.GetComponent<DK_Bullet>();
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].SeatIndex == nPlayer)
			{
				DK_Lock currentGun = DK_GameInfo.getInstance().GameScene.GetCurrentGun(DK_GameInfo.getInstance().UserList[i].SeatIndex);
				if (currentGun.lockProcess != DK_ELOCK.MoveToOtherFish && DK_GameInfo.getInstance().UserList[i].LockFish != null && DK_GameInfo.getInstance().UserList[i].Lock)
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
				component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_NORMAL_LIZI);
				DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_LIZI_SHOOT);
			}
			else
			{
				component.mIsLizi = false;
				DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_NORMAL_SHOOT);
				switch (nPlayer)
				{
				case 1:
					component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_NORMAL1);
					break;
				case 2:
					component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_NORMAL2);
					break;
				case 3:
					component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_NORMAL3);
					break;
				case 4:
					component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_NORMAL4);
					break;
				}
			}
		}
		else if (nPower < 1000 && nPower >= 51)
		{
			if (fLiziTime > 0f)
			{
				component.mIsLizi = true;
				component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_SUPER_LIZI);
				DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_LIZI_SHOOT);
			}
			else
			{
				component.mIsLizi = false;
				DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_NORMAL_SHOOT);
				switch (nPlayer)
				{
				case 1:
					component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_SUPER1);
					break;
				case 2:
					component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_SUPER2);
					break;
				case 3:
					component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_SUPER3);
					break;
				case 4:
					component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_SUPER4);
					break;
				}
			}
		}
		else if (fLiziTime > 0f)
		{
			component.mIsLizi = true;
			component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_SUPER_LIZI4);
			DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_LIZI_SHOOT);
		}
		else
		{
			component.mIsLizi = false;
			DK_MusicMngr.GetSingleton().PlayGameSound(DK_MusicMngr.GAME_SOUND.SOUND_NORMAL_SHOOT);
			switch (nPlayer)
			{
			case 1:
				component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_SUPER4_1);
				break;
			case 2:
				component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_SUPER4_2);
				break;
			case 3:
				component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_SUPER4_3);
				break;
			case 4:
				component.SetBulletType(DK_BULLET_TYPE.BULLET_TYP_SUPER4_4);
				break;
			}
		}
		Vector3 point = Vector3.up;
		if (nPlayer == 3 || nPlayer == 4)
		{
			point = Vector3.down;
			component.SetRotation(fAgl + 180f);
		}
		else
		{
			component.SetRotation(fAgl);
		}
		Quaternion rotation = default(Quaternion);
		rotation.eulerAngles = Vector3.forward * fAgl;
		point = rotation * point;
		for (int j = 0; j < _gameScene.areas.Length; j++)
		{
			_barBettePos[j] = _gameScene.areas[j].sptGC.tfGun.transform.position;
		}
		transform.position = _barBettePos[nPlayer - 1];
		if (_fLiziTime > 0f)
		{
			component.mSpeed = point.normalized * _fBulletSpeed * (ZH2_GVars.shellMultiple + 0.2f);
		}
		else
		{
			component.mSpeed = point.normalized * _fBulletSpeed * ZH2_GVars.shellMultiple;
		}
		_bulletID++;
		if (isTellSvr)
		{
			DK_NetMngr.GetSingleton().MyCreateSocket.SendFired(component.mBulletID, _nPlayerIndex, fAgl, nPower, component.mIsLizi);
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
		if (DK_GameParameter.G_bTest && Input.GetMouseButtonDown(0))
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
		DK_PoolManager.Pools["DKBulletPool"].Despawn(bullet.transform);
	}

	public void RemoveAllBullet()
	{
		_bulletID = 0;
		UnityEngine.Debug.Log("RemoveAllBullet");
		DK_PoolManager.Pools["DKBulletPool"].DespawnAll();
	}

	public bool IsBulletLizi()
	{
		return _fLiziTime > 0f;
	}

	public void LockingBulletToNormal()
	{
		for (int i = 0; i < mCurrentPlayerBullet.Count; i++)
		{
			mCurrentPlayerBullet[i].GetComponent<DK_Bullet>().IsLocking = false;
		}
	}

	public void OtherLockingBulletToNormal(int player)
	{
		for (int i = 0; i < mOtherPlayerBullet.Count; i++)
		{
			DK_Bullet component = mOtherPlayerBullet[i].GetComponent<DK_Bullet>();
			if (component.mPlayerID == player)
			{
				component.IsLocking = false;
			}
		}
	}
}
