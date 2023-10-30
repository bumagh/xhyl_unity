using GameCommon;
using UnityEngine;

public class STTF_NormalFish : STTF_ISwimObj
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
			if (mFishType != STTF_FISH_TYPE.Fish_CoralReefs || mFishType != STTF_FISH_TYPE.Fish_FixBomb || mFishType != STTF_FISH_TYPE.Fish_SuperBomb)
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
		if (mFishType >= STTF_FISH_TYPE.Fish_Shrimp && mFishType <= STTF_FISH_TYPE.Fish_Dragon)
		{
			num = (int)mFishType * 10;
		}
		else if (mFishType >= STTF_FISH_TYPE.Fish_Same_Shrimp && mFishType <= STTF_FISH_TYPE.Fish_Same_Turtle)
		{
			num = 200 + (int)(mFishType - 20) * 30;
		}
		else if (mFishType >= STTF_FISH_TYPE.Fish_FixBomb && mFishType <= STTF_FISH_TYPE.Fish_CoralReefs)
		{
			num = 500 + (int)(mFishType - 31) * 10;
		}
		else if (mFishType >= STTF_FISH_TYPE.Fish_YellowSpot_Group && mFishType <= STTF_FISH_TYPE.Fish_Turtle_Group)
		{
			num = 540 + (int)(mFishType - 29) * 40;
		}
		int num2 = 840 - num;
		int fishIndexInLayer = STTF_FishPoolMngr.GetSingleton().GetFishIndexInLayer(mFishType);
		_childObj.gameObject.SetActive(value: true);
		srChild.sortingOrder = num2 - fishIndexInLayer;
		layer = num2 - fishIndexInLayer;
		if (layer <= 0)
		{
			layer = Random.Range(300, 500);
		}
		switch (mFishType)
		{
		case STTF_FISH_TYPE.Fish_Trailer:
		case STTF_FISH_TYPE.Fish_Butterfly:
		case STTF_FISH_TYPE.Fish_Beauty:
		case STTF_FISH_TYPE.Fish_Arrow:
		case STTF_FISH_TYPE.Fish_Bat:
		case STTF_FISH_TYPE.Fish_SilverShark:
		case STTF_FISH_TYPE.Fish_GoldenShark:
		case STTF_FISH_TYPE.Fish_BigShark:
		case STTF_FISH_TYPE.Fish_Dragon:
		case STTF_FISH_TYPE.Fish_Toad:
			break;
		case STTF_FISH_TYPE.Fish_Same_Shrimp:
		case STTF_FISH_TYPE.Fish_Same_Grass:
		case STTF_FISH_TYPE.Fish_Same_Zebra:
		case STTF_FISH_TYPE.Fish_Same_BigEars:
		case STTF_FISH_TYPE.Fish_Same_YellowSpot:
		case STTF_FISH_TYPE.Fish_Same_Ugly:
		case STTF_FISH_TYPE.Fish_Same_Hedgehog:
		case STTF_FISH_TYPE.Fish_Same_BlueAlgae:
		case STTF_FISH_TYPE.Fish_Same_Lamp:
		case STTF_FISH_TYPE.Fish_Same_Turtle:
			if (objDriftBottle == null)
			{
				objDriftBottle = base.transform.Find("DriftBottle").gameObject;
			}
			objDriftBottle.SetActive(value: true);
			objDriftBottle.GetComponent<SpriteRenderer>().sortingOrder = layer - 2;
			srChild.sortingOrder = layer - 1;
			break;
		case STTF_FISH_TYPE.Fish_Shrimp:
		case STTF_FISH_TYPE.Fish_Grass:
		case STTF_FISH_TYPE.Fish_Zebra:
		case STTF_FISH_TYPE.Fish_BigEars:
		case STTF_FISH_TYPE.Fish_YellowSpot:
		case STTF_FISH_TYPE.Fish_Ugly:
		case STTF_FISH_TYPE.Fish_Hedgehog:
		case STTF_FISH_TYPE.Fish_BlueAlgae:
		case STTF_FISH_TYPE.Fish_Lamp:
		case STTF_FISH_TYPE.Fish_Turtle:
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
		case STTF_FISH_TYPE.Fish_Same_Shrimp:
		case STTF_FISH_TYPE.Fish_Same_Grass:
		case STTF_FISH_TYPE.Fish_Same_Zebra:
		case STTF_FISH_TYPE.Fish_Same_BigEars:
		case STTF_FISH_TYPE.Fish_Same_YellowSpot:
		case STTF_FISH_TYPE.Fish_Same_Ugly:
		case STTF_FISH_TYPE.Fish_Same_Hedgehog:
		case STTF_FISH_TYPE.Fish_Same_BlueAlgae:
		case STTF_FISH_TYPE.Fish_Same_Lamp:
		case STTF_FISH_TYPE.Fish_Same_Turtle:
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
			base.transform.localEulerAngles = Vector3.up * 270f + Vector3.left * 180f;
		}
		else
		{
			base.transform.localEulerAngles = Vector3.up * 270f;
		}
	}
}
