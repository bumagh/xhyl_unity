using GameCommon;
using UnityEngine;

public class STOF_NormalFish : STOF_ISwimObj
{
	protected GameObject _childObj;

	private SpriteRenderer srChild;

	protected Animator anim;

	protected Quaternion _parentRotation;

	protected Quaternion _childRotation;

	[SerializeField]
	private GameObject objDriftBottle;

	private void Awake()
	{
		_childObj = base.transform.Find("Fish").gameObject;
		srChild = _childObj.GetComponent<SpriteRenderer>();
		anim = _childObj.GetComponent<Animator>();
		_parentRotation = base.transform.rotation;
		_childRotation = _childObj.transform.rotation;
	}

	protected override void _doSwimAnim()
	{
		if (anim != null)
		{
			anim.enabled = true;
		}
	}

	protected override void _doDieAnim()
	{
		if (anim != null)
		{
			if (mFishType != STOF_FISH_TYPE.Fish_CoralReefs || mFishType != STOF_FISH_TYPE.Fish_PartBomb || mFishType != STOF_FISH_TYPE.Fish_FixBomb || mFishType != STOF_FISH_TYPE.Fish_SuperBomb)
			{
				anim.SetBool("bDeath", value: true);
			}
			else
			{
				anim.enabled = false;
			}
		}
	}

	protected override void _onDestroy()
	{
		base.transform.rotation = _parentRotation;
		_childObj.transform.rotation = _childRotation;
	}

	protected override void _setFishLayer()
	{
		int num = (int)mFishType * 10;
		switch (mFishType)
		{
		case STOF_FISH_TYPE.Fish_Shrimp:
		case STOF_FISH_TYPE.Fish_Grass:
		case STOF_FISH_TYPE.Fish_Zebra:
		case STOF_FISH_TYPE.Fish_BigEars:
		case STOF_FISH_TYPE.Fish_YellowSpot:
		case STOF_FISH_TYPE.Fish_Ugly:
		case STOF_FISH_TYPE.Fish_Hedgehog:
		case STOF_FISH_TYPE.Fish_BlueAlgae:
		case STOF_FISH_TYPE.Fish_Lamp:
		case STOF_FISH_TYPE.Fish_Turtle:
		case STOF_FISH_TYPE.Fish_Trailer:
		case STOF_FISH_TYPE.Fish_Butterfly:
		case STOF_FISH_TYPE.Fish_Beauty:
		case STOF_FISH_TYPE.Fish_Arrow:
		case STOF_FISH_TYPE.Fish_Bat:
		case STOF_FISH_TYPE.Fish_SilverShark:
		case STOF_FISH_TYPE.Fish_GoldenShark:
		case STOF_FISH_TYPE.Fish_GoldenSharkB:
		case STOF_FISH_TYPE.Fish_GoldenDragon:
		case STOF_FISH_TYPE.Fish_Boss:
		case STOF_FISH_TYPE.Fish_Penguin:
			num = (int)mFishType * 10;
			break;
		case STOF_FISH_TYPE.Fish_Same_Shrimp:
		case STOF_FISH_TYPE.Fish_Same_Grass:
		case STOF_FISH_TYPE.Fish_Same_Zebra:
		case STOF_FISH_TYPE.Fish_Same_BigEars:
		case STOF_FISH_TYPE.Fish_Same_YellowSpot:
		case STOF_FISH_TYPE.Fish_Same_Ugly:
		case STOF_FISH_TYPE.Fish_Same_Hedgehog:
		case STOF_FISH_TYPE.Fish_Same_BlueAlgae:
		case STOF_FISH_TYPE.Fish_Same_Lamp:
		case STOF_FISH_TYPE.Fish_Same_Turtle:
			num = 200 + (int)(mFishType - 20) * 30;
			break;
		case STOF_FISH_TYPE.Fish_SuperBomb:
		case STOF_FISH_TYPE.Fish_FixBomb:
		case STOF_FISH_TYPE.Fish_CoralReefs:
		case STOF_FISH_TYPE.Fish_PartBomb:
			num = 500 + (int)(mFishType - 39) * 10;
			break;
		case STOF_FISH_TYPE.Fish_BigEars_Group:
		case STOF_FISH_TYPE.Fish_YellowSpot_Group:
		case STOF_FISH_TYPE.Fish_Hedgehog_Group:
		case STOF_FISH_TYPE.Fish_Ugly_Group:
		case STOF_FISH_TYPE.Fish_BlueAlgae_Group:
		case STOF_FISH_TYPE.Fish_Turtle_Group:
			num = 540 + (int)(mFishType - 19) * 40;
			break;
		default:
			num = (int)mFishType * 10;
			break;
		}
		int num2 = 840 - num;
		int fishIndexInLayer = STOF_FishPoolMngr.GetSingleton().GetFishIndexInLayer(mFishType);
		_childObj.gameObject.SetActive(value: true);
		srChild.sortingOrder = num2 - fishIndexInLayer;
		layer = num2 - fishIndexInLayer;
		if (layer < 0)
		{
			layer = Random.Range(30, 50);
		}
		switch (mFishType)
		{
		case STOF_FISH_TYPE.Fish_Trailer:
		case STOF_FISH_TYPE.Fish_Butterfly:
		case STOF_FISH_TYPE.Fish_Beauty:
		case STOF_FISH_TYPE.Fish_Arrow:
		case STOF_FISH_TYPE.Fish_Bat:
		case STOF_FISH_TYPE.Fish_SilverShark:
		case STOF_FISH_TYPE.Fish_GoldenShark:
		case STOF_FISH_TYPE.Fish_GoldenSharkB:
		case STOF_FISH_TYPE.Fish_GoldenDragon:
		case STOF_FISH_TYPE.Fish_Boss:
			break;
		case STOF_FISH_TYPE.Fish_Same_Shrimp:
		case STOF_FISH_TYPE.Fish_Same_Grass:
		case STOF_FISH_TYPE.Fish_Same_Zebra:
		case STOF_FISH_TYPE.Fish_Same_BigEars:
		case STOF_FISH_TYPE.Fish_Same_YellowSpot:
		case STOF_FISH_TYPE.Fish_Same_Ugly:
		case STOF_FISH_TYPE.Fish_Same_Hedgehog:
		case STOF_FISH_TYPE.Fish_Same_BlueAlgae:
		case STOF_FISH_TYPE.Fish_Same_Lamp:
		case STOF_FISH_TYPE.Fish_Same_Turtle:
			if (objDriftBottle == null)
			{
				objDriftBottle = base.transform.Find("DriftBottle").gameObject;
			}
			objDriftBottle.SetActive(value: true);
			objDriftBottle.GetComponent<SpriteRenderer>().sortingOrder = layer - 2;
			srChild.sortingOrder = layer - 1;
			break;
		case STOF_FISH_TYPE.Fish_Shrimp:
		case STOF_FISH_TYPE.Fish_Grass:
		case STOF_FISH_TYPE.Fish_Zebra:
		case STOF_FISH_TYPE.Fish_BigEars:
		case STOF_FISH_TYPE.Fish_YellowSpot:
		case STOF_FISH_TYPE.Fish_Ugly:
		case STOF_FISH_TYPE.Fish_Hedgehog:
		case STOF_FISH_TYPE.Fish_BlueAlgae:
		case STOF_FISH_TYPE.Fish_Lamp:
		case STOF_FISH_TYPE.Fish_Turtle:
			if (objDriftBottle == null)
			{
				objDriftBottle = base.transform.Find("DriftBottle").gameObject;
			}
			objDriftBottle.gameObject.SetActive(value: false);
			break;
		}
	}

	protected override void _setCollider()
	{
		switch (mFishType)
		{
		case STOF_FISH_TYPE.Fish_Same_Shrimp:
		case STOF_FISH_TYPE.Fish_Same_Grass:
		case STOF_FISH_TYPE.Fish_Same_Zebra:
		case STOF_FISH_TYPE.Fish_Same_BigEars:
		case STOF_FISH_TYPE.Fish_Same_YellowSpot:
		case STOF_FISH_TYPE.Fish_Same_Ugly:
		case STOF_FISH_TYPE.Fish_Same_Hedgehog:
		case STOF_FISH_TYPE.Fish_Same_BlueAlgae:
		case STOF_FISH_TYPE.Fish_Same_Lamp:
		case STOF_FISH_TYPE.Fish_Same_Turtle:
			if (objDriftBottle == null)
			{
				objDriftBottle = base.transform.Find("DriftBottle").gameObject;
			}
			mBoxCollider = objDriftBottle.GetComponent<BoxCollider2D>();
			break;
		default:
			mBoxCollider = _childObj.GetComponent<BoxCollider2D>();
			break;
		}
		mBoxCollider.enabled = true;
	}

	public override void SetUpDir(bool isR)
	{
		if (!isR)
		{
			_childObj.transform.localEulerAngles = Vector3.up * 270f + Vector3.left * 180f;
		}
		else
		{
			_childObj.transform.localEulerAngles = Vector3.up * 270f;
		}
	}
}
