using JsonFx.Json;
using LitJson;
using System;
using UnityEngine;
using UnityEngine.UI;

public class All_SafeBoxPanelController : MonoBehaviour
{
	private int m_inputNum;

	public Slider slider;

	public InputField inputFieldCoinNum;

	public Text[] textsBaiFen;

	public GameObject[] objZhiXiang;

	public Color pitchOnColor;

	public Color defaultColor;

	public Text CoinText;

	public Text LotteryText;

	public Button maxBtn;

	public Button resBtn;

	public Button sureBtn;

	public Button[] leftBtn;

	public GameObject PwdCheckPanel;

	public InputField[] PwdInputField;

	public Button ChangePwdBtn;

	private int sliderValue = -1;

	private int btnState;

	private void Awake()
	{
		slider.maxValue = 10f;
		slider.minValue = 0f;
		slider.wholeNumbers = true;
	}

	private void Start()
	{
		maxBtn.onClick.AddListener(BtnMax);
		resBtn.onClick.AddListener(BtnRes);
		sureBtn.onClick.AddListener(BtnSure);
		for (int i = 0; i < leftBtn.Length; i++)
		{
			int index = i;
			leftBtn[index].onClick.AddListener(delegate
			{
				LeftBtnClick(index);
			});
		}
	}

	private void OnEnable()
	{
		sliderValue = -1;
		m_inputNum = 0;
		inputFieldCoinNum.text = m_inputNum.ToString();
		LeftBtnClick(0);
		ZH2_GVars.getChangeSafeBoxPwd = (Action<object[]>)Delegate.Combine(ZH2_GVars.getChangeSafeBoxPwd, new Action<object[]>(HandleNetMsg_ChangeSafeBoxPwd));
		ZH2_GVars.getDeposit = (Action<object[]>)Delegate.Combine(ZH2_GVars.getDeposit, new Action<object[]>(HandleNetMsg_Deposit));
		ZH2_GVars.getExtract = (Action<object[]>)Delegate.Combine(ZH2_GVars.getExtract, new Action<object[]>(HandleNetMsg_Extract));
	}

	private void Update()
	{
		if (sliderValue != (int)slider.value)
		{
			sliderValue = (int)slider.value;
			SetTextColoer();
		}
		CoinText.text = ZH2_GVars.gameGold.ToString();
		LotteryText.text = ZH2_GVars.savedGameGold.ToString();
	}

	private void BtnMax()
	{
		slider.value = slider.maxValue;
		if (IsOut())
		{
			inputFieldCoinNum.text = ZH2_GVars.savedGameGold.ToString();
		}
		else
		{
			inputFieldCoinNum.text = ZH2_GVars.gameGold.ToString();
		}
	}

	private void BtnRes()
	{
		slider.value = slider.minValue;
		inputFieldCoinNum.text = "0";
	}

	private void BtnSure()
	{
		UnityEngine.Debug.LogError("====当前是: " + ((!IsOut()) ? "存入" : "取出"));
		if (IsOut())
		{
			OnBtnClick_Out();
		}
		else
		{
			OnBtnClick_In();
		}
	}

	private bool IsOut()
	{
		return btnState == 0;
	}

	private void LeftBtnClick(int index)
	{
		btnState = index;
		inputFieldCoinNum.text = "0";
		slider.value = slider.minValue;
		for (int i = 0; i < leftBtn.Length; i++)
		{
			leftBtn[i].transform.GetChild(1).gameObject.SetActive(value: false);
		}
		leftBtn[index].transform.GetChild(1).gameObject.SetActive(value: true);
		if (index == 2)
		{
			if (PwdCheckPanel.activeSelf)
			{
				PwdCheckPanel.SetActive(value: false);
				return;
			}
			PwdCheckPanel.SetActive(value: true);
			for (int j = 0; j < PwdInputField.Length; j++)
			{
				PwdInputField[j].text = string.Empty;
			}
		}
		else
		{
			PwdCheckPanel.SetActive(value: false);
		}
	}

	private void SetTextColoer()
	{
		for (int i = 0; i < textsBaiFen.Length; i++)
		{
			textsBaiFen[i].color = defaultColor;
		}
		textsBaiFen[sliderValue].color = pitchOnColor;
		for (int j = 0; j < objZhiXiang.Length; j++)
		{
			objZhiXiang[j].SetActive(value: false);
		}
		objZhiXiang[sliderValue].SetActive(value: true);
		if (IsOut())
		{
			inputFieldCoinNum.text = ((int)((float)(ZH2_GVars.savedGameGold * sliderValue) * 0.1f)).ToString();
		}
		else
		{
			inputFieldCoinNum.text = ((int)((float)(ZH2_GVars.gameGold * sliderValue) * 0.1f)).ToString();
		}
	}

	public void OnBtnClick_In()
	{
		string text = inputFieldCoinNum.text;
		if (text == string.Empty)
		{
			string ch = "请输入存入游戏币的数量";
			string en = "Please enter the number of coins that need to be saved";
			string ti = "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48เข\u0e49าไปในเกม";
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip(ch, en, ti));
			return;
		}
		m_inputNum = Convert.ToInt32(text);
		if (m_inputNum <= 0)
		{
			string ch2 = "请输入存入游戏币的数量";
			string en2 = "Please enter the number of coins that need to be saved";
			string ti2 = "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48เข\u0e49าไปในเกม";
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip(ch2, en2, ti2));
		}
		else if (m_inputNum > ZH2_GVars.gameGold)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("游戏币不足", "Lack of game currency", "ไม\u0e48ม\u0e35เง\u0e34นเล\u0e48น"));
		}
		else
		{
			ZH2_GVars.sendDeposit(new object[2]
			{
				ZH2_GVars.userId,
				m_inputNum
			});
		}
	}

	public void OnBtnClick_Out()
	{
		string text = inputFieldCoinNum.text;
		if (text == string.Empty)
		{
			string ch = "请输入取出游戏币的数量";
			string en = "Please enter the number of coins that need to be taken out";
			string ti = "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48จะเอาออกจากเกม";
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip(ch, en, ti));
			return;
		}
		m_inputNum = Convert.ToInt32(text);
		if (m_inputNum <= 0)
		{
			string ch2 = "请输入取出游戏币的数量";
			string en2 = "Please enter the number of coins that need to be taken out";
			string ti2 = "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48จะเอาออกจากเกม";
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip(ch2, en2, ti2));
		}
		else if (m_inputNum > ZH2_GVars.savedGameGold)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("保险柜余额不足", "The balance in the safe is insufficient", "ไม่มีเงินเล่น", "Không đủ số dư két an toàn"));
		}
		else
		{
			ZH2_GVars.sendExtract(new object[2]
			{
				ZH2_GVars.userId,
				m_inputNum
			});
		}
	}

	public void OnBtnClick_Pwd()
	{
		if (PwdInputField[0].text == string.Empty)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip("旧密码不可为空");
		}
		else if (PwdInputField[1].text == string.Empty)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip("新密码不可为空");
		}
		else if (PwdInputField[2].text == string.Empty)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip("重复输入新密码不可为空");
		}
		else if (PwdInputField[0].text == PwdInputField[1].text)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip("旧密码和新密码不可相同");
		}
		else if (PwdInputField[1].text != PwdInputField[2].text)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip("两次输入的新密码不一致");
		}
		else
		{
			ZH2_GVars.sendChangeSafeBoxPwd(new object[3]
			{
				ZH2_GVars.userId,
				PwdInputField[0].text,
				PwdInputField[1].text
			});
		}
	}

	private void HandleNetMsg_Deposit(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		if ((bool)jsonData["success"])
		{
			string tips = string.Format(ZH2_GVars.ShowTip("您已经成功存入{0}游戏币", "You have successfully deposited {0}coins", "ท\u0e48านได\u0e49ทำการฝากเง\u0e34นเข\u0e49าบ\u0e31ญช\u0e35เร\u0e35ยบร\u0e49อยแล\u0e49ว{0}ช\u0e37\u0e48อเกม"), m_inputNum);
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
			ZH2_GVars.savedGameGold = (int)jsonData["boxGameGold"];
			ZH2_GVars.savedLottery = (int)jsonData["boxLottery"];
			ZH2_GVars.gameGold = (int)jsonData["gameGold"];
			inputFieldCoinNum.text = string.Empty;
			slider.value = 0f;
		}
		else
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("余额不足", "balance", "ยอดคงเหลือไม่เพียงพอ", "Số dư không đủ"));
		}
	}

	private void HandleNetMsg_Extract(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		if ((bool)jsonData["success"])
		{
			string tips = string.Format(ZH2_GVars.ShowTip("您已经成功取出{0}游戏币", "You have successfully take out {0}coins", "ค\u0e38ณทำสำเร\u0e47จแล\u0e49ว{0}ช\u0e37\u0e48อเกม"), m_inputNum);
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
			ZH2_GVars.savedGameGold = (int)jsonData["boxGameGold"];
			ZH2_GVars.savedLottery = (int)jsonData["boxLottery"];
			ZH2_GVars.gameGold = (int)jsonData["gameGold"];
			inputFieldCoinNum.text = string.Empty;
			slider.value = 0f;
		}
		else
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("余额不足", "balance", "ยอดคงเหลือไม่เพียงพอ", "Số dư không đủ"));
		}
	}

	private void HandleNetMsg_ChangeSafeBoxPwd(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		if ((bool)jsonData["success"])
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip("密码修改成功");
			for (int i = 0; i < PwdInputField.Length; i++)
			{
				PwdInputField[i].text = string.Empty;
			}
			return;
		}
		switch ((int)jsonData["msgCode"])
		{
		case 1:
			All_GameMiniTipPanel.publicMiniTip.ShowTip("密码格式错误");
			break;
		case 2:
			All_GameMiniTipPanel.publicMiniTip.ShowTip("旧密码错误");
			break;
		default:
			All_GameMiniTipPanel.publicMiniTip.ShowTip("密码修改失败");
			break;
		}
	}

	private void OnDisable()
	{
		ZH2_GVars.getChangeSafeBoxPwd = (Action<object[]>)Delegate.Remove(ZH2_GVars.getChangeSafeBoxPwd, new Action<object[]>(HandleNetMsg_ChangeSafeBoxPwd));
		ZH2_GVars.getDeposit = (Action<object[]>)Delegate.Remove(ZH2_GVars.getDeposit, new Action<object[]>(HandleNetMsg_Deposit));
		ZH2_GVars.getExtract = (Action<object[]>)Delegate.Remove(ZH2_GVars.getExtract, new Action<object[]>(HandleNetMsg_Extract));
		if (ZH2_GVars.closeSafeBox != null)
		{
			ZH2_GVars.closeSafeBox();
		}
	}
}
