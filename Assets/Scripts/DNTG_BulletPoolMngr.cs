using GameCommon;
using System.Collections.Generic;
using UnityEngine;

public class DNTG_BulletPoolMngr : MonoBehaviour
{
	public static DNTG_BulletPoolMngr G_BulletPoolMngr;

	public Transform mBullet;

	public bool bTest;

	private int _bulletID;

	private Vector3[] _barBettePos = new Vector3[4];

	private float _fBarBetteR = 0.6f;

	private float _fBulletSpeed = 6f;

	private int _nPlayerIndex = 1;

	private int _barBettePower = 1;

	public List<GameObject> mCurrentPlayerBullet = new List<GameObject>();

	public List<GameObject> mOtherPlayerBullet = new List<GameObject>();

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

	public static DNTG_BulletPoolMngr GetSingleton()
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
		UnityEngine.Debug.LogError("=====粒子炮=====");
	}

	public void LanchBullet(int nPlayer, float fAgl, int nPower, bool isSpeed)
	{
		if (IsFireEnable())
		{
			Transform transform = LanchBullet2(nPlayer, fAgl, nPower, isTellSvr: true, isSpeed, 0f);
			mCurrentPlayerBullet.Add(transform.gameObject);
		}
		else
		{
			UnityEngine.Debug.LogError("======不能开炮======");
		}
	}

	public void LanchBullet2(int nPlayer, float fAgl, int nPower, bool isLizi, bool isSpeed)
	{
		if (IsOtherPlayerFireEnable())
		{
			Transform transform = LanchBullet2(nPlayer, fAgl, nPower, isTellSvr: false, isSpeed, 0f);
			mOtherPlayerBullet.Add(transform.gameObject);
		}
	}

	public Transform LanchBullet2(int nPlayer, float fAgl, int nPower, bool isTellSvr, bool isSpeed, float fLiziTime)
	{
		Transform transform = DNTG_PoolManager.Pools["DNTGBulletPool"].Spawn(mBullet);
		DNTG_Bullet component = transform.GetComponent<DNTG_Bullet>();
		for (int i = 0; i < DNTG_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DNTG_GameInfo.getInstance().UserList[i].SeatIndex == nPlayer)
			{
				DNTG_Lock currentGun = DNTG_GameInfo.getInstance().GameScene.GetCurrentGun(DNTG_GameInfo.getInstance().UserList[i].SeatIndex);
				if (currentGun.lockProcess != DNTG_ELOCK.MoveToOtherFish && DNTG_GameInfo.getInstance().UserList[i].LockFish != null && DNTG_GameInfo.getInstance().UserList[i].Lock)
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
		DNTG_BULLET_TYPE dNTG_BULLET_TYPE = DNTG_BULLET_TYPE.BULLET_TYP_NORMAL1;
		int index = nPlayer - 1;
		DNTG_MusicMngr.GAME_SOUND gAME_SOUND = DNTG_MusicMngr.GAME_SOUND.SOUND_NORMAL_SHOOT;
		dNTG_BULLET_TYPE = ((nPower <= 50 && nPower >= 1) ? (IsSuperShoot(index) ? DNTG_BULLET_TYPE.BULLET_TYP_SUPER1 : DNTG_BULLET_TYPE.BULLET_TYP_NORMAL1) : ((nPower < 200 && nPower >= 51) ? ((!IsSuperShoot(index)) ? DNTG_BULLET_TYPE.BULLET_TYP_NORMAL2 : DNTG_BULLET_TYPE.BULLET_TYP_SUPER2) : (IsSuperShoot(index) ? DNTG_BULLET_TYPE.BULLET_TYP_SUPER3 : DNTG_BULLET_TYPE.BULLET_TYP_NORMAL3)));
		gAME_SOUND = (IsSuperShoot(index) ? DNTG_MusicMngr.GAME_SOUND.SOUND_LIZI_SHOOT : DNTG_MusicMngr.GAME_SOUND.SOUND_NORMAL_SHOOT);
		DNTG_MusicMngr.GetSingleton().PlayGameSound(gAME_SOUND);
		component.SetBulletType(dNTG_BULLET_TYPE);
		Vector3 point = Vector3.up;
		if (nPlayer == 3 || nPlayer == 4)
		{
			point = Vector3.down;
		}
		component.SetRotation(fAgl);
		if (!isTellSvr && (nPlayer - 1) / 2 != (DNTG_GameMngr.GetSingleton().mPlayerSeatID - 1) / 2)
		{
			component.SetRotation(fAgl + 180f);
		}
		Quaternion rotation = default(Quaternion);
		rotation.eulerAngles = Vector3.forward * fAgl;
		point = rotation * point;
		transform.position = _barBettePos[nPlayer - 1] + _fBarBetteR * point.normalized;
		component.MSpeed = point.normalized * _fBulletSpeed * ZH2_GVars.shellMultiple * ((!IsSuperShoot(index)) ? 1f : 1.5f);
		_bulletID++;
		if (isTellSvr)
		{
			DNTG_NetMngr.GetSingleton().MyCreateSocket.SendFired(component.mBulletID, _nPlayerIndex, fAgl, nPower, isSpeed);
		}
		return transform;
	}

	private bool IsSuperShoot(int index)
	{
		return DNTG_GameInfo.getInstance().IsSuperShoot[index];
	}

	public void DestroyBullet(GameObject bullet)
	{
		mCurrentPlayerBullet.Remove(bullet);
		mOtherPlayerBullet.Remove(bullet);
		DNTG_PoolManager.Pools["DNTGBulletPool"].Despawn(bullet.transform);
	}

	public void RemoveAllBullet()
	{
		_bulletID = 0;
		UnityEngine.Debug.Log("RemoveAllBullet");
		DNTG_PoolManager.Pools["DNTGBulletPool"].DespawnAll();
	}

	public bool IsBulletLizi()
	{
		return false;
	}

	public void LockingBulletToNormal()
	{
		for (int i = 0; i < mCurrentPlayerBullet.Count; i++)
		{
			mCurrentPlayerBullet[i].GetComponent<DNTG_Bullet>().IsLocking = false;
		}
	}

	public void OtherLockingBulletToNormal(int player)
	{
		for (int i = 0; i < mOtherPlayerBullet.Count; i++)
		{
			DNTG_Bullet component = mOtherPlayerBullet[i].GetComponent<DNTG_Bullet>();
			if (component.mPlayerID == player)
			{
				component.IsLocking = false;
			}
		}
	}
}
