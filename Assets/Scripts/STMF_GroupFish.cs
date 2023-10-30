using GameCommon;
using UnityEngine;

public class STMF_GroupFish : STMF_ISwimObj
{
	private GameObject _childObj;

	private Quaternion _parentRotation;

	private Quaternion[] _FishRotation;

	private Quaternion[] _FishRingRotation;

	private Quaternion _FishRingBigRotation;

	private Quaternion _FishRingBigBgRotation;

	private GameObject _Fish;

	private GameObject _FishRing;

	private GameObject _FishRingBigBg;

	private int fishChildCount;

	private int ringChildCount;

	private void Awake()
	{
		_childObj = base.transform.Find("Fish").gameObject;
		_Fish = _childObj;
		fishChildCount = _Fish.transform.childCount;
		_FishRing = base.transform.Find("FishRing").gameObject;
		ringChildCount = _FishRing.transform.childCount;
		_FishRingBigBg = base.transform.Find("FishRingBigBg").gameObject;
		_parentRotation = base.transform.rotation;
		_FishRotation = new Quaternion[fishChildCount];
		for (int i = 0; i < fishChildCount; i++)
		{
			_FishRotation[i] = _Fish.transform.GetChild(i).rotation;
		}
		_FishRingRotation = new Quaternion[ringChildCount];
		for (int j = 0; j < ringChildCount; j++)
		{
			_FishRingRotation[j] = _FishRing.transform.GetChild(j).rotation;
		}
		_FishRingBigBgRotation = _FishRingBigBg.transform.rotation;
	}

	protected override void _doSwimAnim()
	{
		for (int i = 0; i < fishChildCount; i++)
		{
			_Fish.transform.GetChild(i).GetComponent<Animator>().enabled = true;
		}
	}

	protected override void _doDieAnim()
	{
		for (int i = 0; i < fishChildCount; i++)
		{
			_Fish.transform.GetChild(i).GetComponent<Animator>().SetBool("bDeath", value: true);
		}
	}

	protected override void _onDestroy()
	{
		base.transform.rotation = _parentRotation;
		for (int i = 0; i < fishChildCount; i++)
		{
			_Fish.transform.GetChild(i).rotation = _FishRotation[i];
		}
		for (int j = 0; j < ringChildCount; j++)
		{
			_FishRing.transform.GetChild(j).rotation = _FishRingRotation[j];
		}
		_FishRingBigBg.transform.rotation = _FishRingBigBgRotation;
	}

	protected override void _setFishLayer()
	{
		int num = (int)mFishType * 10;
		if (mFishType >= STMF_FISH_TYPE.Fish_Shrimp && mFishType <= STMF_FISH_TYPE.Fish_GoldenDragon)
		{
			num = (int)mFishType * 10;
		}
		else if (mFishType >= STMF_FISH_TYPE.Fish_Same_Shrimp && mFishType <= STMF_FISH_TYPE.Fish_Same_Turtle)
		{
			num = 190 + (int)(mFishType - 20) * 30;
		}
		else if (mFishType >= STMF_FISH_TYPE.Fish_LimitedBomb && mFishType <= STMF_FISH_TYPE.Fish_CoralReefs)
		{
			num = 490 + (int)(mFishType - 30) * 10;
		}
		else if (mFishType >= STMF_FISH_TYPE.Fish_DragonBeauty_Group && mFishType <= STMF_FISH_TYPE.Fish_Knife_Butterfly_Group)
		{
			num = 520 + (int)(mFishType - 33) * 40;
		}
		int num2 = 640 - num;
		int fishIndexInLayer = STMF_FishPoolMngr.GetSingleton().GetFishIndexInLayer(mFishType);
		if (mFishType >= STMF_FISH_TYPE.Fish_DragonBeauty_Group && mFishType <= STMF_FISH_TYPE.Fish_Knife_Butterfly_Group)
		{
			_Fish.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer - 1;
			for (int i = 1; i < fishChildCount; i++)
			{
				_Fish.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer;
			}
			for (int j = 0; j < ringChildCount; j++)
			{
				_FishRing.transform.GetChild(j).GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer - 2;
			}
			_FishRingBigBg.GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer - 3;
		}
	}

	protected override void _setCollider()
	{
		mBoxCollider = base.transform.Find("FishRingBigBg").GetComponent<BoxCollider2D>();
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
