using GameCommon;
using UnityEngine;

public class BZJX_GroupFish : BZJX_ISwimObj
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
		for (int i = 0; i < fishChildCount; i++)
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
		switch (mFishType)
		{
		case BZJX_FISH_TYPE.Fish_Shrimp:
		case BZJX_FISH_TYPE.Fish_Grass:
		case BZJX_FISH_TYPE.Fish_Zebra:
		case BZJX_FISH_TYPE.Fish_BigEars:
		case BZJX_FISH_TYPE.Fish_YellowSpot:
		case BZJX_FISH_TYPE.Fish_Ugly:
		case BZJX_FISH_TYPE.Fish_Hedgehog:
		case BZJX_FISH_TYPE.Fish_BlueAlgae:
		case BZJX_FISH_TYPE.Fish_Lamp:
		case BZJX_FISH_TYPE.Fish_Turtle:
		case BZJX_FISH_TYPE.Fish_Trailer:
		case BZJX_FISH_TYPE.Fish_Butterfly:
		case BZJX_FISH_TYPE.Fish_Beauty:
		case BZJX_FISH_TYPE.Fish_Arrow:
		case BZJX_FISH_TYPE.Fish_Bat:
		case BZJX_FISH_TYPE.Fish_SilverShark:
		case BZJX_FISH_TYPE.Fish_GoldenShark:
		case BZJX_FISH_TYPE.Fish_GoldenSharkB:
		case BZJX_FISH_TYPE.Fish_GoldenDragon:
		case BZJX_FISH_TYPE.Fish_Boss:
		case BZJX_FISH_TYPE.Fish_Penguin:
			num = (int)mFishType * 10;
			break;
		case BZJX_FISH_TYPE.Fish_Same_Shrimp:
		case BZJX_FISH_TYPE.Fish_Same_Grass:
		case BZJX_FISH_TYPE.Fish_Same_Zebra:
		case BZJX_FISH_TYPE.Fish_Same_BigEars:
		case BZJX_FISH_TYPE.Fish_Same_YellowSpot:
		case BZJX_FISH_TYPE.Fish_Same_Ugly:
		case BZJX_FISH_TYPE.Fish_Same_Hedgehog:
		case BZJX_FISH_TYPE.Fish_Same_BlueAlgae:
		case BZJX_FISH_TYPE.Fish_Same_Lamp:
		case BZJX_FISH_TYPE.Fish_Same_Turtle:
			num = 200 + (int)(mFishType - 20) * 30;
			break;
		case BZJX_FISH_TYPE.Fish_SuperBomb:
		case BZJX_FISH_TYPE.Fish_FixBomb:
		case BZJX_FISH_TYPE.Fish_CoralReefs:
		case BZJX_FISH_TYPE.Fish_PartBomb:
			num = 500 + (int)(mFishType - 39) * 10;
			break;
		case BZJX_FISH_TYPE.Fish_BigEars_Group:
		case BZJX_FISH_TYPE.Fish_YellowSpot_Group:
		case BZJX_FISH_TYPE.Fish_Hedgehog_Group:
		case BZJX_FISH_TYPE.Fish_Ugly_Group:
		case BZJX_FISH_TYPE.Fish_BlueAlgae_Group:
		case BZJX_FISH_TYPE.Fish_Turtle_Group:
			num = 540 + (int)(mFishType - 19) * 40;
			break;
		default:
			num = (int)mFishType * 10;
			break;
		}
		int num2 = 840 - num;
		int fishIndexInLayer = BZJX_FishPoolMngr.GetSingleton().GetFishIndexInLayer(mFishType);
		layer = num2 - fishIndexInLayer;
		if (layer < 0)
		{
			layer = Random.Range(30, 50);
		}
		switch (mFishType)
		{
		case BZJX_FISH_TYPE.Fish_BigEars_Group:
		case BZJX_FISH_TYPE.Fish_YellowSpot_Group:
		case BZJX_FISH_TYPE.Fish_Hedgehog_Group:
		case BZJX_FISH_TYPE.Fish_Ugly_Group:
		case BZJX_FISH_TYPE.Fish_BlueAlgae_Group:
		case BZJX_FISH_TYPE.Fish_Turtle_Group:
			for (int i = 0; i < fishChildCount; i++)
			{
				_Fish.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = layer;
			}
			_FishRing.transform.GetComponent<SpriteRenderer>().sortingOrder = layer;
			for (int j = 0; j < ringChildCount; j++)
			{
				_FishRing.transform.GetChild(j).GetComponent<SpriteRenderer>().sortingOrder = layer - 1;
			}
			break;
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
