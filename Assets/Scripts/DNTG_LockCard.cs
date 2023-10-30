using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_LockCard : MonoBehaviour
{
	private Vector3 vec;

	private float rot;

	private Image imgCard;

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
		DNTG_ISwimObj dNTG_ISwimObj = null;
		for (int i = 0; i < DNTG_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DNTG_GameInfo.getInstance().UserList[i].SeatIndex == seatid)
			{
				dNTG_ISwimObj = DNTG_FishPoolMngr.GetSingleton().GetSwimObjByTag(DNTG_GameInfo.getInstance().UserList[i].LockFish);
			}
		}
		if (dNTG_ISwimObj == null)
		{
			UnityEngine.Debug.Log(seatid + " 找不到被锁的鱼");
			return;
		}
		DNTG_FISH_TYPE mFishType = dNTG_ISwimObj.mFishType;
		SpecialFishType specialFishType = dNTG_ISwimObj.specialFishType;
		int num = (int)mFishType;
		num -= 9;
		switch (mFishType)
		{
		case DNTG_FISH_TYPE.Fish_Penguin:
		case DNTG_FISH_TYPE.Fish_SilverDragon:
		case DNTG_FISH_TYPE.Fish_GoldenDragon:
		case DNTG_FISH_TYPE.Fish_GoldDolphin:
		case DNTG_FISH_TYPE.Fish_Octopus:
		case DNTG_FISH_TYPE.Fish_Mermaid:
		case DNTG_FISH_TYPE.Fish_Sailboat:
			num += 16;
			break;
		}
		switch (specialFishType)
		{
		case SpecialFishType.LightningFish:
			num += 39;
			break;
		case SpecialFishType.HeavenFish:
			num += 52;
			break;
		}
		if (num >= spiSingle.Length || num < 0)
		{
			UnityEngine.Debug.LogError(dNTG_ISwimObj.name + " 索引越界: " + num + "  " + spiSingle.Length);
			HideLockCard();
			return;
		}
		if (imgCard == null)
		{
			imgCard = base.transform.GetChild(0).GetComponent<Image>();
		}
		imgCard.sprite = spiSingle[num];
	}
}
