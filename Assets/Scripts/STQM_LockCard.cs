using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class STQM_LockCard : MonoBehaviour
{
	private Vector3 vec;

	private float rot;

	[SerializeField]
	private Image imgCardBg;

	public void ShowLockCard(int seatid, bool bUp)
	{
		if (imgCardBg != null)
		{
			for (int i = 0; i < imgCardBg.transform.childCount; i++)
			{
				imgCardBg.transform.GetChild(i).gameObject.SetActive(value: false);
			}
			base.gameObject.SetActive(value: true);
			SetLockCard(seatid);
		}
	}

	public void HideLockCard()
	{
		base.gameObject.SetActive(value: false);
	}

	private void SetLockCard(int seatid)
	{
	}

	private float GetScale(STQM_FISH_TYPE fishType)
	{
		switch (fishType)
		{
		case STQM_FISH_TYPE.Fish_Shrimp:
			return 1f;
		case STQM_FISH_TYPE.Fish_Grass:
			return 1f;
		case STQM_FISH_TYPE.Fish_Zebra:
			return 0.8f;
		case STQM_FISH_TYPE.Fish_BigEars:
			return 0.8f;
		case STQM_FISH_TYPE.Fish_YellowSpot:
			return 0.8f;
		case STQM_FISH_TYPE.Fish_Ugly:
			return 0.8f;
		case STQM_FISH_TYPE.Fish_Hedgehog:
			return 0.8f;
		case STQM_FISH_TYPE.Fish_BlueAlgae:
			return 0.8f;
		case STQM_FISH_TYPE.Fish_Lamp:
			return 0.8f;
		case STQM_FISH_TYPE.Fish_Turtle:
			return 0.6f;
		default:
			return 1f;
		}
	}

	private int GetIndex(STQM_FISH_TYPE fishType)
	{
		return 0;
	}
}
