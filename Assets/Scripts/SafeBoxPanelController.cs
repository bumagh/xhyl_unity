using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeBoxPanelController : MonoBehaviour
{
	private int m_inputNum;

	private int m_curTabIndex;

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
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("deposit", HandleNetMsg_Deposit);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("extract", HandleNetMsg_Extract);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("changeSafeBoxPwd", HandleNetMsg_ChangeSafeBoxPwd);
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
		FocusTabByIndex(1);
		sliderValue = -1;
		m_inputNum = 0;
		inputFieldCoinNum.text = m_inputNum.ToString();
		LeftBtnClick(0);
	}

	private void Update()
	{
		if (sliderValue != (int)slider.value)
		{
			sliderValue = (int)slider.value;
			SetTextColoer();
		}
		CoinText.text = ZH2_GVars.user.gameGold.ToString();
		LotteryText.text = ZH2_GVars.savedGameGold.ToString();
	}

	private void BtnMax()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		slider.value = slider.maxValue;
		if (IsOut())
		{
			inputFieldCoinNum.text = ZH2_GVars.savedGameGold.ToString();
		}
		else
		{
			inputFieldCoinNum.text = ZH2_GVars.user.gameGold.ToString();
		}
	}

	private void BtnRes()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		slider.value = slider.minValue;
		inputFieldCoinNum.text = "0";
	}

	private void BtnSure()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
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
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
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
			inputFieldCoinNum.text = ((int)((float)(ZH2_GVars.user.gameGold * sliderValue) * 0.1f)).ToString();
		}
	}

	private void UpdateContent()
	{
	}

	public void OnClosePanel()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		base.gameObject.SetActive(value: false);
	}

	public void OnBtnClick_In()
	{
		string text = inputFieldCoinNum.text;
		if (text == string.Empty)
		{
			string text2 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "请输入存入游戏币的数量" : "请输入存入彩票的数量") : "请输入存入游戏币的数量";
			string text3 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Please enter the number of coins that need to be saved" : "Please enter the number of lottery that need to be saved") : "Please enter the number of coins that need to be saved";
			string text4 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48เข\u0e49าไปในเกม" : "โปรดป\u0e49อนจำนวน ท\u0e35\u0e48ใส\u0e48เข\u0e49าไปในล\u0e47อตเตอร\u0e35\u0e48") : "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48เข\u0e49าไปในเกม";
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? text3 : ((ZH2_GVars.language_enum != 0) ? text4 : text2));
			return;
		}
		m_inputNum = Convert.ToInt32(text);
		if (m_inputNum <= 0)
		{
			string text5 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "请输入存入游戏币的数量" : "请输入存入彩票的数量") : "请输入存入游戏币的数量";
			string text6 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Please enter the number of coins that need to be saved" : "Please enter the number of lottery that need to be saved") : "Please enter the number of coins that need to be saved";
			string text7 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48เข\u0e49าไปในเกม" : "โปรดป\u0e49อนจำนวน ท\u0e35\u0e48ใส\u0e48เข\u0e49าไปในล\u0e47อตเตอร\u0e35\u0e48") : "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48เข\u0e49าไปในเกม";
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? text6 : ((ZH2_GVars.language_enum != 0) ? text7 : text5));
		}
		else if (m_curTabIndex == 1 && m_inputNum > ZH2_GVars.user.gameGold)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Lack of game currency" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48ม\u0e35เง\u0e34นเล\u0e48น" : "游戏币不足"));
		}
		else
		{
			MB_Singleton<NetManager>.GetInstance().Send("gcsecurityService/deposit", new object[2]
			{
				m_inputNum,
				m_curTabIndex
			});
			MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		}
	}

	public void OnBtnClick_Out()
	{
		string text = inputFieldCoinNum.text;
		if (text == string.Empty)
		{
			string text2 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "请输入取出游戏币的数量" : "请输入取出彩票的数量") : "请输入取出游戏币的数量";
			string text3 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Please enter the number of coins that need to be taken out" : "Please enter the number of lottery that need to be taken out") : "Please enter the number of coins that need to be taken out";
			string text4 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48จะเอาออกจากเกม" : "โปรดป\u0e49อนจำนวนล\u0e47อตเตอร\u0e35\u0e48 ท\u0e35\u0e48เอาออกมา") : "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48จะเอาออกจากเกม";
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? text3 : ((ZH2_GVars.language_enum != 0) ? text4 : text2));
			return;
		}
		m_inputNum = Convert.ToInt32(text);
		if (m_inputNum <= 0)
		{
			string text5 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "请输入取出游戏币的数量" : "请输入取出彩票的数量") : "请输入取出游戏币的数量";
			string text6 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Please enter the number of coins that need to be taken out" : "Please enter the number of lottery that need to be taken out") : "Please enter the number of coins that need to be taken out";
			string text7 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48จะเอาออกจากเกม" : "โปรดป\u0e49อนจำนวนล\u0e47อตเตอร\u0e35\u0e48 ท\u0e35\u0e48เอาออกมา") : "โปรดป\u0e49อนจำนวนเง\u0e34น ท\u0e35\u0e48จะเอาออกจากเกม";
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? text6 : ((ZH2_GVars.language_enum != 0) ? text7 : text5));
		}
		else if (m_curTabIndex == 1 && m_inputNum > ZH2_GVars.savedGameGold)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("保险柜余额不足", "The balance in the safe is insufficient", "ไม่มีเงินเล่น", "Không đủ số dư két an toàn"));
		}
		else if (m_curTabIndex == 2 && m_inputNum > ZH2_GVars.savedLottery)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Lottery shortage" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48พอสำหร\u0e31บการจ\u0e31บสลาก " : "彩票不足"));
		}
		else
		{
			MB_Singleton<NetManager>.GetInstance().Send("gcsecurityService/extract", new object[2]
			{
				m_inputNum,
				m_curTabIndex
			});
			MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		}
	}

	public void OnBtnClick_Pwd()
	{
		if (PwdInputField[0].text == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog("旧密码不可为空");
		}
		else if (PwdInputField[1].text == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog("新密码不可为空");
		}
		else if (PwdInputField[2].text == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog("重复输入新密码不可为空");
		}
		else if (PwdInputField[0].text == PwdInputField[1].text)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog("旧密码和新密码不可相同");
		}
		else if (PwdInputField[1].text != PwdInputField[2].text)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog("两次输入的新密码不一致");
		}
		else
		{
			MB_Singleton<NetManager>.GetInstance().Send("gcuserService/changeSafeBoxPwd", new object[2]
			{
				PwdInputField[0].text,
				PwdInputField[1].text
			});
		}
	}

	private void FocusTabByIndex(int index)
	{
		m_curTabIndex = index;
		UpdateContent();
	}

	private void HandleNetMsg_Deposit(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		UnityEngine.Debug.Log(dictionary);
		if ((bool)dictionary["success"])
		{
			string content = string.Format((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "You have successfully deposited {0}{1}" : ((ZH2_GVars.language_enum != 0) ? "ท\u0e48านได\u0e49ทำการฝากเง\u0e34นเข\u0e49าบ\u0e31ญช\u0e35เร\u0e35ยบร\u0e49อยแล\u0e49ว{0}{1}" : "您已经成功存入{0}{1}"), m_inputNum, (m_curTabIndex != 1) ? ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "lottery" : ((ZH2_GVars.language_enum != 0) ? "ลอตเตอร\u0e35\u0e48" : "彩票")) : ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "coins" : ((ZH2_GVars.language_enum != 0) ? "ช\u0e37\u0e48อเกม" : "游戏币")));
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(content);
			ZH2_GVars.savedGameGold = (int)dictionary["boxGameGold"];
			ZH2_GVars.savedLottery = (int)dictionary["boxLottery"];
			ZH2_GVars.user.gameGold = (int)dictionary["gameGold"];
			ZH2_GVars.user.lottery = (int)dictionary["lottery"];
			inputFieldCoinNum.text = string.Empty;
			slider.value = 0f;
			MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
			return;
		}
		int num = (int)dictionary["msgCode"];
		UnityEngine.Debug.Log(num);
		switch (num)
		{
		case 2:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
			break;
		case 3:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
			break;
		case 6:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号被冻结,请联系管理员", "This account has been frozen", "เลขบัญชีถูกระงับ", "Tài khoản bị đóng băng, vui lòng liên hệ với quản trị viên"));
			break;
		case 7:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Server maintenance" : ((ZH2_GVars.language_enum != 0) ? "การด\u0e39แลระบบ" : "系统维护"));
			break;
		case 11:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ห้ามสมัครซ้ำซ้อน", "Không lặp lại ứng dụng"));
			break;
		case 12:
			base.transform.Find("neikuang/input").GetComponent<InputField>().text = string.Empty;
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("余额不足", "balance", "ยอดคงเหลือไม่เพียงพอ", "Số dư không đủ"));
			break;
		default:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "failed" : ((ZH2_GVars.language_enum != 0) ? "ล\u0e49มเหลว" : "失败"));
			break;
		}
	}

	private void HandleNetMsg_Extract(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			string content = string.Format((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "You have successfully take out {0}{1}" : ((ZH2_GVars.language_enum != 0) ? "ค\u0e38ณทำสำเร\u0e47จแล\u0e49ว{0}{1}" : "您已经成功取出{0}{1}"), m_inputNum, (m_curTabIndex != 1) ? ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "lottery" : ((ZH2_GVars.language_enum != 0) ? "ลอตเตอร\u0e35\u0e48" : "彩票")) : ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "coins" : ((ZH2_GVars.language_enum != 0) ? "ช\u0e37\u0e48อเกม" : "游戏币")));
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(content);
			ZH2_GVars.savedGameGold = (int)dictionary["boxGameGold"];
			ZH2_GVars.savedLottery = (int)dictionary["boxLottery"];
			ZH2_GVars.user.gameGold = (int)dictionary["gameGold"];
			ZH2_GVars.user.lottery = (int)dictionary["lottery"];
			inputFieldCoinNum.text = string.Empty;
			slider.value = 0f;
			MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
			return;
		}
		int num = (int)dictionary["msgCode"];
		UnityEngine.Debug.Log(num);
		switch (num)
		{
		case 2:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
			break;
		case 3:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
			break;
		case 6:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号被冻结,请联系管理员", "This account has been frozen", "เลขบัญชีถูกระงับ", "Tài khoản bị đóng băng, vui lòng liên hệ với quản trị viên"));
			break;
		case 7:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Server maintenance" : ((ZH2_GVars.language_enum != 0) ? "การด\u0e39แลระบบ" : "系统维护"));
			break;
		case 11:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ห้ามสมัครซ้ำซ้อน", "Không lặp lại ứng dụng"));
			break;
		case 12:
			base.transform.Find("neikuang/input").GetComponent<InputField>().text = string.Empty;
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("余额不足", "balance", "ยอดคงเหลือไม่เพียงพอ", "Số dư không đủ"));
			break;
		default:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "failed" : ((ZH2_GVars.language_enum != 0) ? "ล\u0e49มเหลว" : "失败"));
			break;
		}
	}

	private void HandleNetMsg_ChangeSafeBoxPwd(object[] objs)
	{
		UnityEngine.Debug.Log("HandleNetMsg_ChangeSafeBoxPwd");
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog("密码修改成功", showOkCancel: false, delegate
			{
				for (int i = 0; i < PwdInputField.Length; i++)
				{
					PwdInputField[i].text = string.Empty;
				}
			});
			return;
		}
		int num = (int)dictionary["msgCode"];
		if (num == 15)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog("旧密码错误", showOkCancel: false, delegate
			{
			});
		}
		else
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog("密码修改失败", showOkCancel: false, delegate
			{
			});
		}
	}
}
