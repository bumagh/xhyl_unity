using GameCommon;
using UnityEngine;

public class STQM_NormalFish : STQM_ISwimObj
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
			if (mFishType != STQM_FISH_TYPE.Fish_CoralReefs || mFishType != STQM_FISH_TYPE.Fish_AllBomb)
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
		if (mFishType >= STQM_FISH_TYPE.Fish_Shrimp && mFishType <= STQM_FISH_TYPE.Fish_BuleWhale)
		{
			num = (int)mFishType * 10;
		}
		else if (mFishType >= STQM_FISH_TYPE.Fish_Same_Shrimp && mFishType <= STQM_FISH_TYPE.Fish_Same_Turtle)
		{
			num = 190 + (int)(mFishType - 20) * 30;
		}
		else if (mFishType >= STQM_FISH_TYPE.Fish_AllBomb && mFishType < STQM_FISH_TYPE.Fish_BowlFish)
		{
			num = 490 + (int)(mFishType - 31) * 10;
		}
		else if (mFishType == STQM_FISH_TYPE.Fish_BowlFish)
		{
			num = 510 + (int)(mFishType - 29) * 10;
		}
		int num2 = 640 - num;
		int fishIndexInLayer = STQM_FishPoolMngr.GetSingleton().GetFishIndexInLayer(mFishType);
		_childObj.gameObject.SetActive(value: true);
		srChild.sortingOrder = num2 - fishIndexInLayer;
		layer = num2 - fishIndexInLayer;
		if (mFishType <= STQM_FISH_TYPE.Fish_Same_Turtle && mFishType >= STQM_FISH_TYPE.Fish_Same_Shrimp)
		{
			if (objDriftBottle == null)
			{
				objDriftBottle = base.transform.Find("DriftBottle").gameObject;
			}
			objDriftBottle.SetActive(value: true);
			objDriftBottle.GetComponent<SpriteRenderer>().sortingOrder = num2 - fishIndexInLayer - 2;
			srChild.sortingOrder = num2 - fishIndexInLayer - 1;
		}
		if (mFishType <= STQM_FISH_TYPE.Fish_Turtle && mFishType >= STQM_FISH_TYPE.Fish_Shrimp)
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
		if (mFishType <= STQM_FISH_TYPE.Fish_Same_Turtle && mFishType >= STQM_FISH_TYPE.Fish_Same_Shrimp)
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
