using GameCommon;
using UnityEngine;

public class DK_NormalFish : DK_ISwimObj
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
		if (mFishType == DK_FISH_TYPE.Fish_NiuMoWang)
		{
			_childObj.transform.Find("nmw_other_niumowang_bg").gameObject.SetActive(value: true);
		}
	}

	protected override void _doDieAnim()
	{
		if (anim != null)
		{
			if (mFishType != DK_FISH_TYPE.Fish_CoralReefs || mFishType != DK_FISH_TYPE.Fish_FixBomb || mFishType != DK_FISH_TYPE.Fish_SuperBomb)
			{
				anim.SetBool("bDeath", value: true);
			}
			else
			{
				anim.enabled = false;
			}
		}
		if (mFishType == DK_FISH_TYPE.Fish_NiuMoWang)
		{
			_childObj.transform.Find("nmw_other_niumowang_bg").gameObject.SetActive(value: false);
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
		if (mFishType >= DK_FISH_TYPE.Fish_Shrimp && mFishType <= DK_FISH_TYPE.Fish_NiuMoWang)
		{
			num = (int)mFishType * 10;
		}
		else if (mFishType >= DK_FISH_TYPE.Fish_Same_Shrimp && mFishType <= DK_FISH_TYPE.Fish_Same_Turtle)
		{
			num = 200 + (int)(mFishType - 20) * 30;
		}
		else if (mFishType >= DK_FISH_TYPE.Fish_FixBomb && mFishType <= DK_FISH_TYPE.Fish_CoralReefs)
		{
			num = 500 + (int)(mFishType - 31) * 10;
		}
		else if (mFishType >= DK_FISH_TYPE.Fish_YellowSpot_Group && mFishType <= DK_FISH_TYPE.Fish_Turtle_Group)
		{
			num = 540 + (int)(mFishType - 29) * 40;
		}
		int num2 = 840 - num;
		int fishIndexInLayer = DK_FishPoolMngr.GetSingleton().GetFishIndexInLayer(mFishType);
		_childObj.gameObject.SetActive(value: true);
		srChild.sortingOrder = num2 - fishIndexInLayer;
		layer = num2 - fishIndexInLayer;
		if (mFishType <= DK_FISH_TYPE.Fish_Same_Turtle && mFishType >= DK_FISH_TYPE.Fish_Same_Shrimp)
		{
			if (objDriftBottle == null)
			{
				objDriftBottle = base.transform.Find("DriftBottle").gameObject;
			}
			objDriftBottle.SetActive(value: true);
			objDriftBottle.GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer - 2;
			srChild.sortingOrder = num2 - fishIndexInLayer - 1;
		}
		if (mFishType <= DK_FISH_TYPE.Fish_Turtle && mFishType >= DK_FISH_TYPE.Fish_Shrimp)
		{
			if (objDriftBottle == null)
			{
				objDriftBottle = base.transform.Find("DriftBottle").gameObject;
			}
			objDriftBottle.gameObject.SetActive(value: false);
		}
	}

	protected override void _setCollider()
	{
		if (mFishType <= DK_FISH_TYPE.Fish_Same_Turtle && mFishType >= DK_FISH_TYPE.Fish_Same_Shrimp)
		{
			if (objDriftBottle == null)
			{
				objDriftBottle = base.transform.Find("DriftBottle").gameObject;
			}
			mBoxCollider = objDriftBottle.GetComponent<BoxCollider2D>();
		}
		else
		{
			mBoxCollider = _childObj.GetComponent<BoxCollider2D>();
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
