using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class STQM_CoralReefsFish : STQM_NormalFish
{
	public STQM_FISH_TYPE mRealFishType;

	protected override void _doSwimAnim()
	{
		anim.enabled = true;
	}

	public override void GoDie(object bulletPara)
	{
		if (bFishDead)
		{
			return;
		}
		mBoxCollider.enabled = false;
		bFishDead = true;
		GetComponent<STQM_DoMove>().Stop();
		GetComponent<STQM_DoMove>().enabled = false;
		if (mRealFishType < STQM_FISH_TYPE.Fish_TYPE_NONE && mRealFishType >= STQM_FISH_TYPE.Fish_Shrimp)
		{
			if (mRealFishType <= STQM_FISH_TYPE.Fish_SilverShark && mRealFishType >= STQM_FISH_TYPE.Fish_Turtle)
			{
				Transform transform = STQM_FishPoolMngr.GetSingleton().CreateFishForCoralReefs(mRealFishType);
				transform.tag = "ForCoralReefsDie";
				transform.position = base.transform.position;
				transform.GetComponent<STQM_NormalFish>().GoDie(bulletPara);
				ObjDestroy();
				mRealFishType = STQM_FISH_TYPE.Fish_TYPE_NONE;
			}
			else
			{
				mRealFishType = (STQM_FISH_TYPE)Random.Range(9, 16);
				Transform transform2 = STQM_FishPoolMngr.GetSingleton().CreateFishForCoralReefs(mRealFishType);
				transform2.tag = "ForCoralReefsDie";
				transform2.position = base.transform.position;
				transform2.GetComponent<STQM_NormalFish>().GoDie(bulletPara);
				ObjDestroy();
				mRealFishType = STQM_FISH_TYPE.Fish_TYPE_NONE;
			}
		}
		else
		{
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
