using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class DK_CoralReefsFish : DK_NormalFish
{
	public DK_FISH_TYPE mRealFishType;

	protected override void _doSwimAnim()
	{
		anim.enabled = true;
	}

	public override void GoDie2(object bulletPara)
	{
		if (bFishDead)
		{
			return;
		}
		mBoxCollider.enabled = false;
		bFishDead = true;
		GetComponent<DK_DoMove>().Stop();
		GetComponent<DK_DoMove>().enabled = false;
		if (mRealFishType < DK_FISH_TYPE.Fish_TYPE_NONE && mRealFishType >= DK_FISH_TYPE.Fish_Shrimp)
		{
			if (mRealFishType <= DK_FISH_TYPE.Fish_SilverShark && mRealFishType >= DK_FISH_TYPE.Fish_Turtle)
			{
				Transform transform = DK_FishPoolMngr.GetSingleton().CreateFishForCoralReefs(mRealFishType);
				transform.tag = "ForCoralReefsDie";
				transform.position = base.transform.position;
				transform.GetComponent<DK_NormalFish>().GoDie2(bulletPara);
				ObjDestroy();
				mRealFishType = DK_FISH_TYPE.Fish_TYPE_NONE;
			}
			else
			{
				mRealFishType = (DK_FISH_TYPE)Random.Range(9, 16);
				Transform transform2 = DK_FishPoolMngr.GetSingleton().CreateFishForCoralReefs(mRealFishType);
				transform2.tag = "ForCoralReefsDie";
				transform2.position = base.transform.position;
				transform2.GetComponent<DK_NormalFish>().GoDie2(bulletPara);
				ObjDestroy();
				mRealFishType = DK_FISH_TYPE.Fish_TYPE_NONE;
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
