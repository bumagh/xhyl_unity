using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RechargeController : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	[SerializeField]
	private Button btnRechargeVIP;

	[SerializeField]
	private Button[] btnRechargeType;

	[SerializeField]
	private Button btnRechargeUnion;

	[SerializeField]
	private GameObject[] imgRechargeType;

	[SerializeField]
	private Sprite[] spiTips;

	[SerializeField]
	private RechargeTip rechargeTip;

	[SerializeField]
	private Image imgRechargeTip;

	[SerializeField]
	private Button[] btnRechargeNum;

	private Text[] txtRechargeNum;

	[SerializeField]
	private Button btnClose;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject contentVIP;

	private int[] type = new int[6]
	{
		4,
		2,
		0,
		3,
		1,
		5
	};

	private List<List<int>> rechargeNum = new List<List<int>>();

	private int curIndex;

	private int lastIndex;

	private void Awake()
	{
		InitRechargeNumList();
		InitRechargeBtnType();
		InitTxtRechargeNum();
		btnClose.onClick.AddListener(ClickBtnClose);
	}

	private void OnEnable()
	{
		curIndex = -1;
		RechargeButtonToggle(0, 0);
		ClickBtnRechargeVIP();
	}

	private void InitRechargeNumList()
	{
		List<int> list = new List<int>();
		list.Add(50);
		list.Add(100);
		list.Add(200);
		list.Add(500);
		list.Add(1000);
		list.Add(2000);
		list.Add(3000);
		list.Add(4000);
		List<int> item = list;
		list = new List<int>();
		list.Add(100);
		list.Add(200);
		list.Add(500);
		list.Add(1000);
		list.Add(2000);
		list.Add(3000);
		list.Add(4000);
		list.Add(5000);
		List<int> item2 = list;
		rechargeNum.Add(item);
		rechargeNum.Add(item2);
		rechargeNum.Add(item);
		rechargeNum.Add(item2);
	}

	private void InitRechargeBtnType()
	{
		for (int i = 0; i < btnRechargeType.Length; i++)
		{
			btnRechargeType[i].transform.Find("Image").gameObject.SetActive(value: false);
			int temp = i;
			btnRechargeType[i].onClick.AddListener(delegate
			{
				RechargeButtonToggle(type[temp], temp);
			});
		}
	}

	private void InitTxtRechargeNum()
	{
		int num = btnRechargeNum.Length;
		txtRechargeNum = new Text[num];
		for (int i = 0; i < num; i++)
		{
			txtRechargeNum[i] = btnRechargeNum[i].transform.Find("Text").GetComponent<Text>();
		}
	}

	private void RefreshRechargeTip(int type)
	{
		imgRechargeTip.sprite = spiTips[type];
	}

	private void ShowRechargeNumBtn(int type, int index)
	{
		int count = rechargeNum[index].Count;
		for (int i = 0; i < count; i++)
		{
			int num = rechargeNum[index][i];
			txtRechargeNum[i].text = $"{num}元";
			btnRechargeNum[i].onClick.RemoveAllListeners();
			int temp = i;
			btnRechargeNum[i].onClick.AddListener(delegate
			{
				int num2 = rechargeNum[index][temp];
				ClickBtnRechargeNum(type, num2);
			});
		}
	}

	private void RechargeButtonToggle(int t, int i)
	{
		lastIndex = curIndex;
		if (curIndex == i)
		{
			return;
		}
		curIndex = i;
		GameObject[] array = imgRechargeType;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(value: false);
		}
		imgRechargeType[i].SetActive(value: true);
		if (i > 0 && i < 5)
		{
			RefreshRechargeTip(i - 1);
			ShowRechargeNumBtn(t, i - 1);
			contentVIP.SetActive(value: false);
			content.SetActive(value: true);
			return;
		}
		switch (i)
		{
		case 0:
			ClickBtnRechargeVIP();
			break;
		case 5:
			ClickBtnRechargeUnion();
			break;
		}
	}

	private void ClickBtnRechargeNum(int type, int num)
	{
		MonoBehaviour.print(type + ",," + num);
	}

	private void ClickBtnClose()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		RechargeButtonToggle(0, 0);
		base.gameObject.SetActive(value: false);
	}

	private void ClickBtnRechargeVIP()
	{
		contentVIP.SetActive(value: true);
		content.SetActive(value: false);
	}

	private void ClickBtnRechargeUnion()
	{
		MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("暂未开放", "Not yet open", string.Empty), showOkCancel: false, delegate
		{
			imgRechargeType[lastIndex].SetActive(value: true);
			imgRechargeType[5].SetActive(value: false);
		});
	}
}
