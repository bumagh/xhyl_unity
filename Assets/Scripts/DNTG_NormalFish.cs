using GameCommon;
using UnityEngine;

public class DNTG_NormalFish : DNTG_ISwimObj
{
	protected GameObject[] fishObj;

	private SpriteRenderer[] srChild;

	protected Animator[] anim;

	protected Quaternion _parentRotation;

	protected Quaternion[] _childRotation;

	[SerializeField]
	private GameObject objDriftBottle;

	private GameObject objHeaven;

	private void Awake()
	{
		_parentRotation = base.transform.rotation;
		InitFind();
	}

	private void InitFind()
	{
		anim = GetComponentsInChildren<Animator>(includeInactive: true);
		int num = anim.Length;
		fishObj = new GameObject[num];
		srChild = new SpriteRenderer[num];
		_childRotation = new Quaternion[num];
		for (int i = 0; i < num; i++)
		{
			fishObj[i] = anim[i].gameObject;
		}
		for (int j = 0; j < num; j++)
		{
			srChild[j] = fishObj[j].GetComponent<SpriteRenderer>();
			if (srChild[j] != null)
			{
				_childRotation[j] = fishObj[j].transform.rotation;
			}
		}
	}

	public void SetAnimEnabled(bool isEnabled)
	{
		for (int i = 0; i < anim.Length; i++)
		{
			anim[i].enabled = isEnabled;
		}
	}

	protected override void _doSwimAnim()
	{
		SetAnimEnabled(isEnabled: true);
	}

	protected override void _doDieAnim()
	{
		if (anim == null)
		{
			return;
		}
		if (mFishType != DNTG_FISH_TYPE.Fish_LocalBomb || mFishType != DNTG_FISH_TYPE.Fish_Wheels || mFishType != DNTG_FISH_TYPE.Fish_FixBomb || mFishType != DNTG_FISH_TYPE.Fish_SuperBomb)
		{
			for (int i = 0; i < anim.Length; i++)
			{
				anim[i].SetBool("bDeath", value: true);
			}
		}
		else
		{
			SetAnimEnabled(isEnabled: false);
		}
	}

	protected override void _onDestroy()
	{
		base.transform.rotation = _parentRotation;
		for (int i = 0; i < fishObj.Length; i++)
		{
			fishObj[i].transform.rotation = _childRotation[i];
		}
	}

	protected override void _setFishLayer()
	{
		if (specialFishType == SpecialFishType.HeavenFish || specialFishType == SpecialFishType.LightningFish)
		{
			SetDriftBottle(isShow: true);
		}
		else
		{
			SetDriftBottle(isShow: false);
		}
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
		case DNTG_FISH_TYPE.Fish_GoldFull:
			num = 540 + (int)(mFishType - 18) * 40;
			break;
		default:
			num = (int)mFishType * 10;
			break;
		}
		int num2 = 940 - num;
		int fishIndexInLayer = DNTG_FishPoolMngr.GetSingleton().GetFishIndexInLayer(mFishType);
		for (int i = 0; i < fishObj.Length; i++)
		{
			fishObj[i].gameObject.SetActive(value: true);
		}
		for (int j = 0; j < srChild.Length; j++)
		{
			SpriteRenderer spriteRenderer = srChild[j];
			if (spriteRenderer != null)
			{
				spriteRenderer.sortingOrder = num2 - fishIndexInLayer + j;
			}
		}
		layer = num2 - fishIndexInLayer;
		if (layer < 0)
		{
			layer = 982;
		}
		switch (mFishType)
		{
		case DNTG_FISH_TYPE.Fish_Wheels:
		case DNTG_FISH_TYPE.Fish_KillDouble_One:
		case DNTG_FISH_TYPE.Fish_KillDouble_Two:
		case DNTG_FISH_TYPE.Fish_KillDouble_Three:
		case DNTG_FISH_TYPE.Fish_KillDouble_Four:
		case DNTG_FISH_TYPE.Fish_KillDouble_Five:
		case DNTG_FISH_TYPE.Fish_KillThree_One:
		case DNTG_FISH_TYPE.Fish_KillThree_Two:
		case DNTG_FISH_TYPE.Fish_KillThree_Three:
		case DNTG_FISH_TYPE.Fish_KillThree_Four:
			SetDriftBottle(isShow: true);
			if (objDriftBottle != null)
			{
				objDriftBottle.GetComponent<SpriteRenderer>().sortingOrder = layer - 2;
			}
			for (int l = 0; l < srChild.Length; l++)
			{
				srChild[l].sortingOrder = layer - 1 + l;
			}
			break;
		case DNTG_FISH_TYPE.Fish_GoldFull:
			SetDriftBottle(isShow: true);
			for (int k = 0; k < srChild.Length; k++)
			{
				srChild[k].sortingOrder = layer - k;
			}
			break;
		}
	}

	private void SetDriftBottle(bool isShow)
	{
		if (objDriftBottle == null)
		{
			Transform transform = base.transform.Find("DriftBottle");
			if (transform != null)
			{
				objDriftBottle = transform.gameObject;
			}
		}
		if (objHeaven == null)
		{
			Transform transform2 = base.transform.Find("Heaven");
			if (transform2 != null)
			{
				objHeaven = transform2.gameObject;
			}
		}
		if (isShow)
		{
			if (specialFishType != SpecialFishType.HeavenFish)
			{
				if (objDriftBottle != null)
				{
					objDriftBottle.SetActive(value: true);
				}
			}
			else if (objHeaven != null)
			{
				objHeaven.SetActive(value: true);
			}
		}
		else
		{
			if (objDriftBottle != null)
			{
				objDriftBottle.SetActive(value: false);
			}
			if (objHeaven != null)
			{
				objHeaven.SetActive(value: false);
			}
		}
	}

	protected override void _setCollider()
	{
		SetBoxColl(isEnabled: true);
	}

	public override void SetUpDir(bool isR)
	{
		if (!isR)
		{
			SetFishLocalEulerAngles(Vector3.up * 270f + Vector3.left * 180f, isR);
		}
		else
		{
			SetFishLocalEulerAngles(Vector3.up * 270f, isR);
		}
	}

	private void SetFishLocalEulerAngles(Vector3 vector3, bool isR)
	{
		if (specialFishType == SpecialFishType.MonkeyKing)
		{
			Transform transform = base.transform.Find("Fish");
			if (transform != null)
			{
				if (isR)
				{
					transform.localScale = Vector3.one;
				}
				else
				{
					transform.localScale = new Vector3(-1f, 1f);
				}
			}
			return;
		}
		for (int i = 0; i < fishObj.Length; i++)
		{
			if (fishObj[i] != null)
			{
				fishObj[i].transform.localEulerAngles = vector3;
			}
		}
	}
}
