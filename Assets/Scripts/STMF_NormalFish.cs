using GameCommon;
using UnityEngine;

public class STMF_NormalFish : STMF_ISwimObj
{
	protected GameObject _childObj;

	private SpriteRenderer srChild;

	protected Animator anim;

	protected Quaternion _parentRotation;

	protected Quaternion _childRotation;

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
		anim.enabled = true;
	}

	protected override void _doDieAnim()
	{
		anim.SetBool("bDeath", value: true);
		srChild.sortingOrder = 643;
	}

	protected override void _onDestroy()
	{
		base.transform.rotation = _parentRotation;
		_childObj.transform.rotation = _childRotation;
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
		srChild.sortingOrder = num2 - fishIndexInLayer;
		if (mFishType <= STMF_FISH_TYPE.Fish_Same_Turtle && mFishType >= STMF_FISH_TYPE.Fish_Same_Shrimp)
		{
			base.transform.Find("DriftBottle").gameObject.SetActive(value: true);
			base.transform.Find("DriftBottle").GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer - 2;
			srChild.sortingOrder = num2 - fishIndexInLayer - 1;
		}
		if (mFishType <= STMF_FISH_TYPE.Fish_Turtle && mFishType >= STMF_FISH_TYPE.Fish_Shrimp)
		{
			base.transform.Find("DriftBottle").gameObject.SetActive(value: false);
		}
	}

	protected override void _setCollider()
	{
		if (mFishType <= STMF_FISH_TYPE.Fish_Same_Turtle && mFishType >= STMF_FISH_TYPE.Fish_Same_Shrimp)
		{
			mBoxCollider = base.transform.Find("DriftBottle").GetComponent<BoxCollider2D>();
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
			_childObj.transform.localEulerAngles = Vector3.up * 270f + Vector3.left * 180f;
		}
		else
		{
			_childObj.transform.localEulerAngles = Vector3.up * 270f;
		}
	}
}
