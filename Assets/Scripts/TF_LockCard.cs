using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class TF_LockCard : MonoBehaviour
{
	private Vector3 vec;

	private float rot;

	[SerializeField]
	private Image imgCardBg;

	[SerializeField]
	private Sprite[] spiLockFish;

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
		SetLockCard(seatid);
		base.gameObject.SetActive(value: true);
	}

	public void HideLockCard()
	{
		base.gameObject.SetActive(value: false);
	}

	private void SetLockCard(int seatid)
	{
		TF_ISwimObj tF_ISwimObj = null;
		for (int i = 0; i < TF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (TF_GameInfo.getInstance().UserList[i].SeatIndex == seatid)
			{
				tF_ISwimObj = TF_FishPoolMngr.GetSingleton().GetSwimObjByTag(TF_GameInfo.getInstance().UserList[i].LockFish);
			}
		}
		if (tF_ISwimObj == null)
		{
			UnityEngine.Debug.Log("swimobj is null at GetLockFish");
			return;
		}
		TF_FISH_TYPE mFishType = tF_ISwimObj.mFishType;
		int num = (int)mFishType;
		if (num >= 22)
		{
			num -= 2;
		}
		imgCardBg.sprite = spiLockFish[num];
	}
}
