using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class STOF_LockCard : MonoBehaviour
{
	private Vector3 vec;

	private float rot;

	[SerializeField]
	private Image imgCard;

	[SerializeField]
	private Sprite[] spiSame;

	[SerializeField]
	private Sprite[] spiGroup;

	[SerializeField]
	private Sprite[] spiSingle;

	private void Update()
	{
		rot += 5f * Time.deltaTime;
		vec.x = 15f * Mathf.Cos(rot);
		vec.y = 15f * Mathf.Sin(rot);
		base.transform.localPosition = vec;
	}

	public void ShowLockCard(int seatid, bool bUp)
	{
		base.gameObject.SetActive(value: true);
		SetLockCard(seatid);
	}

	public void HideLockCard()
	{
		base.gameObject.SetActive(value: false);
	}

	private void SetLockCard(int seatid)
	{
		STOF_ISwimObj sTOF_ISwimObj = null;
		for (int i = 0; i < STOF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STOF_GameInfo.getInstance().UserList[i].SeatIndex == seatid)
			{
				sTOF_ISwimObj = STOF_FishPoolMngr.GetSingleton().GetSwimObjByTag(STOF_GameInfo.getInstance().UserList[i].LockFish);
			}
		}
		if (sTOF_ISwimObj == null)
		{
			UnityEngine.Debug.Log("swimobj is null at GetLockFish");
			return;
		}
		STOF_FISH_TYPE mFishType = sTOF_ISwimObj.mFishType;
		int num = (int)mFishType;
		switch (mFishType)
		{
		case STOF_FISH_TYPE.Fish_TYPE_NONE:
			break;
		case STOF_FISH_TYPE.Fish_Same_Shrimp:
		case STOF_FISH_TYPE.Fish_Same_Grass:
		case STOF_FISH_TYPE.Fish_Same_Zebra:
		case STOF_FISH_TYPE.Fish_Same_BigEars:
		case STOF_FISH_TYPE.Fish_Same_YellowSpot:
		case STOF_FISH_TYPE.Fish_Same_Ugly:
		case STOF_FISH_TYPE.Fish_Same_Hedgehog:
		case STOF_FISH_TYPE.Fish_Same_BlueAlgae:
		case STOF_FISH_TYPE.Fish_Same_Lamp:
		case STOF_FISH_TYPE.Fish_Same_Turtle:
			num -= 20;
			imgCard.sprite = spiSame[num];
			break;
		case STOF_FISH_TYPE.Fish_Turtle:
		case STOF_FISH_TYPE.Fish_Trailer:
		case STOF_FISH_TYPE.Fish_Butterfly:
		case STOF_FISH_TYPE.Fish_Beauty:
		case STOF_FISH_TYPE.Fish_Arrow:
		case STOF_FISH_TYPE.Fish_Bat:
		case STOF_FISH_TYPE.Fish_SilverShark:
		case STOF_FISH_TYPE.Fish_GoldenShark:
		case STOF_FISH_TYPE.Fish_GoldenSharkB:
		case STOF_FISH_TYPE.Fish_GoldenDragon:
			num -= 9;
			imgCard.sprite = spiSingle[num];
			break;
		case STOF_FISH_TYPE.Fish_Penguin:
			num -= 31;
			imgCard.sprite = spiSingle[num];
			break;
		case STOF_FISH_TYPE.Fish_Boss:
			num -= 8;
			imgCard.sprite = spiSingle[num];
			break;
		case STOF_FISH_TYPE.Fish_PartBomb:
			num -= 27;
			imgCard.sprite = spiSingle[num];
			break;
		case STOF_FISH_TYPE.Fish_FixBomb:
			num -= 18;
			imgCard.sprite = spiSingle[num];
			break;
		case STOF_FISH_TYPE.Fish_SuperBomb:
			num -= 16;
			imgCard.sprite = spiSingle[num];
			break;
		case STOF_FISH_TYPE.Fish_CoralReefs:
			num -= 17;
			imgCard.sprite = spiSingle[num];
			break;
		case STOF_FISH_TYPE.Fish_BigEars_Group:
		case STOF_FISH_TYPE.Fish_YellowSpot_Group:
		case STOF_FISH_TYPE.Fish_Hedgehog_Group:
		case STOF_FISH_TYPE.Fish_Ugly_Group:
		case STOF_FISH_TYPE.Fish_BlueAlgae_Group:
		case STOF_FISH_TYPE.Fish_Turtle_Group:
			num -= 33;
			imgCard.sprite = spiGroup[num];
			break;
		}
	}
}
