using DG.Tweening;
using JsonFx.Json;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class All_PayPanelController : MonoBehaviour
{
	private class PayLog
	{
		public int ID;

		public int Amount;

		public int Type;

		public int Status;

		public string Order;

		public string Data;
	}

	[SerializeField]
	private GameObject Nonentity;

	[SerializeField]
	private GameObject Content;

	[SerializeField]
	private GameObject Content_Pre;

	private Queue<PayLog> PayLogQueue = new Queue<PayLog>();

	public List<GameObject> mPanels;

	private Color pitchOnColor = new Color(1f, 0.97f, 0.83f, 1f);

	private Color unselectedColor = new Color(0.25f, 0.13f, 0f, 1f);

	public Sprite pitchOnSpr;

	public Sprite unselectedSpr;

	public Transform Btns;

	private List<Button> btns;

	private List<Text> btnsText;

	private UIDepth depth;

	private int tempIndex = -1;

	private void Awake()
	{
		btns = new List<Button>();
		btnsText = new List<Text>();
		for (int i = 0; i < Btns.childCount; i++)
		{
			Button component = Btns.GetChild(i).GetComponent<Button>();
			if (component != null)
			{
				btns.Add(component);
			}
		}
		for (int j = 0; j < btns.Count; j++)
		{
			int tempIndex = j;
			btnsText.Add(btns[j].transform.GetChild(0).GetComponent<Text>());
			btns[j].onClick.AddListener(delegate
			{
				ShowSelectionNum(tempIndex);
			});
		}
		if (depth == null)
		{
			depth = base.gameObject.GetComponent<UIDepth>();
			if (depth == null)
			{
				depth = base.gameObject.AddComponent<UIDepth>();
			}
		}
		if (depth != null)
		{
			depth.order = 8;
			depth.isHaveButton = true;
		}
	}

	private void ShowSelectionNum(int index)
	{
		if (index != tempIndex)
		{
			tempIndex = index;
			if (index == 1)
			{
				GetTransactionRecord();
			}
			for (int i = 0; i < btnsText.Count; i++)
			{
				btnsText[i].DOColor(unselectedColor, 0.2f);
			}
			for (int j = 0; j < mPanels.Count; j++)
			{
				mPanels[j].SetActive(value: false);
			}
			for (int k = 0; k < btns.Count; k++)
			{
				btns[k].GetComponent<Image>().sprite = unselectedSpr;
			}
			btnsText[index].DOColor(pitchOnColor, 0.2f);
			mPanels[index].SetActive(value: true);
			btns[index].GetComponent<Image>().sprite = pitchOnSpr;
		}
	}

	private void OnEnable()
	{
		tempIndex = -1;
		ZH2_GVars.getTransactionRecord = (Action<object[]>)Delegate.Combine(ZH2_GVars.getTransactionRecord, new Action<object[]>(HandleNetMsg_getTransactionRecord));
		ShowSelectionNum(1);
	}

	private void GetTransactionRecord()
	{
		InitPalyJLu();
		ZH2_GVars.sendTransactionRecord(new object[1]
		{
			ZH2_GVars.userId
		});
	}

	private void OnDisable()
	{
		ZH2_GVars.getTransactionRecord = (Action<object[]>)Delegate.Remove(ZH2_GVars.getTransactionRecord, new Action<object[]>(HandleNetMsg_getTransactionRecord));
		InitPalyJLu();
	}

	public void HandleNetMsg_getTransactionRecord(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		int num = 0;
		for (int i = 0; i < jsonData.Count; i++)
		{
			JsonData jsonData2 = jsonData[i];
			PayLog payLog = new PayLog();
			payLog.ID = num;
			payLog.Amount = (int)jsonData2["amount"];
			payLog.Type = (int)jsonData2["transactionType"];
			payLog.Status = (int)jsonData2["status"];
			payLog.Order = (string)jsonData2["orderNumber"];
			payLog.Data = (string)jsonData2["transactionDate"];
			PayLog item = payLog;
			num++;
			PayLogQueue.Enqueue(item);
		}
		if (PayLogQueue.Count <= 0)
		{
			InitPalyJLu();
			return;
		}
		Nonentity.SetActive(value: false);
		for (int j = 0; j < PayLogQueue.Count; j++)
		{
			PayLog payLog2 = PayLogQueue.ToArray()[j];
			Transform transform = null;
			transform = UnityEngine.Object.Instantiate(Content_Pre, Content.transform).transform;
			transform.gameObject.SetActive(value: true);
			transform.GetChild(0).GetComponent<Text>().text = payLog2.Data;
			transform.GetChild(1).GetComponent<Text>().text = payLog2.Order;
			string empty = string.Empty;
			switch (payLog2.Type)
			{
			case 0:
				empty = "充值";
				break;
			case 1:
				empty = "兑换";
				break;
			default:
				empty = "其他";
				break;
			}
			transform.GetChild(2).GetComponent<Text>().text = empty;
			transform.GetChild(3).transform.Find("Text").GetComponent<Text>().text = payLog2.Amount.ToString();
			string empty2 = string.Empty;
			switch (payLog2.Status)
			{
			case 0:
				empty2 = "等待";
				break;
			case 1:
				empty2 = "处理";
				break;
			case 2:
				empty2 = "拒绝";
				break;
			default:
				empty2 = "异常";
				break;
			}
			transform.GetChild(4).GetComponent<Text>().text = empty2;
		}
	}

	private void InitPalyJLu()
	{
		PayLogQueue = new Queue<PayLog>();
		Nonentity.SetActive(value: true);
		for (int i = 0; i < Content.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(Content.transform.GetChild(i).gameObject);
		}
	}

	public void OnBtnClick_Return()
	{
		base.gameObject.SetActive(value: false);
	}
}
