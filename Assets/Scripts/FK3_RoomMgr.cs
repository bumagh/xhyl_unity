using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Message;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class FK3_RoomMgr : FK3_BaseUIForm
{
	public static FK3_RoomMgr instance;

	private float _lastEnterRoomTime = -2f;

	private int roomId = -1;

	private int hallId = -1;

	private Transform scrollView;

	private Transform viewport;

	public int RoomId
	{
		get
		{
			return roomId;
		}
		set
		{
			roomId = value;
		}
	}

	public int HallId
	{
		get
		{
			return hallId;
		}
		set
		{
			hallId = value;
		}
	}

	public static FK3_RoomMgr GetInstance()
	{
		return instance;
	}

	public void OnQuit()
	{
		instance = null;
	}

	private void Awake()
	{
		instance = this;
		uiType.uiFormType = FK3_UIFormTypes.Normal;
		FK3_MessageCenter.RegisterHandle("RoomMove", RoomMoveAni);
		scrollView = base.transform.Find("Scroll View");
		viewport = scrollView.Find("Viewport");
		scrollView.GetComponent<Image>().raycastTarget = false;
		viewport.GetComponent<Image>().raycastTarget = false;
	}

	public void SendEnterHall(int type)
	{
		_lastEnterRoomTime = Time.realtimeSinceStartup;
		hallId = type;
		FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/selectHall", new object[1]
		{
			hallId
		});
	}

	public void SelectRoom(int type)
	{
		FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("选座选厅自动发炮");
		FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("选座选厅自动发炮", 1f);
		if (Time.realtimeSinceStartup - _lastEnterRoomTime < 2f)
		{
			UnityEngine.Debug.LogError("点击过快!");
		}
		_lastEnterRoomTime = Time.realtimeSinceStartup;
		roomId = type;
		FK3_MB_Singleton<FK3_NetManager>.Get().Send("roomService/enterRoom", new object[1]
		{
			roomId
		});
	}

	public void RoomMoveAni(FK3_KeyValueInfo keyValueInfo)
	{
	}
}
