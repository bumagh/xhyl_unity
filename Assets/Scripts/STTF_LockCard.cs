using GameCommon;
using UnityEngine;
using UnityEngine.UI;

public class STTF_LockCard : MonoBehaviour
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
		STTF_ISwimObj sTTF_ISwimObj = null;
		for (int i = 0; i < STTF_GameInfo.getInstance().UserList.Count; i++)
		{
			if (STTF_GameInfo.getInstance().UserList[i].SeatIndex == seatid)
			{
				sTTF_ISwimObj = STTF_FishPoolMngr.GetSingleton().GetSwimObjByTag(STTF_GameInfo.getInstance().UserList[i].LockFish);
			}
		}
		if (sTTF_ISwimObj == null)
		{
			UnityEngine.Debug.Log("swimobj is null at GetLockFish");
			return;
		}
		STTF_FISH_TYPE mFishType = sTTF_ISwimObj.mFishType;
		int num = (int)mFishType;
		if (num >= 0 && num < spiLockFish.Length)
		{
			imgCardBg.sprite = spiLockFish[num];
		}
		else
		{
			UnityEngine.Debug.LogError("索引异常" + num);
		}
	}
}
