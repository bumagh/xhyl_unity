using GameCommon;
using UnityEngine;

public class TF_Bullet : TF_IMoveObj
{
	[SerializeField]
	private GameObject[] objBullets;

	private TF_BULLET_TYPE _bulletType;

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

	public TF_BULLET_TYPE GetBulletType()
	{
		return _bulletType;
	}

	public void SetBulletType(TF_BULLET_TYPE type)
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
		if (_isDead)
		{
			return;
		}
		_isDead = true;
		if (_isLizi)
		{
			TF_MusicMngr.GetSingleton().PlayGameSound(TF_MusicMngr.GAME_SOUND.SOUND_LIZI_EXPLODE);
		}
		else
		{
			TF_MusicMngr.GetSingleton().PlayGameSound(TF_MusicMngr.GAME_SOUND.SOUND_NORMAL_BULLET_EXPLODE);
		}
		Vector3 pos = (fish.transform.position + base.transform.position) / 2f;
		TF_FishNetpoolMngr.GetSingleton().CreateFishNet(pos, mIsLizi, mPower, base.mPlayerID);
		if (TF_GameMngr.GetSingleton().mPlayerSeatID == base.mPlayerID)
		{
			bool flag = false;
			for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
			{
				if (TF_GameInfo.getInstance().UserList[i].SeatIndex == base.mPlayerID)
				{
					if (TF_GameInfo.getInstance().UserList[i].Lock)
					{
						flag = true;
					}
					break;
				}
			}
			if (!flag)
			{
				TF_FishPoolMngr.GetSingleton().Fishing(base.transform.position, this, fish);
			}
			else
			{
				TF_FishPoolMngr.GetSingleton().OneFishing(base.transform.position, this, fish);
			}
		}
		TF_BulletPoolMngr.GetSingleton().DestroyBullet(base.gameObject);
	}
}
