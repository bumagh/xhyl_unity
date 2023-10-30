using DG.Tweening;

using LitJson;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class New_AllPayPanel : MonoBehaviour
{
    private class PayLog
    {
        public int ID;

        public int Amount;

        public int Type;

        public int Status;

        public string Order;

        public string Data;
    }

    [SerializeField]
    private GameObject Nonentity;

    [SerializeField]
    private GameObject Content;

    [SerializeField]
    private GameObject Content_Pre;

    private Queue<PayLog> PayLogQueue = new Queue<PayLog>();

    public List<GameObject> mPanels;

    private Color pitchOnColor = new Color(1f, 0.97f, 0.83f, 1f);

    private Color unselectedColor = new Color(0.25f, 0.13f, 0f, 1f);

    public Sprite pitchOnSpr;

    public Sprite unselectedSpr;

    public Transform Btns;

    public List<Button> btns;

    private List<Text> btnsText;

    private Tween_SlowAction tween_SlowAction;

    private int tempIndex = -1;

    private void Awake()
    {
        btns = new List<Button>();
        btnsText = new List<Text>();
        for (int i = 0; i < Btns.childCount; i++)
        {
            Button component = Btns.GetChild(i).GetComponent<Button>();
            if (component != null)
            {
                btns.Add(component);
            }
        }
        for (int j = 0; j < btns.Count; j++)
        {
            int tempIndex = j;
            btnsText.Add(btns[j].transform.GetChild(0).GetComponent<Text>());
            btns[j].onClick.AddListener(delegate
            {
                ShowSelectionNum(tempIndex);
            });
        }
        tween_SlowAction = GetComponent<Tween_SlowAction>();
    }

    private void ShowSelectionNum(int index)
    {
        All_TipCanvas.GetInstance().SourcePlayClip();
        if (index != tempIndex)
        {
            tempIndex = index;
            if (index == 1)
            {
                GetTransactionRecord();
            }
            for (int i = 0; i < btnsText.Count; i++)
            {
                btnsText[i].DOColor(unselectedColor, 0.2f);
            }
            for (int j = 0; j < mPanels.Count; j++)
            {
                mPanels[j].SetActive(value: false);
            }
            for (int k = 0; k < btns.Count; k++)
            {
                btns[k].GetComponent<Image>().sprite = unselectedSpr;
            }
            btnsText[index].DOColor(pitchOnColor, 0.2f);
            mPanels[index].SetActive(value: true);
            btns[index].GetComponent<Image>().sprite = pitchOnSpr;
        }
    }

    private void OnEnable()
    {
        if ((object)tween_SlowAction != null)
        {
            tween_SlowAction.Show();
        }
        tempIndex = -1;
        ShowSelectionNum(1);
    }

    private void GetTransactionRecord()
    {
        InitPalyJLu();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("userId", ZH2_GVars.userId);
        hashtable.Add("gameId", ZH2_GVars.allSetType);
        hashtable.Add("password", ZH2_GVars.pwd);
        Hashtable obj = hashtable;
        StartCoroutine(GetGetTransaction(ZH2_GVars.EncodeMessage(obj)));
    }

    private IEnumerator GetGetTransaction(string msg)
    {
        string url2 = ZH2_GVars.shortConnection + "transactionRecord";
        url2 = url2 + "?jsonStr=" + msg;
        UnityWebRequest www = UnityWebRequest.Get(url2);
        www.timeout = 10000;
        yield return www.Send();
        if (www.error == null)
        {
            string text = www.downloadHandler.text;
            if (text != string.Empty)
            {
                text = ZH2_GVars.DecodeMessage(text);
                JsonData dictionary = JsonMapper.ToObject(text);
                HandleNetMsg_getTransactionRecord(dictionary);
            }
            else
            {
                ShowError();
            }
        }
        else
        {
            UnityEngine.Debug.LogError("===访问错误===: " + www.error);
            ShowError();
        }
    }

    private void ShowError()
    {
        string tips = ZH2_GVars.ShowTip("系统错误", "System failure", "ข้อผิดพลาดของระบบ", "Lỗi hệ thống");
        All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
    }

    private void OnDisable()
    {
        InitPalyJLu();
    }

    public void HandleNetMsg_getTransactionRecord(JsonData dictionary)
    {
        if ((bool)dictionary["success"])
        {
            int num = 0;
            JsonData jsonData = dictionary["transactionRecord"];
            for (int i = 0; i < jsonData.Count; i++)
            {
                JsonData jsonData2 = jsonData[i];
                PayLog payLog = new PayLog();
                payLog.ID = num;
                payLog.Amount = int.Parse(jsonData2["amount"].ToString());
                payLog.Type = int.Parse(jsonData2["transactionType"].ToString());
                payLog.Status = int.Parse(jsonData2["status"].ToString());
                payLog.Order = jsonData2["orderNumber"].ToString();
                payLog.Data = jsonData2["transactionDate"].ToString();
                PayLog item = payLog;
                num++;
                PayLogQueue.Enqueue(item);
            }
            if (PayLogQueue.Count <= 0)
            {
                InitPalyJLu();
                return;
            }
            Nonentity.SetActive(value: false);
            for (int j = 0; j < PayLogQueue.Count; j++)
            {
                PayLog payLog2 = PayLogQueue.ToArray()[j];
                Transform transform = null;
                transform = Instantiate(Content_Pre, Content.transform).transform;
                transform.gameObject.SetActive(value: true);
                transform.GetChild(0).GetComponent<Text>().text = payLog2.Data;
                transform.GetChild(1).GetComponent<Text>().text = payLog2.Order;
                string empty = string.Empty;
                switch (payLog2.Type)
                {
                    case 0:
                        empty = ZH2_GVars.ShowTip("充值", "Recharge", "เติมเงิน", "Nạp tiền");
                        break;
                    case 1:
                        empty = ZH2_GVars.ShowTip("兑换", "exchange", "แลก", "Chuyển đổi");
                        break;
                    default:
                        empty = ZH2_GVars.ShowTip("充值", "other", "อื่นๆ", "Khác");
                        break;
                }
                transform.GetChild(2).GetComponent<Text>().text = empty;
                transform.GetChild(3).transform.Find("Text").GetComponent<Text>().text = payLog2.Amount.ToString();
                string empty2 = string.Empty;
                switch (payLog2.Status)
                {
                    case 0:
                        empty2 = ZH2_GVars.ShowTip("等待", "wait", "รอ", "Chờ chút");
                        break;
                    case 1:
                        empty2 = ZH2_GVars.ShowTip("处理", "handle", "การประมวลผล", "Xử lý");
                        break;
                    case 2:
                        empty2 = ZH2_GVars.ShowTip("拒绝", "refuse", "ปฏิเสธ", "Từ chối");
                        break;
                    default:
                        empty2 = ZH2_GVars.ShowTip("异常", "abnormal", "ความผิดปกติ", "Bất thường");
                        break;
                }
                transform.GetChild(4).GetComponent<Text>().text = empty2;
            }
        }
        else
        {
            InitPalyJLu();
        }
    }

    private void InitPalyJLu()
    {
        PayLogQueue = new Queue<PayLog>();
        Nonentity.SetActive(value: true);
        for (int i = 0; i < Content.transform.childCount; i++)
        {
           Destroy(Content.transform.GetChild(i).gameObject);
        }
    }

    public void OnBtnClick_Return()
    {
        All_TipCanvas.GetInstance().SourcePlayClip();
        if ((object)tween_SlowAction != null)
        {
            tween_SlowAction.Hide(base.gameObject);
        }
    }
}
