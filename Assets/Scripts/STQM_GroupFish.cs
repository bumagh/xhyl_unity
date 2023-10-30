using GameCommon;
using UnityEngine;

public class STQM_GroupFish : STQM_ISwimObj
{
	private Quaternion _parentRotation;

	private Quaternion[] _FishRotation;

	private Quaternion _FishRingBigRotation;

	private Quaternion _FishRingBigBgRotation;

	private GameObject _Fish;

	private GameObject _FishRing;

	private Animator[] anims;

	private int fishChildCount;

	private void Awake()
	{
		_Fish = base.transform.Find("Fish").gameObject;
		fishChildCount = _Fish.transform.childCount;
		_FishRing = base.transform.Find("FishRing").gameObject;
		_parentRotation = base.transform.rotation;
		_FishRotation = new Quaternion[fishChildCount];
		anims = new Animator[fishChildCount];
		for (int i = 0; i < fishChildCount; i++)
		{
			_FishRotation[i] = _Fish.transform.GetChild(i).rotation;
			anims[i] = _Fish.transform.GetChild(i).GetComponent<Animator>();
		}
	}

	public void InitDoubleFish()
	{
		fishChildCount = _Fish.transform.childCount;
		_FishRotation = new Quaternion[fishChildCount];
		anims = new Animator[fishChildCount];
		for (int i = 0; i < fishChildCount; i++)
		{
			_FishRotation[i] = _Fish.transform.GetChild(i).rotation;
			anims[i] = _Fish.transform.GetChild(i).GetComponent<Animator>();
		}
	}

	protected override void _doSwimAnim()
	{
		for (int i = 0; i < fishChildCount; i++)
		{
			anims[i].enabled = true;
		}
	}

	protected override void _doDieAnim()
	{
		for (int i = 0; i < fishChildCount; i++)
		{
			anims[i].SetBool("bDeath", value: true);
		}
	}

	protected override void _onDestroy()
	{
		base.transform.rotation = _parentRotation;
		for (int i = 0; i < _Fish.transform.childCount; i++)
		{
			_Fish.transform.GetChild(i).rotation = _FishRotation[i];
		}
	}

	protected override void _setFishLayer()
	{
		int num = (int)mFishType * 10;
		if (mFishType >= STQM_FISH_TYPE.Fish_Shrimp && mFishType <= STQM_FISH_TYPE.Fish_BuleWhale)
		{
			num = (int)mFishType * 10;
		}
		else if (mFishType >= STQM_FISH_TYPE.Fish_Same_Shrimp && mFishType <= STQM_FISH_TYPE.Fish_Same_Turtle)
		{
			num = 200 + (int)(mFishType - 20) * 30;
		}
		else if (mFishType >= STQM_FISH_TYPE.Fish_AllBomb)
		{
			num = 490 + (int)(mFishType - 31) * 10;
		}
		else if (mFishType == STQM_FISH_TYPE.Fish_BowlFish)
		{
			num = 510;
		}
		int num2 = 640 - num;
		int fishIndexInLayer = STQM_FishPoolMngr.GetSingleton().GetFishIndexInLayer(mFishType);
		layer = num2 - fishIndexInLayer;
		if (mFishType == STQM_FISH_TYPE.Fish_BowlFish)
		{
			for (int i = 0; i < fishChildCount; i++)
			{
				_Fish.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer;
			}
			_FishRing.transform.GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer - 1;
		}
	}

	protected override void _setCollider()
	{
		mBoxCollider.enabled = true;
	}

	public override void SetUpDir(bool isR)
	{
		for (int i = 0; i < fishChildCount; i++)
		{
			if (!isR)
			{
			}
		}
	}
}
