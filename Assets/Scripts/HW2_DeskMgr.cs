using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Demo;
using M__M.HaiWang.Message;
using System;
using System.Collections;
using UIFrameWork;
using UnityEngine;

public class HW2_DeskMgr : BaseUIForm
{
	public GameObject desk;

	public GameObject mask;

	public GameObject Fewer;

	private static HW2_DeskMgr instance;

	private float _lastRequestSeat = -2f;

	public static HW2_DeskMgr Get()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
		uiType.uiFormType = UIFormTypes.Normal;
		HW2_MessageCenter.RegisterHandle("ShowDeskAni", ShowDeskAni);
		HW2_MessageCenter.RegisterHandle("ReshDeskInfo", ReshDeskInfo);
	}

	private void OnDisable()
	{
	}

	private void ShowDeskAni(KeyValueInfo keyValueInfo)
	{
		int num = Convert.ToInt32(keyValueInfo._value);
	}

	private void ReshDeskInfo(KeyValueInfo keyValueInfo)
	{
		desk.GetComponent<DeskController>().UpdateUI_RoomInfoPush(0, HW2_GVars.lobby.curRoomId, SeatBeClickCall);
		Fewer.GetComponent<TableController>().InitTableList(RefreshDeskShow);
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
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("roomService/requestSeat", new object[2]
			{
				HW2_GVars.lobby.curDeskId,
				HW2_GVars.lobby.curSeatId
			});
		}
	}

	public void SeatBeClickCall(int curSeatId, int curDeskId)
	{
		if (Time.realtimeSinceStartup - _lastRequestSeat < 2f)
		{
			UnityEngine.Debug.LogError("进入桌子过快!");
			return;
		}
		HW2_GVars.lobby.curDeskId = curDeskId;
		HW2_GVars.lobby.curSeatId = curSeatId;
		_lastRequestSeat = Time.realtimeSinceStartup;
		mask.gameObject.SetActive(value: true);
		StartCoroutine(DelayCall(2f, delegate
		{
			mask.gameObject.SetActive(value: false);
		}));
		HW2_MB_Singleton<HW2_NetManager>.Get().Send("roomService/requestSeat", new object[2]
		{
			HW2_GVars.lobby.curDeskId,
			HW2_GVars.lobby.curSeatId
		});
	}

	private void RefreshDeskShow(int index)
	{
		desk.GetComponent<DeskController>().UpdateUI_Click(index);
	}

	private IEnumerator DelayCall(float delay, Action call)
	{
		yield return new WaitForSeconds(delay);
		call();
	}
}
