using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class TF_CoralReefsFish : TF_NormalFish
{
	public TF_FISH_TYPE mRealFishType;

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
		GetComponent<TF_DoMove>().Stop();
		GetComponent<TF_DoMove>().enabled = false;
		if (mRealFishType < TF_FISH_TYPE.Fish_TYPE_NONE && mRealFishType >= TF_FISH_TYPE.Fish_Shrimp)
		{
			if (mRealFishType <= TF_FISH_TYPE.Fish_SilverShark && mRealFishType >= TF_FISH_TYPE.Fish_Turtle)
			{
				Transform transform = TF_FishPoolMngr.GetSingleton().CreateFishForCoralReefs(mRealFishType);
				transform.tag = "ForCoralReefsDie";
				transform.position = base.transform.position;
				transform.GetComponent<TF_NormalFish>().GoDie(bulletPara);
				ObjDestroy();
				mRealFishType = TF_FISH_TYPE.Fish_TYPE_NONE;
			}
			else
			{
				mRealFishType = (TF_FISH_TYPE)Random.Range(9, 16);
				Transform transform2 = TF_FishPoolMngr.GetSingleton().CreateFishForCoralReefs(mRealFishType);
				transform2.tag = "ForCoralReefsDie";
				transform2.position = base.transform.position;
				transform2.GetComponent<TF_NormalFish>().GoDie(bulletPara);
				ObjDestroy();
				mRealFishType = TF_FISH_TYPE.Fish_TYPE_NONE;
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
