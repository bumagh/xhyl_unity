using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class STTF_CoralReefsFish : STTF_NormalFish
{
	public STTF_FISH_TYPE mRealFishType;

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
		GetComponent<STTF_DoMove>().Stop();
		GetComponent<STTF_DoMove>().enabled = false;
		if ((mRealFishType < STTF_FISH_TYPE.Fish_TYPE_NONE && mRealFishType >= STTF_FISH_TYPE.Fish_Shrimp) || mRealFishType == STTF_FISH_TYPE.Fish_Boss)
		{
			switch (mRealFishType)
			{
			case STTF_FISH_TYPE.Fish_Turtle:
			case STTF_FISH_TYPE.Fish_Trailer:
			case STTF_FISH_TYPE.Fish_Butterfly:
			case STTF_FISH_TYPE.Fish_Beauty:
			case STTF_FISH_TYPE.Fish_Arrow:
			case STTF_FISH_TYPE.Fish_Bat:
			case STTF_FISH_TYPE.Fish_SilverShark:
			{
				Transform transform2 = STTF_FishPoolMngr.GetSingleton().CreateFishForCoralReefs(mRealFishType);
				transform2.tag = "ForCoralReefsDie";
				transform2.position = base.transform.position;
				transform2.GetComponent<STTF_NormalFish>().GoDie(bulletPara);
				ObjDestroy();
				mRealFishType = STTF_FISH_TYPE.Fish_TYPE_NONE;
				break;
			}
			default:
			{
				mRealFishType = (STTF_FISH_TYPE)Random.Range(9, 16);
				Transform transform = STTF_FishPoolMngr.GetSingleton().CreateFishForCoralReefs(mRealFishType);
				transform.tag = "ForCoralReefsDie";
				transform.position = base.transform.position;
				transform.GetComponent<STTF_NormalFish>().GoDie(bulletPara);
				ObjDestroy();
				mRealFishType = STTF_FISH_TYPE.Fish_TYPE_NONE;
				break;
			}
			}
		}
		else
		{
			UnityEngine.Debug.Log("有这条鱼吗 瞎搞: " + mRealFishType);
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
