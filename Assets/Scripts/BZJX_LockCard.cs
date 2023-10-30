using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class BZJX_LockCard : MonoBehaviour
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
		BZJX_ISwimObj bZJX_ISwimObj = null;
		for (int i = 0; i < BZJX_GameInfo.getInstance().UserList.Count; i++)
		{
			if (BZJX_GameInfo.getInstance().UserList[i].SeatIndex == seatid)
			{
				bZJX_ISwimObj = BZJX_FishPoolMngr.GetSingleton().GetSwimObjByTag(BZJX_GameInfo.getInstance().UserList[i].LockFish);
			}
		}
		if (bZJX_ISwimObj == null)
		{
			UnityEngine.Debug.Log("swimobj is null at GetLockFish");
			return;
		}
		BZJX_FISH_TYPE mFishType = bZJX_ISwimObj.mFishType;
		int num = (int)mFishType;
		switch (mFishType)
		{
		case BZJX_FISH_TYPE.Fish_TYPE_NONE:
			break;
		case BZJX_FISH_TYPE.Fish_Same_Shrimp:
		case BZJX_FISH_TYPE.Fish_Same_Grass:
		case BZJX_FISH_TYPE.Fish_Same_Zebra:
		case BZJX_FISH_TYPE.Fish_Same_BigEars:
		case BZJX_FISH_TYPE.Fish_Same_YellowSpot:
		case BZJX_FISH_TYPE.Fish_Same_Ugly:
		case BZJX_FISH_TYPE.Fish_Same_Hedgehog:
		case BZJX_FISH_TYPE.Fish_Same_BlueAlgae:
		case BZJX_FISH_TYPE.Fish_Same_Lamp:
		case BZJX_FISH_TYPE.Fish_Same_Turtle:
			num -= 20;
			imgCard.sprite = spiSame[num];
			break;
		case BZJX_FISH_TYPE.Fish_Turtle:
		case BZJX_FISH_TYPE.Fish_Trailer:
		case BZJX_FISH_TYPE.Fish_Butterfly:
		case BZJX_FISH_TYPE.Fish_Beauty:
		case BZJX_FISH_TYPE.Fish_Arrow:
		case BZJX_FISH_TYPE.Fish_Bat:
		case BZJX_FISH_TYPE.Fish_SilverShark:
		case BZJX_FISH_TYPE.Fish_GoldenShark:
		case BZJX_FISH_TYPE.Fish_GoldenSharkB:
		case BZJX_FISH_TYPE.Fish_GoldenDragon:
			num -= 9;
			imgCard.sprite = spiSingle[num];
			break;
		case BZJX_FISH_TYPE.Fish_Penguin:
			num -= 31;
			imgCard.sprite = spiSingle[num];
			break;
		case BZJX_FISH_TYPE.Fish_Boss:
			num -= 8;
			imgCard.sprite = spiSingle[num];
			break;
		case BZJX_FISH_TYPE.Fish_PartBomb:
			num -= 27;
			imgCard.sprite = spiSingle[num];
			break;
		case BZJX_FISH_TYPE.Fish_FixBomb:
			num -= 18;
			imgCard.sprite = spiSingle[num];
			break;
		case BZJX_FISH_TYPE.Fish_SuperBomb:
			num -= 16;
			imgCard.sprite = spiSingle[num];
			break;
		case BZJX_FISH_TYPE.Fish_CoralReefs:
			num -= 17;
			imgCard.sprite = spiSingle[num];
			break;
		case BZJX_FISH_TYPE.Fish_BigEars_Group:
		case BZJX_FISH_TYPE.Fish_YellowSpot_Group:
		case BZJX_FISH_TYPE.Fish_Hedgehog_Group:
		case BZJX_FISH_TYPE.Fish_Ugly_Group:
		case BZJX_FISH_TYPE.Fish_BlueAlgae_Group:
		case BZJX_FISH_TYPE.Fish_Turtle_Group:
			num -= 33;
			imgCard.sprite = spiGroup[num];
			break;
		}
	}
}
