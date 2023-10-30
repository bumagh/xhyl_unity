using BCBM_GameCommon;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_AppUIMngr : BCBM_Observer
{
	public static BCBM_AppUIMngr G_AppUIMngr;

	public BCBM_TableList mTableList;

	public Text coinCount;

	public Text coinTestCount;

	public Text gameName;

	public GameObject u_Canvas;

	public List<Sprite> icoSprite = new List<Sprite>();

	public Transform Content;

	public GameObject[] tablePre;

	public Image imaIco;

	public List<GameObject> tableList = new List<GameObject>();

	private bool isFirstTimeEnterRoom = true;

	private bool isAttach;

	public JsonData hallInfo = new JsonData();

	private Coroutine waitOnEnable;

	private BCBM_Desk[] allTableArr;

	public AppState GetAppState
	{
		get;
		set;
	} = AppState.App_On_RoomList_Panel;


	public static BCBM_AppUIMngr GetSingleton()
	{
		return G_AppUIMngr;
	}

	private void Awake()
	{
		G_AppUIMngr = this;
	}

	public void OnEnable1()
	{
		hallInfo = new JsonData();
		if ((bool)gameName)
		{
			gameName.text = ZH2_GVars.GetBreviaryName(ZH2_GVars.nickname);
		}
	}

	public void OnEnable()
	{
		hallInfo = new JsonData();
		if (waitOnEnable != null)
		{
			StopCoroutine(waitOnEnable);
		}
		waitOnEnable = StartCoroutine(WaitOnEnable());
		UpdateUserInfo();
	}

	private IEnumerator WaitOnEnable()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.25f);
			if (ZH2_GVars.hallInfo2 != null && hallInfo != ZH2_GVars.hallInfo2)
			{
				hallInfo = ZH2_GVars.hallInfo2;
				ShowHall();
			}
		}
	}

	private void Update()
	{
		if (!isAttach)
		{
			isAttach = true;
			BCBM_GameMsgMngr.GetSingleton().BackBtnMsgAttach(this);
			BCBM_GameMsgMngr.GetSingleton().QuitMsgAttach(this);
		}
	}

	public void ShowHall()
	{
		if (hallInfo.Count <= 0)
		{
			UnityEngine.Debug.LogError("======信息个数少于1个");
			return;
		}
		UnityEngine.Debug.LogError("房间导航栏信息: " + hallInfo.ToJson());
		BCBM_Desk[] array = new BCBM_Desk[hallInfo.Count];
		for (int i = 0; i < hallInfo.Count; i++)
		{
			array[i] = new BCBM_Desk();
			string prop_name = (i + 1).ToString();
			array[i].minGold = (int)hallInfo[prop_name]["minGold"];
			array[i].id = (int)hallInfo[prop_name]["id"];
			array[i].onlineNumber = (int)hallInfo[prop_name]["onlineNumber"];
			array[i].roomId = (int)hallInfo[prop_name]["roomId"];
			array[i].name = (string)hallInfo[prop_name]["name"];
			array[i].minBet = (int)hallInfo[prop_name]["minBet"];
			array[i].maxBet = (int)hallInfo[prop_name]["maxBet"];
		}
		InItTable(array, 0);
	}

	public void UpdateUserInfo()
	{
		if (BCBM_GameInfo.getInstance() != null)
		{
			coinCount.text = (BCBM_GameInfo.getInstance().IsSpecial ? (BCBM_GameInfo.getInstance().UserInfo.CoinCount % 10000).ToString() : BCBM_GameInfo.getInstance().UserInfo.CoinCount.ToString());
			coinTestCount.text = (BCBM_GameInfo.getInstance().IsSpecial ? (BCBM_GameInfo.getInstance().UserInfo.ExpCoinCount % 10000).ToString() : BCBM_GameInfo.getInstance().UserInfo.ExpCoinCount.ToString());
			if (GetSingleton() != null)
			{
				int num = BCBM_GameInfo.getInstance().UserInfo.IconIndex;
				if (num <= 0 || num >= icoSprite.Count)
				{
					num = 1;
					BCBM_GameInfo.getInstance().UserInfo.IconIndex = 1;
				}
				imaIco.sprite = icoSprite[num];
			}
			else
			{
				UnityEngine.Debug.LogError("====BCBM_AppUIMngr为空====");
			}
		}
		else
		{
			UnityEngine.Debug.LogError("====updateUserInfo为空====");
		}
	}

	public void InItTable(BCBM_Desk[] tableArr, int roomId)
	{
		for (int i = 0; i < tableList.Count; i++)
		{
			UnityEngine.Object.Destroy(tableList[i].gameObject);
		}
		tableList = new List<GameObject>();
		if (tableArr == null)
		{
			UnityEngine.Debug.LogError("====tableArr为空====");
			return;
		}
		allTableArr = new BCBM_Desk[tableArr.Length];
		allTableArr = tableArr;
		for (int j = 0; j < allTableArr.Length; j++)
		{
			int num = j % tablePre.Length;
			GameObject item = Object.Instantiate(tablePre[num], Content);
			tableList.Add(item);
		}
		for (int k = 0; k < tableList.Count; k++)
		{
			tableList[k].transform.Find("Inifo/Name").GetComponent<Text>().text = allTableArr[k].name;
			tableList[k].transform.Find("OnLineText").GetComponent<Text>().text = allTableArr[k].onlineNumber.ToString();
			tableList[k].transform.Find("Inifo/min").GetComponent<Text>().text = ZH2_GVars.ShowTip("最小携带: ", "MiniCoin: ", string.Empty) + allTableArr[k].minGold;
		}
		for (int l = 0; l < tableList.Count; l++)
		{
			int index = l;
			tableList[index].transform.GetComponent<Button>().onClick.AddListener(delegate
			{
				ClickBtnArrow(allTableArr[index].name, allTableArr[index].roomId, allTableArr[index].id, allTableArr[index].onlineNumber, allTableArr[index].minGold);
			});
		}
	}

	private void ClickBtnArrow(string roomName, int roomId, int deskId, int index, int minCoin)
	{
		int num = 0;
		num = ((roomId > 0) ? (BCBM_GameInfo.getInstance().IsSpecial ? (BCBM_GameInfo.getInstance().UserInfo.CoinCount % 10000) : BCBM_GameInfo.getInstance().UserInfo.CoinCount) : (BCBM_GameInfo.getInstance().IsSpecial ? (BCBM_GameInfo.getInstance().UserInfo.ExpCoinCount % 10000) : BCBM_GameInfo.getInstance().UserInfo.ExpCoinCount));
		UnityEngine.Debug.LogError("min: " + minCoin + "  " + num);
		if (num < minCoin)
		{
			BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.Custom, "账号余额不足");
			return;
		}
		BCBM_GameInfo.getInstance().UserInfo.RoomId = roomId;
		BCBM_GameInfo.getInstance().UserInfo.RoomName = roomName;
		BCBM_GameInfo.getInstance().UserInfo.TableId = deskId;
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendDeskInfo(BCBM_GameInfo.getInstance().UserInfo.RoomId, deskId);
		BCBM_GameInfo.getInstance().UserInfo.SeatId = index;
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendSelectSeat(BCBM_GameInfo.getInstance().UserInfo.TableId, index);
	}

	public override void OnRcvMsg(int type, object obj)
	{
		switch (type)
		{
		case 0:
			switch (GetAppState)
			{
			case AppState.App_On_RoomList_Panel:
				break;
			case AppState.App_On_TableList_Panel:
				break;
			case AppState.App_On_Table:
				break;
			case AppState.App_On_Game:
				BCBM_GameMngr.GetSingleton().Reset();
				break;
			case AppState.APP_NET_ERROR:
				BCBM_GameMngr.GetSingleton().Reset();
				break;
			}
			break;
		case 1:
		{
			int num = (int)obj;
			if (num == 1 && GetAppState != AppState.App_On_RoomList_Panel)
			{
				BCBM_GameMngr.GetSingleton().Reset();
			}
			break;
		}
		}
	}
}
