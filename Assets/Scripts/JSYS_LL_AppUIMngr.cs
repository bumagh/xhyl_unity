using com.miracle9.game.bean;
using com.miracle9.game.entity;
using DG.Tweening;
using JSYS_LL_GameCommon;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSYS_LL_AppUIMngr : JSYS_LL_Observer
{
	public static JSYS_LL_AppUIMngr G_AppUIMngr;

	public JSYS_LL_RoomList mRoomList;

	public Text coinCount;

	public Text coinTestCount;

	public Text userName;

	public List<Sprite> icoSprite = new List<Sprite>();

	private Transform Content;

	private Transform ContentOldPos;

	private Transform ContentTagPos;

	public GameObject tablePreTest;

	public GameObject tablePreCoin;

	public Image imaIco;

	public GameObject noHallTip;

	public GameObject u_Canvas;

	private Transform scrollView;

	public List<GameObject> tableList = new List<GameObject>();

	public JSYS_SelectionManger circleScrollRect;

	public List<JSYS_LL_BtnInfo> selectBtnList = new List<JSYS_LL_BtnInfo>();

	private AppState m_appState = AppState.App_On_RoomList_Panel;

	private bool isFirstTimeEnterRoom = true;

	private bool isAttach;

	public JsonData hallInfo = new JsonData();

	private bool isOnEnter = true;

	[HideInInspector]
	public int tempSelectId = -1;

	private GoldSharkDesk[] allTableArr;

	public AppState GetAppState
	{
		get
		{
			return m_appState;
		}
		set
		{
			m_appState = value;
		}
	}

	public static JSYS_LL_AppUIMngr GetSingleton()
	{
		return G_AppUIMngr;
	}

	private void Awake()
	{
		scrollView = u_Canvas.transform.Find("JSYS_Tables/Scroll View");
		Content = scrollView.Find("Viewport/Content");
		ContentOldPos = scrollView.Find("Viewport/ContentOldPos");
		ContentTagPos = scrollView.Find("Viewport/ContentTagPos");
		init();
	}

	public void OnEnable()
	{
		tempSelectId = -1;
		isOnEnter = true;
		hallInfo = new JsonData();
		selectBtnList = new List<JSYS_LL_BtnInfo>();
		for (int i = 0; i < circleScrollRect.Selection_List.Count; i++)
		{
			selectBtnList.Add(circleScrollRect.Selection_List[i].GetComponent<JSYS_LL_BtnInfo>());
		}
		if (ZH2_GVars.hallInfo2 != null && hallInfo != ZH2_GVars.hallInfo2)
		{
			ShowHall();
			hallInfo = ZH2_GVars.hallInfo2;
		}
	}

	private void Update()
	{
		if (tempSelectId != ZH2_GVars.selectRoomId)
		{
			tempSelectId = ZH2_GVars.selectRoomId;
			StartCoroutine(ClickBtnRoom(tempSelectId));
		}
		if (!isAttach)
		{
			isAttach = true;
			JSYS_LL_GameMsgMngr.GetSingleton().BackBtnMsgAttach(this);
			JSYS_LL_GameMsgMngr.GetSingleton().QuitMsgAttach(this);
		}
	}

	private IEnumerator ClickBtnRoom(int index)
	{
		SetNoHall(isHavHall: true);
		Transform content = Content;
		Vector3 localPosition = ContentTagPos.localPosition;
		content.DOLocalMoveY(localPosition.y, 0.2f);
		float Time = (!isOnEnter) ? 0.25f : 0.05f;
		isOnEnter = false;
		yield return new WaitForSeconds(Time);
		int id = selectBtnList[index].hallId;
		int roomId = selectBtnList[index].hallType;
		if (roomId <= 0)
		{
			coinCount.gameObject.SetActive(value: false);
			coinTestCount.gameObject.SetActive(value: true);
		}
		else
		{
			coinCount.gameObject.SetActive(value: true);
			coinTestCount.gameObject.SetActive(value: false);
		}
		JSYS_LL_GameInfo.getInstance().UserInfo.RoomId = roomId;
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendEnterHall(id);
		updateUserInfo();
	}

	public void ShowHall()
	{
		JsonData jsonData = new JsonData();
		jsonData = ZH2_GVars.hallInfo2;
		UnityEngine.Debug.LogError("房间导航栏信息: " + jsonData.ToJson());
		if (selectBtnList.Count <= 0)
		{
			UnityEngine.Debug.LogError("未获取完毕");
			return;
		}
		for (int i = 0; i < jsonData.Count; i++)
		{
			selectBtnList[i].hallId = (int)jsonData[i.ToString()]["hallId"];
			selectBtnList[i].hallType = (int)jsonData[i.ToString()]["roomId"] - 1;
			selectBtnList[i].name = jsonData[i.ToString()]["hallName"].ToString();
			selectBtnList[i].minGlod = jsonData[i.ToString()]["minGold"].ToString();
			selectBtnList[i].onlinePeople = "0";
			selectBtnList[i].UpdateText();
		}
	}

	public void updateUserInfo()
	{
		if (JSYS_LL_GameInfo.getInstance() != null)
		{
			coinCount.text = (JSYS_LL_GameInfo.getInstance().IsSpecial ? (JSYS_LL_GameInfo.getInstance().UserInfo.CoinCount % 10000).ToString() : JSYS_LL_GameInfo.getInstance().UserInfo.CoinCount.ToString());
			coinTestCount.text = (JSYS_LL_GameInfo.getInstance().IsSpecial ? (JSYS_LL_GameInfo.getInstance().UserInfo.ExpCoinCount % 10000).ToString() : JSYS_LL_GameInfo.getInstance().UserInfo.ExpCoinCount.ToString());
			if (GetSingleton() != null)
			{
				if (ZH2_GVars.hallInfo2 != null && hallInfo != ZH2_GVars.hallInfo2)
				{
					ShowHall();
					hallInfo = ZH2_GVars.hallInfo2;
				}
				int num = JSYS_LL_GameInfo.getInstance().UserInfo.IconIndex;
				if (num <= 0 || num >= icoSprite.Count)
				{
					num = 1;
					JSYS_LL_GameInfo.getInstance().UserInfo.IconIndex = 1;
				}
				imaIco.sprite = icoSprite[num];
			}
			else
			{
				UnityEngine.Debug.LogError("====JSYS_LL_AppUIMngr为空====");
			}
		}
		else
		{
			UnityEngine.Debug.LogError("====updateUserInfo为空====");
		}
	}

	public void SetNoHall(bool isHavHall)
	{
		noHallTip.SetActive(!isHavHall);
	}

	public void InItTable(GoldSharkDesk[] tableArr, int roomId)
	{
		for (int i = 0; i < tableList.Count; i++)
		{
			UnityEngine.Object.Destroy(tableList[i].gameObject);
		}
		if (Content != null)
		{
			Transform content = Content;
			Vector3 localPosition = ContentOldPos.localPosition;
			content.DOLocalMoveY(localPosition.y, 0.2f);
		}
		tableList = new List<GameObject>();
		if (tableArr != null)
		{
			allTableArr = new GoldSharkDesk[tableArr.Length];
			allTableArr = tableArr;
			for (int j = 0; j < allTableArr.Length; j++)
			{
				GameObject item = Object.Instantiate((allTableArr[j].roomId != 1) ? tablePreCoin : tablePreTest, Content);
				tableList.Add(item);
			}
			for (int k = 0; k < tableList.Count; k++)
			{
				tableList[k].transform.Find("Inifo/Name").GetComponent<Text>().text = allTableArr[k].name;
				tableList[k].transform.Find("Inifo/min").GetComponent<Text>().text = ZH2_GVars.ShowTip($"最小押注: {allTableArr[k].minBet}", $"MinimumBet: {allTableArr[k].minBet}", string.Empty);
				tableList[k].transform.Find("PlayerNum/Text").GetComponent<Text>().text = allTableArr[k].onlineNumber.ToString();
			}
			for (int l = 0; l < tableList.Count; l++)
			{
				int index = l;
				tableList[index].GetComponent<Button>().onClick.AddListener(delegate
				{
					ClickBtnArrow(allTableArr[index].id);
				});
			}
		}
	}

	private void ClickBtnArrow(int deskId)
	{
		JSYS_LL_GameInfo.getInstance().UserInfo.TableId = deskId;
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendDeskInfo(JSYS_LL_GameInfo.getInstance().UserInfo.RoomId, deskId);
	}

	private void init()
	{
		if (G_AppUIMngr == null)
		{
			G_AppUIMngr = this;
		}
	}

	public void InSeat(Seat[] netSeats)
	{
		int num = 0;
		for (int i = 0; i < netSeats.Length; i++)
		{
			int num2 = i;
			if (netSeats[num2].isFree && num <= 0)
			{
				num = 1;
				JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendSelectSeat(JSYS_LL_GameInfo.getInstance().UserInfo.TableId, netSeats[num2].id);
				JSYS_LL_GameInfo.getInstance().UserInfo.SeatId = netSeats[num2].id;
				UnityEngine.Debug.LogError("选择了: " + netSeats[num2].id + " 号位");
				break;
			}
		}
		if (num > 0)
		{
		}
	}

	public override void OnRcvMsg(int type, object obj)
	{
		switch (type)
		{
		case 0:
			switch (m_appState)
			{
			case AppState.App_On_RoomList_Panel:
				break;
			case AppState.App_On_TableList_Panel:
				break;
			case AppState.App_On_Table:
				break;
			case AppState.App_On_Game:
				JSYS_LL_GameMngr.GetSingleton().Reset();
				break;
			case AppState.APP_NET_ERROR:
				JSYS_LL_GameMngr.GetSingleton().Reset();
				break;
			}
			break;
		case 1:
		{
			int num = (int)obj;
			if (num == 1 && m_appState != AppState.App_On_RoomList_Panel)
			{
				JSYS_LL_GameMngr.GetSingleton().Reset();
			}
			break;
		}
		}
	}
}
