using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class BCBM_LoginData
{
	public enum SceneType
	{
		Login,
		Lobby,
		DanTiao,
		Baile2,
		DanShuang,
		_208,
		DanZhuangBaiLe,
		XiaWeiYi
	}

	public string user_id;

	public string token;

	public string username;

	public string ALLScroce;

	public string Fraction;

	public string login_ip;

	public string telephone;

	public string status;

	public string userStatus;

	public int coindown;

	public int room_id;

	public int choosegame;

	public string roomlitmit;

	public string roomcount;

	public string seating;

	public string version;

	public string iosversion;

	public string gameModle;

	public string dropContent;

	public string farURL;

	public string nearURL;

	public string fullURL;

	public string gameType;

	public string QRcode;

	public string DownloadUrl;

	public string Promotion;

	public string AgentNumber;

	public string ServiceUrl;

	public string BankUser;

	public string BankName;

	public string BankAddress;

	public string BankNumber;

	public string LiveKey;

	public string periods;

	public string season;

	public string drop_date;

	public string winnings;

	public int overTime;

	public int gamepanelnum;

	public static bool IsConnect;

	public static bool IsLogin;

	public static bool IsOnPing;

	public static float OverTime;

	public static bool IsVideoPlay;

	public bool isOpenError = true;

	public bool isFull = true;

	public string isAnchor;

	public List<int> game_id;

	public List<string> snid;

	public string servicesInfo;

	public bool picOrWord = true;

	public readonly string URL;

	public readonly string loginAPI;

	public readonly string raganstionAPI;

	public readonly string gamelistAPI;

	public readonly string roomlistAPI;

	public readonly string roominstartAPI;

	public readonly string roominendAPI;

	public readonly string counwtAPI;

	public readonly string winlistAPI;

	public readonly string gameinfoPollingAPI;

	public readonly string betinAPI;

	public readonly string wingetforinterfroAPI;

	public readonly string caneldownAPI;

	public readonly string hallaliveAPI;

	public readonly string VersioninfoAPI;

	public readonly string changepasswordAPI;

	public readonly string usercoininhistoryAPI;

	public readonly string userCut;

	public readonly string userCutSend;

	public readonly string newInit;

	public readonly string BetDown_mpzzs;

	public readonly string winHistory;

	public readonly string playerOnLine;

	public readonly string playerHistory;

	public readonly string winInfo;

	public readonly string betCancel_mpzzs;

	public readonly string betDown_bl;

	public readonly string betCancel_bl;

	public readonly string betDown_ds;

	public readonly string betCancel_ds;

	public readonly string betDown_elb;

	public readonly string betCancel_elb;

	public readonly string betDown_td;

	public readonly string betCancel_td;

	public readonly string betCancel_lh;

	public readonly string betDown_lh;

	public readonly string liveVideo;

	public readonly string betDown_xwy;

	public readonly string betCancel_xwy;

	public readonly string betDown_dxb;

	public readonly string betCancel_dxb;

	public readonly string services;

	public BCBM_LoginData(string url, string loginapi, string raganstionapi, string gamelistapi, string roomlistapi, string roominstart, string roominend, string counwtdown, string winlist, string gameinfoPolling, string betin, string wingetforinterform, string caneldown, string hallalive, string version, string changepassword, string coininhistory, string usercut, string usercutSend, string NewInit, string newBetDown, string winHistory, string playerOnLine, string playerHistory, string winInfo, string betCancel, string betDown_bl, string betCancel_bl, string betDown_ds, string betCancel_ds, string betDown_elb, string betCancel_elb, string betDown_td, string betCancel_td, string betDown_lh, string betCancel_lh, string liveVideo, string betDown_xwy, string betCancel_xwy, string betDown_dxb, string betCancel_dxb, string services)
	{
		URL = url;
		loginAPI = loginapi;
		raganstionAPI = raganstionapi;
		gamelistAPI = gamelistapi;
		roomlistAPI = roomlistapi;
		roominstartAPI = roominstart;
		roominendAPI = roominend;
		counwtAPI = counwtdown;
		winlistAPI = winlist;
		gameinfoPollingAPI = gameinfoPolling;
		betinAPI = betin;
		wingetforinterfroAPI = wingetforinterform;
		caneldownAPI = caneldown;
		hallaliveAPI = hallalive;
		VersioninfoAPI = version;
		changepasswordAPI = changepassword;
		usercoininhistoryAPI = coininhistory;
		userCut = usercut;
		userCutSend = usercutSend;
		newInit = NewInit;
		BetDown_mpzzs = newBetDown;
		this.winHistory = winHistory;
		this.playerOnLine = playerOnLine;
		this.playerHistory = playerHistory;
		this.winInfo = winInfo;
		betCancel_mpzzs = betCancel;
		this.betDown_bl = betDown_bl;
		this.betCancel_bl = betCancel_bl;
		this.betDown_ds = betDown_ds;
		this.betCancel_ds = betCancel_ds;
		this.betDown_elb = betDown_elb;
		this.betCancel_elb = betCancel_elb;
		this.betDown_td = betDown_td;
		this.betCancel_td = betCancel_td;
		this.betDown_lh = betDown_lh;
		this.betCancel_lh = betCancel_lh;
		this.liveVideo = liveVideo;
		this.betDown_xwy = betDown_xwy;
		this.betCancel_xwy = betCancel_xwy;
		this.betDown_dxb = betDown_dxb;
		this.betCancel_dxb = betCancel_dxb;
		this.services = services;
	}

	public static bool JsonDataContainsKey(JsonData data, string key)
	{
		bool result = false;
		if (data == null)
		{
			return result;
		}
		if (!data.IsObject)
		{
			return result;
		}
		if (data == null)
		{
			return result;
		}
		if (((IDictionary)data).Contains((object)key))
		{
			result = true;
		}
		return result;
	}
}
