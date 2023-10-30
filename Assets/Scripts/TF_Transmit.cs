using GameCommon;
using GameConfig;
using JsonFx.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TF_Transmit : MonoBehaviour
{
	private static TF_Transmit _MyTransmit;

	private TF_Sockets m_CreateSocket;

	public static TF_Transmit GetSingleton()
	{
		return _MyTransmit;
	}

	private void Awake()
	{
		if (_MyTransmit == null)
		{
			UnityEngine.Debug.Log("_MyTransmit");
			_MyTransmit = this;
		}
	}

	public void TransmitGetPoint(TF_Sockets MyCreateSocket)
	{
		m_CreateSocket = MyCreateSocket;
	}

	public void PostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] array = table["args"] as object[];
		UnityEngine.Debug.Log("接收：" + text + "|||" + JsonWriter.Serialize(array));
		if (text == "sendServerTime")
		{
			if (TF_GameInfo.getInstance().Key != string.Empty)
			{
				TF_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(TF_GameInfo.getInstance().UserId, TF_GameInfo.getInstance().Pwd, (int)((TF_GameInfo.getInstance().currentState < TF_GameState.On_SelectRoom) ? TF_GameState.On_SelectRoom : TF_GameInfo.getInstance().currentState), string.Empty);
			}
			TF_GameInfo.getInstance().Key = "sendServerTime";
		}
		else if (text == "checkVersion")
		{
			DoCheckVersion(array);
		}
		else if (text == "notUpdate")
		{
			DoNotUpdate(array);
		}
		else if (text == "checkLogin")
		{
			DoCheckLogin(array);
		}
		else if (text == "roomInfo")
		{
			DoRoomInfo(array);
		}
		else if (text == "updateRoomInfo")
		{
			DoUpdateRoomInfo(array);
		}
		else if (text == "deskOnlineNumber")
		{
			DoDeskOnlineNumber(array);
		}
		else if (text == "requestSeat")
		{
			DoRequestSeat(array);
		}
		else if (text == "updateDeskInfo")
		{
			DoUpdateDeskInfo(array);
		}
		else if (text == "gameScore")
		{
			DoGameScore(table["args"] as int[]);
		}
		else if (text == "notFired")
		{
			DoNotFired(table["args"] as int[]);
		}
		else if (text == "fired")
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange((object[])table["args"]);
			DoFired(arrayList);
		}
		else if (text == "checkstatus")
		{
			DoCheckStatus(array);
		}
		else if (text == "gunHitFish")
		{
			ArrayList arrayList2 = new ArrayList();
			arrayList2.AddRange((object[])table["args"]);
			DoGunHitFish(arrayList2);
		}
		else if (text == "newFishGroup")
		{
			DoNewFishGroup(table["args"] as int[]);
		}
		else if (text == "newFish")
		{
			DoNewFish(table["args"] as string[]);
		}
		else if (text == "forbitFired")
		{
			DoForbitFired(array);
		}
		else if (text == "clearFish")
		{
			DoClearFish(table["args"] as int[], isBackGround: false);
		}
		else if (text == "scrollMessage")
		{
			DoScrollMessage(array);
		}
		else if (text == "FishLimit")
		{
			DoFishLimit(array);
		}
		else if (text == "playerInfo")
		{
			DoPlayerInfo(array);
		}
		else if (text == "sendChat")
		{
			DoSendChat(array);
		}
		else if (text == "gameShutup")
		{
			DoGameShutup(table["args"] as bool[]);
		}
		else if (text == "userShutup")
		{
			DoUserShutup(table["args"] as bool[]);
		}
		else if (text == "userAward")
		{
			DoUserAward(table["args"] as int[]);
		}
		else if (!(text == "heart"))
		{
			if (text == "gameGold")
			{
				DoGameGold(table["args"] as int[]);
			}
			else if (text == "expeGold")
			{
				DoExpeGold(table["args"] as int[]);
			}
			else if (text == "addExpeGoldAuto")
			{
				DoAddExpeGoldAuto(table["args"] as bool[]);
			}
			else if (text == "overflow")
			{
				DoOverflow(array);
			}
			else if (text == "quitToLogin")
			{
				DoQuitToLogin(table["args"] as int[]);
			}
			else if (text == "quitToRoom")
			{
				DoQuitToRoom(table["args"] as int[]);
			}
			else if (text == "NetThread/NetDown")
			{
				DoNetDown(array);
			}
			else if (text == "lockFish")
			{
				ArrayList arrayList3 = new ArrayList();
				arrayList3.AddRange((object[])table["args"]);
				DoLockFish(arrayList3);
			}
			else if (text == "unLockFish")
			{
				DoUnLockFish(table["args"] as int[]);
			}
			else if (text == "unLockScreen")
			{
				DoUnLockScreen(array);
			}
			else
			{
				Console.WriteLine("recv No Message Type!");
			}
		}
	}

	public void SelectPostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] args = table["args"] as object[];
		UnityEngine.Debug.Log("接收：" + text);
		if (text == "sendServerTime")
		{
			if (TF_GameInfo.getInstance().Key != string.Empty)
			{
				TF_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(TF_GameInfo.getInstance().UserId, TF_GameInfo.getInstance().Pwd, (int)TF_GameInfo.getInstance().currentState, string.Empty);
			}
			TF_GameInfo.getInstance().Key = "sendServerTime";
		}
		else if (text == "checkVersion")
		{
			DoCheckVersion(args);
		}
		else if (text == "notUpdate")
		{
			DoNotUpdate(args);
		}
		else if (text == "checkLogin")
		{
			DoCheckLogin(args);
		}
		else if (text == "roomInfo")
		{
			MonoBehaviour.print(text);
			DoRoomInfo(args);
		}
		else if (text == "updateRoomInfo")
		{
			MonoBehaviour.print(text);
			DoUpdateRoomInfo(args);
		}
		else if (text == "deskOnlineNumber")
		{
			DoDeskOnlineNumber(args);
		}
		else if (text == "requestSeat")
		{
			DoRequestSeat(args);
		}
		else if (text == "updateDeskInfo")
		{
			DoUpdateDeskInfo(args);
		}
		else if (text == "gameScore")
		{
			DoGameScore(table["args"] as int[]);
		}
		else if (text == "checkstatus")
		{
			DoCheckStatus(args);
		}
		else
		{
			if (text == "notFired" || text == "fired" || text == "gunHitFish" || text == "newFishGroup" || text == "newFish" || text == "forbitFired")
			{
				return;
			}
			if (text == "clearFish")
			{
				DoClearFish(table["args"] as int[], isBackGround: true);
			}
			else if (text == "scrollMessage")
			{
				DoScrollMessage(args);
			}
			else if (text == "FishLimit")
			{
				DoFishLimit(args);
			}
			else if (text == "playerInfo")
			{
				DoPlayerInfo(args);
			}
			else if (text == "sendChat")
			{
				DoSendChat(args);
			}
			else if (text == "gameShutup")
			{
				DoGameShutup(table["args"] as bool[]);
			}
			else if (text == "userShutup")
			{
				DoUserShutup(table["args"] as bool[]);
			}
			else if (text == "userAward")
			{
				DoUserAward(table["args"] as int[]);
			}
			else
			{
				if (text == "heart")
				{
					return;
				}
				if (text == "gameGold")
				{
					DoGameGold(table["args"] as int[]);
				}
				else if (text == "expeGold")
				{
					DoExpeGold(table["args"] as int[]);
				}
				else if (text == "addExpeGoldAuto")
				{
					DoAddExpeGoldAuto(table["args"] as bool[]);
				}
				else if (text == "overflow")
				{
					DoOverflow(args);
				}
				else if (text == "quitToLogin")
				{
					DoQuitToLogin(table["args"] as int[]);
				}
				else if (text == "quitToRoom")
				{
					DoQuitToRoom(table["args"] as int[]);
				}
				else if (text == "NetThread/NetDown")
				{
					DoNetDown(args);
				}
				else if (text != "lockFish")
				{
					if (text != "unLockFish" && text == "unLockScreen")
					{
						DoUnLockScreen(args);
					}
				}
				else
				{
					Console.WriteLine("recv No Message Type!");
				}
			}
		}
	}

	private void DoNetDown(object[] args)
	{
		if (TF_Sockets.GetSingleton().isReconnect)
		{
			if (m_CreateSocket.GetRelineCount() < 5)
			{
				m_CreateSocket.CreateReceiveThread();
				return;
			}
			Console.WriteLine("30秒重连失败，网络断开，请重新登录网络大厅...");
			TF_TipManager.getInstance().ShowTip(TF_TipType.Net_ConnectionError);
		}
	}

	private void DoCheckLogin(object[] args)
	{
		Console.WriteLine("**********1*********DoCheckLogin**************************************");
		TF_User tF_User = new TF_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["isLogin"];
		Console.WriteLine("isLogin: " + flag);
		int num = (int)dictionary["messageStatus"];
		Console.WriteLine("messageStatus: " + num);
		if (flag)
		{
			bool flag2 = (bool)dictionary["isShutup"];
			bool flag3 = (bool)dictionary["special"];
			Console.WriteLine("isShutup: " + flag2);
			Console.WriteLine("special: " + flag3);
			Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
			tF_User.id = (int)dictionary2["id"];
			tF_User.username = (string)dictionary2["username"];
			tF_User.nickname = (string)dictionary2["nickname"];
			tF_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
			tF_User.level = (int)dictionary2["level"];
			tF_User.gameGold = (int)dictionary2["gameGold"];
			tF_User.expeGold = (int)dictionary2["expeGold"];
			tF_User.photoId = (int)dictionary2["photoId"];
			tF_User.overflow = (int)dictionary2["overflow"];
			tF_User.gameScore = (int)dictionary2["gameScore"];
			tF_User.expeScore = (int)dictionary2["expeScore"];
			tF_User.type = (int)dictionary2["type"];
			TF_GameInfo.getInstance().IsSpecial = flag3;
			TF_GameInfo.getInstance().createUser(tF_User);
			TF_GameInfo.getInstance().IsGameShuUp = flag2;
		}
		else
		{
			switch (num)
			{
			case 0:
				TF_TipManager.getInstance().ShowTip(TF_TipType.UserIdDeleted);
				break;
			case 1:
				TF_TipManager.getInstance().ShowTip(TF_TipType.Game_UserIdFrozen);
				break;
			default:
				TF_TipManager.getInstance().ShowTip(TF_TipType.ServerUpdate);
				break;
			}
		}
		Console.WriteLine("**********2*********DoCheckLogin**************************************");
	}

	private void DoCheckVersion(object[] args)
	{
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
		string text = (string)args[0];
		Console.WriteLine("haveNewVersionIDFlag: " + text);
		TF_IOSGameStart.UpdateGameVesion(text);
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
	}

	private void DoNotUpdate(object[] args)
	{
		UnityEngine.Debug.Log("DoNotUpdate");
		TF_NetMngr.GetSingleton().MyCreateSocket.SendPublicKey();
	}

	private void DoRoomInfo(object[] args)
	{
		UnityEngine.Debug.Log("**********1*********DoRoomInfo**************************************");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int length = (dictionary["FishDesk"] as Array).Length;
		if (length > 0)
		{
			TF_FishDesk[] array = new TF_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["FishDesk"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new TF_FishDesk();
				array[i].id = (int)dictionary2["id"];
				array[i].roomId = (int)dictionary2["roomId"];
				array[i].name = (string)dictionary2["name"];
				array[i].minGold = (int)dictionary2["minGold"];
				array[i].minGunValue = (int)dictionary2["minGunValue"];
				array[i].maxGunValue = (int)dictionary2["maxGunValue"];
				array[i].addstepGunValue = (int)dictionary2["addstepGunValue"];
				array[i].exchange = (int)dictionary2["exchange"];
				array[i].onceExchangeValue = (int)dictionary2["onceExchangeValue"];
				for (int j = 0; j < 4; j++)
				{
					Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
					dictionary3 = ((dictionary2["seats"] as Array).GetValue(j) as Dictionary<string, object>);
					array[i].seats[j] = new TF_Seat();
					array[i].seats[j].seatId = (int)dictionary3["id"];
					array[i].seats[j].isFree = (bool)dictionary3["isFree"];
					array[i].seats[j].gunValue = (int)dictionary3["gunValue"];
					if (!array[i].seats[j].isFree)
					{
						array2[i]++;
						Dictionary<string, object> dictionary4 = dictionary3["user"] as Dictionary<string, object>;
						array[i].seats[j].user.id = (int)dictionary4["id"];
						array[i].seats[j].user.username = (string)dictionary4["username"];
						array[i].seats[j].user.nickname = (string)dictionary4["nickname"];
						array[i].seats[j].user.sex = ((string)dictionary4["sex"]).ToCharArray()[0];
						array[i].seats[j].user.level = (int)dictionary4["level"];
						array[i].seats[j].user.gameGold = (int)dictionary4["gameGold"];
						array[i].seats[j].user.expeGold = (int)dictionary4["expeGold"];
						array[i].seats[j].user.photoId = (int)dictionary4["photoId"];
						array[i].seats[j].user.overflow = (int)dictionary4["overflow"];
						array[i].seats[j].user.gameScore = (int)dictionary4["gameScore"];
						array[i].seats[j].user.expeScore = (int)dictionary4["expeScore"];
						array[i].seats[j].user.type = (int)dictionary4["type"];
					}
				}
			}
			TF_GameInfo.getInstance().updateTableList(array);
			for (int k = 0; k < length; k++)
			{
				TF_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			TF_TipManager.getInstance().ShowTip(TF_TipType.RoomEmpty);
			TF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(1);
		}
		UnityEngine.Debug.Log("**********2*********DoRoomInfo**************************************");
	}

	private void DoUpdateRoomInfo(object[] args)
	{
		int length = (args[0] as Array).Length;
		if (length > 0)
		{
			TF_FishDesk[] array = new TF_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				int num = 0;
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary = ((args[0] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new TF_FishDesk();
				array[i].id = (int)dictionary["id"];
				array[i].roomId = (int)dictionary["roomId"];
				array[i].name = (string)dictionary["name"];
				array[i].minGold = (int)dictionary["minGold"];
				array[i].minGunValue = (int)dictionary["minGunValue"];
				array[i].maxGunValue = (int)dictionary["maxGunValue"];
				array[i].addstepGunValue = (int)dictionary["addstepGunValue"];
				array[i].exchange = (int)dictionary["exchange"];
				array[i].onceExchangeValue = (int)dictionary["onceExchangeValue"];
				for (int j = 0; j < 4; j++)
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2 = ((dictionary["seats"] as Array).GetValue(j) as Dictionary<string, object>);
					array[i].seats[j] = new TF_Seat();
					array[i].seats[j].seatId = (int)dictionary2["id"];
					array[i].seats[j].isFree = (bool)dictionary2["isFree"];
					array[i].seats[j].gunValue = (int)dictionary2["gunValue"];
					if (!array[i].seats[j].isFree)
					{
						array2[i]++;
						num++;
						Dictionary<string, object> dictionary3 = dictionary2["user"] as Dictionary<string, object>;
						array[i].seats[j].user.id = (int)dictionary3["id"];
						array[i].seats[j].user.username = (string)dictionary3["username"];
						array[i].seats[j].user.nickname = (string)dictionary3["nickname"];
						array[i].seats[j].user.sex = ((string)dictionary3["sex"]).ToCharArray()[0];
						array[i].seats[j].user.level = (int)dictionary3["level"];
						array[i].seats[j].user.gameGold = (int)dictionary3["gameGold"];
						array[i].seats[j].user.expeGold = (int)dictionary3["expeGold"];
						array[i].seats[j].user.photoId = (int)dictionary3["photoId"];
						array[i].seats[j].user.overflow = (int)dictionary3["overflow"];
						array[i].seats[j].user.gameScore = (int)dictionary3["gameScore"];
						array[i].seats[j].user.expeScore = (int)dictionary3["expeScore"];
						array[i].seats[j].user.type = (int)dictionary3["type"];
					}
					TF_GameInfo.getInstance().updateTableUserNumber(j, num);
				}
				TF_GameInfo.getInstance().updateTableList(array);
				for (int k = 0; k < length; k++)
				{
					TF_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
				}
			}
		}
		UnityEngine.Debug.Log("**********2*********DoUpdateRoomInfo**************************************");
	}

	private void DoDeskOnlineNumber(object[] args)
	{
		Console.WriteLine("**********1*********DoDeskOnlineNumber**************************************");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["deskId"];
		int num2 = (int)dictionary["onlineNumber"];
		Console.WriteLine("deskId:" + num);
		Console.WriteLine("onlineNumber:" + num2);
		Console.WriteLine("**********2*********DoDeskOnlineNumber**************************************");
		TF_GameInfo.getInstance().updateTableUserNumber(num, num2);
	}

	private void DoRequestSeat(object[] args)
	{
		Console.WriteLine("**********1*********DoRequestSeat**************************************");
		int num = 0;
		int num2 = 0;
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["bCanSeat"];
		Console.WriteLine("bCanSeat:" + flag);
		if (!flag)
		{
			num = (int)dictionary["messageStatus"];
			Console.WriteLine("messageStatus:" + num);
		}
		else
		{
			num2 = (int)dictionary["bgId"];
			int seatIndex = (int)dictionary["seatId"];
			TF_GameInfo.getInstance().User.SeatIndex = seatIndex;
		}
		if (!TF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			TF_NetMngr.GetSingleton().mSceneBg = num2;
		}
		else
		{
			TF_SceneBgMngr.GetSingleton().SetScene(num2);
		}
		if (flag)
		{
			TF_GameInfo.getInstance().UIScene.EnterGame();
			return;
		}
		switch (num)
		{
		case 0:
			TF_TipManager.getInstance().ShowTip(TF_TipType.TableDeleted);
			break;
		case 1:
			if (TF_GameInfo.getInstance().User.RoomId == 1)
			{
				TF_TipManager.getInstance().ShowTip(TF_TipType.SelectTable_CreditBelowRistrict);
			}
			else
			{
				TF_TipManager.getInstance().ShowTip(TF_TipType.SelectTable_SendExpCoin);
			}
			break;
		case 2:
			TF_TipManager.getInstance().ShowTip(TF_TipType.SelectSeat_NotEmpty);
			break;
		}
	}

	private void DoUpdateDeskInfo(object[] args)
	{
		UnityEngine.Debug.Log("**********1*********DoUpdateDeskInfo**************************************");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["deskId"];
		int length = (dictionary["Seat"] as Array).Length;
		if (length > 0)
		{
			TF_Seat[] array = new TF_Seat[length];
			int num2 = 0;
			for (int i = 0; i < length; i++)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["Seat"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new TF_Seat();
				array[i].seatId = (int)dictionary2["id"];
				array[i].isFree = (bool)dictionary2["isFree"];
				array[i].gunValue = (int)dictionary2["gunValue"];
				if (!array[i].isFree)
				{
					num2++;
					Dictionary<string, object> dictionary3 = dictionary2["user"] as Dictionary<string, object>;
					array[i].user = new TF_User();
					array[i].user.id = (int)dictionary3["id"];
					array[i].user.username = (string)dictionary3["username"];
					array[i].user.nickname = (string)dictionary3["nickname"];
					array[i].user.sex = ((string)dictionary3["sex"]).ToCharArray()[0];
					array[i].user.level = (int)dictionary3["level"];
					array[i].user.gameGold = (int)dictionary3["gameGold"];
					array[i].user.expeGold = (int)dictionary3["expeGold"];
					array[i].user.photoId = (int)dictionary3["photoId"];
					array[i].user.overflow = (int)dictionary3["overflow"];
					array[i].user.gameScore = (int)dictionary3["gameScore"];
					array[i].user.expeScore = (int)dictionary3["expeScore"];
					array[i].user.type = (int)dictionary3["type"];
				}
				else
				{
					UnityEngine.Debug.Log("Seat" + array[i].seatId + "：该座位上没有人!!!");
				}
			}
			TF_GameInfo.getInstance().updateOtherUsers(array, num);
			TF_GameInfo.getInstance().updateTableUserNumber(num, num2);
		}
		UnityEngine.Debug.Log("**********2*********DoUpdateDeskInfo**************************************");
	}

	private void DoGameScore(int[] args)
	{
		Console.WriteLine("**********1*********DoGameScore**************************************");
		int num = Convert.ToInt32(args[0]);
		MonoBehaviour.print("gameScore: " + num);
		Console.WriteLine("**********2*********DoGameScore**************************************");
		TF_GameInfo.getInstance().updateUser("gameScore", num);
	}

	private void DoNotFired(int[] args)
	{
		Console.WriteLine("**********1*********DoNotfired**************************************");
		int num = args[0];
		Console.WriteLine("gunId: " + num);
		int num2 = args[1];
		Console.WriteLine("seatId: " + num2);
		int num3 = args[2];
		Console.WriteLine("totalScore: " + num3);
		Console.WriteLine("**********2********DoNotfired**************************************");
	}

	private void DoFired(ArrayList args)
	{
		UnityEngine.Debug.Log("**********1*********DoFired**************************************");
		int num = int.Parse((string)args[0]);
		double num2 = Convert.ToDouble(args[1]);
		int num3 = int.Parse((string)args[2]);
		int num4 = int.Parse((string)args[3]);
		bool isLizi = Convert.ToBoolean(args[4]);
		int newScore = int.Parse((string)args[5]);
		if (TF_NetMngr.GetSingleton().IsGameSceneLoadOk && !TF_GameParameter.G_bTest && num3 != TF_GameMngr.GetSingleton().mPlayerSeatID)
		{
			TF_BulletPoolMngr.GetSingleton().LanchBullet(num3, (float)num2, num4, isLizi);
		}
		Console.WriteLine("**********2*********DoFired**************************************");
		if (TF_GameInfo.getInstance().currentState == TF_GameState.On_Game)
		{
			TF_GameInfo.getInstance().GameScene.UpdateWhenFired(num3, newScore, num4, (float)num2);
		}
	}

	private void DoGunHitFish(ArrayList args)
	{
		int num = int.Parse((string)args[0]);
		Console.WriteLine("gunid: " + num);
		int num2 = int.Parse((string)args[1]);
		Console.WriteLine("gunValue: " + num2);
		int num3 = int.Parse((string)args[2]);
		Console.WriteLine("seatid: " + num3);
		bool flag = Convert.ToBoolean(args[3]);
		Console.WriteLine("bCreateX2Gun: " + flag);
		bool flag2 = Convert.ToBoolean(args[4]);
		Console.WriteLine("bDeadKnife: " + flag2);
		int num4 = int.Parse((string)args[5]);
		UnityEngine.Debug.Log("fishDeadScore: " + num4);
		int num5 = int.Parse((string)args[6]);
		Console.WriteLine("totalScore: " + num5);
		if (TF_GameInfo.getInstance().currentState == TF_GameState.On_Game)
		{
			TF_GameInfo.getInstance().GameScene.UpdateWhenDead(num3, num5, num4, num2);
			TF_FishPoolMngr.GetSingleton().totalScore = num4;
		}
		if (flag && num3 == TF_GameMngr.GetSingleton().mPlayerSeatID && TF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			TF_BulletPoolMngr.GetSingleton().SetLizi();
		}
		string text = (string)args[7];
		string[] array = text.Split('|');
		TF_HitFish[] array2 = new TF_HitFish[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string[] array3 = array[i].Split('#');
			array2[i] = new TF_HitFish();
			array2[i].fishid = int.Parse(array3[0]);
			array2[i].fishtype = int.Parse(array3[1]);
			array2[i].fx = Convert.ToDouble(array3[2]);
			array2[i].fy = Convert.ToDouble(array3[3]);
			array2[i].bet = int.Parse(array3[4]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (!TF_GameParameter.G_bTest && TF_NetMngr.GetSingleton().IsGameSceneLoadOk && TF_FishPoolMngr.GetSingleton() != null && TF_EffectMngr.GetSingleton() != null)
			{
				Vector3 pos = Vector3.right * (float)array2[j].fx + Vector3.up * (float)array2[j].fy;
				TF_FishPoolMngr.GetSingleton().SetFishDie(array2[j].fishid, num2, array2[j].fishtype, array2[j].bet, num3, pos);
				if (flag && j == 0)
				{
					TF_EffectMngr.GetSingleton().PlayLiziCardFly(num3, pos);
				}
			}
		}
		if (flag2)
		{
			string text2 = (string)args[8];
			Console.WriteLine("strHitFish2：" + text2);
			string[] array4 = text2.Split('|');
			TF_HitFish[] array5 = new TF_HitFish[array4.Length];
			for (int k = 0; k < array4.Length; k++)
			{
				string[] array6 = array4[k].Split('#');
				array5[k] = new TF_HitFish();
				array5[k].fishid = int.Parse(array6[0]);
				array5[k].fishtype = int.Parse(array6[1]);
				array5[k].fx = Convert.ToDouble(array6[2]);
				array5[k].fy = Convert.ToDouble(array6[3]);
				array5[k].bet = int.Parse(array6[4]);
			}
			for (int l = 0; l < array4.Length; l++)
			{
				if (!TF_GameParameter.G_bTest && TF_FishPoolMngr.GetSingleton() != null && TF_EffectMngr.GetSingleton() != null)
				{
					Vector3 pos2 = Vector3.right * (float)array5[l].fx + Vector3.up * (float)array5[l].fy;
					TF_FishPoolMngr.GetSingleton().SetFishDie(array5[l].fishid, num2, array5[l].fishtype, array5[l].bet, num3, pos2);
				}
			}
		}
		UnityEngine.Debug.Log("**********2*********DoGunHitfish**************************************");
	}

	private void DoNewFishGroup(int[] args)
	{
		Console.WriteLine("**********1*********DoNewfishGroup**************************************");
		int num = args[0];
		UnityEngine.Debug.Log("fishGroupType: " + num);
		Console.WriteLine("**********2********DoNewfishGroup**************************************");
		TF_SceneBgMngr.GetSingleton().BigFishFormatReset();
		TF_Formation.GetSingleton().ShowFormation((TF_FORMATION)num);
		if (TF_GameInfo.getInstance().currentState == TF_GameState.On_Game)
		{
			TF_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	private void DoNewFish(string[] args)
	{
		int num = 0;
		int num2 = int.Parse(args[0]);
		int num3 = int.Parse(args[1]);
		int num4 = int.Parse(args[2]);
		int newFishPathType = int.Parse(args[3]);
		int[] array = null;
		if (args.Length == 5)
		{
			int num5 = int.Parse(args[4]);
			array = new int[1]
			{
				num5
			};
			num = 1;
		}
		else if (args.Length == 6)
		{
			int num6 = int.Parse(args[4]);
			int num7 = int.Parse(args[5]);
			array = new int[2]
			{
				num6,
				num7
			};
			num = 2;
		}
		TF_FISH_TYPE[] array2 = null;
		if (num > 0)
		{
			array2 = new TF_FISH_TYPE[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = (TF_FISH_TYPE)array[i];
			}
		}
		int num8 = num4;
		if (num8 <= 0)
		{
			return;
		}
		TF_FishPathType[] array3 = new TF_FishPathType[num8];
		for (int j = 0; j < num8; j++)
		{
			array3[j] = new TF_FishPathType();
			array3[j].fishId = num2 + j;
			array3[j].newFishPathType = newFishPathType;
		}
		if (num3 != 41 && TF_NetMngr.GetSingleton().IsGameSceneLoadOk && TF_FishPoolMngr.GetSingleton() != null && !TF_GameParameter.G_bTest)
		{
			if (num3 == 32 && array2 != null)
			{
				TF_FishPoolMngr.GetSingleton().CreateCoralReefsFish(array2[0], array3[0].newFishPathType, array3[0].fishId);
			}
			else
			{
				TF_FishPoolMngr.GetSingleton().CreateFish((TF_FISH_TYPE)num3, array3[0].newFishPathType, array3[0].fishId, num4);
			}
			if (TF_GameInfo.getInstance().CountTime)
			{
				TF_FishPoolMngr.GetSingleton().UnFixAllFish();
			}
		}
		if (TF_GameInfo.getInstance().currentState == TF_GameState.On_Game)
		{
			TF_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	private void DoClearFish(int[] args, bool isBackGround)
	{
		Console.WriteLine("**********1*********DoClearfish**************************************");
		if (TF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			TF_SceneBgMngr.GetSingleton().ChangeScene();
		}
	}

	private void DoForbitFired(object[] args)
	{
		Console.WriteLine("**********1*********DoForbitFired**************************************");
		Console.WriteLine("**********2*********DoForbitFired**************************************");
		if (TF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			TF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = false;
		}
	}

	private void DoScrollMessage(object[] args)
	{
		Console.WriteLine("**********1*********DoScrollMessage**************************************");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string text = (string)dictionary["Surname"];
		int num = (int)dictionary["Roomid"];
		string text2 = (num != 1) ? "竞技厅" : "练习厅";
		string text3 = (string)dictionary["deskName"];
		string text4 = (string)dictionary["Fishname"];
		int num2 = (int)dictionary["Score"];
		string noticeMessage = $"<color=red>恭喜:</color>玩家<color=red>{text}</color>在<color=green>{text2}</color><color=blue>{text3}</color>中捕获<color=yellow>{text4}</color>，获得<color=red>{num2}</color>分";
		Console.WriteLine("**********2*********DoScrollMessage**************************************");
		if (TF_GameInfo.getInstance().currentState == TF_GameState.On_Game)
		{
			TF_GameInfo.getInstance().GameScene.AddNotice(noticeMessage);
		}
	}

	private void DoPlayerInfo(object[] args)
	{
		Console.WriteLine("**********1*********DoPlayerInfo**************************************");
		TF_User tF_User = new TF_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["seatId"];
		int num2 = (int)dictionary["honor"];
		Console.WriteLine("seatid: " + num);
		Console.WriteLine("honor: " + num2);
		Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
		tF_User.id = (int)dictionary2["id"];
		tF_User.username = (string)dictionary2["username"];
		tF_User.nickname = (string)dictionary2["nickname"];
		tF_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
		tF_User.level = (int)dictionary2["level"];
		tF_User.gameGold = (int)dictionary2["gameGold"];
		tF_User.expeGold = (int)dictionary2["expeGold"];
		tF_User.photoId = (int)dictionary2["photoId"];
		tF_User.overflow = (int)dictionary2["overflow"];
		tF_User.gameScore = (int)dictionary2["gameScore"];
		tF_User.expeScore = (int)dictionary2["expeScore"];
		tF_User.type = (int)dictionary2["type"];
		TF_GameInfo.getInstance().getPersonInfo(new TF_UserInfo(tF_User), num2);
	}

	private void DoSendChat(object[] args)
	{
		Console.WriteLine("**********1*********DoSendChat**************************************");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["chatType"];
		Console.WriteLine("chatType: " + num);
		int num2 = (int)dictionary["senderSeatId"];
		Console.WriteLine("senderSeatId: " + num2);
		string text = (string)dictionary["chatMessage"];
		Console.WriteLine("chatMessage: " + text);
		Console.WriteLine("**********2*********DoSendChat**************************************");
		if (TF_GameInfo.getInstance().currentState == TF_GameState.On_Game)
		{
			TF_GameInfo.getInstance().GameScene.sptDlgChat.UpdateChatInfo(num, num2, text);
		}
	}

	private void DoGameShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoGameShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoGameShutup**************************************");
		TF_GameInfo.getInstance().IsGameShuUp = flag;
	}

	private void DoUserShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoUserShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoUserShutup**************************************");
		TF_GameInfo.getInstance().IsUserShutUp = flag;
	}

	private void DoUserAward(int[] args)
	{
		Console.WriteLine("**********1*********DoUserAward**************************************");
		int num = args[0];
		Console.WriteLine("gold: " + num);
		Console.WriteLine("**********2*********DoUserAward**************************************");
		TF_GameInfo.getInstance().updateUser("gameCoin", num);
		TF_TipManager.getInstance().ShowTip(TF_TipType.GivingCoin, num);
	}

	private void DoGameGold(int[] args)
	{
		Console.WriteLine("**********1*********DoGameGold**************************************");
		int num = args[0];
		MonoBehaviour.print("gameGold: " + num);
		Console.WriteLine("**********2*********DoGameGold**************************************");
		TF_GameInfo.getInstance().updateUser("gameCoin", num);
	}

	private void DoExpeGold(int[] args)
	{
		Console.WriteLine("**********1*********DoExpeGold**************************************");
		int num = args[0];
		MonoBehaviour.print("expeGold: " + num);
		Console.WriteLine("**********2*********DoExpeGold**************************************");
		TF_GameInfo.getInstance().updateUser("testCoin", num);
	}

	private void DoAddExpeGoldAuto(bool[] args)
	{
		Console.WriteLine("**********1*********DoAddExpeGoldAuto**************************************");
		bool flag = args[0];
		Console.WriteLine("bOk: " + flag);
		Console.WriteLine("**********2*********DoAddExpeGoldAuto**************************************");
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.getTestCoin);
		TF_TipManager.getInstance().ShowTip(TF_TipType.ApplyForExpCoin_Success);
	}

	private void DoOverflow(object[] args)
	{
		Console.WriteLine("**********1*********DoOverflow**************************************");
		TF_TipManager.getInstance().ShowTip(TF_TipType.CoinOverFlow);
	}

	private void DoQuitToLogin(int[] args)
	{
		Console.WriteLine("**********1*********DoQuitToLogin**************************************");
		int num = args[0];
		Console.WriteLine("type: " + num);
		Console.WriteLine("**********2*********DoQuitToLogin**************************************");
		switch (num)
		{
		case 1:
			TF_TipManager.getInstance().ShowTip(TF_TipType.ServerUpdate);
			break;
		case 2:
			TF_TipManager.getInstance().ShowTip(TF_TipType.Game_UserIdFrozen);
			break;
		case 3:
			TF_TipManager.getInstance().ShowTip(TF_TipType.UserIdDeleted);
			break;
		case 4:
			TF_TipManager.getInstance().ShowTip(TF_TipType.UserIdRepeative);
			TF_Sockets.GetSingleton().isReconnect = false;
			break;
		case 5:
			TF_TipManager.getInstance().ShowTip(TF_TipType.UserPwdChanged);
			break;
		case 6:
			TF_TipManager.getInstance().ShowTip(TF_TipType.LoseTheServer);
			break;
		}
	}

	private void DoQuitToRoom(int[] args)
	{
		Console.WriteLine("**********1*********DoQuitToRoom**************************************");
		int num = args[0];
		Console.WriteLine("type: " + num);
		Console.WriteLine("**********2*********DoQuitToRoom**************************************");
		switch (num)
		{
		case 1:
			TF_TipManager.getInstance().ShowTip(TF_TipType.TableDeleted);
			break;
		case 2:
			TF_TipManager.getInstance().ShowTip(TF_TipType.TableConfigChanged);
			break;
		default:
			TF_TipManager.getInstance().ShowTip(TF_TipType.LongTimeNoHandle);
			break;
		}
	}

	private void DoLockFish(ArrayList args)
	{
		bool locking = (bool)args[0];
		int num = (int)args[1];
		Console.WriteLine("fishId: " + num);
		int num2 = (int)args[2];
		Console.WriteLine("seatId: " + num2);
		TF_FishPoolMngr.GetSingleton().LockFish(num, num2, locking);
	}

	private void DoUnLockFish(int[] args)
	{
		Console.WriteLine("**********1*********DoUnLockFish**************************************");
		int num = args[0];
		Console.WriteLine("fishId: " + num);
		int num2 = args[1];
		Console.WriteLine("seatId: " + num2);
		Console.WriteLine("**********2*********DoUnLockFish**************************************");
		TF_FishPoolMngr.GetSingleton().UnLockFish(num, num2);
	}

	private void DoUnLockScreen(object[] args)
	{
		Console.WriteLine("**********1*********unLockScreen**************************************");
		TF_FishPoolMngr.GetSingleton().UnFixAllFish();
		Console.WriteLine("**********1*********unLockScreen**************************************");
	}

	private void DoCheckStatus(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["messageStatus"];
		TF_BulletPoolMngr.GetSingleton().BGameMaintained = ((num == 0) ? true : false);
		if (num == 0)
		{
			TF_TipManager.getInstance().ShowTip(TF_TipType.GameMaintained);
		}
	}

	private void DoFishLimit(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int[] fishCount = new int[6]
		{
			(int)dictionary["15"],
			(int)dictionary["16"],
			(int)dictionary["17"],
			(int)dictionary["18"],
			(int)dictionary["19"],
			(int)dictionary["23"]
		};
		TF_GameMngr.GetSingleton().UpdateFishRecord(fishCount);
	}
}
