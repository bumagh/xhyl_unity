using DG.Tweening;
using LitJson;
using STDT_GameConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_ShowUI : MonoBehaviour
{
	public GameObject mTitleObj;

	public GameObject objTableListItem;

	public XLDT_DlgBase sptDlgBase;

	public XLDT_DlgPersonInfo sptDlgPersonInfo;

	private Text mNameLab;

	private Text mCoinLab;

	private Text mTestCoinLab;

	private int mRoomId;

	public bool m_bTableMove;

	public bool bInit;

	private Button btnBack;

	private Transform Content;

	private Transform ContentOldPos;

	private Transform ContentTagPos;

	private Transform scrollView;

	private Button btnLeft;

	private Button btnRight;

	public GameObject tablePre;

	public Image imaIco;

	[HideInInspector]
	public List<GameObject> tableList = new List<GameObject>();

	public CircleScrollRect circleScrollRect;

	public List<RotateBtnInfo> selectBtnList = new List<RotateBtnInfo>();

	public GameObject RotateButton_new;

	public List<Sprite> icoSprite = new List<Sprite>();

	public List<Sprite> icoHeadSprite = new List<Sprite>();

	private int mPlayerSeatId;

	private static XLDT_ShowUI _mShowUI;

	[SerializeField]
	private XLDT_UserPhoto uPhoto;

	private Dictionary<string, object> hallInfo = new Dictionary<string, object>();

	private bool isOnEnter = true;

	private int tempSelectId = -1;

	private float contentanchoredPositionX;

	private RectTransform contentRectTransform;

	private BYSD_TwoContentSizeCtrl bYSD_TwoContentSize;

	private XLDT_CardDesk[] allTableArr;

	private void Awake()
	{
		if (_mShowUI == null)
		{
			_mShowUI = this;
		}
		Init();
		StartCoroutine(InitFinished());
	}

	public static XLDT_ShowUI getInstance()
	{
		return _mShowUI;
	}

	public void Init()
	{
		mNameLab = mTitleObj.transform.Find("TxtName").GetComponent<Text>();
        mNameLab.text = ZH2_GVars.ShowTip("单挑", "OnePoker", "OnePoker", "Đơn đấu");
        mCoinLab = mTitleObj.transform.Find("Coin/TxtCoin").GetComponent<Text>();
		mTestCoinLab = mTitleObj.transform.Find("Coin/TxtTestCoin").GetComponent<Text>();
		XLDT_GameInfo.getInstance().currentState = XLDT_GameState.On_SelectRoom;
		SetTableInfoVisible();
		if (XLDT_GameInfo.getInstance().User.IsOverFlow == 1)
		{
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.CoinOverFlow);
		}
		scrollView = base.transform.Find("Tables/Scroll View");
		Content = scrollView.Find("Viewport/Content");
		ContentOldPos = scrollView.Find("Viewport/ContentOldPos");
		ContentTagPos = scrollView.Find("Viewport/ContentTagPos");
		btnLeft = scrollView.Find("Viewport/Buttons/LeftBtn").GetComponent<Button>();
		btnRight = scrollView.Find("Viewport/Buttons/RightBtn").GetComponent<Button>();
		bYSD_TwoContentSize = Content.GetComponent<BYSD_TwoContentSizeCtrl>();
		contentRectTransform = Content.GetComponent<RectTransform>();
		btnLeft.onClick.AddListener(delegate
		{
			LeftAndRightBtnClick(isLeft: true);
		});
		btnRight.onClick.AddListener(delegate
		{
			LeftAndRightBtnClick(isLeft: false);
		});
		btnBack = mTitleObj.transform.Find("BtnBack").GetComponent<Button>();
		btnBack.onClick.AddListener(ClickBtnBack);
		ShowUserInfo();
		XLDT_SoundManage.getInstance().IsButtonMusic = XLDT_GameInfo.getInstance().Setted.bIsButtonVolum;
		XLDT_SoundManage.getInstance().IsGameMusic = XLDT_GameInfo.getInstance().Setted.bIsGameVolum;
		XLDT_GameInfo.getInstance().CurAward.awardType = XLDT_EAwardType.None;
	}

	public void UnOpen(int roomId)
	{
		mRoomId = roomId;
		m_bTableMove = false;
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendLeaveRoom(XLDT_GameManager.getInstance().mRoomId);
		XLDT_GameInfo.getInstance().currentState = XLDT_GameState.On_SelectRoom;
		XLDT_GameInfo.getInstance().User.RoomId = -1;
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

	public void ShowUserInfo()
	{
		UpdateExpCoin();
		UpdateGameCoin();
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
			else if (allTableArr != null && allTableArr.Length > 4)
			{
				SetBtnLeft(isInteractable: true);
			}
			float num = Mathf.Abs(contentanchoredPositionX);
			Vector2 sizeDelta = bYSD_TwoContentSize.content.sizeDelta;
			if (num >= Mathf.Abs(sizeDelta.x) - 1f)
			{
				SetBtnRight(isInteractable: false);
			}
			else if (allTableArr != null && allTableArr.Length > 4)
			{
				SetBtnRight(isInteractable: true);
			}
		}
	}

	public void SetTableInfoVisible()
	{
		m_bTableMove = false;
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_SelectSeat)
		{
			XLDT_TipManager.getInstance().HideTip();
		}
	}

	public void UpdateExpCoin()
	{
		mTestCoinLab.text = (XLDT_GameInfo.getInstance().IsSpecial ? (XLDT_GameManager.getInstance()._mGameInfo.User.TestCoinCount % 10000).ToString() : XLDT_GameManager.getInstance()._mGameInfo.User.TestCoinCount.ToString());
		UpdateHall();
	}

	public void UpdateGameCoin()
	{
		UnityEngine.Debug.LogError("金币: " + XLDT_GameManager.getInstance()._mGameInfo.User.CoinCount);
		mCoinLab.text = (XLDT_GameInfo.getInstance().IsSpecial ? (XLDT_GameManager.getInstance()._mGameInfo.User.CoinCount % 10000).ToString() : XLDT_GameManager.getInstance()._mGameInfo.User.CoinCount.ToString());
		UpdateHall();
	}

	private void UpdateHall()
	{
		if (ZH2_GVars.hallInfo != null && hallInfo != ZH2_GVars.hallInfo)
		{
			UnityEngine.Debug.LogError("更新厅信息");
			ShowHall();
			hallInfo = ZH2_GVars.hallInfo;
		}
		int num = XLDT_GameManager.getInstance()._mGameInfo.User.Icon;
		if (num <= 0 || num >= icoSprite.Count)
		{
			XLDT_GameManager.getInstance()._mGameInfo.User.Icon = 0;
			num = 1;
		}
		imaIco.sprite = icoHeadSprite[num];
	}

	public void CreateTableAList(int count, int curtabindex)
	{
		XLDT_GameInfo.getInstance().TotalTabNum = count;
		XLDT_GameManager.getInstance()._mGameInfo.CurTabIndex = curtabindex;
		if (count <= 0)
		{
			UnOpen(mRoomId);
		}
	}

	public void ClickBtnBack()
	{
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		XLDT_TipManager.getInstance().ClickBtnBack();
	}

	private IEnumerator ClickBtnRoom(int index)
	{
		SetNoHall(isHavHall: true);
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
		if ((bool)mNameLab && ZH2_GVars.isShowTingName)
		{
			mNameLab.text = selectBtnList[index].name;
		}
		BackToRoom();
		float Time = (!isOnEnter) ? 0.25f : 0.05f;
		isOnEnter = false;
		yield return new WaitForSeconds(Time);
		int id = selectBtnList[index].hallId;
		int roomId = selectBtnList[index].hallType;
		if (roomId <= 0)
		{
			mCoinLab.gameObject.SetActive(value: false);
			mTestCoinLab.gameObject.SetActive(value: true);
		}
		else
		{
			mCoinLab.gameObject.SetActive(value: true);
			mTestCoinLab.gameObject.SetActive(value: false);
		}
		XLDT_GameManager.getInstance().mRoomId = roomId;
		XLDT_GameInfo.getInstance().currentState = XLDT_GameState.On_SelectSeat;
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendEnterHall(id);
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
			selectBtnList[i].hallType = (int)jsonData[i.ToString()]["roomId"] - 1;
			selectBtnList[i].name = jsonData[i.ToString()]["hallName"].ToString();
			selectBtnList[i].minGlod = jsonData[i.ToString()]["minGold"].ToString();
			selectBtnList[i].onlinePeople = "0";
			selectBtnList[i].UpdateText();
		}
	}

	public void SetNoHall(bool isHavHall)
	{
	}

	public void InItTable(XLDT_CardDesk[] tableArr)
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
		if (tableArr == null)
		{
			SetBtnLeft(isInteractable: false);
			SetBtnRight(isInteractable: false);
			return;
		}
		allTableArr = new XLDT_CardDesk[tableArr.Length];
		allTableArr = tableArr;
		for (int j = 0; j < allTableArr.Length; j++)
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
			if (allTableArr[k] == null || allTableArr[k].seats == null || allTableArr[k].seats.Length <= 0)
			{
				continue;
			}
			for (int l = 0; l < allTableArr[k].seats.Length; l++)
			{
				tableList[k].transform.Find("Ico/Image" + l).GetComponent<Image>().sprite = icoSprite[0];
				tableList[k].transform.Find("Ico/Image" + l + "/head").gameObject.SetActive(value: false);
			}
			for (int m = 0; m < allTableArr[k].seats.Length; m++)
			{
				if (allTableArr[k].seats[m].isFree)
				{
					continue;
				}
				int num2 = allTableArr[k].seats[m].photoId;
				int num3 = allTableArr[k].seats[m].id - 1;
				if (num2 >= icoSprite.Count)
				{
					num2 %= icoSprite.Count;
					UnityEngine.Debug.LogError("num2: " + num2);
					if (num2 >= icoSprite.Count)
					{
						num2 = icoSprite.Count - 1;
						UnityEngine.Debug.LogError("num3: " + num2);
					}
				}
				if (num2 <= 0)
				{
					num2 = icoSprite.Count - 1;
				}
				tableList[k].transform.Find("Ico/Image" + num3 + "/head").gameObject.SetActive(value: true);
				tableList[k].transform.Find("Ico/Image" + num3 + "/head").GetComponent<Image>().sprite = icoSprite[num2];
			}
			tableList[k].transform.Find("Inifo/Name").GetComponent<Text>().text = allTableArr[k].name;
			tableList[k].transform.Find("Inifo/min/Text").GetComponent<Text>().text = allTableArr[k].minYaFen.ToString();
			tableList[k].transform.Find("Inifo/Max/Text").GetComponent<Text>().text = allTableArr[k].gameXianHong.ToString();


            Destroy(tableList[k].transform.Find("Inifo/min").GetComponent<Translation_Game>());
            Destroy(tableList[k].transform.Find("Inifo/Max").GetComponent<Translation_Game>());
            tableList[k].transform.Find("Inifo/min").GetComponent<Text>().text = ZH2_GVars.ShowTip("最小押注:", "MiniBet:", "MiniBet:", "Tối thiểu:");
            tableList[k].transform.Find("Inifo/Max").GetComponent<Text>().text = ZH2_GVars.ShowTip("最大押注:", "MaxBet:", "MaxBet:", "Tối đa:");
        }
		if (ZH2_GVars.isClickTableEnterGame)
		{
			for (int n = 0; n < tableList.Count; n++)
			{
				int index = n;
				tableList[n].gameObject.GetComponent<Button>().onClick.AddListener(delegate
				{
					ClickBtnTable(allTableArr[index].seats, allTableArr[index].id);
				});
			}
		}
		else
		{
			for (int num4 = 0; num4 < tableList.Count; num4++)
			{
				int num5 = num4;
				tableList[num4].gameObject.GetComponent<LongPressBtn3>().onClickUp.AddListener(delegate
				{
				});
			}
		}
		for (int num6 = 0; num6 < tableList.Count; num6++)
		{
			int index2 = num6;
			for (int num7 = 0; num7 < 8; num7++)
			{
				int deskid = num7;
				tableList[index2].transform.Find("Ico/Image" + num7 + "/Btn").GetComponent<Button>().onClick.AddListener(delegate
				{
					ClickBtnArrow(allTableArr[index2].id, deskid + 1);
				});
			}
		}
	}

	public void ClickBtnTable(XLDT_Seat[] netSeats, int deskNum)
	{
		UnityEngine.Debug.LogError("点击了桌子: " + deskNum);
		XLDT_GameInfo.getInstance().CurTable.Id = deskNum;
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		EnterSeat(netSeats);
	}

	public void EnterSeat(XLDT_Seat[] netSeats)
	{
		int num = 0;
		for (int i = 0; i < netSeats.Length; i++)
		{
			int num2 = i;
			if (netSeats[num2].isFree && num <= 0)
			{
				num = 1;
				ClickBtnArrow(netSeats[num2].id);
				UnityEngine.Debug.LogError("EnterSeat选择了: " + netSeats[num2].id + " 号位");
				break;
			}
		}
		if (num <= 0)
		{
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.SelectSeat_NotEmpty);
		}
	}

	private void ClickBtnArrow(int index)
	{
		UnityEngine.Debug.LogError("TableId: " + XLDT_GameInfo.getInstance().CurTable.Id + "  座位ID: " + index);
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendSelectSeat(XLDT_GameInfo.getInstance().CurTable.Id, index);
	}

	private void ClickBtnArrow(int deskId, int index)
	{
		XLDT_GameInfo.getInstance().CurTable.Id = deskId;
		UnityEngine.Debug.LogError("TableId: " + XLDT_GameInfo.getInstance().CurTable.Id + "  座位ID: " + index);
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendSelectSeat(XLDT_GameInfo.getInstance().CurTable.Id, index);
	}

	public void UpdateSeat(XLDT_Seat[] netSeats, int deskId)
	{
		if (!base.gameObject)
		{
			return;
		}
		for (int i = 0; i < allTableArr.Length; i++)
		{
			if (allTableArr[i].id != deskId)
			{
				continue;
			}
			allTableArr[i].seats = netSeats;
			for (int j = 0; j < netSeats.Length; j++)
			{
				tableList[i].transform.Find("Ico/Image" + j + "/head").gameObject.SetActive(value: false);
				tableList[i].transform.Find("Ico/Image" + j).GetComponent<Image>().sprite = icoSprite[0];
			}
			for (int k = 0; k < netSeats.Length; k++)
			{
				if (allTableArr[i].seats[k].isFree)
				{
					continue;
				}
				int num = allTableArr[i].seats[k].photoId;
				int num2 = allTableArr[i].seats[k].id - 1;
				if (num >= icoSprite.Count)
				{
					num %= icoSprite.Count;
					UnityEngine.Debug.LogError("num2: " + num);
					if (num >= icoSprite.Count)
					{
						num = icoSprite.Count - 1;
						UnityEngine.Debug.LogError("num3: " + num);
					}
				}
				if (num <= 0)
				{
					num = icoSprite.Count - 1;
				}
				tableList[i].transform.Find("Ico/Image" + num2 + "/head").gameObject.SetActive(value: true);
				tableList[i].transform.Find("Ico/Image" + num2 + "/head").GetComponent<Image>().sprite = icoSprite[num];
			}
		}
	}

	public void EnterRoom(int index)
	{
		mRoomId = index;
	}

	public void BackToRoom()
	{
		XLDT_GameInfo.getInstance().currentState = XLDT_GameState.On_SelectRoom;
		XLDT_GameInfo.getInstance().User.RoomId = -1;
	}

	private IEnumerator InitFinished()
	{
		yield return new WaitForEndOfFrame();
		bInit = false;
	}
}
