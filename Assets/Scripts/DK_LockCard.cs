using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class DK_LockCard : MonoBehaviour
{
	private Vector3 vec;

	private float rot;

	[SerializeField]
	private Image imgCardBg;

	[SerializeField]
	private Sprite[] spiCardBg;

	[SerializeField]
	private Image imgSingFish;

	[SerializeField]
	private Image imgSameFish;

	[SerializeField]
	private Image[] imgGroupFish3;

	[SerializeField]
	private Image[] imgGroupFish4;

	[SerializeField]
	private Image[] imgDoubleFish;

	[SerializeField]
	private Sprite[] spiFish;

	private void Update()
	{
		rot += 5f * Time.deltaTime;
		vec.x = 15f * Mathf.Cos(rot);
		vec.y = 15f * Mathf.Sin(rot);
		base.transform.localPosition = vec;
	}

	public void ShowLockCard(int seatid, bool bUp)
	{
		for (int i = 0; i < imgCardBg.transform.childCount; i++)
		{
			imgCardBg.transform.GetChild(i).gameObject.SetActive(value: false);
		}
		base.gameObject.SetActive(value: true);
		SetLockCard(seatid);
	}

	public void HideLockCard()
	{
		base.gameObject.SetActive(value: false);
	}

	private void SetLockCard(int seatid)
	{
		DK_ISwimObj dK_ISwimObj = null;
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].SeatIndex == seatid)
			{
				dK_ISwimObj = DK_FishPoolMngr.GetSingleton().GetSwimObjByTag(DK_GameInfo.getInstance().UserList[i].LockFish);
			}
		}
		if (dK_ISwimObj == null)
		{
			UnityEngine.Debug.Log("swimobj is null at GetLockFish");
			return;
		}
		DK_FISH_TYPE mFishType = dK_ISwimObj.mFishType;
		int num = (int)mFishType;
		switch (mFishType)
		{
		case DK_FISH_TYPE.Fish_Turtle:
			imgCardBg.sprite = spiCardBg[2];
			break;
		case DK_FISH_TYPE.Fish_Trailer:
		case DK_FISH_TYPE.Fish_Butterfly:
		case DK_FISH_TYPE.Fish_Beauty:
		case DK_FISH_TYPE.Fish_Arrow:
		case DK_FISH_TYPE.Fish_Bat:
		case DK_FISH_TYPE.Fish_SilverShark:
		case DK_FISH_TYPE.Fish_GoldenShark:
		case DK_FISH_TYPE.Fish_BigRed:
		case DK_FISH_TYPE.Fish_BigShark:
		case DK_FISH_TYPE.Fish_NiuMoWang:
			imgCardBg.sprite = spiCardBg[0];
			imgSingFish.sprite = spiFish[num];
			imgSingFish.SetNativeSize();
			imgCardBg.transform.GetChild(0).gameObject.SetActive(value: true);
			break;
		default:
			if (mFishType >= DK_FISH_TYPE.Fish_SuperBomb && mFishType <= DK_FISH_TYPE.Fish_CoralReefs)
			{
				imgCardBg.sprite = spiCardBg[0];
				imgSingFish.sprite = spiFish[num - 10];
				imgSingFish.SetNativeSize();
				imgCardBg.transform.GetChild(0).gameObject.SetActive(value: true);
			}
			else if (mFishType >= DK_FISH_TYPE.Fish_Same_Shrimp && mFishType <= DK_FISH_TYPE.Fish_Same_Turtle)
			{
				imgCardBg.sprite = spiCardBg[1];
				imgSameFish.sprite = spiFish[num - 20];
				imgSameFish.SetNativeSize();
				imgCardBg.transform.GetChild(1).gameObject.SetActive(value: true);
			}
			else if (mFishType >= DK_FISH_TYPE.Fish_BigEars_Group && mFishType <= DK_FISH_TYPE.Fish_Hedgehog_Group)
			{
				imgCardBg.sprite = spiCardBg[0];
				for (int j = 0; j < 3; j++)
				{
					imgGroupFish3[j].sprite = spiFish[GetIndex(mFishType)];
					imgGroupFish3[j].SetNativeSize();
				}
				imgCardBg.transform.GetChild(2).gameObject.SetActive(value: true);
			}
			else if (mFishType >= DK_FISH_TYPE.Fish_Ugly_Group && mFishType <= DK_FISH_TYPE.Fish_Turtle_Group)
			{
				imgCardBg.sprite = spiCardBg[0];
				for (int k = 0; k < 4; k++)
				{
					imgGroupFish4[k].sprite = spiFish[GetIndex(mFishType)];
					imgGroupFish4[k].SetNativeSize();
				}
				imgCardBg.transform.GetChild(3).gameObject.SetActive(value: true);
			}
			else if (mFishType == DK_FISH_TYPE.Fish_Double_Kill)
			{
				imgCardBg.sprite = spiCardBg[1];
				imgDoubleFish[0].sprite = spiFish[(int)dK_ISwimObj.mFirstFishType];
				imgDoubleFish[0].SetNativeSize();
				imgDoubleFish[0].transform.localScale = Vector3.one * GetScale(dK_ISwimObj.mFirstFishType);
				imgDoubleFish[1].sprite = spiFish[(int)dK_ISwimObj.mSecondFishType];
				imgDoubleFish[1].SetNativeSize();
				imgDoubleFish[1].transform.localScale = Vector3.one * GetScale(dK_ISwimObj.mSecondFishType);
				imgCardBg.transform.GetChild(4).gameObject.SetActive(value: true);
			}
			break;
		}
		imgCardBg.SetNativeSize();
	}

	private float GetScale(DK_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case DK_FISH_TYPE.Fish_Shrimp:
			return 1f;
		case DK_FISH_TYPE.Fish_Grass:
			return 1f;
		case DK_FISH_TYPE.Fish_Zebra:
			return 0.8f;
		case DK_FISH_TYPE.Fish_BigEars:
			return 0.8f;
		case DK_FISH_TYPE.Fish_YellowSpot:
			return 0.8f;
		case DK_FISH_TYPE.Fish_Ugly:
			return 0.8f;
		case DK_FISH_TYPE.Fish_Hedgehog:
			return 0.8f;
		case DK_FISH_TYPE.Fish_BlueAlgae:
			return 0.8f;
		case DK_FISH_TYPE.Fish_Lamp:
			return 0.8f;
		case DK_FISH_TYPE.Fish_Turtle:
			return 0.6f;
		default:
			return 1f;
		}
	}

	private int GetIndex(DK_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case DK_FISH_TYPE.Fish_BigEars_Group:
			return 3;
		case DK_FISH_TYPE.Fish_YellowSpot_Group:
			return 4;
		case DK_FISH_TYPE.Fish_Hedgehog_Group:
			return 6;
		case DK_FISH_TYPE.Fish_Ugly_Group:
			return 5;
		case DK_FISH_TYPE.Fish_BlueAlgae_Group:
			return 7;
		case DK_FISH_TYPE.Fish_Turtle_Group:
			return 9;
		default:
			return 0;
		}
	}
}
