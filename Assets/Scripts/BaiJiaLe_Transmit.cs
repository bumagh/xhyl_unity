using com.miracle9.game.bean;
using com.miracle9.game.entity;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaiJiaLe_Transmit : MonoBehaviour
{
	private static BaiJiaLe_Transmit _MyTransmit;

	private BaiJiaLe_Sockets m_CreateSocket;

	private BaiJiaLe_DataEncrypt m_BaiJiaLe_DataEncrypt;

	public static BaiJiaLe_Transmit GetSingleton()
	{
		return _MyTransmit;
	}

	private void Awake()
	{
		if (_MyTransmit == null)
		{
			_MyTransmit = this;
		}
	}

	private void Start()
	{
		StartCoroutine(Polling());
	}

	public void TransmitGetPoint(BaiJiaLe_Sockets MyCreateSocket)
	{
		m_CreateSocket = MyCreateSocket;
	}

	public void PostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] args = table["args"] as object[];
		object obj = table["args"];
		UnityEngine.Debug.Log(text);
		UnityEngine.Debug.Log("接收" + text + ":" + JsonMapper.ToJson(table).ToString());
		switch (text)
		{
		case "leaveSeat":
			break;
		case "sendServerTime":
			BaiJiaLe_Sockets.GetSingleton().SendUserLogin(BaiJiaLe_GameInfo.getInstance().UserId, BaiJiaLe_GameInfo.getInstance().Pwd, 1, string.Empty);
			break;
		case "userLogin":
			DoUserLogin(args);
			break;
		case "enterRoom":
			DoEnterRoom(args);
			break;
		case "DeskInfo":
			DoUpdateRoomInfo(args);
			break;
		case "deskInfo":
			DoDeskInfo(args);
			break;
		case "deskInfo1":
			DoDeskInfo1(args);
			break;
		case "updateDeskInfo":
			DoUpdateDeskInfo(args);
			break;
		case "selectSeat":
			DoSelectSeat(args);
			break;
		case "gameRestart":
			DoGameRestart(args);
			break;
		case "gameResult":
			DoGameResult(args);
			break;
		case "newGameGold":
			DoNewGameGold(obj);
			break;
		case "newGameScore":
			DoNewGameScore(obj);
			break;
		case "currentBet":
			DoCurrentBet(args);
			break;
		case "seatBet":
		case "seatbet":
			DoSeatBet(args);
			break;
		case "notUpdate":
			DoNotUpdate(args);
			break;
		case "quitToLogin":
			DoQuitToLogin(obj as int[]);
			break;
		case "NetThread/NetDown":
			DoNetDown(args);
			break;
		}
	}

	public void SelectPostMsgControl(Hashtable table)
	{
		string str = table["method"].ToString();
		object[] array = table["args"] as object[];
		UnityEngine.Debug.LogError("接收" + str + ":" + JsonMapper.ToJson(table).ToString());
	}

	private void DoUserLogin(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		UnityEngine.Debug.Log(dictionary["messageStatus"]);
		bool flag = (bool)dictionary["isLogin"];
		int num = (int)dictionary["messageStatus"];
		if (flag)
		{
			BaiJiaLe_GameInfo.getInstance().PlayerUser = new BaiJiaLe_User();
			bool flag2 = (bool)dictionary["isShutup"];
			bool flag3 = (bool)dictionary["special"];
			Console.WriteLine("isShutup: " + flag2);
			Console.WriteLine("special: " + flag3);
			Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
			BaiJiaLe_GameInfo.getInstance().PlayerUser.id = (int)dictionary2["id"];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.username = (string)dictionary2["username"];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.nickname = (string)dictionary2["nickname"];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.level = (int)dictionary2["level"];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.gameGold = (int)dictionary2["gameGold"];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.expeGold = (int)dictionary2["expeGold"];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.photoId = (int)dictionary2["photoId"];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.overflow = (int)dictionary2["overflow"];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.gameScore = (int)dictionary2["gameScore"];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.expeScore = (int)dictionary2["expeScore"];
			BaiJiaLe_GameInfo.getInstance().PlayerUser.type = (int)dictionary2["type"];
			BaiJiaLe_Sockets.GetSingleton().SendEnterRoom();
			if (BaiJiaLe_ChooseRoom.instance != null)
			{
				BaiJiaLe_GameInfo.getInstance().PlayerId = BaiJiaLe_GameInfo.getInstance().PlayerUser.id;
				BaiJiaLe_GameInfo.getInstance().NickName = BaiJiaLe_GameInfo.getInstance().PlayerUser.nickname;
				BaiJiaLe_GameInfo.getInstance().GameGold = BaiJiaLe_GameInfo.getInstance().PlayerUser.gameGold.ToString();
				BaiJiaLe_GameInfo.getInstance().GameScore = BaiJiaLe_GameInfo.getInstance().PlayerUser.gameScore.ToString();
				BaiJiaLe_ChooseRoom.instance.GetUserInfo();
			}
			BaiJiaLe_LoadPanel.instance.IsActive = true;
		}
		else
		{
			switch (num)
			{
			case 0:
				BaiJiaLe_TipManager.instance.ShowError(-1, "登录失败，用户不存在！");
				break;
			case 1:
				BaiJiaLe_TipManager.instance.ShowError(-1, "该用户已被冻结！");
				break;
			default:
				BaiJiaLe_TipManager.instance.ShowError(-1, "服务器维护中！");
				break;
			}
		}
	}

	private void DoEnterRoom(object[] args)
	{
		BaiJiaLe_GameInfo.IsJoinRoom = true;
		object[] array = args[0] as object[];
		BaiJiaLe_GameInfo.getInstance().GameDeskList.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> dictionary = array[i] as Dictionary<string, object>;
			BaiJiaLe_FreeBankerDesk baiJiaLe_FreeBankerDesk = new BaiJiaLe_FreeBankerDesk();
			baiJiaLe_FreeBankerDesk.id = (int)dictionary["id"];
			baiJiaLe_FreeBankerDesk.autoKick = (int)dictionary["autoKick"];
			baiJiaLe_FreeBankerDesk.betTime = (int)dictionary["betTime"];
			baiJiaLe_FreeBankerDesk.exchange = (int)dictionary["exchange"];
			baiJiaLe_FreeBankerDesk.maxBet = (int)dictionary["maxBet"];
			baiJiaLe_FreeBankerDesk.maxDz = (int)dictionary["maxDz"];
			baiJiaLe_FreeBankerDesk.maxH = (int)dictionary["maxH"];
			baiJiaLe_FreeBankerDesk.maxZx = (int)dictionary["maxZx"];
			baiJiaLe_FreeBankerDesk.minBet = (int)dictionary["minBet"];
			baiJiaLe_FreeBankerDesk.minGold = (int)dictionary["minGold"];
			baiJiaLe_FreeBankerDesk.minZxh = (int)dictionary["minZxh"];
			baiJiaLe_FreeBankerDesk.name = (string)dictionary["name"];
			baiJiaLe_FreeBankerDesk.onceExchangeValue = (int)dictionary["onceExchangeValue"];
			baiJiaLe_FreeBankerDesk.orderBy = (int)dictionary["orderBy"];
			baiJiaLe_FreeBankerDesk.roomId = (int)dictionary["roomId"];
			baiJiaLe_FreeBankerDesk.round = (int)dictionary["round"];
			baiJiaLe_FreeBankerDesk.state = (int)dictionary["state"];
			baiJiaLe_FreeBankerDesk.sumDeFen = (int)dictionary["sumDeFen"];
			baiJiaLe_FreeBankerDesk.sumDzDeFen = (int)dictionary["sumDzDeFen"];
			baiJiaLe_FreeBankerDesk.sumDzYaFen = (int)dictionary["sumDzYaFen"];
			baiJiaLe_FreeBankerDesk.sumYaFen = (int)dictionary["sumYaFen"];
			baiJiaLe_FreeBankerDesk.waterType = (int)dictionary["waterType"];
			baiJiaLe_FreeBankerDesk.waterValue = (int)dictionary["waterValue"];
			baiJiaLe_FreeBankerDesk.onlineNumber = (int)dictionary["onlineNumber"];
			baiJiaLe_FreeBankerDesk.ludan = (string)dictionary["ludan"];
			baiJiaLe_FreeBankerDesk.djs = (int)dictionary["djs"];
			baiJiaLe_FreeBankerDesk.status = (int)dictionary["status"];
			BaiJiaLe_GameInfo.getInstance().GameDeskList.Add(baiJiaLe_FreeBankerDesk);
		}
		if (BaiJiaLe_ChooseRoom.instance != null)
		{
			BaiJiaLe_ChooseRoom.instance.UpdateLuDan();
		}
	}

	private void DoUpdateRoomInfo(object[] args)
	{
		object[] array = args[0] as object[];
		BaiJiaLe_GameInfo.getInstance().GameDeskList.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> dictionary = array[i] as Dictionary<string, object>;
			BaiJiaLe_FreeBankerDesk baiJiaLe_FreeBankerDesk = new BaiJiaLe_FreeBankerDesk();
			baiJiaLe_FreeBankerDesk.id = (int)dictionary["id"];
			baiJiaLe_FreeBankerDesk.autoKick = (int)dictionary["autoKick"];
			baiJiaLe_FreeBankerDesk.betTime = (int)dictionary["betTime"];
			baiJiaLe_FreeBankerDesk.exchange = (int)dictionary["exchange"];
			baiJiaLe_FreeBankerDesk.maxBet = (int)dictionary["maxBet"];
			baiJiaLe_FreeBankerDesk.maxDz = (int)dictionary["maxDz"];
			baiJiaLe_FreeBankerDesk.maxH = (int)dictionary["maxH"];
			baiJiaLe_FreeBankerDesk.maxZx = (int)dictionary["maxZx"];
			baiJiaLe_FreeBankerDesk.minBet = (int)dictionary["minBet"];
			baiJiaLe_FreeBankerDesk.minGold = (int)dictionary["minGold"];
			baiJiaLe_FreeBankerDesk.minZxh = (int)dictionary["minZxh"];
			baiJiaLe_FreeBankerDesk.name = (string)dictionary["name"];
			baiJiaLe_FreeBankerDesk.onceExchangeValue = (int)dictionary["onceExchangeValue"];
			baiJiaLe_FreeBankerDesk.orderBy = (int)dictionary["orderBy"];
			baiJiaLe_FreeBankerDesk.roomId = (int)dictionary["roomId"];
			baiJiaLe_FreeBankerDesk.round = (int)dictionary["round"];
			baiJiaLe_FreeBankerDesk.state = (int)dictionary["state"];
			baiJiaLe_FreeBankerDesk.sumDeFen = (int)dictionary["sumDeFen"];
			baiJiaLe_FreeBankerDesk.sumDzDeFen = (int)dictionary["sumDzDeFen"];
			baiJiaLe_FreeBankerDesk.sumDzYaFen = (int)dictionary["sumDzYaFen"];
			baiJiaLe_FreeBankerDesk.sumYaFen = (int)dictionary["sumYaFen"];
			baiJiaLe_FreeBankerDesk.waterType = (int)dictionary["waterType"];
			baiJiaLe_FreeBankerDesk.waterValue = (int)dictionary["waterValue"];
			baiJiaLe_FreeBankerDesk.onlineNumber = (int)dictionary["onlineNumber"];
			baiJiaLe_FreeBankerDesk.ludan = (string)dictionary["ludan"];
			baiJiaLe_FreeBankerDesk.djs = (int)dictionary["djs"];
			baiJiaLe_FreeBankerDesk.status = (int)dictionary["status"];
			BaiJiaLe_GameInfo.getInstance().GameDeskList.Add(baiJiaLe_FreeBankerDesk);
		}
		if (BaiJiaLe_ChooseRoom.instance != null)
		{
			BaiJiaLe_ChooseRoom.instance.UpdateLuDan();
		}
	}

	private void DoDeskInfo(object[] args)
	{
		SceneManager.LoadSceneAsync("BaiJiaLe_Game");
	}

	private void DoDeskInfo1(object[] args)
	{
		object[] array = args[0] as object[];
		BaiJiaLe_GameInfo.getInstance().GameSeatList.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> dictionary = array[i] as Dictionary<string, object>;
			BaiJiaLe_Seat baiJiaLe_Seat = new BaiJiaLe_Seat();
			baiJiaLe_Seat.id = (int)dictionary["id"];
			baiJiaLe_Seat.isFree = (bool)dictionary["isFree"];
			baiJiaLe_Seat.userNickname = (string)dictionary["userNickname"];
			baiJiaLe_Seat.userSex = (string)dictionary["userSex"];
			baiJiaLe_Seat.photoId = (int)dictionary["photoId"];
			baiJiaLe_Seat.userId = (int)dictionary["userId"];
			baiJiaLe_Seat.totalbet = (int)dictionary["totalbet"];
			baiJiaLe_Seat.totalwin = (int)dictionary["totalwin"];
			baiJiaLe_Seat.gamescore = (int)dictionary["gamescore"];
			BaiJiaLe_GameInfo.getInstance().GameSeatList.Add(baiJiaLe_Seat);
		}
		if (BaiJiaLe_Game.instance != null)
		{
			BaiJiaLe_Game.instance.UpdatePlayerList1();
		}
	}

	private void DoUpdateDeskInfo(object[] args)
	{
		object[] array = args[0] as object[];
		BaiJiaLe_GameInfo.getInstance().GameSeatList.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> dictionary = array[i] as Dictionary<string, object>;
			BaiJiaLe_Seat baiJiaLe_Seat = new BaiJiaLe_Seat();
			baiJiaLe_Seat.id = (int)dictionary["id"];
			baiJiaLe_Seat.isFree = (bool)dictionary["isFree"];
			baiJiaLe_Seat.userNickname = (string)dictionary["userNickname"];
			baiJiaLe_Seat.userSex = (string)dictionary["userSex"];
			baiJiaLe_Seat.photoId = (int)dictionary["photoId"];
			baiJiaLe_Seat.userId = (int)dictionary["userId"];
			baiJiaLe_Seat.totalbet = (int)dictionary["totalbet"];
			baiJiaLe_Seat.totalwin = (int)dictionary["totalwin"];
			baiJiaLe_Seat.gamescore = (int)dictionary["gamescore"];
			BaiJiaLe_GameInfo.getInstance().GameSeatList.Add(baiJiaLe_Seat);
		}
		if (BaiJiaLe_Game.instance != null)
		{
			BaiJiaLe_Game.instance.UpdatePlayerList();
		}
	}

	private void DoSelectSeat(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		BaiJiaLe_GameInfo.getInstance().BetTime = (int)dictionary["betTime"];
	}

	private void DoUpdateGame(object[] args)
	{
	}

	private void DoGameRestart(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		BaiJiaLe_GameInfo.getInstance().BetTime = (int)dictionary["BetTime"];
		if (BaiJiaLe_Game.instance != null)
		{
			BaiJiaLe_Game.instance.GameRestart((int)dictionary["Chang"], dictionary["beilv"] as double[]);
		}
	}

	private void DoGameResult(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		Dictionary<string, object> dictionary2 = dictionary["deskResult"] as Dictionary<string, object>;
		BaiJiaLe_GameInfo.getInstance().GameResult = new BaiJiaLe_FreeBankerDeskResult();
		BaiJiaLe_GameInfo.getInstance().GameResult.datetime = (string)dictionary2["datetime"];
		BaiJiaLe_GameInfo.getInstance().GameResult.deskId = (int)dictionary2["deskId"];
		BaiJiaLe_GameInfo.getInstance().GameResult.ludan = (int)dictionary2["ludan"];
		BaiJiaLe_GameInfo.getInstance().GameResult.pai = (string)dictionary2["pai"];
		BaiJiaLe_GameInfo.getInstance().GameResult.paiCount = (int)dictionary2["paiCount"];
		BaiJiaLe_GameInfo.getInstance().GameResult.roomId = (int)dictionary2["roomId"];
		BaiJiaLe_GameInfo.getInstance().GameWinResult = (dictionary["incomeInfo"] as object[]);
		if (BaiJiaLe_Game.instance != null)
		{
			BaiJiaLe_Game.instance.GameResult();
		}
	}

	private void DoDeskTotalBet(object[] args)
	{
	}

	private void DoCurrentBet(object[] args)
	{
		int[] array = args[0] as int[];
		if (BaiJiaLe_Game.instance != null)
		{
			BaiJiaLe_Game.instance.GetCurrentBet(array[0], array[1], array[2], array[3], array[4]);
		}
	}

	private void DoSeatBet(object[] args)
	{
		if (args.ToString() == "[[]]")
		{
			UnityEngine.Debug.Log("空的");
			return;
		}
		object[] array = args[0] as object[];
		for (int i = 0; i < array.Length; i++)
		{
			BaiJiaLe_DeskSeatBet baiJiaLe_DeskSeatBet = new BaiJiaLe_DeskSeatBet();
			Dictionary<string, object> dictionary = array[i] as Dictionary<string, object>;
			baiJiaLe_DeskSeatBet.userid = (int)dictionary["userid"];
			baiJiaLe_DeskSeatBet.betzhuang = (int)dictionary["betzhuang"];
			baiJiaLe_DeskSeatBet.betxian = (int)dictionary["betxian"];
			baiJiaLe_DeskSeatBet.bethe = (int)dictionary["bethe"];
			baiJiaLe_DeskSeatBet.betzhuangdui = (int)dictionary["betzhuangdui"];
			baiJiaLe_DeskSeatBet.betxiandui = (int)dictionary["betxiandui"];
			bool flag = false;
			for (int j = 0; j < BaiJiaLe_GameInfo.getInstance().PlayerChips.Count; j++)
			{
				if (BaiJiaLe_GameInfo.getInstance().PlayerChips[j][0] == baiJiaLe_DeskSeatBet.userid)
				{
					BaiJiaLe_GameInfo.getInstance().PlayerChips[j][1] = baiJiaLe_DeskSeatBet.betzhuang;
					BaiJiaLe_GameInfo.getInstance().PlayerChips[j][2] = baiJiaLe_DeskSeatBet.betxian;
					BaiJiaLe_GameInfo.getInstance().PlayerChips[j][3] = baiJiaLe_DeskSeatBet.bethe;
					BaiJiaLe_GameInfo.getInstance().PlayerChips[j][4] = baiJiaLe_DeskSeatBet.betzhuangdui;
					BaiJiaLe_GameInfo.getInstance().PlayerChips[j][5] = baiJiaLe_DeskSeatBet.betxiandui;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				int[] item = new int[6]
				{
					baiJiaLe_DeskSeatBet.userid,
					baiJiaLe_DeskSeatBet.betzhuang,
					baiJiaLe_DeskSeatBet.betxian,
					baiJiaLe_DeskSeatBet.bethe,
					baiJiaLe_DeskSeatBet.betzhuangdui,
					baiJiaLe_DeskSeatBet.betxiandui
				};
				BaiJiaLe_GameInfo.getInstance().PlayerChips.Add(item);
			}
		}
		try
		{
			BaiJiaLe_Game.instance.UpdatePlayerBet();
		}
		catch (Exception)
		{
		}
	}

	private void DoNewGameGold(object args)
	{
		BaiJiaLe_GameInfo.getInstance().GameGold = args.ToString();
		if (BaiJiaLe_Game.instance != null)
		{
			BaiJiaLe_Game.instance.UpdateUserInfo();
		}
	}

	private void DoNewGameScore(object args)
	{
		BaiJiaLe_GameInfo.getInstance().GameScore = args.ToString();
		if (BaiJiaLe_Game.instance != null)
		{
			BaiJiaLe_Game.instance.UpdateUserInfo();
		}
	}

	private void DoLeaveSeat(object args)
	{
	}

	private void DoResultList(object[] args)
	{
	}

	private void DoSendNotice(object[] args)
	{
	}

	private void DoOverflow(object[] args)
	{
	}

	private void DoQuitToLogin(int[] args)
	{
		int num = args[0];
		if (num == 4)
		{
			BaiJiaLe_TipManager.instance.ShowError(-1, "该账户已在异地登录");
			BaiJiaLe_Sockets.GetSingleton().isReconnect = false;
		}
		else
		{
			BaiJiaLe_TipManager.instance.ShowError(-1, "连接失败");
			BaiJiaLe_Sockets.GetSingleton().isReconnect = false;
		}
	}

	private void DoQuitToRoom(object[] args)
	{
	}

	private void DoGameShutup(object[] args)
	{
	}

	private void DoUserShutup(object[] args)
	{
	}

	private void DoUserAward(object[] args)
	{
	}

	private void DoNetDown(object[] args)
	{
		if (BaiJiaLe_Sockets.GetSingleton().isReconnect)
		{
			if (m_CreateSocket.GetRelineCount() < 5)
			{
				m_CreateSocket.CreateReceiveThread();
				return;
			}
			UnityEngine.Debug.Log("30秒重连失败，网络断开，请重新登录网络大厅...");
			BaiJiaLe_TipManager.instance.ShowError(-1, "网络连接断开！");
			BaiJiaLe_Sockets.GetSingleton().isReconnect = false;
		}
	}

	private void DoHeart(object[] args)
	{
		m_CreateSocket.SendHeart();
	}

	private void DoLogoffNotice(object[] args)
	{
	}

	private void DoCheckVersion(object[] args)
	{
	}

	private void DoNotUpdate(object[] args)
	{
		BaiJiaLe_NetMngr.GetSingleton().MyCreateSocket.SendPublicKey();
	}

	private IEnumerator Polling()
	{
		while (true)
		{
			if (BaiJiaLe_GameInfo.IsJoinRoom && !BaiJiaLe_GameInfo.IsJoinDesk)
			{
				BaiJiaLe_Sockets.GetSingleton().SendUpdateRoom();
			}
			else if (BaiJiaLe_GameInfo.IsJoinDesk)
			{
				BaiJiaLe_Sockets.GetSingleton().SenddeskInfo1(int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID));
			}
			yield return new WaitForSeconds(2f);
		}
	}

	private ArrayList _ConvertStringSongDengforUI(string moreInfo)
	{
		ArrayList arrayList = new ArrayList();
		string text = string.Empty;
		for (int i = 0; i < moreInfo.Length; i++)
		{
			if (moreInfo[i] != ',')
			{
				text += moreInfo[i];
			}
			else
			{
				int num = Convert.ToInt32(text);
				arrayList.Add(num);
				text = string.Empty;
			}
			if (i == moreInfo.Length - 1)
			{
				int num2 = Convert.ToInt32(text);
				arrayList.Add(num2);
			}
		}
		return arrayList;
	}
}
