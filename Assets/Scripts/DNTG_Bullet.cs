using GameCommon;
using UnityEngine;

public class DNTG_Bullet : DNTG_IMoveObj
{
	[SerializeField]
	private GameObject[] objBullets;

	[HideInInspector]
	public DNTG_BULLET_TYPE _bulletType;

	public bool _isDead;

	public int mBulletID
	{
		get;
		set;
	}

	public int mPower
	{
		get;
		set;
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

	public DNTG_BULLET_TYPE GetBulletType()
	{
		return _bulletType;
	}

	public void SetBulletType(DNTG_BULLET_TYPE type)
	{
		_bulletType = type;
		int bulletType = (int)_bulletType;
		for (int i = 0; i < objBullets.Length; i++)
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

	public void ObjDestroy(GameObject fish)
	{
		if (!_isDead)
		{
			_isDead = true;
			DNTG_MusicMngr.GetSingleton().PlayGameSound(DNTG_MusicMngr.GAME_SOUND.SOUND_NORMAL_BULLET_EXPLODE);
			Vector3 pos = (fish.transform.position + base.transform.position) / 2f;
			DNTG_FishNetpoolMngr.GetSingleton().CreateFishNet(pos, mPower, base.mPlayerID, this);
			if (DNTG_GameMngr.GetSingleton().mPlayerSeatID == base.mPlayerID)
			{
				DNTG_FishPoolMngr.GetSingleton().Fishing2(base.transform.position, this, fish);
			}
			DNTG_BulletPoolMngr.GetSingleton().DestroyBullet(base.gameObject);
		}
	}
}
