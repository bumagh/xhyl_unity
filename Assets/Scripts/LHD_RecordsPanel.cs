using System;
using UnityEngine;
using UnityEngine.UI;

public class LHD_RecordsPanel : MonoBehaviour
{
	public Button CloseBtn;

	public Text LongCount;

	public Text HuCount;

	public Text HeCount;

	public Text AllCount;

	public Text LongMinBet;

	public Text HeMinBet;

	public Text HuMinBet;

	public Text LongMaxBet;

	public Text HuMaxBet;

	public Text HeMaxBet;

	public GameObject NextLong;

	public GameObject NextHu;

	public GameObject[] Contents;

	public void Awake()
	{
		CloseBtn = base.transform.Find("CloseBtn").GetComponent<Button>();
		LongCount = base.transform.Find("LongCount").GetComponent<Text>();
		HuCount = base.transform.Find("HuCount").GetComponent<Text>();
		HeCount = base.transform.Find("HeCount").GetComponent<Text>();
		AllCount = base.transform.Find("AllCount").GetComponent<Text>();
		NextLong = base.transform.Find("NextLong").gameObject;
		NextHu = base.transform.Find("NextHu").gameObject;
		LongMinBet = base.transform.Find("LongMinBet").GetComponent<Text>();
		HuMinBet = base.transform.Find("HuMinBet").GetComponent<Text>();
		HeMinBet = base.transform.Find("HeMinBet").GetComponent<Text>();
		LongMaxBet = base.transform.Find("LongMaxBet").GetComponent<Text>();
		HuMaxBet = base.transform.Find("HuMaxBet").GetComponent<Text>();
		HeMaxBet = base.transform.Find("HeMaxBet").GetComponent<Text>();
		Contents = new GameObject[5];
		for (int i = 0; i < Contents.Length; i++)
		{
			Contents[i] = base.transform.Find("Content" + (i + 1)).gameObject;
		}
	}

	private void Start()
	{
		CloseBtn.onClick.AddListener(delegate
		{
			base.gameObject.SetActive(value: false);
		});
	}

	private void OnEnable()
	{
		LongCount.text = string.Empty;
		HuCount.text = string.Empty;
		HeCount.text = string.Empty;
		AllCount.text = string.Empty;
		LongMinBet.text = string.Empty;
		HuMinBet.text = string.Empty;
		HeMinBet.text = string.Empty;
		LongMaxBet.text = string.Empty;
		HuMaxBet.text = string.Empty;
		HeMaxBet.text = string.Empty;
		GetLuDan();
		LHD_GameInfo instance = LHD_GameInfo.getInstance();
		instance.updateLuDan = (Action)Delegate.Combine(instance.updateLuDan, new Action(SetResultText));
	}

	private void OnDisable()
	{
		LHD_GameInfo instance = LHD_GameInfo.getInstance();
		instance.updateLuDan = (Action)Delegate.Remove(instance.updateLuDan, new Action(SetResultText));
	}

	public void GetLuDan()
	{
		if (LHD_NetMngr.GetSingleton() != null)
		{
			LHD_NetMngr.GetSingleton().MyCreateSocket.SendLuDan();
		}
	}

	private void SetResultText()
	{
		LongCount.text = LHD_LuDanManager.instance.longNum.ToString("00");
		HuCount.text = LHD_LuDanManager.instance.huNum.ToString("00");
		HeCount.text = LHD_LuDanManager.instance.heNum.ToString("00");
		int num = LHD_LuDanManager.instance.longNum + LHD_LuDanManager.instance.huNum + LHD_LuDanManager.instance.heNum;
		AllCount.text = num.ToString("00");
		LongMaxBet.text = LHD_GameInfo.getInstance().roominfo.maxBet.ToString();
		HuMaxBet.text = LongMaxBet.text;
		LongMinBet.text = LHD_GameInfo.getInstance().roominfo.minBet.ToString();
		HuMinBet.text = LongMinBet.text;
		HeMaxBet.text = LHD_GameInfo.getInstance().roominfo.tieMaxBet.ToString();
		HeMinBet.text = LongMinBet.text;
	}

	private void UpdateRecords()
	{
	}
}
