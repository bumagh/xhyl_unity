using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Message;
using UIFrameWork;
using UnityEngine;

public class RoomMgr : BaseUIForm
{
	public static RoomMgr instance;

	private float _lastEnterRoomTime = -2f;

	private int roomId = -1;

	public int RoomId => roomId;

	public static RoomMgr GetInstance()
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
		uiType.uiFormType = UIFormTypes.Normal;
		HW2_MessageCenter.RegisterHandle("RoomMove", RoomMoveAni);
	}

	public void SelectRoom(int type)
	{
		HW2_Singleton<SoundMgr>.Get().PlayClip("选座选厅自动发炮");
		HW2_Singleton<SoundMgr>.Get().SetVolume("选座选厅自动发炮", 1f);
		if (Time.realtimeSinceStartup - _lastEnterRoomTime < 2f)
		{
			UnityEngine.Debug.LogError("点击过快!");
		}
		_lastEnterRoomTime = Time.realtimeSinceStartup;
		roomId = type;
		HW2_MB_Singleton<HW2_NetManager>.Get().Send("roomService/enterRoom", new object[1]
		{
			roomId
		});
	}

	public void RoomMoveAni(KeyValueInfo keyValueInfo)
	{
	}
}
