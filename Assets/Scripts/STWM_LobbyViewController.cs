using DG.Tweening;

using LitJson;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class STWM_LobbyViewController : STWM_MB_Singleton<STWM_LobbyViewController>
{
    [SerializeField]
    private GameObject _goContainer;

    [SerializeField]
    private STWM_HeadViewController _headViewController;

    private List<STWM_Desk> _deskList = new List<STWM_Desk>();

    private int _curHallType;

    private int _curRoomType;

    private bool _bCanClick;

    public int curDeskIndex;

    public bool _IsAning;

    private Vector3 _posRoomLeft;

    private Vector3 _posRoomRight;

    private string _curState;

    private bool _inputForbidden;

    private Vector3 _deskOffset = Vector3.right * 12.8f;

    private Transform Content;

    private Transform ContentOldPos;

    private Transform ContentTagPos;

    private Transform scrollView;

    private Button btnLeft;

    private Button btnRight;

    public GameObject tablePre;

    [HideInInspector]
    public List<GameObject> tableList = new List<GameObject>();

    public CircleScrollRect circleScrollRect;

    public List<RotateBtnInfo> selectBtnList = new List<RotateBtnInfo>();

    public GameObject RotateButton_new;

    public List<Sprite> icoSprite = new List<Sprite>();

    public Dictionary<string, object> hallInfo = new Dictionary<string, object>();

    private bool isOnEnter = true;

    private int tempSelectId = -1;

    private float contentanchoredPositionX;

    private RectTransform contentRectTransform;

    private ContentSizeCtrl_WaterMargin bYSD_TwoContentSize;

    private List<STWM_Desk> allTableArr;

    public bool HasDesk => _deskList.Count > 0;

    private void Awake()
    {
        scrollView = base.transform.Find("STWM_Tables/Scroll View");
        Content = scrollView.Find("Viewport/Content");
        ContentOldPos = scrollView.Find("Viewport/ContentOldPos");
        ContentTagPos = scrollView.Find("Viewport/ContentTagPos");
        btnLeft = scrollView.Find("Viewport/Buttons/LeftBtn").GetComponent<Button>();
        btnRight = scrollView.Find("Viewport/Buttons/RightBtn").GetComponent<Button>();
        bYSD_TwoContentSize = Content.GetComponent<ContentSizeCtrl_WaterMargin>();
        contentRectTransform = Content.GetComponent<RectTransform>();
        btnLeft.onClick.AddListener(delegate
        {
            LeftAndRightBtnClick(isLeft: true);
        });
        btnRight.onClick.AddListener(delegate
        {
            LeftAndRightBtnClick(isLeft: false);
        });
        if (STWM_MB_Singleton<STWM_LobbyViewController>._instance == null)
        {
            STWM_MB_Singleton<STWM_LobbyViewController>.SetInstance(this);
            PreInit();
        }


    }

    private void OnEnable()
    {
        tempSelectId = -1;
        isOnEnter = true;
        Transform transform = base.transform.Find("RotateButton_new");
        if (transform != null)
        {
            UnityEngine.Object.Destroy(transform.gameObject);
        }
        GameObject gameObject = UnityEngine.Object.Instantiate(RotateButton_new, base.transform);
        gameObject.name = "RotateButton_new";
        circleScrollRect = gameObject.transform.Find("Button").GetComponent<CircleScrollRect>();
        tempSelectId = -1;
        hallInfo = new Dictionary<string, object>();
        selectBtnList = new List<RotateBtnInfo>();
        for (int i = 0; i < circleScrollRect.listItems.Length; i++)
        {
            selectBtnList.Add(circleScrollRect.listItems[i].GetComponent<RotateBtnInfo>());
        }
        if (ZH2_GVars.hallInfo != null && hallInfo != ZH2_GVars.hallInfo)
        {
            ShowHall();
            hallInfo = ZH2_GVars.hallInfo;
        }
    }

    private void Start()
    {
        _init();
    }

    private void LeftAndRightBtnClick(bool isLeft)
    {
        if (isLeft)
        {
            RectTransform target = contentRectTransform;
            Vector2 anchoredPosition = contentRectTransform.anchoredPosition;
            target.DOLocalMoveX(anchoredPosition.x + 994f, 0.35f);
        }
        else
        {
            RectTransform target2 = contentRectTransform;
            Vector2 anchoredPosition2 = contentRectTransform.anchoredPosition;
            target2.DOLocalMoveX(anchoredPosition2.x - 994f, 0.35f);
        }
    }

    public void SetBtnLeft(bool isInteractable)
    {
        btnLeft.interactable = isInteractable;
    }

    public void SetBtnRight(bool isInteractable)
    {
        btnRight.interactable = isInteractable;
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance()._IsAning && !STWM_LockManager.IsLocked("Esc"))
        {
            UnityEngine.Debug.Log("ESC Btn Down In Lobby View");
            STWM_MB_Singleton<STWM_GameManager>.GetInstance().Handle_BtnReturn();
        }
        if (tempSelectId != ZH2_GVars.selectRoomId)
        {
            tempSelectId = ZH2_GVars.selectRoomId;
            StartCoroutine(ClickBtnRoom(tempSelectId));
        }
        if (Content != null && contentRectTransform != null)
        {
            Vector2 anchoredPosition = contentRectTransform.anchoredPosition;
            contentanchoredPositionX = anchoredPosition.x;
            if (UnityEngine.Input.GetKeyDown(KeyCode.H))
            {
                UnityEngine.Debug.LogError("content: " + contentanchoredPositionX);
            }
            if (Mathf.Abs(contentanchoredPositionX) <= 1f)
            {
                SetBtnLeft(isInteractable: false);
            }
            else if (allTableArr != null && allTableArr.Count > 4)
            {
                SetBtnLeft(isInteractable: true);
            }
            float num = Mathf.Abs(contentanchoredPositionX);
            Vector2 sizeDelta = bYSD_TwoContentSize.content.sizeDelta;
            if (num >= Mathf.Abs(sizeDelta.x) - 1f)
            {
                SetBtnRight(isInteractable: false);
            }
            else if (allTableArr != null && allTableArr.Count > 4)
            {
                SetBtnRight(isInteractable: true);
            }
        }
    }

    public void BtnReturn()
    {
        STWM_MB_Singleton<STWM_GameManager>.GetInstance().Handle_BtnReturn(0);
    }

    private IEnumerator ClickBtnRoom(int index)
    {
        Transform content = Content;
        Vector3 localPosition = ContentTagPos.localPosition;
        content.DOLocalMoveY(localPosition.y, 0.2f);
        Content.GetComponent<DK_DislodgShader>().SetImaAndText(0.2f);
        switch (index)
        {
            case 0:
                index = 1;
                break;
            case 1:
                index = 2;
                break;
            case 2:
                index = 4;
                break;
            case 3:
                index = 0;
                break;
            case 4:
                index = 3;
                break;
            default:
                index = 0;
                break;
        }
        if (_headViewController != null && _headViewController._textHeadInfo != null && ZH2_GVars.isShowTingName)
        {
            _headViewController._textHeadInfo.text = selectBtnList[index].name;
        }
        float Time = (!isOnEnter) ? 0.25f : 0.05f;
        isOnEnter = false;
        yield return new WaitForSeconds(Time);
        int id = selectBtnList[index].hallId;
        int roomId = selectBtnList[index].hallType;
        if (roomId <= 0)
        {
            _headViewController._textGoldCoin.gameObject.SetActive(value: false);
            _headViewController._textTestGoldCoin.gameObject.SetActive(value: true);
        }
        else
        {
            _headViewController._textGoldCoin.gameObject.SetActive(value: true);
            _headViewController._textTestGoldCoin.gameObject.SetActive(value: false);
        }
        _curHallType = id;
        _curRoomType = roomId;
        OnBtnHall_Click(_curHallType);
    }

    public void ShowHall()
    {
        JsonData jsonData = new JsonData();
        jsonData = JsonMapper.ToObject(JsonMapper.ToJson(ZH2_GVars.hallInfo));
        if (selectBtnList.Count <= 0)
        {
            UnityEngine.Debug.LogError("未获取完毕");
            return;
        }
        for (int i = 0; i < jsonData.Count; i++)
        {
            selectBtnList[i].hallId = (int)jsonData[i.ToString()]["hallId"];
            selectBtnList[i].hallType = (int)jsonData[i.ToString()]["roomId"];
            selectBtnList[i].name = jsonData[i.ToString()]["hallName"].ToString();
            selectBtnList[i].minGlod = jsonData[i.ToString()]["minGold"].ToString();
            selectBtnList[i].onlinePeople = "0";
            selectBtnList[i].UpdateText();
        }
    }

    public void InItTable(List<STWM_Desk> tableArr)
    {
        for (int i = 0; i < tableList.Count; i++)
        {
            UnityEngine.Object.Destroy(tableList[i].gameObject);
        }
        Transform content = Content;
        Vector3 localPosition = ContentOldPos.localPosition;
        content.DOLocalMoveY(localPosition.y, 0.2f);
        Content.GetComponent<DK_DislodgShader>().SetOver();
        tableList = new List<GameObject>();
        allTableArr = new List<STWM_Desk>();
        allTableArr = tableArr;
        for (int j = 0; j < allTableArr.Count; j++)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(tablePre, Content);
            LongPressBtn3 component = gameObject.GetComponent<LongPressBtn3>();
            if (component == null)
            {
                component = gameObject.AddComponent<LongPressBtn3>();
            }
            tableList.Add(gameObject);
        }
        for (int k = 0; k < tableList.Count; k++)
        {
            int num = k;
            if (allTableArr[k].userPhotoId != -1)
            {
                int num2 = allTableArr[k].userPhotoId;
                if (num2 != 0 && num2 >= icoSprite.Count)
                {
                    num2 %= icoSprite.Count;
                    if (num2 >= icoSprite.Count)
                    {
                        num2 = icoSprite.Count - 1;
                    }
                }
                tableList[k].transform.Find("Ico").gameObject.SetActive(value: true);
                tableList[k].transform.Find("Arrows").gameObject.SetActive(value: false);
                tableList[k].transform.Find("Ico/Image").GetComponent<Image>().sprite = icoSprite[num2];
            }
            else
            {
                tableList[k].transform.Find("Ico").gameObject.SetActive(value: false);
                tableList[k].transform.Find("Arrows").gameObject.SetActive(value: true);
            }
            tableList[k].transform.Find("Inifo/Name").GetComponent<Text>().text = allTableArr[k].name;
            tableList[k].transform.Find("Inifo/min/Text").GetComponent<Text>().text = allTableArr[k].minSinglelineBet.ToString();
            tableList[k].transform.Find("Inifo/Max/Text").GetComponent<Text>().text = allTableArr[k].maxSinglelineBet.ToString();

            Destroy(tableList[k].transform.Find("Inifo/min").GetComponent<Translation_Game>());
            Destroy(tableList[k].transform.Find("Inifo/Max").GetComponent<Translation_Game>());
            Destroy(tableList[k].transform.Find("Arrows/Text").GetComponent<Translation_Game>());
            tableList[k].transform.Find("Inifo/min").GetComponent<Text>().text = ZH2_GVars.ShowTip("最小押注:", "MiniBet:", "MiniBet:", "Tối thiểu:");
            tableList[k].transform.Find("Inifo/Max").GetComponent<Text>().text = ZH2_GVars.ShowTip("最大押注:", "MaxBet:", "MaxBet:", "Tối đa:");
            tableList[k].transform.Find("Arrows/Text").GetComponent<Text>().text = ZH2_GVars.ShowTip("点击加入座位", "ClickJoinSeat", "ClickJoinSeat", "Click vào join seat");

        }
        for (int l = 0; l < tableList.Count; l++)
        {
            int index = l;
            tableList[l].gameObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                _onBtnDesk_Click(index);
            });
        }
    }

    public void PreInit()
    {
        if (_goContainer == null)
        {
            _goContainer = base.gameObject;
        }
    }

    private void _init()
    {
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("roomInfo", HandleNetMsg_RoomInfo);
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("selectHall", HandleNetMsg_HallInfo);
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("enterDesk", HandleNetMsg_EnterDesk);
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("updateRoomInfo", HandleNetMsg_UpdateRoomInfo);
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("updateHallInfo", HandleNetMsg_UpdateHallInfo);
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("addExpeGoldAuto", HandleNetMsg_AddExpeGoldAuto);
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("expeGold", HandleNetMsg_ExpeGold);
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("playerInfo", HandleNetMsg_PlayerInfo);
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("updateDeskInfo", HandleNetMsg_UpdateDeskInfo);
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("updateGoldAndScore", UpdateGoldAndScore);
        int num = (STWM_GVars.language == "en") ? 1 : 0;
        _headViewController.Show();
        STWM_Drag_DeskController._instance.left = OnBtnLeftDesk_Click;
        STWM_Drag_DeskController._instance.right = OnBtnRightDesk_Click;
        _curState = "RoomView";
    }

    public void UpdateGoldAndScore(object[] args)
    {
        UnityEngine.Debug.Log(STWM_LogHelper.NetHandle("更新金币和分数"));
        STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().HandleNetMsg_UpdateGoldAndScore(args);
    }

    public void Show()
    {
        _goContainer.SetActive(value: true);
        string empty = string.Empty;
        STWM_MB_Singleton<STWM_GameManager>.GetInstance().ChangeView(empty);
        _headViewController.UpdateView();
    }

    public void Hide()
    {
        _goContainer.SetActive(value: false);
    }

    public void OnBtnHall_Click(int hallType)
    {
        _curHallType = hallType;
        Send_EnterHall(hallType);
    }

    public void OnBtnLeftDesk_Click()
    {
        if (!STWM_GVars.tryLockOnePoint && STWM_DeskPullDownListController.Instance.bMoveFinished)
        {
            _desksMoveToLeft();
        }
    }

    public void OnBtnRightDesk_Click()
    {
        if (!STWM_GVars.tryLockOnePoint && STWM_DeskPullDownListController.Instance.bMoveFinished)
        {
            _desksMoveToRight();
        }
    }

    public void OnBtnReturn_Click()
    {
        if (!STWM_GVars.tryLockOnePoint && _bCanClick)
        {
            _bCanClick = false;
            STWM_SoundManager.Instance.PlayClickAudio();
            STWM_MB_Singleton<STWM_RoomSelectionViewController>.GetInstance().Show();
            ResetRoomView();
        }
    }

    public void Send_EnterHall(int hallType)
    {
        object[] args = new object[1]
        {
            hallType
        };
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/selectHall", args);
        STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "Send_SelectHall");
    }

    public void Send_EnterRoom(int roomType)
    {
        object[] args = new object[1]
        {
            roomType
        };
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/enterRoom", args);
        STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "Send_EnterRoom");
    }

    public void Send_EnterDesk(int deskId)
    {
        Send_EnterRoom(_curRoomType);
        object[] args = new object[1]
        {
            deskId
        };
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/enterDesk", args);
        STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "Send_EnterDesk");
    }

    public void Send_LeaveRoom()
    {
        object[] args = new object[1]
        {
            _curRoomType
        };
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/leaveRoom", args);
    }

    public void Send_GetUserAward()
    {
        object[] args = new object[0];
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/getUserAward", args);
    }

    public void Send_AddExpeGoldAuto()
    {
        object[] args = new object[0];
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/addExpeGoldAuto", args);
    }

    public void Send_PlayerInfo(int userId)
    {
        object[] args = new object[1]
        {
            userId
        };
        STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/playerInfo", args);
    }

    public void HandleNetMsg_HallInfo(object[] args)
    {
        UnityEngine.Debug.LogError("收到HallInfo: " + JsonMapper.ToJson(args));
        if (STWM_GVars.curView == "MajorGame" || STWM_GVars.curView == "DiceGame")
        {
            UnityEngine.Debug.Log(STWM_LogHelper.Orange("游戏中，不处理"));
            return;
        }
        _curState = "DeskView";
        Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
        UpdateOnline(dictionary);
        STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "Send_EnterRoom done");
        List<STWM_Desk> list = new List<STWM_Desk>();
        int length = (dictionary["deskInfo"] as Array).Length;
        for (int i = 0; i < length; i++)
        {
            Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
            dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
            list.Add(STWM_Desk.CreateWithDic(dictionary2));
        }
        _deskList = list;
        InItTable(_deskList);
        STWM_DeskPullDownListController.Instance.ChangeListHeightByDeskNum(_deskList.Count, _deskList);
        _hideRoomViewAni();
        StartCoroutine(_showDeskViewAni());
    }

    private void UpdateOnline(Dictionary<string, object> dictionary)
    {
        if (dictionary.ContainsKey("onlineNumber"))
        {
            Dictionary<string, object> dictionary2 = dictionary["onlineNumber"] as Dictionary<string, object>;
            for (int i = 0; i < dictionary2.Count; i++)
            {
                for (int j = 0; j < selectBtnList.Count; j++)
                {
                    if (i == selectBtnList[j].hallId)
                    {
                        selectBtnList[j].onlinePeople = dictionary2[i.ToString()].ToString();
                        selectBtnList[j].UpdateText();
                        break;
                    }
                }
            }
        }
        else
        {
            UnityEngine.Debug.Log("更新在线人数失败 不存在减值");
        }
    }

    public void HandleNetMsg_RoomInfo(object[] args)
    {
    }

    public void HandleNetMsg_EnterDesk(object[] args)
    {
        UnityEngine.Debug.LogError("收到EnterDesk: " + JsonMapper.ToJson(args));
        STWM_LockManager.UnLock("EnterDesk");
        Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
        bool flag = (bool)dictionary["success"];
        STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "Send_EnterDesk done");
        if (flag)
        {
            Hide();
            STWM_GVars.desk = _deskList[curDeskIndex];
            STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().Show();
            STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().InitGame();
            STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().SetKeepScore(0);
            STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().SetRate(STWM_GVars.desk.exchange);
            return;
        }
        switch ((int)dictionary["messageStatus"])
        {
            case 0:
                STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog((STWM_GVars.language == "zh") ? "桌子不存在" : "Game table not exist");
                break;
            case 1:
                STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog((STWM_GVars.language == "zh") ? "桌已满" : "Don't Have Free Table");
                break;
            case 2:
                STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog((STWM_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge");
                break;
            case 3:
                STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog((STWM_GVars.language == "zh") ? "体验币不足" : "Experience Coins insufficient");
                break;
            case 4:
                STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog((STWM_GVars.language == "zh") ? "游戏币不足" : "Coins insufficient");
                break;
            default:
                UnityEngine.Debug.Log(string.Empty);
                break;
        }
    }

    public void HandleNetMsg_UpdateRoomInfo(object[] args)
    {
    }

    public void HandleNetMsg_UpdateHallInfo(object[] args)
    {
        UnityEngine.Debug.LogError("收到UpdateHallInfo: " + JsonMapper.ToJson(args));
        Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
        UpdateOnline(dictionary);
        List<STWM_Desk> list = new List<STWM_Desk>();
        int length = (dictionary["deskInfo"] as Array).Length;
        for (int i = 0; i < length; i++)
        {
            Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
            dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
            list.Add(STWM_Desk.CreateWithDic(dictionary2));
        }
        if (length == 0)
        {
            curDeskIndex = 0;
            _deskList.Clear();
            return;
        }
        if (_deskList.Count == 0)
        {
            curDeskIndex = 0;
        }
        int count = _deskList.Count;
        _deskList = list;
        InItTable(_deskList);
        if (count != list.Count)
        {
            UpdateAllDeskWidgets();
            STWM_DeskPullDownListController.Instance.UpdateDeskInList(_deskList);
        }
        else
        {
            UpdateAllDeskWidgets();
            STWM_DeskPullDownListController.Instance.UpdateDeskInList(_deskList);
        }
    }

    public void HandleNetMsg_AddExpeGoldAuto(object[] args)
    {
    }

    public void HandleNetMsg_ExpeGold(object[] args)
    {
        UnityEngine.Debug.Log(STWM_LogHelper.NetHandle("HandleNetMsg_ExpeGold"));
        if ((bool)args[0])
        {
            STWM_GVars.user.expeGold = 10000;
            _headViewController.UpdateExpeAndGold();
        }
        else
        {
            UnityEngine.Debug.LogError("ExpeGold> should not be false");
        }
    }

    public void HandleNetMsg_PlayerInfo(object[] args)
    {
        UnityEngine.Debug.LogError("收到PlayerInfo: " + JsonMapper.ToJson(args));
    }

    public void HandleNetMsg_UpdateDeskInfo(object[] args)
    {
        UnityEngine.Debug.LogError("收到UpdateDeskInfo: " + JsonMapper.ToJson(args));
    }

    private IEnumerator _showDeskViewAni()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private STWM_DeskWidget _createDeskWidget()
    {
        UnityEngine.Debug.Log("_createDeskWidget");
        return null;
    }

    private void _desksMoveToLeft()
    {
        STWM_DeskPullDownListController.Instance.bMoveFinished = false;
        STWM_SoundManager.Instance.PlayClickAudio();
        if (_deskList.Count != 0 && !_inputForbidden)
        {
            STWM_DeskPullDownListController.Instance.ShowListButtonHighBG(_bIsLeft: true, curDeskIndex);
            Sequence sequence = DOTween.Sequence();
            _inputForbidden = true;
            sequence.onComplete = delegate
            {
                curDeskIndex = (curDeskIndex + _deskList.Count - 1) % _deskList.Count;
                _inputForbidden = false;
                STWM_DeskPullDownListController.Instance.bMoveFinished = true;
            };
        }
    }

    private void _desksMoveToRight()
    {
        STWM_DeskPullDownListController.Instance.bMoveFinished = false;
        STWM_SoundManager.Instance.PlayClickAudio();
        if (_deskList.Count != 0 && !_inputForbidden)
        {
            STWM_DeskPullDownListController.Instance.ShowListButtonHighBG(_bIsLeft: false, curDeskIndex);
            Sequence sequence = DOTween.Sequence();
            _inputForbidden = true;
            sequence.onComplete = delegate
            {
                curDeskIndex = (curDeskIndex + 1) % _deskList.Count;
                _inputForbidden = false;
                STWM_DeskPullDownListController.Instance.bMoveFinished = true;
            };
        }
    }

    public void ForceMoveDesksToLeft(int newIndex)
    {
        STWM_DeskPullDownListController.Instance.bMoveFinished = false;
        STWM_SoundManager.Instance.PlayClickAudio();
        curDeskIndex = newIndex;
        if (!_inputForbidden)
        {
            Sequence sequence = DOTween.Sequence();
            _inputForbidden = true;
            sequence.onComplete = delegate
            {
                _inputForbidden = false;
                STWM_DeskPullDownListController.Instance.bMoveFinished = true;
            };
            StartCoroutine(STWM_Utils.DelayCall(0.5f, delegate
            {
            }));
        }
    }

    public void ForceMoveDesksToRight(int newIndex)
    {
        STWM_DeskPullDownListController.Instance.bMoveFinished = false;
        STWM_SoundManager.Instance.PlayClickAudio();
        curDeskIndex = newIndex;
        if (!_inputForbidden)
        {
            Sequence sequence = DOTween.Sequence();
            _inputForbidden = true;
            sequence.onComplete = delegate
            {
                _inputForbidden = false;
                STWM_DeskPullDownListController.Instance.bMoveFinished = true;
            };
        }
    }

    public void UpdateAllDeskWidgets()
    {
        int count = _deskList.Count;
        if (count != 0 && count > 0 && curDeskIndex >= count)
        {
            UnityEngine.Debug.Log($"_curDeskIndex: {curDeskIndex}, deskCount: {count}");
            curDeskIndex = 0;
        }
        STWM_DeskPullDownListController.Instance.ChangeListHeightByDeskNum(_deskList.Count, _deskList);
    }

    private void _onBtnDesk_Click(int curIndex)
    {
        curDeskIndex = curIndex;
        STWM_SoundManager.Instance.PlayClickAudio();
        if (_deskList[curDeskIndex].userId > 0)
        {
            return;
        }
        if (_curRoomType == 1)
        {
            if (STWM_GVars.user.expeGold == 0 || STWM_GVars.user.expeGold < _deskList[curDeskIndex].minGold)
            {
                UnityEngine.Debug.Log($"expeGold: {STWM_GVars.user.expeGold}, _curDeskIndex: {curDeskIndex}, minGold: {_deskList[curDeskIndex].minGold}");
                STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("体验币不足，是否领取体验币？", "Not experiential.Want to receive experiential?", string.Empty), showOkCancel: true, delegate
                {
                    Send_AddExpeGoldAuto();
                });
            }
            else
            {
                STWM_LockManager.Lock("EnterDesk");
                Send_EnterDesk(_deskList[curDeskIndex].id);
                STWM_SoundManager.Instance.StopLobbyBGM();
            }
        }
        else if (STWM_GVars.user.gameGold == 0 || STWM_GVars.user.gameGold < _deskList[curDeskIndex].minGold)
        {
            UnityEngine.Debug.Log($"gameGold: {STWM_GVars.user.gameGold}, _curDeskIndex: {curDeskIndex}, minGold: {_deskList[curDeskIndex].minGold}");
            STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游戏币不足！", "Lack of game currency!", string.Empty), showOkCancel: true, delegate
            {
                if (ZH2_GVars.OpenPlyBoxPanel != null)
                {
                    ZH2_GVars.OpenPlyBoxPanel(ZH2_GVars.GameType_DJ.water_desk);
                }
            });
        }
        else
        {
            STWM_LockManager.Lock("EnterDesk");
            Send_EnterDesk(_deskList[curDeskIndex].id);
            STWM_SoundManager.Instance.StopLobbyBGM();
        }
    }

    private void _hideRoomViewAni()
    {
        _IsAning = true;
        STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "_hideRoomViewAni");
        Sequence sequence = DOTween.Sequence();
        Color white = Color.white;
        white.a = 0f;
        sequence.onComplete = delegate
        {
            STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "_hideRoomViewAni done");
            _IsAning = false;
            _bCanClick = true;
        };
    }

    public void ResetRoomView()
    {
        UnityEngine.Debug.Log("ResetRoomView");
        STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "ResetRoomView");
        Sequence sequence = DOTween.Sequence();
        sequence.onComplete = delegate
        {
            _curState = "RoomView";
            _bCanClick = false;
            STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "ResetRoomView_Over");
        };
    }

    public void HideDeskViewAni()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.onComplete = delegate
        {
        };
    }
}
