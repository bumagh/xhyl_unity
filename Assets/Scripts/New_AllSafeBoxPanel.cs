using LitJson;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class New_AllSafeBoxPanel : MonoBehaviour
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

	private Tween_SlowAction tween_SlowAction;

	private int sliderValue = -1;

	private int btnState;

	private void Awake()
	{
		slider.maxValue = 10f;
		slider.minValue = 0f;
		slider.wholeNumbers = true;
		tween_SlowAction = GetComponent<Tween_SlowAction>();
	}

	private void Start()
	{
		maxBtn.onClick.AddListener(BtnMax);
		resBtn.onClick.AddListener(BtnRes);
		sureBtn.onClick.AddListener(BtnSure);
		ChangePwdBtn.onClick.AddListener(OnBtnClick_Pwd);
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
		if ((object)tween_SlowAction != null)
		{
			tween_SlowAction.Show();
		}
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
		All_TipCanvas.GetInstance().SourcePlayClip();
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
		All_TipCanvas.GetInstance().SourcePlayClip();
		slider.value = slider.minValue;
		inputFieldCoinNum.text = "0";
	}

	private void BtnSure()
	{
		All_TipCanvas.GetInstance().SourcePlayClip();
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
		All_TipCanvas.GetInstance().SourcePlayClip();
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
				LeftBtnClick(0);
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

	public void OnClosePanel()
	{
		All_TipCanvas.GetInstance().SourcePlayClip();
		if ((object)tween_SlowAction != null)
		{
			tween_SlowAction.Hide(base.gameObject);
		}
	}

	public void OnBtnClick_In()
	{
		string text = inputFieldCoinNum.text;
		if (text == string.Empty)
		{
			string ch = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "请输入存入游戏币的数量" : "请输入存入彩票的数量") : "请输入存入游戏币的数量";
			string en = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Please enter the number of coins that need to be saved" : "Please enter the number of lottery that need to be saved") : "Please enter the number of coins that need to be saved";
			string ti = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "กรุณาใส่จำนวนเหรียญที่ฝากเงิน" : "กรุณาใส่จำนวนที่ฝากสลากกินแบ่งรัฐบาล") : "กรุณาใส่จำนวนเหรียญที่ฝากเงิน";
			string vn = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Vui lòng nhập số tiền gửi trong game" : "Vui lòng nhập số tiền đặt vé") : "Vui lòng nhập số tiền gửi trong game";
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip(ch, en, ti,vn));
			return;
		}
		m_inputNum = Convert.ToInt32(text);
		if (m_inputNum <= 0)
		{
			string ch2 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "请输入存入游戏币的数量" : "请输入存入彩票的数量") : "请输入存入游戏币的数量";
			string en2 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Please enter the number of coins that need to be saved" : "Please enter the number of lottery that need to be saved") : "Please enter the number of coins that need to be saved";
			string ti2 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "กรุณาใส่จำนวนเหรียญที่ฝากเงิน" : "กรุณาใส่จำนวนที่ฝากสลากกินแบ่งรัฐบาล") : "กรุณาใส่จำนวนเหรียญที่ฝากเงิน";
            string vn2 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Vui lòng nhập số tiền gửi trong game" : "Vui lòng nhập số tiền đặt vé") : "Vui lòng nhập số tiền gửi trong game";
            All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip(ch2, en2, ti2,vn2));
		}
		else if (m_curTabIndex == 1 && m_inputNum > ZH2_GVars.user.gameGold)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("游戏币不足", "Lack of game currency", "เงินเกมไม่เพียงพอ", "Game không đủ tiền"));
		}
		else
		{
			SenDepositOrExtract(m_inputNum);
		}
	}

	private void SenDepositOrExtract(int amount)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("userId", ZH2_GVars.userId);
		hashtable.Add("gameId", ZH2_GVars.allSetType);
		hashtable.Add("password", ZH2_GVars.pwd);
		hashtable.Add("amount", amount);
		hashtable.Add("safeBoxPassword", ZH2_GVars.safeBoxPassword);
		Hashtable obj = hashtable;
		StartCoroutine(GetDepositOrExtract(ZH2_GVars.EncodeMessage(obj)));
	}

	public void OnBtnClick_Pwd()
	{
		All_TipCanvas.GetInstance().SourcePlayClip();
		if (PwdInputField[0].text == string.Empty)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip( "旧密码不可为空", "Old password cannot be empty", "รหัสผ่านเก่าต้องไม่ว่างเปล่า", "Mật khẩu cũ không được để trống"));
		}
		else if (PwdInputField[1].text == string.Empty)
		{
            All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("新密码不可为空", "The new password cannot be empty", "รหัสผ่านใหม่จะไม่ว่างเปล่า", "Mật khẩu mới không được để trống"));
        }
		else if (PwdInputField[2].text == string.Empty)
		{
            All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("重复输入新密码不可为空", "Repeatedly entering a new password cannot be empty", "ป้อนรหัสผ่านใหม่ซ้ำ ๆ จะไม่ว่างเปล่า", "Lặp lại nhập mật khẩu mới không được để trống"));
        }
		else if (PwdInputField[0].text == PwdInputField[1].text)
		{
            All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("旧密码和新密码不可相同", "Old password and new password cannot be the same", "รหัสผ่านเก่าและรหัสผ่านใหม่ไม่เหมือนกัน", "Mật khẩu cũ và mật khẩu mới không giống nhau"));
        }
		else if (PwdInputField[1].text != PwdInputField[2].text)
		{
            All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("两次输入的新密码不一致", "The new password entered twice does not match", "รหัสผ่านใหม่ที่ป้อนสองครั้งไม่สอดคล้องกัน", "Mật khẩu mới nhập hai lần không phù hợp"));
        }
		else if (PwdInputField[1].text.Length != 6)
		{
            All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("密码格式错误", "Password format error", "รูปแบบรหัสผ่านไม่ถูกต้อง", "Định dạng mật khẩu sai"));
        }
		else
		{
			SenChangSafeBoxPwd(int.Parse(PwdInputField[0].text), int.Parse(PwdInputField[1].text));
		}
	}

	private void SenChangSafeBoxPwd(int safeBoxPassword, int newSafeBoxPassword)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("userId", ZH2_GVars.userId);
		hashtable.Add("gameId", ZH2_GVars.allSetType);
		hashtable.Add("password", ZH2_GVars.pwd);
		hashtable.Add("oldPassword", safeBoxPassword);
		hashtable.Add("newPassword", newSafeBoxPassword);
		Hashtable obj = hashtable;
		ZH2_GVars.safeBoxPassword = safeBoxPassword;
		StartCoroutine(GetChangSafeBoxPwd(ZH2_GVars.EncodeMessage(obj)));
	}

	private IEnumerator GetDepositOrExtract(string msg)
	{
		string url2 = ZH2_GVars.shortConnection + ((!IsOut()) ? "safeBoxDepositGold" : "safeBoxExtractGold");
		url2 = url2 + "?jsonStr=" + msg;
		UnityWebRequest www = UnityWebRequest.Get(url2);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			string text = www.downloadHandler.text;
			if (text != string.Empty)
			{
				text = ZH2_GVars.DecodeMessage(text);
				JsonData dictionary = JsonMapper.ToObject(text);
				if (!IsOut())
				{
					HandleNetMsg_Deposit(dictionary);
				}
				else
				{
					HandleNetMsg_Extract(dictionary);
				}
			}
			else
			{
				ShowError();
			}
		}
		else
		{
			UnityEngine.Debug.LogError("===访问错误===: " + www.error);
			ShowError();
		}
	}

	private IEnumerator GetChangSafeBoxPwd(string msg)
	{
		string url2 = ZH2_GVars.shortConnection + "safeBoxChangePassword";
		url2 = url2 + "?jsonStr=" + msg;
		UnityWebRequest www = UnityWebRequest.Get(url2);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			string text = www.downloadHandler.text;
			if (text != string.Empty)
			{
				text = ZH2_GVars.DecodeMessage(text);
				JsonData dictionary = JsonMapper.ToObject(text);
				HandleNetMsg_ChangeSafeBoxPwd(dictionary);
			}
			else
			{
				ShowError();
			}
		}
		else
		{
			UnityEngine.Debug.LogError("===访问错误===: " + www.error);
			ShowError();
		}
	}

	private void ShowError()
	{
		string tips = ZH2_GVars.ShowTip("系统错误", "System failure", string.Empty);
		All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
	}

	public void OnBtnClick_Out()
	{
		string text = inputFieldCoinNum.text;
		if (text == string.Empty)
		{
			string ch = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "请输入取出游戏币的数量" : "请输入取出彩票的数量") : "请输入取出游戏币的数量";
			string en = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Please enter the number of coins that need to be taken out" : "Please enter the number of lottery that need to be taken out") : "Please enter the number of coins that need to be taken out";
			string th= (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "กรุณาใส่จำนวนที่นำเงินเกมออก" : "กรุณาใส่จำนวนลอตเตอรี่ที่นำออก") : "กรุณาใส่จำนวนที่นำเงินเกมออก";
			string vn= (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Vui lòng nhập số tiền đã rút ra khỏi trò chơi" : "Vui lòng nhập số lượng vé đã rút") : "Vui lòng nhập số tiền đã rút ra khỏi trò chơi";
            All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip(ch, en, th,vn));
			return;
		}
		m_inputNum = Convert.ToInt32(text);
		if (m_inputNum <= 0)
		{
			string ch2 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "请输入取出游戏币的数量" : "请输入取出彩票的数量") : "请输入取出游戏币的数量";
			string en2 = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Please enter the number of coins that need to be taken out" : "Please enter the number of lottery that need to be taken out") : "Please enter the number of coins that need to be taken out";
            string th = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "กรุณาใส่จำนวนที่นำเงินเกมออก" : "กรุณาใส่จำนวนลอตเตอรี่ที่นำออก") : "กรุณาใส่จำนวนที่นำเงินเกมออก";
            string vn = (ZH2_GVars.payMode == 1) ? string.Format((m_curTabIndex == 1) ? "Vui lòng nhập số tiền đã rút ra khỏi trò chơi" : "Vui lòng nhập số lượng vé đã rút") : "Vui lòng nhập số tiền đã rút ra khỏi trò chơi";
            All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip(ch2, en2, th,vn));
		}
		else if (m_curTabIndex == 1 && m_inputNum > ZH2_GVars.savedGameGold)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("保险柜余额不足", "The balance in the safe is insufficient", "ไม่มีเงินเล่น", "Không đủ số dư két an toàn"));
		}
		else if (m_curTabIndex == 2 && m_inputNum > ZH2_GVars.savedLottery)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("彩票不足", "Lottery shortage", "สลากไม่เพียงพอ", "Xổ số không đủ"));
		}
		else
		{
			SenDepositOrExtract(m_inputNum);
		}
	}

	private void FocusTabByIndex(int index)
	{
		m_curTabIndex = index;
	}

	private void HandleNetMsg_Deposit(JsonData dictionary)
	{
		UnityEngine.Debug.LogError("存入：" + dictionary.ToJson());
		if ((bool)dictionary["success"])
		{
			string tips = string.Format(ZH2_GVars.ShowTip("您已经成功存入{0}游戏币", "You have successfully deposited {0}coins", "ท่านได้ทำการฝากเงินเข้าบัญชีเรียบร้อยแล้ว{0}ชื่อเกม", "Bạn đã gửi thành công {0} tiền trò chơi"), m_inputNum);
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
			ZH2_GVars.savedGameGold = (int)dictionary["safeBoxGold"];
			ZH2_GVars.gameGold = (int)dictionary["gameGold"];
			if (ZH2_GVars.user != null)
			{
				ZH2_GVars.user.gameGold = ZH2_GVars.gameGold;
			}
			inputFieldCoinNum.text = string.Empty;
			slider.value = 0f;
		}
		else
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("余额不足", "balance", "ยอดคงเหลือไม่เพียงพอ", "Số dư không đủ"));
		}
	}

	private void HandleNetMsg_Extract(JsonData dictionary)
	{
		UnityEngine.Debug.LogError("取出：" + dictionary.ToJson());
		if ((bool)dictionary["success"])
		{
			string tips = string.Format(ZH2_GVars.ShowTip("您已经成功取出{0}游戏币", "You have successfully take out {0}coins", "คุณทำสำเร็จแล้ว{0}ชื่อเกม", "Bạn đã rút thành công {0} tiền trò chơi"), m_inputNum);
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
			ZH2_GVars.savedGameGold = (int)dictionary["safeBoxGold"];
			ZH2_GVars.gameGold = (int)dictionary["gameGold"];
			if (ZH2_GVars.user != null)
			{
				ZH2_GVars.user.gameGold = ZH2_GVars.gameGold;
			}
			inputFieldCoinNum.text = string.Empty;
			slider.value = 0f;
		}
		else
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("余额不足", "balance", "ยอดคงเหลือไม่เพียงพอ", "Số dư không đủ"));
		}
	}

	private void HandleNetMsg_ChangeSafeBoxPwd(JsonData dictionary)
	{
		UnityEngine.Debug.LogError("修改：" + dictionary.ToJson());
		if ((bool)dictionary["success"])
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("密码修改成功", "Password modification successful", "แก้ไขรหัสผ่านสำเร็จ", "Thay đổi mật khẩu thành công"));
			for (int i = 0; i < PwdInputField.Length; i++)
			{
				PwdInputField[i].text = string.Empty;
			}
		}
		else
		{
			int num = (int)dictionary["msg"];
			if (num == -2)
			{
				All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("旧密码错误", "Old password error", "รหัสผ่านเก่าผิดพลาด", "Lỗi mật khẩu cũ"));
			}
			else
			{
				All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("密码修改失败", "Password modification failed", "การแก้ไขรหัสผ่านล้มเหลว", "Sửa đổi mật khẩu không thành công"));
			}
		}
	}

	private void OnDisable()
	{
		if (ZH2_GVars.closeSafeBox != null)
		{
			ZH2_GVars.closeSafeBox();
		}
		else
		{
			UnityEngine.Debug.LogError("========关闭保险柜回调为空=======");
		}
	}
}
