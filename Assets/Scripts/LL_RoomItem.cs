using LL_UICommon;
using UnityEngine;

public class LL_RoomItem : MonoBehaviour
{
	protected LL_PersonInfo mUserInfo;

	public int mRoomId;

	public int RoomId
	{
		get
		{
			return mRoomId;
		}
		set
		{
			mRoomId = value;
		}
	}

	public bool IsEnabled
	{
		get
		{
			Collider component = GetComponent<Collider>();
			return (bool)component && component.enabled;
		}
		set
		{
			Collider component = GetComponent<Collider>();
			if ((bool)component && component.enabled != value)
			{
				component.enabled = value;
			}
		}
	}

	private void Start()
	{
		mUserInfo = LL_GameInfo.getInstance().UserInfo;
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.CoinOverFlow, string.Empty);
	}
}
