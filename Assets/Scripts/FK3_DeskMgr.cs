using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Demo;
using M__M.HaiWang.Message;
using System;
using System.Collections;
using UIFrameWork;
using UnityEngine;

public class FK3_DeskMgr : FK3_BaseUIForm
{
	public GameObject desk;

	public GameObject mask;

	public GameObject Fewer;

	private static FK3_DeskMgr instance;

	private float _lastRequestSeat = -2f;

	public static FK3_DeskMgr Get()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
		uiType.uiFormType = FK3_UIFormTypes.Normal;
		FK3_MessageCenter.RegisterHandle("ShowDeskAni", ShowDeskAni);
		FK3_MessageCenter.RegisterHandle("ReshDeskInfo", ReshDeskInfo);
	}

	private void OnDisable()
	{
	}

	private void ShowDeskAni(FK3_KeyValueInfo keyValueInfo)
	{
		int num = Convert.ToInt32(keyValueInfo._value);
	}

	private void ReshDeskInfo(FK3_KeyValueInfo keyValueInfo)
	{
		desk.GetComponent<FK3_DeskController>().UpdateUI_RoomInfoPush(0, FK3_GVars.lobby.curRoomId, SeatBeClickCall);
		Fewer.GetComponent<FK3_TableController>().InitTableList(RefreshDeskShow);
	}

	private void SeatBeClickCall()
	{
		if (!(Time.realtimeSinceStartup - _lastRequestSeat < 2f))
		{
			_lastRequestSeat = Time.realtimeSinceStartup;
			mask.gameObject.SetActive(value: true);
			StartCoroutine(DelayCall(2f, delegate
			{
				mask.gameObject.SetActive(value: false);
			}));
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("roomService/requestSeat", new object[2]
			{
				FK3_GVars.lobby.curDeskId,
				FK3_GVars.lobby.curSeatId
			});
		}
	}

	public void SeatBeClickCall(int roomId, int curSeatId, int curDeskId)
	{
		if (Time.realtimeSinceStartup - _lastRequestSeat < 2f)
		{
			UnityEngine.Debug.LogError("进入桌子过快!");
			return;
		}
		FK3_GVars.lobby.curDeskId = curDeskId;
		FK3_GVars.lobby.curSeatId = curSeatId;
		_lastRequestSeat = Time.realtimeSinceStartup;
		mask.gameObject.SetActive(value: true);
		StartCoroutine(DelayCall(2f, delegate
		{
			mask.gameObject.SetActive(value: false);
		}));
		FK3_MB_Singleton<FK3_NetManager>.Get().Send("roomService/enterRoom", new object[1]
		{
			roomId
		});
		FK3_MB_Singleton<FK3_NetManager>.Get().Send("roomService/requestSeat", new object[2]
		{
			FK3_GVars.lobby.curDeskId,
			FK3_GVars.lobby.curSeatId
		});
	}

	private void RefreshDeskShow(int index)
	{
		desk.GetComponent<FK3_DeskController>().UpdateUI_Click(index);
	}

	private IEnumerator DelayCall(float delay, Action call)
	{
		yield return new WaitForSeconds(delay);
		call();
	}
}
