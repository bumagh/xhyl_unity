using DG.Tweening;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayPanelController : MonoBehaviour
{
	private class PayLog
	{
		public int ID;

		public int Amount;

		public int Type;

		public string Status;

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

	public List<Button> btns;

	private List<Text> btnsText;

	private int tempIndex = -1;

	private void Awake()
	{
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("getTransactionRecord", HandleNetMsg_getTransactionRecord);
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
	}

	private void ShowSelectionNum(int index)
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
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
		ShowSelectionNum(1);
	}

	private void GetTransactionRecord()
	{
		InitPalyJLu();
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getTransactionRecord", new object[1]
		{
			ZH2_GVars.user.id
		});
	}

	private void OnDisable()
	{
		InitPalyJLu();
	}

	public void HandleNetMsg_getTransactionRecord(object[] objs)
	{
		UnityEngine.Debug.LogError("====更新交易记录: " + JsonMapper.ToJson(objs));
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		int num = 0;
		while (true)
		{
			num++;
			if (!dictionary.ContainsKey(num.ToString()))
			{
				break;
			}
			Dictionary<string, object> dictionary2 = dictionary[num.ToString()] as Dictionary<string, object>;
			PayLog payLog = new PayLog();
			payLog.ID = num;
			payLog.Amount = (int)dictionary2["amount"];
			payLog.Type = (int)dictionary2["transactionType"];
			payLog.Status = (string)dictionary2["status"];
			payLog.Order = (string)dictionary2["orderNumber"];
			payLog.Data = (string)dictionary2["transactionDate"];
			PayLog item = payLog;
			PayLogQueue.Enqueue(item);
		}
		if (num <= 0)
		{
			InitPalyJLu();
			return;
		}
		Nonentity.SetActive(value: false);
		for (int i = 0; i < PayLogQueue.Count; i++)
		{
			PayLog payLog2 = PayLogQueue.ToArray()[i];
			Transform transform = null;
			transform = UnityEngine.Object.Instantiate(Content_Pre, Content.transform).transform;
			transform.SetActive();
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
			case "PENDING":
				empty2 = "等待";
				break;
			case "HANDLED":
				empty2 = "处理";
				break;
			case "WAIVE":
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
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		UnityEngine.Debug.Log(base.gameObject.name + ">>OnBtnClick_Return:");
		base.gameObject.SetActive(value: false);
	}
}
