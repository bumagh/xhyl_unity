using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class STMF_CoralReefsFish : STMF_NormalFish
{
	public STMF_FISH_TYPE mRealFishType;

	protected override void _doSwimAnim()
	{
		anim.enabled = true;
	}

	public override void GoDie(object bulletPara)
	{
		if (mIsFishDead)
		{
			return;
		}
		mBoxCollider.enabled = false;
		mIsFishDead = true;
		GetComponent<STMF_HoMove>().Stop();
		if (mRealFishType < STMF_FISH_TYPE.Fish_TYPE_NONE && mRealFishType >= STMF_FISH_TYPE.Fish_Shrimp)
		{
			if (mRealFishType <= STMF_FISH_TYPE.Fish_SilverShark && mRealFishType >= STMF_FISH_TYPE.Fish_Turtle)
			{
				Transform transform = STMF_FishPoolMngr.GetSingleton().CreateFishForCoralReefs(mRealFishType);
				transform.tag = "ForCoralReefsDie";
				transform.position = transform.localPosition;
				transform.GetComponent<STMF_NormalFish>().GoDie(bulletPara);
				ObjDestroy();
				mRealFishType = STMF_FISH_TYPE.Fish_TYPE_NONE;
			}
			else
			{
				RecordError(isUp: true, (int)mRealFishType);
				UnityEngine.Debug.Log("@CoralReefsFish GoDie Error, with error mRealFishType1 : " + mRealFishType);
			}
		}
		else
		{
			RecordError(isUp: false, (int)mRealFishType);
			UnityEngine.Debug.Log("@CoralReefsFish GoDie Error, with error mRealFishType2 : " + mRealFishType);
		}
	}

	public void RecordError(bool isUp, int nType)
	{
		if (isUp)
		{
			Text component = GameObject.Find("LOGOWORDS").GetComponent<Text>();
			component.text = component.text + "+" + nType;
		}
		else
		{
			Text component2 = GameObject.Find("LOGOWORDS").GetComponent<Text>();
			component2.text = component2.text + "-" + nType;
		}
	}
}
