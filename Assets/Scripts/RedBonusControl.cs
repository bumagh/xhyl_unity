using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedBonusControl : MonoBehaviour
{
	public GameObject redBonus;

	public static RedBonusControl _redBonusControl;

	private int bonusCount;

	private int bonusGold;

	private bool hasInitialize;

	private ArrayList redBonuses = new ArrayList();

	private float randomNum = 300f;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		if (_redBonusControl == null)
		{
			_redBonusControl = this;
		}
		GameObject gameObject = base.transform.Find("TestButton").gameObject;
		GameObject gameObject2 = base.transform.Find("TestButton2").gameObject;
		gameObject.GetComponent<Button>().onClick.AddListener(TestBonus);
		gameObject2.GetComponent<Button>().onClick.AddListener(TestBonus2);
		MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Click_RedBonus, this, ClickRedBonus);
	}

	private void Start()
	{
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("getBonus", HandleNetMsg_GetBonus);
	}

	public void RedBonusStart()
	{
		RedBonusInitialize();
	}

	private void RedBonusInitialize()
	{
		if (hasInitialize)
		{
			return;
		}
		bonusCount = 0;
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = base.transform;
		gameObject.name = "Collection";
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		for (int i = 0; i < 30; i++)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(redBonus);
			gameObject2.transform.parent = base.transform.Find("Collection");
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject2.name = Convert.ToString(i);
			if (randomNum > 0f)
			{
				randomNum = UnityEngine.Random.Range(-590, -1);
			}
			else
			{
				randomNum = UnityEngine.Random.Range(1, 590);
			}
			gameObject2.transform.localPosition = new Vector3(randomNum, 535 + i * 160, 0f);
			gameObject2.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, UnityEngine.Random.Range(65, -65)));
			redBonuses.Add(gameObject2);
		}
		hasInitialize = true;
	}

	private void ClickRedBonus(object obj)
	{
		if (!ZH2_GVars.lockSend && bonusCount < 3)
		{
			UnityEngine.Debug.Log("getBonusgetBonusgetBonus");
			MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getBonus", new object[0]);
			ZH2_GVars.lockSend = true;
		}
	}

	public void RedBonusStop()
	{
		RedBonusClose();
		base.transform.Find("RedBonus_No").gameObject.SetActive(value: false);
		base.transform.Find("RedBonus_Yes").gameObject.SetActive(value: false);
		base.transform.gameObject.SetActive(value: false);
		MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The red packet money rain has finished" : ((ZH2_GVars.language_enum != 0) ? "ถ\u0e38งส\u0e35แดงหมดแล\u0e49ว " : "红包雨已经下完"));
	}

	public void MaintenanceRedBonusClose()
	{
		RedBonusClose();
		base.transform.Find("RedBonus_No").gameObject.SetActive(value: false);
		base.transform.Find("RedBonus_Yes").gameObject.SetActive(value: false);
		base.transform.gameObject.SetActive(value: false);
	}

	public void RedBonusClose()
	{
		hasInitialize = false;
		for (int i = 0; i < redBonuses.Count; i++)
		{
			UnityEngine.Object.Destroy((GameObject)redBonuses[i]);
		}
		redBonuses.Clear();
	}

	private void TestBonus()
	{
		RedBonusStart();
	}

	private void TestBonus2()
	{
		RedBonusClose();
	}

	private IEnumerator waitForEffect()
	{
		yield return new WaitForSeconds(2.5f);
		base.transform.Find("RedBonus_Yes/gameGoldAdd").gameObject.SetActive(bonusGold > 0);
		base.transform.Find("RedBonus_Yes/gameGoldAdd").GetComponent<Text>().text = string.Format("+{0}{1}", bonusGold, (ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "coins" : ((ZH2_GVars.language_enum != 0) ? "ช\u0e37\u0e48อเกม" : "游戏币"));
		MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
		if (bonusCount >= 3)
		{
			RedBonusClose();
		}
		yield return new WaitForSeconds(2.5f);
		base.transform.Find("RedBonus_No").gameObject.SetActive(value: false);
		base.transform.Find("RedBonus_Yes").gameObject.SetActive(value: false);
		base.transform.Find("RedBonus_Yes/gameGoldAdd").gameObject.SetActive(value: false);
		if (bonusCount >= 3)
		{
			base.transform.gameObject.SetActive(value: false);
		}
	}

	private void HandleNetMsg_GetBonus(object[] objs)
	{
		UnityEngine.Debug.Log("##########bonusCount############: " + bonusCount);
		ZH2_GVars.lockSend = false;
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		UnityEngine.Debug.Log(dictionary);
		if ((bool)dictionary["success"])
		{
			bonusGold = Convert.ToInt32(dictionary["bonusGold"]);
			if (bonusGold == -1)
			{
				MaintenanceRedBonusClose();
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The red is finished, don't miss it next time!" : ((ZH2_GVars.language_enum != 0) ? "กระเป\u0e4bาส\u0e35แดงหมดแล\u0e49วคราวหน\u0e49าอย\u0e48าพลาดนะ " : "本次红包已被抢完，下次可不要错过哦！"));
				return;
			}
			ZH2_GVars.user.gameGold = Convert.ToInt32(dictionary["gameGold"]);
			base.transform.Find("RedBonus_Yes").gameObject.SetActive(bonusGold != 0);
			base.transform.Find("RedBonus_No").gameObject.SetActive(bonusGold == 0);
			bonusCount++;
			StartCoroutine("waitForEffect");
		}
		else
		{
			int num = (int)dictionary["msgCode"];
			UnityEngine.Debug.Log(num);
			int num2 = num;
			if (num2 != 20)
			{
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Receive failed" : ((ZH2_GVars.language_enum != 0) ? "การร\u0e31บค\u0e48าล\u0e49มเหลว" : "领取失败"));
				return;
			}
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Receive red packets more than 3 times,Not receive again" : ((ZH2_GVars.language_enum != 0) ? "ปลอกกระเป\u0e4bาส\u0e35แดงมากกว\u0e48า 3 คร\u0e31\u0e49งไม\u0e48สามารถร\u0e31บได\u0e49" : "领红包超过3次，不能再领"));
			RedBonusClose();
			base.transform.gameObject.SetActive(value: false);
		}
	}
}
