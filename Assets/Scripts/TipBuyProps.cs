using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipBuyProps : MonoBehaviour
{
	private int m_itemId;

	private int m_buyCount;

	[SerializeField]
	private int presentLevel = 5;

	[SerializeField]
	private ShopPropList _shopProplist;

	private void Start()
	{
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("bugByGameGold", HandleNetMsg_Buy);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("giftByGameGold", HandleNetMsg_PresentBack);
	}

	private void SwitchIntoEnglish()
	{
		if (ZH2_GVars.language_enum != 0)
		{
			base.transform.Find("tip").GetComponent<Text>().fontSize = 28;
			base.transform.Find("tip").GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 25);
			base.transform.Find("btnSure/Text").GetComponent<Text>().text = "Sure";
			base.transform.Find("InputPassword/Placeholder").GetComponent<Text>().text = "please input password";
		}
	}

	public void ShowUI(int itemId)
	{
		m_itemId = itemId;
		m_buyCount = ((_shopProplist.list[m_itemId - 1].price <= ZH2_GVars.user.gameGold) ? 1 : 0);
		UpdateBuycount();
		UpdateTopTip();
		if (ZH2_GVars.giftMode == 0)
		{
			base.transform.Find("btnPresent").gameObject.SetActive(value: false);
			base.transform.Find("btnSure").localPosition = new Vector3(0f, -187.7f, 0f);
		}
		else
		{
			base.transform.Find("btnPresent").gameObject.SetActive(value: true);
			base.transform.Find("btnSure").localPosition = new Vector3(150f, -187.7f, 0f);
		}
	}

	public void OnBtnClick_Present()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		if (_shopProplist.list[m_itemId - 1].price > ZH2_GVars.user.gameGold)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Coin shortage" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48ม\u0e35เง\u0e34นเล\u0e48น" : "游戏币不足"));
			return;
		}
		string text = base.transform.Find("InputPassword").GetComponent<InputField>().text;
		if (m_buyCount == 0)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please input correct num" : ((ZH2_GVars.language_enum != 0) ? "โปรดป\u0e49อนจำนวน ท\u0e35\u0e48ถ\u0e39กต\u0e49อง" : "请输入正确的赠送数量"));
		}
		else if (text == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please input password" : ((ZH2_GVars.language_enum != 0) ? "โปรดป\u0e49อนรห\u0e31สผ\u0e48าน" : "请输入密码"));
		}
		else if (text != ZH2_GVars.pwd)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "password error" : ((ZH2_GVars.language_enum != 0) ? "รห\u0e31สผ\u0e48านไม\u0e48ถ\u0e39กต\u0e49อง" : "密码错误"));
		}
		else
		{
			base.transform.Find("PresentPanel").gameObject.SetActive(value: true);
		}
	}

	public void OnBtnClick_PresentSure()
	{
		string text = base.transform.Find("PresentPanel/bg/UserName").GetComponent<InputField>().text;
		string text2 = base.transform.Find("PresentPanel/bg/Repeat").GetComponent<InputField>().text;
		string text3 = base.transform.Find("PresentPanel/bg/Beizhu").GetComponent<InputField>().text;
		string text4 = base.transform.Find("InputPassword").GetComponent<InputField>().text;
		int length = text.Length;
		if (text == string.Empty)
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please enter the other account" : ((ZH2_GVars.language_enum != 0) ? "กร\u0e38ณาพ\u0e34มพ\u0e4cเลข ท\u0e35\u0e48บ\u0e31ญช\u0e35ของก\u0e31น และก\u0e31น" : "请输入对方账号"));
		}
		else if (InputCheck.CheckUserName(text))
		{
			if (text != text2)
			{
				MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The two input are different" : ((ZH2_GVars.language_enum != 0) ? "ป\u0e49อนสองคร\u0e31\u0e49งไม\u0e48ตรงก\u0e31น" : "两次输入不一致"));
			}
			else if (text == ZH2_GVars.user.username)
			{
				MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Can not give yourself" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48สามารถให\u0e49ม\u0e31นก\u0e31บต\u0e31วเองได\u0e49 " : "无法赠送给自己"));
			}
			else
			{
				MB_Singleton<NetManager>.GetInstance().Send("gcshopService/giftByGameGold", new object[5]
				{
					m_itemId,
					m_buyCount,
					text4,
					text,
					text3
				});
			}
		}
	}

	private void HandleNetMsg_PresentBack(object[] obj)
	{
		Dictionary<string, object> dictionary = obj[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			ZH2_GVars.user.gameGold = (int)dictionary["gameGold"];
			MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
			UnityEngine.Object.Destroy(base.transform.gameObject);
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Presented successfully" : ((ZH2_GVars.language_enum != 0) ? "ประสบความสำเร\u0e47จ " : "赠送成功"));
			return;
		}
		int num = (int)dictionary["msgCode"];
		switch (num)
		{
		case 23:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Accounting error" : ((ZH2_GVars.language_enum != 0) ? "บ\u0e31ญช\u0e35ผ\u0e34ดปกต\u0e34 " : "账目异常"));
			break;
		case 24:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Insufficient level" : ((ZH2_GVars.language_enum != 0) ? "ระด\u0e31บความช\u0e37\u0e48นชอบต\u0e48ำ " : "等级不足"));
			break;
		case 25:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Today,the maximum number of gifts is reached,please try again tomorrow" : ((ZH2_GVars.language_enum != 0) ? "ว\u0e31นน\u0e35\u0e49 ม\u0e35ข\u0e35ด จำก\u0e31ด ส\u0e39งส\u0e38ดแล\u0e49วค\u0e48ะลองว\u0e31นพร\u0e38\u0e48งน\u0e35\u0e49 นะคะ" : "今日赠送个数已达上限，请明日再试"));
			break;
		default:
			UnityEngine.Debug.Log(LogHelper.Yellow("msgCode: " + num));
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "allocation failed" : ((ZH2_GVars.language_enum != 0) ? "ล\u0e49มเหลวในการแจกจ\u0e48าย" : "赠送失败"));
			break;
		case 12:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Coin is insufficient" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48ม\u0e35เง\u0e34นเล\u0e48น" : "游戏币不足"));
			break;
		case 3:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "password error" : ((ZH2_GVars.language_enum != 0) ? "รห\u0e31สผ\u0e48านไม\u0e48ถ\u0e39กต\u0e49อง" : "密码错误"));
			break;
		case 2:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The account does not exist" : ((ZH2_GVars.language_enum != 0) ? "ย\u0e31งไม\u0e48ม\u0e35ช\u0e37\u0e48อผ\u0e39\u0e49ใช\u0e49  " : "用户名不存在"));
			break;
		case 27:
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Can not give yourself" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48สามารถให\u0e49ม\u0e31นก\u0e31บต\u0e31วเองได\u0e49 " : "无法赠送给自己"));
			break;
		}
	}

	public void OnbtnClick_PresentClose()
	{
		base.transform.Find("PresentPanel/bg/UserName").GetComponent<InputField>().text = string.Empty;
		base.transform.Find("PresentPanel/bg/Repeat").GetComponent<InputField>().text = string.Empty;
		base.transform.Find("PresentPanel/bg/Beizhu").GetComponent<InputField>().text = string.Empty;
		base.transform.Find("PresentPanel").gameObject.SetActive(value: false);
	}

	public void OnBtnClick_Sure()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		if (_shopProplist.list[m_itemId - 1].price > ZH2_GVars.user.gameGold)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Coin is insufficient" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48ม\u0e35เง\u0e34นเล\u0e48น" : "游戏币不足"));
			return;
		}
		string text = base.transform.Find("InputPassword").GetComponent<InputField>().text;
		if (m_buyCount == 0)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please input correct num" : ((ZH2_GVars.language_enum != 0) ? "โปรดป\u0e49อนจำนวนการซ\u0e37\u0e49อ ท\u0e35\u0e48ถ\u0e39กต\u0e49อง" : "请输入正确的购买数量"));
		}
		else if (text == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please input password" : ((ZH2_GVars.language_enum != 0) ? "โปรดป\u0e49อนรห\u0e31สผ\u0e48าน" : "请输入密码"));
		}
		else
		{
			MB_Singleton<NetManager>.GetInstance().Send("gcshopService/bugByGameGold", new object[3]
			{
				m_itemId,
				m_buyCount,
				text
			});
		}
	}

	public void OnBtnClick_Cancel()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnBtnClick_Add()
	{
		if ((m_buyCount + 1) * _shopProplist.list[m_itemId - 1].price > ZH2_GVars.user.gameGold)
		{
			m_buyCount = ((_shopProplist.list[m_itemId - 1].price <= ZH2_GVars.user.gameGold) ? 1 : 0);
		}
		else
		{
			m_buyCount++;
		}
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Click_PlusMinus);
		UpdateBuycount();
	}

	public void OnBtnClick_Reduce()
	{
		if (m_buyCount <= 0)
		{
			m_buyCount = 0;
		}
		else if (m_buyCount == 1)
		{
			m_buyCount = Convert.ToInt32(ZH2_GVars.user.gameGold / _shopProplist.list[m_itemId - 1].price);
		}
		else
		{
			m_buyCount--;
		}
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Click_PlusMinus);
		UpdateBuycount();
	}

	private void UpdateBuycount()
	{
		base.transform.Find("InputBuyCount").GetComponent<InputField>().text = m_buyCount.ToString();
	}

	public void UpdateTopTip()
	{
		string text = base.transform.Find("InputBuyCount").GetComponent<InputField>().text;
		if (text.Contains("-"))
		{
			m_buyCount = Convert.ToInt32(ZH2_GVars.user.gameGold / _shopProplist.list[m_itemId - 1].price);
		}
		else
		{
			m_buyCount = Convert.ToInt32(text);
		}
		if (m_buyCount * _shopProplist.list[m_itemId - 1].price > ZH2_GVars.user.gameGold)
		{
			m_buyCount = Convert.ToInt32(ZH2_GVars.user.gameGold / _shopProplist.list[m_itemId - 1].price);
		}
		else if (m_buyCount < 0)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Illegal number" : ((ZH2_GVars.language_enum != 0) ? "หมายเลขผ\u0e34ดกฎหมาย" : "非法数字"));
			m_buyCount = Convert.ToInt32(ZH2_GVars.user.gameGold / _shopProplist.list[m_itemId - 1].price);
		}
		UpdateBuycount();
		base.transform.Find("tip").GetComponent<Text>().font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? MB_Singleton<AppManager>.Get().zh_font : MB_Singleton<AppManager>.Get().en_font);
		base.transform.Find("tip").GetComponent<Text>().fontSize = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? 33 : 26);		
		base.transform.Find("tip").GetComponent<Text>().text = string.Format(ZH2_GVars.ShowTip( "确认花费<size=38><color=#E4C751>{0}币</color></size>购买{1}么？" , "Confirm to spend <size=38><color=#E4C751>{0} coins</color></size> to buy {1}？", "ยืนยันการใช้จ่าย<size=38><color=#E4C751>{0} เหรียญ</color></size>ซื้อ {1} หรือไม่?", "Xác nhận chi phí<size=38><color=#E4C751>{0}Tiền xu</color></size>Mua hàng{1}？"), m_buyCount * _shopProplist.list[m_itemId - 1].price, ZH2_GVars.ShowTip( _shopProplist.list[m_itemId - 1].zh_name , _shopProplist.list[m_itemId - 1].en_name, _shopProplist.list[m_itemId - 1].en_name, _shopProplist.list[m_itemId - 1].en_name));
	}

	private void HandleNetMsg_Buy(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			UnityEngine.Object.Destroy(base.gameObject);
			ZH2_GVars.user.gameGold = (int)dictionary["gameGold"];
			ZH2_GVars.user.lottery = (int)dictionary["lottery"];
			ZH2_GVars.ownedProps.Clear();
			object[] array = (object[])dictionary["prop"];
			for (int i = 0; i < array.Length; i++)
			{
				Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
				ZH2_GVars.ownedProps.Add(Convert.ToInt32(dictionary2["propId"]), new OwnShopProp(Convert.ToInt32(dictionary2["propId"]), Convert.ToInt64(dictionary2["remainTime"]), ishow: false));
			}
			for (int j = 9; j < 12; j++)
			{
				if (ZH2_GVars.ownedProps.ContainsKey(j))
				{
					MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_FreshDealer, j - 8);
					break;
				}
				if (j == 11)
				{
					MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_FreshDealer, 0);
				}
			}
			MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
			MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_OwnedProps);
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("购买成功", "Buy success!", "การซื้อสำเร็จแล้ว", "Mua thành công"));
			return;
		}
		int num = (int)dictionary["msgCode"];
		Debug.Log(num);
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
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("系统维护,敬请谅解", "Server maintenance", "การดูแลระบบ", "Bảo trì hệ thống, xin thông cảm"));
			break;
		case 11:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ห้ามสมัครซ้ำซ้อน", "Không lặp lại ứng dụng"));
			break;
		case 12:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("余额不足", "balance", "ยอดคงเหลือไม่เพียงพอ", "Số dư không đủ"));
			break;
		default:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("登录失败,请检查网络", "Login failed", "การล็อกอินล้มเหลว", "Đăng nhập thất bại, vui lòng kiểm tra mạng"));
                break;
		}
	}
}
