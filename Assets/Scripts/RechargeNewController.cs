using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RechargeNewController : MonoBehaviour
{
	public class ReqData
	{
		public string id;

		public string type;
	}

	public GameObject kefuButtonPre;

	public GameObject vipButtonPre;

	public GameObject zhiShaoMaPre;

	public GameObject zhiXiaoEPre;

	public GameObject weiWangYePre;

	public GameObject jiLuPre;

	public GameObject eucPre;

	public Sprite[] icos;

	private List<Button> buttons = new List<Button>();

	private List<Button> kefuButtons = new List<Button>();

	private List<Button> vipButtons = new List<Button>();

	private List<Text> vipTexts = new List<Text>();

	private List<string> keFuUrl_List = new List<string>();

	private Transform leftShow;

	private Transform rightShow;

	private List<Button> zhiShaoMaBtns = new List<Button>();

	private List<string> zhiShaoMaUrl_List = new List<string>();

	private List<Button> zhiXiaoEBtns = new List<Button>();

	private List<string> zhiXiaoEUrl_List = new List<string>();

	private List<Button> weiXinBtns = new List<Button>();

	private List<string> weiXinUrl_List = new List<string>();

	private List<Button> EUCBtns = new List<Button>();

	private List<GameObject> JiLuGameObject = new List<GameObject>();

	private Text tuiJianTxt;

	private AndroidJavaObject jo;

	private AndroidJavaClass jc;

	private GameObject LoadAnim;

	private GameObject Tisp;

	private GameObject EUCPayGameObject;

	private int payNum;

	private string getKeFuUrl = string.Empty;

	private string getVIPUrl = string.Empty;

	private string getZhiShaoUrl = string.Empty;

	private string getZhiXiaoUrl = string.Empty;

	private string getWeiXinUrl = string.Empty;

	private string getJiLuUrl = string.Empty;

	private Text text;

	private InputField payInput;

	private void Awake()
	{
		leftShow = base.transform.Find("Container/RankGrid1/Viewport/Grid");
		rightShow = base.transform.Find("Container/RankGrid0/Viewport/Grid");
		LoadAnim = base.transform.Find("LoadAnim").gameObject;
		Tisp = base.transform.Find("Tisp").gameObject;
		tuiJianTxt = base.transform.Find("Container/TuiJianText").GetComponent<Text>();
		if (Application.platform == RuntimePlatform.Android)
		{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}
		InitUrl();
	}

	private void OnEnable()
	{
		base.transform.Find("Container").localScale = Vector3.zero;
		base.transform.Find("Container").DOScale(Vector3.one, 0.2f);
		LoadAnim.SetActive(value: true);
		Tisp.SetActive(value: false);
		tuiJianTxt.gameObject.SetActive(value: false);
		StartCoroutine(GetVipList(getVIPUrl));
		StartCoroutine(GetZhiShaoList(getZhiShaoUrl));
		StartCoroutine(GetZhiXiaoEList(getZhiXiaoUrl));
		StartCoroutine(GetWeiXinList(getWeiXinUrl));
		for (int i = 0; i < buttons.Count; i++)
		{
			buttons[i].transform.GetChild(0).gameObject.SetActive(value: false);
		}
		if (buttons.Count > 1)
		{
			buttons[1].transform.GetChild(0).gameObject.SetActive(value: true);
		}
		for (int j = 0; j < rightShow.childCount; j++)
		{
			if (rightShow.GetChild(j).name.Equals("VipChong"))
			{
				rightShow.GetChild(j).gameObject.SetActive(value: true);
			}
			else
			{
				rightShow.GetChild(j).gameObject.SetActive(value: false);
			}
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < rightShow.childCount; i++)
		{
			UnityEngine.Object.Destroy(rightShow.GetChild(i).gameObject);
		}
	}

	private void Start()
	{
		for (int i = 0; i < leftShow.childCount; i++)
		{
			buttons.Add(leftShow.GetChild(i).GetComponent<Button>());
			LevelButtonEvent component = buttons[i].GetComponent<LevelButtonEvent>();
			component.id = i;
			component.onLevelButtonOnClick += LevelButtonNum_onLevelButtonOnClick;
		}
	}

	private void InitUrl()
	{
		getKeFuUrl = "http://" + ZH2_GVars.IPAddress + ":81/Aeqw1FOd312s.php/index/index/kefu_cz";
		getVIPUrl = "http://" + ZH2_GVars.IPAddress + ":81/Aeqw1FOd312s.php/index/index/weixin_cz";
		getZhiShaoUrl = "http://" + ZH2_GVars.IPAddress + ":81/Aeqw1FOd312s.php/index/index/zfb_cz";
		getZhiXiaoUrl = "http://" + ZH2_GVars.IPAddress + ":81/Aeqw1FOd312s.php/index/index/zfb_min_cz";
		getWeiXinUrl = "http://" + ZH2_GVars.IPAddress + ":81/Aeqw1FOd312s.php/index/index/weixin_cz";
		getJiLuUrl = "http://" + ZH2_GVars.IPAddress + ":81/Aeqw1FOd312s.php/index/index/cz_record";
	}

	private IEnumerator GetKeFuList(string url)
	{
		keFuUrl_List = new List<string>();
		kefuButtons = new List<Button>();
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			UnityEngine.Debug.LogError("JD: " + jsonData.ToJson());
			try
			{
				for (int i = 0; i < int.Parse(jsonData["count"].ToString()); i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(kefuButtonPre);
					gameObject.name = "keFuChong";
					gameObject.transform.parent = rightShow;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.Find("text").GetComponent<Text>().text = jsonData[i]["name"].ToString();
					keFuUrl_List.Add(jsonData[i]["url"].ToString());
					kefuButtons.Add(gameObject.transform.Find("Button").GetComponent<Button>());
				}
				for (int j = 0; j < kefuButtons.Count; j++)
				{
					LevelButtonEvent component = kefuButtons[j].GetComponent<LevelButtonEvent>();
					component.id = j;
					component.onLevelButtonOnClick += LevelKeFuButtonOnClick;
				}
				LoadAnim.SetActive(value: false);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private IEnumerator GetVipList(string url)
	{
		UnityEngine.Debug.LogError("Vip链接: " + url);
		vipButtons = new List<Button>();
		vipTexts = new List<Text>();
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			UnityEngine.Debug.LogError("JD: " + jsonData.ToJson());
			try
			{
				for (int i = 0; i < int.Parse(jsonData["count"].ToString()); i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(vipButtonPre);
					gameObject.name = "VipChong";
					gameObject.transform.parent = rightShow;
					gameObject.transform.localScale = Vector3.one;
					if (jsonData[i]["type"].ToString() == "1")
					{
						gameObject.transform.Find("ico").GetComponent<Image>().sprite = icos[0];
					}
					else
					{
						gameObject.transform.Find("ico").GetComponent<Image>().sprite = icos[1];
					}
					gameObject.transform.Find("text").GetComponent<Text>().text = jsonData[i]["url"].ToString();
					vipTexts.Add(gameObject.transform.Find("text").GetComponent<Text>());
					vipButtons.Add(gameObject.transform.Find("Button").GetComponent<Button>());
				}
				for (int j = 0; j < vipButtons.Count; j++)
				{
					LevelButtonEvent component = vipButtons[j].GetComponent<LevelButtonEvent>();
					component.id = j;
					component.onLevelButtonOnClick += LevelVIPButtonOnClick;
				}
				EUCPayGameObject = UnityEngine.Object.Instantiate(eucPre);
				EUCPayGameObject.name = "EucPay";
				EUCPayGameObject.transform.parent = rightShow;
				EUCPayGameObject.transform.localScale = Vector3.one;
				EUCPayGameObject.SetActive(value: false);
				LoadAnim.SetActive(value: false);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private IEnumerator GetZhiShaoList(string url)
	{
		zhiShaoMaUrl_List = new List<string>();
		zhiShaoMaBtns = new List<Button>();
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			try
			{
				for (int i = 0; i < int.Parse(jsonData["count"].ToString()); i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(zhiShaoMaPre);
					gameObject.name = "ZhiShaoMa";
					gameObject.transform.parent = rightShow;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.Find("name").GetComponent<Text>().text = jsonData[i]["name"].ToString();
					gameObject.transform.Find("num").GetComponent<Text>().text = jsonData[i]["amount"].ToString();
					zhiShaoMaBtns.Add(gameObject.transform.Find("Button").GetComponent<Button>());
					zhiShaoMaUrl_List.Add(jsonData[i]["url"].ToString());
				}
				for (int j = 0; j < zhiShaoMaBtns.Count; j++)
				{
					LevelButtonEvent component = zhiShaoMaBtns[j].GetComponent<LevelButtonEvent>();
					component.id = j;
					component.onLevelButtonOnClick += LevelZhiShaoMaButtonOnClick;
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private IEnumerator GetZhiXiaoEList(string url)
	{
		zhiXiaoEUrl_List = new List<string>();
		zhiXiaoEBtns = new List<Button>();
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			try
			{
				for (int i = 0; i < int.Parse(jsonData["count"].ToString()); i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(zhiXiaoEPre);
					gameObject.name = "ZhiXiaoE";
					gameObject.transform.parent = rightShow;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.Find("name").GetComponent<Text>().text = jsonData[i]["name"].ToString();
					gameObject.transform.Find("num").GetComponent<Text>().text = jsonData[i]["amount"].ToString();
					zhiXiaoEBtns.Add(gameObject.transform.Find("Button").GetComponent<Button>());
					zhiXiaoEUrl_List.Add(jsonData[i]["url"].ToString());
				}
				for (int j = 0; j < zhiXiaoEBtns.Count; j++)
				{
					LevelButtonEvent component = zhiXiaoEBtns[j].GetComponent<LevelButtonEvent>();
					component.id = j;
					component.onLevelButtonOnClick += LevelZhiXiaoEButtonOnClick;
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private IEnumerator GetWeiXinList(string url)
	{
		weiXinUrl_List = new List<string>();
		weiXinBtns = new List<Button>();
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			try
			{
				for (int i = 0; i < int.Parse(jsonData["count"].ToString()); i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(weiWangYePre);
					gameObject.name = "WeiWangYe";
					gameObject.transform.parent = rightShow;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.Find("name").GetComponent<Text>().text = jsonData[i]["name"].ToString();
					gameObject.transform.Find("num").GetComponent<Text>().text = jsonData[i]["amount"].ToString();
					weiXinBtns.Add(gameObject.transform.Find("Button").GetComponent<Button>());
					weiXinUrl_List.Add(jsonData[i]["url"].ToString());
				}
				for (int j = 0; j < weiXinBtns.Count; j++)
				{
					LevelButtonEvent component = weiXinBtns[j].GetComponent<LevelButtonEvent>();
					component.id = j;
					component.onLevelButtonOnClick += LevelWeiXinButtonOnClick;
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private IEnumerator GetJiLuList(string url)
	{
		url = url + "?user_id=" + ZH2_GVars.user.id;
		UnityEngine.Debug.LogError("访问链接: " + url);
		LoadAnim.SetActive(value: true);
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			UnityEngine.Debug.LogError("JD: " + jsonData.ToJson());
			try
			{
				for (int i = 0; i < JiLuGameObject.Count; i++)
				{
					UnityEngine.Object.Destroy(JiLuGameObject[i]);
				}
				JiLuGameObject = new List<GameObject>();
				for (int j = 0; j < int.Parse(jsonData["count"].ToString()); j++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(jiLuPre);
					gameObject.name = "JiLu";
					gameObject.transform.parent = rightShow;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.Find("time").GetComponent<Text>().text = jsonData[j]["pay_time"].ToString();
					gameObject.transform.Find("danHao").GetComponent<Text>().text = jsonData[j]["external_no"].ToString();
					gameObject.transform.Find("tongDao").GetComponent<Text>().text = ((int.Parse(jsonData[j]["pay_way"].ToString()) != 0) ? "余额支付" : "承兑商支付");
					gameObject.transform.Find("jinE").GetComponent<Text>().text = jsonData[j]["amount"].ToString();
					gameObject.transform.Find("zhuangKuang").GetComponent<Text>().text = ((int.Parse((!(jsonData[j]["status"].ToString() == string.Empty)) ? jsonData[j]["status"].ToString() : "0") != 1) ? "异常" : "成功");
					gameObject.SetActive(value: true);
					JiLuGameObject.Add(gameObject);
				}
				LoadAnim.SetActive(value: false);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private void LevelButtonNum_onLevelButtonOnClick(int obj)
	{
		UnityEngine.Debug.Log("点击左第 " + obj + " 个按钮");
		Tisp.SetActive(value: false);
		LoadAnim.SetActive(value: false);
		tuiJianTxt.gameObject.SetActive(value: false);
		for (int i = 0; i < buttons.Count; i++)
		{
			buttons[i].transform.GetChild(0).gameObject.SetActive(value: false);
		}
		buttons[obj].transform.GetChild(0).gameObject.SetActive(value: true);
		switch (obj)
		{
		case 0:
			for (int n = 0; n < rightShow.childCount; n++)
			{
				if (rightShow.GetChild(n).name.Equals("keFuChong"))
				{
					rightShow.GetChild(n).gameObject.SetActive(value: true);
				}
				else
				{
					rightShow.GetChild(n).gameObject.SetActive(value: false);
				}
			}
			return;
		case 1:
			for (int j = 0; j < rightShow.childCount; j++)
			{
				if (rightShow.GetChild(j).name.Equals("VipChong"))
				{
					rightShow.GetChild(j).gameObject.SetActive(value: true);
				}
				else
				{
					rightShow.GetChild(j).gameObject.SetActive(value: false);
				}
			}
			return;
		case 3:
			for (int l = 0; l < rightShow.childCount; l++)
			{
				if (rightShow.GetChild(l).name.Equals("ZhiShaoMa"))
				{
					rightShow.GetChild(l).gameObject.SetActive(value: true);
				}
				else
				{
					rightShow.GetChild(l).gameObject.SetActive(value: false);
				}
			}
			return;
		case 2:
			tuiJianTxt.gameObject.SetActive(value: true);
			for (int num = 0; num < rightShow.childCount; num++)
			{
				if (rightShow.GetChild(num).name.Equals("ZhiXiaoE"))
				{
					rightShow.GetChild(num).gameObject.SetActive(value: true);
				}
				else
				{
					rightShow.GetChild(num).gameObject.SetActive(value: false);
				}
			}
			return;
		case 4:
			for (int m = 0; m < rightShow.childCount; m++)
			{
				if (rightShow.GetChild(m).name.Equals("WeiWangYe"))
				{
					rightShow.GetChild(m).gameObject.SetActive(value: true);
				}
				else
				{
					rightShow.GetChild(m).gameObject.SetActive(value: false);
				}
			}
			return;
		case 5:
			Tisp.SetActive(value: true);
			StartCoroutine(GetJiLuList(getJiLuUrl));
			for (int k = 0; k < rightShow.childCount; k++)
			{
				if (rightShow.GetChild(k).name.Equals("JiLu"))
				{
					rightShow.GetChild(k).gameObject.SetActive(value: true);
				}
				else
				{
					rightShow.GetChild(k).gameObject.SetActive(value: false);
				}
			}
			return;
		}
		for (int num2 = 0; num2 < rightShow.childCount; num2++)
		{
			if (rightShow.GetChild(num2).name.Equals("keFuChong"))
			{
				rightShow.GetChild(num2).gameObject.SetActive(value: true);
			}
			else
			{
				rightShow.GetChild(num2).gameObject.SetActive(value: false);
			}
		}
	}

	private void ZhiShaoMaButtonOnClick()
	{
	}

	private void LevelVIPButtonOnClick(int obj)
	{
		UnityEngine.Debug.Log("点击vip第 " + obj + " 个按钮");
		ClickBtnCopy(vipTexts[obj]);
	}

	private void ClickBtnCopy(Text text)
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		string empty = string.Empty;
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			CopyTextToClipboard.instance.OnCopy(text.text);
		}
		else
		{
			TextEditor textEditor = new TextEditor();
			textEditor.text = text.text;
			TextEditor textEditor2 = textEditor;
			textEditor2.OnFocus();
			textEditor2.Copy();
		}
		All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("复制成功", "Copy successfully", string.Empty));
	}

	private void LevelKeFuButtonOnClick(int obj)
	{
		UnityEngine.Debug.Log("点击客服第 " + obj + " 个按钮");
		OpenWeb.intance.OnOpenWebUrl(keFuUrl_List[obj]);
	}

	private void LevelZhiShaoMaButtonOnClick(int obj)
	{
		UnityEngine.Debug.Log("点击支付宝扫码第 " + obj + " 个按钮");
		OpenWeb.intance.OnOpenWebUrl(zhiShaoMaUrl_List[obj]);
	}

	private void LevelEUCButtonOnClick(int obj)
	{
		UnityEngine.Debug.Log("点击EUC第 " + obj + " 个按钮");
		switch (obj)
		{
		case 0:
			payNum = 100;
			payInput.text = payNum + string.Empty;
			break;
		case 1:
			payNum = 200;
			payInput.text = payNum + string.Empty;
			break;
		case 2:
			payNum = 500;
			payInput.text = payNum + string.Empty;
			break;
		case 3:
			if (payInput.text == string.Empty || int.Parse(payInput.text) < 50 || int.Parse(payInput.text) > 500)
			{
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please check the amount of recharge!" : ((ZH2_GVars.language_enum != 0) ? "โปรดตรวจสอบจำนวนเง\u0e34น ท\u0e35\u0e48ชาร\u0e4cจ" : "请检查充值金额"));
				payInput.text = string.Empty;
			}
			else
			{
				StartCoroutine(PostPay0(zhiXiaoEUrl_List[0], ZH2_GVars.user.id.ToString(), payInput.text));
			}
			break;
		}
	}

	private IEnumerator PostPay0(string url, string id, string payNum)
	{
		yield return null;
		LoadAnim.SetActive(value: true);
		string Url = url + "?user_id=" + id + "&amount=" + payNum;
		yield return new WaitForSeconds(0.5f);
		LoadAnim.SetActive(value: false);
		PayLongLink();
		JsonUtility.ToJson(new ReqData
		{
			id = ZH2_GVars.user.id.ToString(),
			type = "1"
		});
		UnityEngine.Debug.LogError("支付链接: " + Url);
		OpenWeb.intance.OnOpenWebUrl(Url);
	}

	private void PayLongLink()
	{
	}

	private IEnumerator GetMoney(string url)
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			UnityWebRequest www = UnityWebRequest.Get(url);
			www.timeout = 10000;
			yield return www.Send();
			if (www.error == null)
			{
				JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
				UnityEngine.Debug.LogError("JD: " + jsonData.ToJson());
				try
				{
					ZH2_GVars.user.gameGold = int.Parse(jsonData["game_gold"].ToString());
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("     " + arg);
					throw;
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	private IEnumerator GetPay(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			UnityEngine.Debug.LogError("JD: " + jsonData.ToJson());
			try
			{
				OpenWeb.intance.OnOpenWebUrl(jsonData["acc_buy_url"].ToString());
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private IEnumerator PostPay(string url, string id, string payNum)
	{
		id = "ding888";
		payNum = 100 + string.Empty;
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("user_id ", id);
		wwwform.AddField("amount", payNum);
		WWW request = new WWW(url, wwwform);
		yield return request;
		if (request.error != null)
		{
			UnityEngine.Debug.LogError("访问外连接错误：" + request.error);
			yield break;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			UnityEngine.Debug.Log(request.error);
			yield break;
		}
		JsonData jsonData = JsonMapper.ToObject(request.text);
		UnityEngine.Debug.LogError(jsonData);
		UnityEngine.Debug.LogError("收到返回: " + jsonData["msg"]);
	}

	private void LevelZhiXiaoEButtonOnClick(int obj)
	{
		UnityEngine.Debug.Log("点击支付宝小额第 " + obj + " 个按钮");
		tuiJianTxt.gameObject.SetActive(value: false);
		for (int i = 0; i < rightShow.childCount; i++)
		{
			rightShow.GetChild(i).gameObject.SetActive(value: false);
		}
		EUCBtns = new List<Button>();
		EUCBtns.Add(EUCPayGameObject.transform.Find("Button100").GetComponent<Button>());
		EUCBtns.Add(EUCPayGameObject.transform.Find("Button200").GetComponent<Button>());
		EUCBtns.Add(EUCPayGameObject.transform.Find("Button500").GetComponent<Button>());
		EUCBtns.Add(EUCPayGameObject.transform.Find("ButtonSure").GetComponent<Button>());
		for (int j = 0; j < EUCBtns.Count; j++)
		{
			LevelButtonEvent component = EUCBtns[j].GetComponent<LevelButtonEvent>();
			component.id = j;
			component.onLevelButtonOnClick += LevelEUCButtonOnClick;
		}
		EUCPayGameObject.SetActive(value: true);
		payInput = EUCPayGameObject.transform.Find("InputField").GetComponent<InputField>();
		payInput.text = string.Empty;
	}

	private void LevelWeiXinButtonOnClick(int obj)
	{
		UnityEngine.Debug.Log("点击微信第 " + obj + " 个按钮");
		OpenWeb.intance.OnOpenWebUrl(weiXinUrl_List[obj]);
	}
}
