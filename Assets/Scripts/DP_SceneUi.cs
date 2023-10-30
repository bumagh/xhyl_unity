using com.miracle9.game.entity;
using DP_GameCommon;
using DP_UICommon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DP_SceneUi : MonoBehaviour
{
	private Button btnBack;

	private Transform tfTable;

	private Transform tfSeat;

	private Image imgTableIdBg;

	private Text txtTableId;

	private DP_TableItem[] tableItems;

	private DP_SeatItems[] seatItems;

	[SerializeField]
	private Sprite[] spiTableIdBgs = new Sprite[2];

	[SerializeField]
	private Sprite[] spiPersonSex = new Sprite[2];

	private DP_GameInfo mGameInfo;

	private DP_PersonInfo mUserInfo;

	private int roomId = 1;

	private List<DP_TableInfo> mTableInfoList = new List<DP_TableInfo>();

	private int mCurrentTableId = -1;

	private GameObject objLoad;

	private Slider slider;

	private AsyncOperation async;

	public void Init()
	{
		btnBack = base.transform.Find("BtnBack").GetComponent<Button>();
		tfTable = base.transform.Find("Table");
		tfSeat = base.transform.Find("Seat");
		imgTableIdBg = tfSeat.Find("ImgTableNameBg").GetComponent<Image>();
		txtTableId = imgTableIdBg.transform.GetChild(0).GetComponent<Text>();
		tableItems = new DP_TableItem[3];
		btnBack.onClick.AddListener(ClickBtnBack);
		for (int i = 0; i < 3; i++)
		{
			tableItems[i] = tfTable.Find($"TableItem{i}").GetComponent<DP_TableItem>();
			tableItems[i].Init();
			int index = i;
			tableItems[index].btnTable.onClick.AddListener(delegate
			{
				ClickBtnTable(index);
			});
		}
		seatItems = new DP_SeatItems[8];
		for (int j = 0; j < 8; j++)
		{
			seatItems[j] = tfSeat.Find($"SeatItem{j}").GetComponent<DP_SeatItems>();
			seatItems[j].Init();
			seatItems[j].txtSeatId.text = (j + 1).ToString();
			int index2 = j;
			seatItems[index2].btnSeat.onClick.AddListener(delegate
			{
				ClickBtnSeat(index2);
			});
		}
		objLoad = base.transform.Find("Loading").gameObject;
		objLoad.SetActive(value: false);
		slider = objLoad.transform.Find("Slider").GetComponent<Slider>();
		slider.value = 0f;
		async = null;
		mGameInfo = DP_GameInfo.getInstance();
		mGameInfo.SceneUi = this;
		mUserInfo = mGameInfo.UserInfo;
		tfSeat.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		if (async != null)
		{
			slider.value = async.progress;
		}
	}

	private void ClickBtnBack()
	{
		DP_SoundManager.GetSingleton().playButtonSound();
		if (mGameInfo.GetAppState == AppState.App_On_TableList_Panel)
		{
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.IsExitGame);
		}
		else if (mGameInfo.GetAppState == AppState.App_On_Table)
		{
			DP_NetMngr.GetSingleton().MyCreateSocket.SendLeaveDesk(mUserInfo.TableId);
			mUserInfo.TableId = -1;
			mUserInfo.SeatId = -1;
			tfSeat.gameObject.SetActive(value: false);
			tfTable.gameObject.SetActive(value: true);
		}
		else
		{
			BackToMainGame();
		}
	}

	public void BackToMainGame()
	{
		DP_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		SceneManager.LoadScene("MainScene");
		UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
	}

	private void ClickBtnTable(int index)
	{
		DP_SoundManager.GetSingleton().playButtonSound();
		mCurrentTableId = index;
		mGameInfo.TableInfo = mTableInfoList[mCurrentTableId];
		mUserInfo.TableId = mGameInfo.TableInfo.TableServerID;
		imgTableIdBg.sprite = tableItems[index].imgTableIdBg.sprite;
		txtTableId.text = tableItems[index].txtTableId.text;
		txtTableId.font = tableItems[index].txtTableId.font;
		DP_NetMngr.GetSingleton().MyCreateSocket.SendDeskInfo(mUserInfo.TableId);
	}

	private void ClickBtnSeat(int index)
	{
		DP_SoundManager.GetSingleton().playButtonSound();
		int num = mUserInfo.RoomId + 1;
		int num2 = (num != 1) ? mGameInfo.CoinCount : mGameInfo.ExpCoinCount;
		if (seatItems[index].bSeated)
		{
			if (index != mUserInfo.SeatId)
			{
				DP_NetMngr.GetSingleton().MyCreateSocket.SendPlayerInfo(mGameInfo.TableInfo.GetUserKeyID(index));
			}
		}
		else if (mUserInfo.SeatId == -1)
		{
			if (num2 >= mGameInfo.TableInfo.Ristrict)
			{
				mUserInfo.SeatId = index + 1;
				DP_NetMngr.GetSingleton().MyCreateSocket.SendSelectSeat(mUserInfo.TableId, index + 1);
			}
			else if (num == 1)
			{
				DP_TipManager.GetSingleton().ShowTip(EGameTipType.SelectTable_SendExpCoin);
			}
			else
			{
				DP_TipManager.GetSingleton().ShowTip(EGameTipType.SelectTable_CreditBelowRistrict);
			}
		}
	}

	public void InitTableList(DreamlandDesk[] deskList, int iRoomId = 1)
	{
		if (mGameInfo == null)
		{
			mGameInfo = DP_GameInfo.getInstance();
			mGameInfo.SceneUi = this;
		}
		if (mUserInfo == null)
		{
			mUserInfo = mGameInfo.UserInfo;
		}
		roomId = iRoomId;
		mUserInfo.RoomId = iRoomId - 1;
		mTableInfoList.Clear();
		mCurrentTableId = 0;
		for (int i = 0; i < deskList.Length; i++)
		{
			DP_TableInfo dP_TableInfo = new DP_TableInfo();
			dP_TableInfo.RoomId = deskList[i].roomId;
			dP_TableInfo.TableServerID = deskList[i].id;
			if ((mGameInfo.GetAppState == AppState.App_On_Game || mGameInfo.GetAppState == AppState.App_On_Table) && mGameInfo.TableInfo.TableServerID == dP_TableInfo.TableServerID)
			{
				mCurrentTableId = i;
			}
			dP_TableInfo.TableName = deskList[i].name;
			dP_TableInfo.MinBet = deskList[i].minBet;
			dP_TableInfo.MaxBet = deskList[i].maxBet;
			dP_TableInfo.Ristrict = deskList[i].minGold;
			dP_TableInfo.CreditPerCoin = deskList[i].exchange;
			dP_TableInfo.MinZHXBet = deskList[i].min_zxh;
			dP_TableInfo.MaxCD = deskList[i].betTime;
			dP_TableInfo.MaxZXBet = deskList[i].max_zx;
			dP_TableInfo.MaxHBet = deskList[i].max_h;
			dP_TableInfo.PersonCount = deskList[i].onlineNumber;
			dP_TableInfo.CoinInSetting = deskList[i].onceExchangeValue;
			dP_TableInfo.IsAutoKick = deskList[i].autoKick;
			mTableInfoList.Add(dP_TableInfo);
			if (mGameInfo.GetAppState == AppState.App_On_TableList_Panel)
			{
				tableItems[i].txtTableInfo.text = $"{dP_TableInfo.PersonCount}/8";
			}
		}
		mGameInfo.TableInfo = mTableInfoList[mCurrentTableId];
	}

	public void EnterDesk()
	{
		tfTable.gameObject.SetActive(value: false);
		tfSeat.gameObject.SetActive(value: true);
	}

	public void EnterGame()
	{
		DP_TipManager.GetSingleton().StartNetTiming();
		DP_MusicMngr.GetSingleton().SetGameMusicVolume(mGameInfo.Setted.bIsGameVolum ? 0.5f : 0f);
		objLoad.SetActive(value: true);
		async = SceneManager.LoadSceneAsync("DP_Game");
	}

	public void AddPerson(int iSeatId, string strNickname, int iIconIndex = 1)
	{
		if (strNickname.CompareTo(string.Empty) == 0)
		{
			seatItems[iSeatId - 1].bSeated = false;
			seatItems[iSeatId - 1].imgPerson.gameObject.SetActive(value: false);
			seatItems[iSeatId - 1].objNoPerson.SetActive(value: true);
			return;
		}
		if (!seatItems[iSeatId - 1].bSeated)
		{
			seatItems[iSeatId - 1].bSeated = true;
		}
		seatItems[iSeatId - 1].imgPerson.gameObject.SetActive(value: true);
		seatItems[iSeatId - 1].imgPerson.sprite = spiPersonSex[0];
		seatItems[iSeatId - 1].objNoPerson.SetActive(value: false);
	}
}
