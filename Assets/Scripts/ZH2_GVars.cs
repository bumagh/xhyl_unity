using JsonFx.Json;

using LitJson;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;

public static class ZH2_GVars
{
    public enum LanguageType
    {
        Chinese,//中文
        English,//英文
        Vietnam,//越南
        Thai,//泰文
    }

    public enum GameType
    {
        water_desk,
        fish_king_desk3,
        desk,
        card_desk,
        bullet_fish_desk,
        mermaid_desk,
        fish_desk,
        bcbm_desk,
        jsys_desk,
        golden_cicada_fish_desk,
        li_kui_fish_desk,
        elephant_fish_desk
    }

    public enum GameType_DJ
    {
        hall = 0,
        water_desk = 6,
        fish_king_desk3 = 9,
        lion_desk = 1,
        card_desk = 2,
        bullet_fish_desk = 4,
        mermaid_desk = 5,
        fish_desk = 1,
        bcbm_desk = 20,
        jsys_desk = 8,
        golden_cicada_fish_desk = 11,
        li_kui_fish_desk = 10,
        elephant_fish_desk = 12,
        heaven_fish_desk = 39,
        lhd_desk = 33
    }

    public static string shortConnection = string.Empty;

    public const string ABDir = "AB";

    public const string touristPwd = "123456";

    public const string touristAccountPrefix_cn = "游客";

    public const string touristAccountPrefix_en = "tourist";

    public static string IPShortLink = "http://129.204.104.203:911/";

    public static string IPAddress = string.Empty;

    public static string IPAddress_Game = string.Empty;

    public static float moveTime = 0.25f;

    public static int IPPort = 0;

    public static string versionURL = string.Empty;

    public static string versionCode = string.Empty;

    public static int userId;

    public static int gameGold;

    public const string shader_ImgLine = "FlowLightShader_ImgLine";

    public static bool isStartGame = false;

    public static bool isConnect = false;

    public static bool isLock = false;

    public static bool isShowTingName = false;

    public static Dictionary<string, object> hallInfo = new Dictionary<string, object>();

    public static JsonData hallInfo2 = new JsonData();

    public static JsonData activity_signIn = new JsonData();

    public static Dictionary<string, object> functionOpen = new Dictionary<string, object>();

    public static Dictionary<string, string> loginUsers = new Dictionary<string, string>();

    public static bool isShowSelectionPanel = true;

    public static bool isClickTableEnterGame;

    public static int safeBoxPassword;

    public static Action<GameObject> DestroyFishNet;

    public static Action<GameObject> DestroyEffSimilarLightning;

    public static int alreadySignInDay = 0;

    public static bool isUnload = false;

    public static string tagGame = string.Empty;

    public static LanguageType language_enum = LanguageType.Chinese;

    public static string language = "en";

    public static GameType gameType = GameType.water_desk;

    public static string selectionNum = "selectionNum";

    public static string username = string.Empty;

    public static string pwd = string.Empty;

    public static string nickname = string.Empty;

    public static string netVersion = "1.2.14";

    public static bool isTourist = false;

    public static bool isOverflow = false;

    public static int payMode = 1;

    public static int signInDay = -1;

    public static bool useRaffleStore = false;

    public static int pwdSafeBox = 0;

    public static bool hasSetUpProtectQuestion = false;

    public static int gold = 0;

    public static int expeGold = 0;

    public static int raffle = 0;

    public static int savedGameGold = 0;

    public static int savedLottery = 0;

    public static Dictionary<int, OwnShopProp> ownedProps = new Dictionary<int, OwnShopProp>();

    public static Dictionary<string, int> StatusSwitch = new Dictionary<string, int>();

    public static bool isInternational = true;

    public static bool isFirstToDaTing = true;

    public static bool isGameToDaTing = false;

    public static bool isCanSenEnterGame = false;

    public static float firingInterval = 0.33f;

    public static float shellMultiple = 1.1f;

    public static int niuMoWangBeiLv = -1;

    public static int antiInitNum = 0;

    public static int selectRoomId = 1;

    public static string payIp = "139.186.166.238";

    public static int payPort1 = 8283;

    public static int payPort2 = 8283;

    public static int payPort3 = 51118;

    public static int[] orders = new int[64]
    {
        0,
        1,
        4,
        3,
        2,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13,
        14,
        15,
        16,
        17,
        18,
        19,
        20,
        21,
        22,
        23,
        24,
        25,
        26,
        27,
        28,
        29,
        30,
        31,
        32,
        33,
        34,
        35,
        36,
        37,
        38,
        39,
        40,
        41,
        42,
        43,
        44,
        45,
        46,
        47,
        48,
        49,
        50,
        51,
        52,
        53,
        54,
        55,
        56,
        57,
        58,
        59,
        60,
        61,
        62,
        63
    };

    public static string[] ReMen = new string[]
    {
        "摇钱树",
        "金蟾捕鱼",
        "QQ美人鱼",
        "牛魔王",
        "大闹天宫",
        "幸运六狮",
        "李逵劈鱼",
        "沙巴体育",
        "水浒传",
        "单挑",
        "金沙银沙",
        "抢庄牛牛",
        "炸金花",
        "三公",
        "21点",
        "通比牛牛",
        "极速炸金花",
        "百家乐"
    };

    public static string[] XingLi = new string[]
    {
        "摇钱树",
        "单挑",
        "QQ美人鱼",
        "幸运六狮",
        "奔驰宝马",
        "大闹天宫",
        "牛魔王",
        "水浒传",
        "李逵劈鱼",
        "金蟾捕鱼",
    };

    public static string[] ShiXun = new string[9]
    {
        "AsiaGaming",
        "牛牛",
        "欧洲厅百家乐",
        "龙虎",
        "牛牛2",
        "亚星",
        "21点",
        "旗舰国际厅",
        "多台"
    };

    public static string[] QiPai = new string[20]
    {
        "二八杠",
        "三公",
        "通比牛牛",
        "抢庄牌九",
        "斗地主",
        "百人牛牛",
        "血流成河",
        "二人麻将",
        "单挑牛牛",
        "鱼虾蟹",
        "牛牛",
        "炸金花",
        "21点",
        "极速炸金花",
        "十三水",
        "百家乐",
        "万人炸金花",
        "看牌抢庄牛牛",
        "百人骰宝",
        "押宝抢庄牛牛"
    };

    public static string[] DianZi = new string[28]
    {
        "小爱神",
        "孙二娘",
        "西游记",
        "斗鸡",
        "跳起来",
        "五福临门",
        "武圣",
        "宙斯",
        "一炮捕鱼",
        "跳过来",
        "蹦迪",
        "东方神起",
        "火烧连环船2",
        "金钱树",
        "火爆777",
        "福神报喜",
        "精灵",
        "跳高高",
        "跳高高2",
        "洪福齐天",
        "跳更高",
        "单手跳高高",
        "直式蹦迪",
        "飞起来",
        "直式洪福齐天",
        "冰雪女王",
        "金鸡报喜",
        "飞龙在天"
    };

    public static string[] TiYu = new string[2]
    {
        "CQ9体育",
        "沙巴体育"
    };

    public static string[] ShowDown = new string[0];

    public static int[] HotUpdatesGameID = new int[14]
    {
        1,
        2,
        3,
        4,
        5,
        8,
        79,
        43,
        52,
        21,
        38,
        39,
        92,
        103
    };

    public static int[] HotUpdatesGameType = new int[15]
    {
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        10,
        12,
        14,
        17,
        23,
        40,
        102
    };

    public static List<string> daTingResourceName;

    public static string heartStr = string.Empty;

    public static string[] versions = new string[10]
    {
        "9.0.10",
        "9.0.1",
        "9.0.1",
        "9.0.1",
        "9.0.1",
        "9.0.1",
        "9.0.1",
        "9.0.1",
        "9.0.1",
        "9.0.1"
    };

    public static string[] QianpaoName = new string[2]
    {
        "千炮",
        "qianpao"
    };

    public static string[] WanpaoName = new string[2]
    {
        "万炮",
        "wanpao"
    };

    public static bool isShowHallName = true;

    public static User user;

    public static string downloadStr = string.Empty;

    public static int giftMode = 0;

    public static int shareMode = 2;

    public static int specialMark = 2;

    public static int ag = 1;

    public static bool ScoreOverflow = false;

    public static string gamesJsonFilePath = "Assets/Resources/Saves/games.json";

    private static bool localDown = false;

    public static bool isStartedFromLuaGame = false;

    public static bool isStartedFromGame = false;

    public static bool lockSend = false;

    public static bool lockQuit = false;

    public static bool lockReconnect = false;

    public static bool lockRelogin = false;

    public static string downloadVersion = "23.06.13.01";

    public static int allSetType = 0;

    public static bool isGetPublicKey = false;

    public static Action<object[]> getCheckSafeBoxPwd;

    public static Action<object[]> sendCheckSafeBoxPwd;

    public static Action<object[]> getChangeSafeBoxPwd;

    public static Action<object[]> sendChangeSafeBoxPwd;

    public static Action<object[]> getDeposit;

    public static Action<object[]> sendDeposit;

    public static Action<object[]> getExtract;

    public static Action<object[]> sendExtract;

    public static Action closeSafeBox;

    public static Action<object[]> getTransactionRecord;

    public static Action<object[]> sendTransactionRecord;

    public static Action<object[]> getPay;

    public static Action<object[]> sendPay;

    public static Action<object[]> getGamePay;

    public static Action<object[]> sendGamePay;

    public static Action sendLogin;

    public static Action<GameType_DJ> OpenCheckSafeBoxPwdPanel;

    public static Action OpenSafeBoxPwdPanel;

    public static Action<GameType_DJ> OpenPlyBoxPanel;

    public static Action saveScore;

    public static Action<object[]> setMonkeyKingBet;

    public static string AccountName
    {
        get
        {
            string str = string.Empty;
            if (isTourist)
            {
                str = ((language_enum == LanguageType.Chinese) ? "游客" : ((language_enum != LanguageType.English) ? "น\u0e31กท\u0e48องเท\u0e35\u0e48ยว" : "Tourist"));
            }
            return str + username;
        }
    }

    public static string GetSoleIdentify
    {
        get
        {
            string text = (DeviceIdentifier + DeviceModel).Replace(" ", string.Empty);
            char[] array = text.ToCharArray();
            string text2 = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                if (!IsNumber(array[i].ToString()))
                {
                    text2 += array[i];
                }
            }
            return text2.Replace(" ", string.Empty);
        }
    }

    private static string DeviceIdentifier => SystemInfo.deviceUniqueIdentifier;

    private static string DeviceModel => SystemInfo.deviceModel;

    public static string DownBaseUrl => "http://192.168.1.80:8080/";

    public static string DataPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/";
            }
            return Path.GetFullPath(Path.Combine(Application.dataPath, "../Down/")).Replace('\\', '/');
        }
    }

    public static string WWWDataPath
    {
        get
        {
            if (Application.isEditor)
            {
                return "file://" + DataPath;
            }
            if (Application.isMobilePlatform || Application.isConsolePlatform)
            {
                return "file:///" + Application.persistentDataPath + "/";
            }
            return "file://" + DataPath;
        }
    }

    public static string PlatformDir => "Android";

    public static void SetHeartStr(string type, string game_table = "", string desk_id = "", string game_score = "")
    {
        HeartBeatData heartBeatData = new HeartBeatData();
        heartBeatData.type = type;
        heartBeatData.game_table = game_table;
        heartBeatData.desk_id = desk_id;
        heartBeatData.game_score = game_score;
        heartBeatData.user_id = user.id.ToString();
        HeartBeatData obj = heartBeatData;
        heartStr = JsonUtility.ToJson(obj);
    }

    public static bool IsOpen(string name)
    {
        if (functionOpen.Count < 1)
        {
            UnityEngine.Debug.LogError("====functionOpen为空====");
            return false;
        }
        for (int i = 0; i < functionOpen.Count; i++)
        {
            if (functionOpen.ContainsKey(name) && functionOpen[name].ToString() != string.Empty && functionOpen[name].ToString() != "null")
            {
                int num = int.Parse(functionOpen[name].ToString());
                return num == 0;
            }
        }
        return false;
    }

    public static string GetLoginUserPassword(string user)
    {
        if (loginUsers.ContainsKey(user))
        {
            return loginUsers[user];
        }
        return string.Empty;
    }

    public static void ClearLoginUser()
    {
        loginUsers.Clear();
        SaveLoginUsers();
    }

    public static void AddLoginUser()
    {
        string key = username;
        if (!loginUsers.ContainsKey(key))
        {
            loginUsers.Add(key, pwd);
        }
        else
        {
            loginUsers[key] = pwd;
        }
        SaveLoginUsers();
    }

    public static void RemoveLoginUser(int index)
    {
        if (index >= 0 && index < loginUsers.Count)
        {
            int num = 0;
            string key = string.Empty;
            foreach (KeyValuePair<string, string> loginUser in loginUsers)
            {
                if (num == index)
                {
                    key = loginUser.Key;
                    break;
                }
                num++;
            }
            if (loginUsers.ContainsKey(key))
            {
                loginUsers.Remove(key);
            }
            SaveLoginUsers();
        }
    }

    public static void SaveLoginUsers()
    {
        string text = string.Empty;
        foreach (KeyValuePair<string, string> loginUser in loginUsers)
        {
            text = text + loginUser.Key + "~" + loginUser.Value;
            text += "&";
        }
        UnityEngine.Debug.Log("保存字符串:" + text);
        PlayerPrefs.SetString("LoginUserList", text);
    }

    public static void LoadLoginUsers()
    {
        loginUsers.Clear();
        string @string = PlayerPrefs.GetString("LoginUserList");
        UnityEngine.Debug.Log("LoginUserList:" + @string);
        string text = string.Empty;
        string text2 = string.Empty;
        int num = 0;
        string text3 = @string;
        for (int i = 0; i < text3.Length; i++)
        {
            char c = text3[i];
            switch (c)
            {
                case '&':
                    if (!loginUsers.ContainsKey(text))
                    {
                        loginUsers.Add(text, text2);
                    }
                    text = string.Empty;
                    text2 = string.Empty;
                    num = 0;
                    break;
                case '~':
                    num = 1;
                    break;
                default:
                    if (num == 0)
                    {
                        text += c.ToString();
                    }
                    if (num == 1)
                    {
                        text2 += c.ToString();
                    }
                    break;
            }
        }
    }

    public static string GetBreviaryName(string name)
    {
        string result = name;
        if (!string.IsNullOrEmpty(name) && name.Length > 4)
        {
            result = $"{name.Substring(0, 2)}..{name.Substring(name.Length - 2, 2)}";
        }
        return result;
    }

    public static bool IsObjectInTheArray(string[] sits, string name)
    {
        for (int i = 0; i < sits.Length; i++)
        {
            if (name.Equals(sits[i]))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsObjectInTheArray(int[] sits, int id)
    {
        for (int i = 0; i < sits.Length; i++)
        {
            if (id.Equals(sits[i]))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsObjectInTheArray(List<string> sits, string name)
    {
        for (int i = 0; i < sits.Count; i++)
        {
            if (name.Equals(sits[i]))
            {
                return true;
            }
        }
        return false;
    }

    public static string ShowTip(string Ch, string En, string Ti = "", string Vn = "")
    {
        switch (language_enum)
        {
            case LanguageType.Chinese: return Ch;
            case LanguageType.English: return En;
            case LanguageType.Thai: return Ti;
            case LanguageType.Vietnam: return Vn;
            default: return Ch;
        }
    }

    public static void ClearConsole()
    {
    }

    public static string EncodeMessage(object obj)
    {
        string s = JsonFx.Json.JsonWriter.Serialize(obj);
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        s = Convert.ToBase64String(bytes);
        string randomStr = GetRandomStr(s);
        bytes = Encoding.UTF8.GetBytes(randomStr);
        randomStr = Convert.ToBase64String(bytes);
        return Rever(randomStr);
    }

    public static string DecodeMessage(string str)
    {
        str = Rever(str);
        byte[] bytes = Convert.FromBase64String(str);
        str = Encoding.UTF8.GetString(bytes);
        string s = str.Substring(2, str.Length - 5);
        bytes = Convert.FromBase64String(s);
        str = Encoding.UTF8.GetString(bytes);
        return str;
    }

    public static bool IsNumber(string strInput)
    {
        Regex regex = new Regex("[^a-zA-Z0-9]");
        if (regex.IsMatch(strInput))
        {
            return true;
        }
        return false;
    }

    public static string GetRandomStr(string str)
    {
        string[] collection = new string[62]
        {
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "G",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"
        };
        List<string> original = new List<string>(collection);
        original = Shuffle(original);
        string text = string.Empty;
        for (int i = 0; i < 5; i++)
        {
            text = ((i == 2) ? (text + str + original[i]) : (text + original[i]));
        }
        return text;
    }

    public static string Rever(string str)
    {
        char[] array = str.ToCharArray();
        Array.Reverse((Array)array);
        return new string(array);
    }

    public static List<T> Shuffle<T>(List<T> original)
    {
        System.Random random = new System.Random();
        int num = 0;
        for (int i = 0; i < original.Count; i++)
        {
            num = random.Next(0, original.Count - 1);
            if (num != i)
            {
                T value = original[i];
                original[i] = original[num];
                original[num] = value;
            }
        }
        return original;
    }

    public static string GetOSDir()
    {
        return "Android";
    }
}
