using JsonFx.Json;

using LitJson;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SharePanelController : Tween_SlowAction
{
    private string QRString = string.Empty;

    [SerializeField]
    private Sprite[] sprites;

    public Image m_QRCode;

    private int DataType = -1;

    private List<ShareLog> sList = new List<ShareLog>();

    public AndroidJavaClass jc;

    public AndroidJavaObject jo;

    private Color chooseColor;

    private Color defaultColor;

    public Button[] rankBtns;

    public GameObject[] gamePanels;

    public Text[] MyShareText;

    public Text[] GetInfoText;

    public GameObject GetInfoContent;

    public GameObject GetInfoNoText;

    public GameObject ExtensionContent;

    public Text[] ZhiShuChaText;

    public InputField ZhiShuChaInput;

    public GameObject ZhiShuChaDataPanel;

    public GameObject ZhiShuChaContent;

    public GameObject ShouYiContent;

    public GameObject QRcodePanel;

    public Text QRcodeText;

    public Image QRcodeImage;

    private void Awake()
    {
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("myShare", HandleNetMsg_myShare);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("getIncome", HandleNetMsg_getIncome);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("incomeDetail", HandleNetMsg_incomeDetail);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("promotionTutorial", HandleNetMsg_promotionTutorial);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("queryDirectlyUnderUser", HandleNetMsg_queryDirectlyUnderUser);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("searchSubordinate", HandleNetMsg_searchSubordinate);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("searchSubordinateForTime", HandleNetMsg_searchSubordinateForTime);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("getIncomeRank", HandleNetMsg_getIncomeRank);
        if (Application.platform == RuntimePlatform.Android)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        for (int i = 0; i < rankBtns.Length; i++)
        {
            int index = i;
            rankBtns[index].onClick.AddListener(delegate
            {
                ChangeTab(index);
            });
        }
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        Show();
        ChangeTab(0);
        MB_Singleton<NetManager>.GetInstance().Send("shareService/myShare", new object[1]
        {
            ZH2_GVars.user.id
        });
    }

    public void OnClosePanel()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Hide(base.gameObject);
    }

    public void ShareBtnClick_1()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        MB_Singleton<NetManager>.GetInstance().Send("shareService/myShare", new object[1]
        {
            ZH2_GVars.user.id
        });
    }

    public void ShareBtnClick_1_1()
    {
        MB_Singleton<NetManager>.GetInstance().Send("shareService/getIncome", new object[1]
        {
            ZH2_GVars.user.id
        });
    }

    public void ShareBtnClick_1_2()
    {
        QRcodePanel.SetActive(value: true);
    }

    public void ShareBtnClick_1_3()
    {
    }

    public void ShareBtnClick_2()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        MB_Singleton<NetManager>.GetInstance().Send("shareService/incomeDetail", new object[1]
        {
            ZH2_GVars.user.id
        });
    }

    public void ShareBtnClick_3()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        MB_Singleton<NetManager>.GetInstance().Send("shareService/promotionTutorial", new object[0]);
    }

    public void ShareBtnClick_4()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        MB_Singleton<NetManager>.GetInstance().Send("shareService/queryDirectlyUnderUser", new object[1]
        {
            ZH2_GVars.user.id
        });
    }

    public void ShareBtnClick_4_1()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        ZhiShuChaInput.text = string.Empty;
        ZhiShuChaText[3].text = ZH2_GVars.ShowTip("请选择时间", "Please select a time", "กรุณาเลือกเวลา", "Vui lòng chọn thời gian");
        ZhiShuChaDataPanel.SetActive(value: false);
        DataType = -1;
        for (int i = 0; i < ZhiShuChaContent.transform.childCount; i++)
        {
            ZhiShuChaContent.transform.GetChild(i).gameObject.SetActive(value: false);
        }
    }

    public void ShareBtnClick_4_2()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZhiShuChaDataPanel.activeSelf)
        {
            ZhiShuChaDataPanel.SetActive(value: false);
        }
        else
        {
            ZhiShuChaDataPanel.SetActive(value: true);
        }
    }

    public void ShareBtnClick_4_3(int data)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        DataType = data;
        switch (data)
        {
            case 0:
                ZhiShuChaText[3].text = ZH2_GVars.ShowTip("所有", "All", "ทั้งหมด", "Tất cả");
                break;
            case 1:
                ZhiShuChaText[3].text = ZH2_GVars.ShowTip("今天", "Today", "วันนี้", "Hôm nay");
                break;
            case 2:
                ZhiShuChaText[3].text = ZH2_GVars.ShowTip("昨天", "YesterDay", "เมื่อวานนี้", "Hôm qua");
                break;
            case 3:
                ZhiShuChaText[3].text = ZH2_GVars.ShowTip("本周", "Week", "สัปดาห์นี้", "Tuần này");
                break;
            case 4:
                ZhiShuChaText[3].text = "";
                ZhiShuChaText[3].text = ZH2_GVars.ShowTip("本月", "Month", "เดือนนี้", "Tháng này");
                break;
        }
        ZhiShuChaDataPanel.SetActive(value: false);
    }

    public void ShareBtnClick_4_4()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (DataType == -1)
        {
            if (!(ZhiShuChaInput.text == string.Empty))
            {
                int num = int.Parse(ZhiShuChaInput.text);
                MB_Singleton<NetManager>.GetInstance().Send("shareService/searchSubordinate", new object[2]
                {
                    ZH2_GVars.user.id,
                    num
                });
            }
        }
        else
        {
            MB_Singleton<NetManager>.GetInstance().Send("shareService/searchSubordinateForTime", new object[2]
            {
                ZH2_GVars.user.id,
                DataType
            });
        }
    }

    public void ShareBtnClick_4_5()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        ZhiShuChaInput.text = string.Empty;
        ZhiShuChaText[3].text = ZH2_GVars.ShowTip("请选择时间", "Please select a time", "กรุณาเลือกเวลา", "Vui lòng chọn thời gian");
        ZhiShuChaDataPanel.SetActive(value: false);
        DataType = -1;
        for (int i = 0; i < ZhiShuChaContent.transform.childCount; i++)
        {
            ZhiShuChaContent.transform.GetChild(i).gameObject.SetActive(value: false);
        }
    }

    public void ShareBtnClick_5()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        MB_Singleton<NetManager>.GetInstance().Send("shareService/getIncomeRank", new object[0]);
    }

    public void OnBtnClick_Copy()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        string qRString = QRString;
        if (Application.platform == RuntimePlatform.Android)
        {
            qRString = jo.Call<string>("Docopy", new object[1]
            {
                string.Empty
            });
        }
        else if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            TextEditor textEditor = new TextEditor();
            textEditor.text = string.Empty;
            TextEditor textEditor2 = textEditor;
            textEditor2.OnFocus();
            textEditor2.Copy();
        }
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Copy Done" : ((ZH2_GVars.language_enum != 0) ? "ค\u0e31ดลอก" : "已复制"));
    }

    public void ShowQRCode(string str, Image QRimage)
    {
        QRCodeMaker.QRCodeCreate(str);
        QRimage.sprite = Sprite.Create(QRCodeMaker.encoded, new Rect(0f, 0f, 512f, 512f), new Vector2(0f, 0f));
    }

    private void Fresh_SharePanel(object obj)
    {
        int sMode = (int)obj;
        ShowShareContent(sMode);
        if (sList != null)
        {
            ShowShareItem(sList.Count, sList);
        }
    }

    private void ShowShareContent(int sMode)
    {
        switch (sMode)
        {
        }
    }

    public void ChangeTab(int tabIndex)
    {
        for (int i = 0; i < rankBtns.Length; i++)
        {
            rankBtns[i].transform.GetChild(1).gameObject.SetActive(value: false);
        }
        rankBtns[tabIndex].transform.GetChild(1).gameObject.SetActive(value: true);
        for (int j = 0; j < gamePanels.Length; j++)
        {
            gamePanels[j].SetActive(value: false);
        }
        gamePanels[tabIndex].SetActive(value: true);
    }

    private void ShowShareItem(int length, List<ShareLog> sha)
    {
    }

    private void HandleNetMsg_myShare(object[] objs)
    {
        UnityEngine.Debug.Log("HandleNetMsg_myShare");
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        MyShareText[0].text = dictionary["gameId"].ToString();
        MyShareText[1].text = dictionary["promoterId"].ToString();
        MyShareText[2].text = dictionary["teamMemberPeopleNumber"].ToString();
        MyShareText[3].text = dictionary["teamMemberActivePeopleNumber"].ToString();
        MyShareText[4].text = dictionary["teamMemberAddPeopleNumber"].ToString();
        MyShareText[5].text = dictionary["directlyUnderUserPeopleNumber"].ToString();
        MyShareText[6].text = dictionary["directlyUnderUserActivePeopleNumber"].ToString();
        MyShareText[7].text = dictionary["directlyUnderUserAddPeopleNumber"].ToString();
        MyShareText[8].text = dictionary["directlyUnderAgentPeopleNumber"].ToString();
        MyShareText[9].text = dictionary["directlyUnderAgentActivePeopleNumber"].ToString();
        MyShareText[10].text = dictionary["directlyUnderAgentAddPeopleNumber"].ToString();
        MyShareText[11].text = dictionary["yesterdayIncome"].ToString();
        MyShareText[12].text = dictionary["todayIncome"].ToString();
        MyShareText[13].text = dictionary["todayIncome"].ToString();
        MyShareText[14].text = dictionary["gotIncome"].ToString();
        MyShareText[15].text = dictionary["canIncome"].ToString();
        m_QRCode.transform.GetChild(0).GetComponent<Text>().text = "ID:" + ZH2_GVars.user.gameid;
        QRcodeText.text = ZH2_GVars.user.gameid.ToCoinString();
        QRString = dictionary["shareAddress"].ToString();
        ShowQRCode(QRString, m_QRCode);
        ShowQRCode(QRString, QRcodeImage);
        MyShareText[16].text = QRString;
    }

    private void HandleNetMsg_getIncome(object[] objs)
    {
        UnityEngine.Debug.Log("HandleNetMsg_getIncome");
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        if ((bool)dictionary["success"])
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(dictionary["msg"].ToString());
            MB_Singleton<NetManager>.GetInstance().Send("shareService/myShare", new object[1]
            {
                ZH2_GVars.user.id
            });
            MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getGameGold", new object[1]
            {
                ZH2_GVars.user.id
            });
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(dictionary["msg"].ToString());
        }
    }

    private void HandleNetMsg_incomeDetail(object[] objs)
    {
        UnityEngine.Debug.Log("HandleNetMsg_incomeDetail");
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        GetInfoText[0].text = dictionary["todayDirectlyUnderUserWater"].ToString();
        GetInfoText[1].text = dictionary["todayDirectlyUnderUserTax"].ToString();
        GetInfoText[2].text = dictionary["todayTeamTax"].ToString();
        GetInfoText[3].text = ((double)dictionary["taxRatio"] * 100.0).ToString("f2") + "%";
        GetInfoText[4].text = dictionary["todayIncome"].ToString();
        object[] array = dictionary["IncomeDetailLog"] as object[];
        if (array.Length > 0)
        {
            GetInfoNoText.SetActive(value: false);
            GetInfoContent.SetActive(value: true);
            if (GetInfoContent.transform.childCount >= array.Length)
            {
                for (int i = 0; i < GetInfoContent.transform.childCount; i++)
                {
                    if (i < array.Length)
                    {
                        Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
                        GetInfoContent.transform.GetChild(i).gameObject.SetActive(value: true);
                        GetInfoContent.transform.GetChild(i).GetChild(0).GetComponent<Text>()
                            .text = dictionary2["dateTime"].ToString();
                        GetInfoContent.transform.GetChild(i).GetChild(1).GetComponent<Text>()
                            .text = dictionary2["directlyUnderWater"].ToString();
                        GetInfoContent.transform.GetChild(i).GetChild(2).GetComponent<Text>()
                            .text = dictionary2["directlyUnderTax"].ToString();
                        GetInfoContent.transform.GetChild(i).GetChild(3).GetComponent<Text>()
                            .text = dictionary2["teamTax"].ToString();
                        GetInfoContent.transform.GetChild(i).GetChild(4).GetComponent<Text>()
                            .text = dictionary2["income"].ToString();
                    }
                    else
                    {
                        GetInfoContent.transform.GetChild(i).gameObject.SetActive(value: false);
                    }
                }
                return;
            }
            for (int j = 0; j < array.Length; j++)
            {
                Dictionary<string, object> dictionary3 = array[j] as Dictionary<string, object>;
                if (j < GetInfoContent.transform.childCount)
                {
                    GetInfoContent.transform.GetChild(j).gameObject.SetActive(value: true);
                    GetInfoContent.transform.GetChild(j).GetChild(0).GetComponent<Text>()
                        .text = dictionary3["dateTime"].ToString();
                    GetInfoContent.transform.GetChild(j).GetChild(1).GetComponent<Text>()
                        .text = dictionary3["directlyUnderWater"].ToString();
                    GetInfoContent.transform.GetChild(j).GetChild(2).GetComponent<Text>()
                        .text = dictionary3["directlyUnderTax"].ToString();
                    GetInfoContent.transform.GetChild(j).GetChild(3).GetComponent<Text>()
                        .text = dictionary3["teamTax"].ToString();
                    GetInfoContent.transform.GetChild(j).GetChild(4).GetComponent<Text>()
                        .text = dictionary3["income"].ToString();
                }
                else
                {
                    GameObject gameObject = Object.Instantiate(GetInfoContent.transform.GetChild(0).gameObject, GetInfoContent.transform);
                    gameObject.SetActive(value: true);
                    gameObject.transform.GetChild(0).GetComponent<Text>().text = dictionary3["dateTime"].ToString();
                    gameObject.transform.GetChild(1).GetComponent<Text>().text = dictionary3["directlyUnderWater"].ToString();
                    gameObject.transform.GetChild(2).GetComponent<Text>().text = dictionary3["directlyUnderTax"].ToString();
                    gameObject.transform.GetChild(3).GetComponent<Text>().text = dictionary3["teamTax"].ToString();
                    gameObject.transform.GetChild(4).GetComponent<Text>().text = dictionary3["income"].ToString();
                }
            }
        }
        else
        {
            GetInfoNoText.SetActive(value: true);
            GetInfoContent.SetActive(value: false);
        }
    }

    private void HandleNetMsg_promotionTutorial(object[] objs)
    {
        JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
        UnityEngine.Debug.Log(jsonData.ToJson().ToString());
        UnityEngine.Debug.Log("HandleNetMsg_promotionTutorial");
        for (int i = 0; i < ExtensionContent.transform.childCount; i++)
        {
            ExtensionContent.transform.GetChild(i).GetChild(0).GetComponent<Text>()
                .text = jsonData[0][i].ToString();
            ExtensionContent.transform.GetChild(i).GetChild(1).GetComponent<Text>()
                .text = jsonData[1][i].ToString();
            ExtensionContent.transform.GetChild(i).GetChild(2).GetComponent<Text>()
                .text = "(" + jsonData[1][i] + ")²";
            ExtensionContent.transform.GetChild(i).GetChild(3).GetComponent<Text>()
                .text = "(" + jsonData[1][i] + ")³";
            ExtensionContent.transform.GetChild(i).GetChild(4).GetComponent<Text>()
                .text = "...";
            ExtensionContent.transform.GetChild(i).GetChild(5).GetComponent<Text>()
                .text = "(" + jsonData[1][i] + ")\u207f";
        }
    }

    private void HandleNetMsg_queryDirectlyUnderUser(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        UnityEngine.Debug.Log("HandleNetMsg_queryDirectlyUnderUser");
        ZhiShuChaText[0].text = ZH2_GVars.ShowTip( "直属玩家：", "Direct player：", "ผู้เล่นโดยตรง：", "Người chơi:") + dictionary["directlyUnderUser"].ToString();
        ZhiShuChaText[1].text = ZH2_GVars.ShowTip( "直属代理：", "Agent：", "ตัวแทนโดยตรง:", "Đại lý：") + dictionary["directlyUnderAgent"].ToString();
        ZhiShuChaText[2].text = ZH2_GVars.ShowTip("直属税收：", "Direct Taxation：", "ภาษีโดยตรง：", "Thuế trực tiếp：") + dictionary["directlyUnderUserTax"].ToString();
        for (int i = 0; i < ZhiShuChaContent.transform.childCount; i++)
        {
            ZhiShuChaContent.transform.GetChild(i).gameObject.SetActive(value: false);
        }
        ZhiShuChaInput.text = string.Empty;
        ZhiShuChaText[3].text = ZH2_GVars.ShowTip("请选择时间", "Please select a time", "กรุณาเลือกเวลา", "Vui lòng chọn thời gian");
        ZhiShuChaDataPanel.SetActive(value: false);
        DataType = -1;
    }

    private void HandleNetMsg_searchSubordinate(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        UnityEngine.Debug.Log("HandleNetMsg_searchSubordinate");
        if (dictionary.ContainsKey("success"))
        {
            if (!(bool)dictionary["success"])
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(dictionary["msg"].ToString());
            }
            return;
        }
        for (int i = 0; i < ZhiShuChaContent.transform.childCount; i++)
        {
            ZhiShuChaContent.transform.GetChild(i).gameObject.SetActive(value: false);
        }
        ZhiShuChaContent.transform.GetChild(0).gameObject.SetActive(value: true);
        ZhiShuChaContent.transform.GetChild(0).GetChild(0).GetComponent<Text>()
            .text = dictionary["subordinateId"].ToString();
        ZhiShuChaContent.transform.GetChild(0).GetChild(1).GetComponent<Text>()
            .text = dictionary["subordinateName"].ToString();
        ZhiShuChaContent.transform.GetChild(0).GetChild(2).GetComponent<Text>()
            .text = dictionary["subordinateTodayTax"].ToString();
        ZhiShuChaContent.transform.GetChild(0).GetChild(3).GetComponent<Text>()
            .text = dictionary["subordinateTotalTax"].ToString();
        ZhiShuChaContent.transform.GetChild(0).GetChild(4).GetComponent<Text>()
            .text = dictionary["subordinateTeamPeopleNumber"].ToString();
        ZhiShuChaContent.transform.GetChild(0).GetChild(5).GetComponent<Text>()
            .text = dictionary["subordinateDirectlyUnderPeopleNumber"].ToString();
    }

    private void HandleNetMsg_searchSubordinateForTime(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        UnityEngine.Debug.Log("HandleNetMsg_searchSubordinateForTime");
        for (int i = 0; i < ZhiShuChaContent.transform.childCount; i++)
        {
            ZhiShuChaContent.transform.GetChild(i).gameObject.SetActive(value: false);
        }
        int count = dictionary.Keys.Count;
        int num = 0;
        foreach (string key in dictionary.Keys)
        {
            Dictionary<string, object> dictionary2 = dictionary[key] as Dictionary<string, object>;
            if (num < dictionary.Keys.Count)
            {
                ZhiShuChaContent.transform.GetChild(num).gameObject.SetActive(value: true);
                ZhiShuChaContent.transform.GetChild(num).GetChild(0).GetComponent<Text>()
                    .text = dictionary2["userId"].ToString();
                ZhiShuChaContent.transform.GetChild(num).GetChild(1).GetComponent<Text>()
                    .text = dictionary2["username"].ToString();
                ZhiShuChaContent.transform.GetChild(num).GetChild(2).GetComponent<Text>()
                    .text = dictionary2["todayTax"].ToString();
                ZhiShuChaContent.transform.GetChild(num).GetChild(3).GetComponent<Text>()
                    .text = dictionary2["totalTax"].ToString();
                ZhiShuChaContent.transform.GetChild(num).GetChild(4).GetComponent<Text>()
                    .text = dictionary2["teamPeopleNumber"].ToString();
                ZhiShuChaContent.transform.GetChild(num).GetChild(5).GetComponent<Text>()
                    .text = dictionary2["directlyUnderUserPeopleNumber"].ToString();
            }
            else
            {
                GameObject gameObject = Object.Instantiate(ZhiShuChaContent.transform.GetChild(num).gameObject, ZhiShuChaContent.transform);
                gameObject.SetActive(value: true);
                gameObject.transform.GetChild(0).GetComponent<Text>().text = dictionary2["userId"].ToString();
                gameObject.transform.GetChild(1).GetComponent<Text>().text = dictionary2["username"].ToString();
                gameObject.transform.GetChild(2).GetComponent<Text>().text = dictionary2["todayTax"].ToString();
                gameObject.transform.GetChild(3).GetComponent<Text>().text = dictionary2["totalTax"].ToString();
                gameObject.transform.GetChild(4).GetComponent<Text>().text = dictionary2["teamPeopleNumber"].ToString();
                gameObject.transform.GetChild(5).GetComponent<Text>().text = dictionary2["directlyUnderUserPeopleNumber"].ToString();
            }
            num++;
        }
    }

    private void HandleNetMsg_getIncomeRank(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        UnityEngine.Debug.Log("HandleNetMsg_getIncomeRank");
        for (int i = 0; i < ShouYiContent.transform.childCount; i++)
        {
            if (dictionary.ContainsKey(i.ToString()))
            {
                Dictionary<string, object> dictionary2 = dictionary[i.ToString()] as Dictionary<string, object>;
                ShouYiContent.transform.GetChild(i).gameObject.SetActive(value: true);
                ShouYiContent.transform.GetChild(i).Find("Badge").GetComponent<Text>()
                    .text = (i + 1).ToString();
                ShouYiContent.transform.GetChild(i).Find("Nickname").GetComponent<Text>()
                    .text = dictionary2["username"].ToString();
                ShouYiContent.transform.GetChild(i).Find("Value").GetComponent<Text>()
                    .text = (dictionary2["income"]).ToString();
            }
            else
            {
                ShouYiContent.transform.GetChild(i).gameObject.SetActive(value: false);
            }
        }
    }
}
