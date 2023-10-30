using GameCommon;
using UnityEngine;

public class STMF_Bullet : STMF_IMoveObj
{
	private GameObject[] objBullets;

	private STMF_BULLET_TYPE _bulletType;

	private int _bulletID;

	private int _nPlayerID;

	private bool _isLizi;

	private int _nPower;

	private bool _isDead;

	public int mBulletID
	{
		get
		{
			return _bulletID;
		}
		set
		{
			_bulletID = value;
		}
	}

	public int mPlayerID
	{
		get
		{
			return _nPlayerID;
		}
		set
		{
			_nPlayerID = value;
		}
	}

	public bool mIsLizi
	{
		get
		{
			return _isLizi;
		}
		set
		{
			_isLizi = value;
		}
	}

	public int mPower
	{
		get
		{
			return _nPower;
		}
		set
		{
			_nPower = value;
		}
	}

	public bool mIsDead
	{
		get
		{
			return _isDead;
		}
		set
		{
			_isDead = value;
		}
	}

	public STMF_BULLET_TYPE GetBulletType()
	{
		return _bulletType;
	}

	private void Awake()
	{
		objBullets = new GameObject[10];
		for (int i = 0; i < 10; i++)
		{
			objBullets[i] = base.transform.GetChild(i).gameObject;
		}
	}

	public void SetBulletType(STMF_BULLET_TYPE type)
	{
		_bulletType = type;
		int bulletType = (int)_bulletType;
		for (int i = 0; i < 10; i++)
		{
			objBullets[i].SetActive(value: false);
		}
		objBullets[bulletType].SetActive(value: true);
	}

	private void Update()
	{
		if (!mIsDead)
		{
			MoveLogic();
		}
	}

	public void OnSpawned()
	{
		mPlayerID = 1;
		mPower = 1;
		_isDead = false;
	}

	public void OnDespawned()
	{
		mIsLizi = false;
	}

	public void ObjDestroy(GameObject fish)
	{
		if (!_isDead)
		{
			_isDead = true;
			if (_isLizi)
			{
				STMF_MusicMngr.GetSingleton().PlayGameSound(STMF_MusicMngr.GAME_SOUND.SOUND_LIZI_EXPLODE);
			}
			else
			{
				STMF_MusicMngr.GetSingleton().PlayGameSound(STMF_MusicMngr.GAME_SOUND.SOUND_NORMAL_BULLET_EXPLODE);
			}
			Vector3 pos = (fish.transform.position + base.transform.position) / 2f;
			STMF_FishNetpoolMngr.GetSingleton().CreateFishNet(pos, mIsLizi, mPower, mPlayerID);
			if (STMF_GameMngr.GetSingleton().mPlayerSeatID == mPlayerID)
			{
				STMF_FishPoolMngr.GetSingleton().Fishing(base.transform.position, this, fish);
			}
			STMF_BulletPoolMngr.GetSingleton().DestroyBullet(base.gameObject);
		}
	}
}
