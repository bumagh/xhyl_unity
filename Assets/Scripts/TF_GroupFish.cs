using GameCommon;
using UnityEngine;

public class TF_GroupFish : TF_ISwimObj
{
	private Quaternion _parentRotation;

	private Quaternion[] _FishRotation;

	private Quaternion[] _FishRingRotation;

	private Quaternion _FishRingBigRotation;

	private Quaternion _FishRingBigBgRotation;

	private GameObject _Fish;

	private GameObject _FishRing;

	private Animator[] anims;

	private int fishChildCount;

	private int ringChildCount;

	private void Awake()
	{
		_Fish = base.transform.Find("Fish").gameObject;
		fishChildCount = _Fish.transform.childCount;
		_FishRing = base.transform.Find("FishRing").gameObject;
		ringChildCount = _FishRing.transform.childCount;
		_parentRotation = base.transform.rotation;
		_FishRotation = new Quaternion[fishChildCount];
		anims = new Animator[fishChildCount];
		for (int i = 0; i < fishChildCount; i++)
		{
			_FishRotation[i] = _Fish.transform.GetChild(i).rotation;
			anims[i] = _Fish.transform.GetChild(i).GetComponent<Animator>();
		}
		_FishRingRotation = new Quaternion[ringChildCount];
		for (int j = 0; j < ringChildCount; j++)
		{
			_FishRingRotation[j] = _FishRing.transform.GetChild(j).rotation;
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
		for (int j = 0; j < ringChildCount; j++)
		{
			_FishRing.transform.GetChild(j).rotation = _FishRingRotation[j];
		}
	}

	protected override void _setFishLayer()
	{
		int num = (int)mFishType * 10;
		if (mFishType >= TF_FISH_TYPE.Fish_Shrimp && mFishType <= TF_FISH_TYPE.Fish_Dragon)
		{
			num = (int)mFishType * 10;
		}
		else if (mFishType >= TF_FISH_TYPE.Fish_Same_Shrimp && mFishType <= TF_FISH_TYPE.Fish_Same_Turtle)
		{
			num = 200 + (int)(mFishType - 25) * 30;
		}
		else if (mFishType >= TF_FISH_TYPE.Fish_FixBomb && mFishType <= TF_FISH_TYPE.Fish_CoralReefs)
		{
			num = 500 + (int)(mFishType - 22) * 10;
		}
		else if (mFishType >= TF_FISH_TYPE.Fish_BigEars_Group && mFishType <= TF_FISH_TYPE.Fish_Turtle_Group)
		{
			num = 540 + (int)(mFishType - 35) * 40;
		}
		int num2 = 840 - num;
		int fishIndexInLayer = TF_FishPoolMngr.GetSingleton().GetFishIndexInLayer(mFishType);
		layer = num2 - fishIndexInLayer;
		if (mFishType >= TF_FISH_TYPE.Fish_BigEars_Group && mFishType <= TF_FISH_TYPE.Fish_Turtle_Group)
		{
			for (int i = 0; i < fishChildCount; i++)
			{
				_Fish.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer;
			}
			_FishRing.transform.GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer;
			for (int j = 0; j < ringChildCount; j++)
			{
				_FishRing.transform.GetChild(j).GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer - 1;
			}
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
