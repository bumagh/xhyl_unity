using GameCommon;
using UnityEngine;

public class DNTG_GroupFish : DNTG_ISwimObj
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
		case DNTG_FISH_TYPE.Fish_Shrimp:
		case DNTG_FISH_TYPE.Fish_Grass:
		case DNTG_FISH_TYPE.Fish_Zebra:
		case DNTG_FISH_TYPE.Fish_BigEars:
		case DNTG_FISH_TYPE.Fish_YellowSpot:
		case DNTG_FISH_TYPE.Fish_Ugly:
		case DNTG_FISH_TYPE.Fish_Hedgehog:
		case DNTG_FISH_TYPE.Fish_BlueAlgae:
		case DNTG_FISH_TYPE.Fish_Lamp:
		case DNTG_FISH_TYPE.Fish_Turtle:
		case DNTG_FISH_TYPE.Fish_Trailer:
		case DNTG_FISH_TYPE.Fish_Butterfly:
		case DNTG_FISH_TYPE.Fish_Beauty:
		case DNTG_FISH_TYPE.Fish_Arrow:
		case DNTG_FISH_TYPE.Fish_Bat:
		case DNTG_FISH_TYPE.Fish_SilverShark:
		case DNTG_FISH_TYPE.Fish_GoldenShark:
		case DNTG_FISH_TYPE.Fish_BlueDolphin:
		case DNTG_FISH_TYPE.Fish_Boss:
		case DNTG_FISH_TYPE.Fish_Penguin:
		case DNTG_FISH_TYPE.Fish_GoldenDragon:
			num = (int)mFishType * 10;
			break;
		case DNTG_FISH_TYPE.Fish_Same_Shrimp:
		case DNTG_FISH_TYPE.Fish_Same_Grass:
		case DNTG_FISH_TYPE.Fish_Same_Zebra:
		case DNTG_FISH_TYPE.Fish_Same_BigEars:
		case DNTG_FISH_TYPE.Fish_Same_YellowSpot:
		case DNTG_FISH_TYPE.Fish_KillDouble_One:
		case DNTG_FISH_TYPE.Fish_KillDouble_Two:
		case DNTG_FISH_TYPE.Fish_KillDouble_Three:
		case DNTG_FISH_TYPE.Fish_KillDouble_Four:
		case DNTG_FISH_TYPE.Fish_KillDouble_Five:
			num = 200 + (int)(mFishType - 20) * 30;
			break;
		case DNTG_FISH_TYPE.Fish_SuperBomb:
		case DNTG_FISH_TYPE.Fish_FixBomb:
		case DNTG_FISH_TYPE.Fish_LocalBomb:
		case DNTG_FISH_TYPE.Fish_Wheels:
			num = 500 + (int)(mFishType - 28) * 10;
			break;
		case DNTG_FISH_TYPE.Fish_BigEars_Group:
		case DNTG_FISH_TYPE.Fish_YellowSpot_Group:
		case DNTG_FISH_TYPE.Fish_Hedgehog_Group:
		case DNTG_FISH_TYPE.Fish_Ugly_Group:
		case DNTG_FISH_TYPE.Fish_BlueAlgae_Group:
		case DNTG_FISH_TYPE.Fish_Turtle_Group:
			num = 540 + (int)(mFishType - 18) * 40;
			break;
		default:
			num = (int)mFishType * 10;
			break;
		}
		int num2 = 840 - num;
		int fishIndexInLayer = DNTG_FishPoolMngr.GetSingleton().GetFishIndexInLayer(mFishType);
		layer = num2 - fishIndexInLayer;
		if (layer < 0)
		{
			layer = Random.Range(30, 50);
		}
		switch (mFishType)
		{
		case DNTG_FISH_TYPE.Fish_BigEars_Group:
		case DNTG_FISH_TYPE.Fish_YellowSpot_Group:
		case DNTG_FISH_TYPE.Fish_Hedgehog_Group:
		case DNTG_FISH_TYPE.Fish_Ugly_Group:
		case DNTG_FISH_TYPE.Fish_BlueAlgae_Group:
		case DNTG_FISH_TYPE.Fish_Turtle_Group:
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
		SetBoxColl(isEnabled: true);
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
