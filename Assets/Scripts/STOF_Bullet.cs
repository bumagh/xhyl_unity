using GameCommon;
using UnityEngine;

public class STOF_Bullet : STOF_IMoveObj
{
	[SerializeField]
	private GameObject[] objBullets;

	private STOF_BULLET_TYPE _bulletType;

	private int _bulletID;

	private bool _isLizi;

	private int _nPower;

	public bool _isDead;

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

	public STOF_BULLET_TYPE GetBulletType()
	{
		return _bulletType;
	}

	public void SetBulletType(STOF_BULLET_TYPE type)
	{
		_bulletType = type;
		int bulletType = (int)_bulletType;
		for (int i = 0; i < 15; i++)
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
		base.mPlayerID = 1;
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
				STOF_MusicMngr.GetSingleton().PlayGameSound(STOF_MusicMngr.GAME_SOUND.SOUND_LIZI_EXPLODE);
			}
			else
			{
				STOF_MusicMngr.GetSingleton().PlayGameSound(STOF_MusicMngr.GAME_SOUND.SOUND_NORMAL_BULLET_EXPLODE);
			}
			Vector3 pos = (fish.transform.position + base.transform.position) / 2f;
			STOF_FishNetpoolMngr.GetSingleton().CreateFishNet(pos, mIsLizi, mPower, base.mPlayerID);
			if (STOF_GameMngr.GetSingleton().mPlayerSeatID == base.mPlayerID)
			{
				STOF_FishPoolMngr.GetSingleton().Fishing2(base.transform.position, this, fish);
			}
			STOF_BulletPoolMngr.GetSingleton().DestroyBullet(base.gameObject);
		}
	}
}
