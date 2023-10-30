using DG.Tweening;
using GameConfig;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DNTG_UIScene : MonoBehaviour
{
	private Button btnBG;

	private Button btnBack;

	private bool isSomeonePlayAction;

	private DNTG_GameInfo gameInfo;

	private DNTG_TipManager tipManager;

	public List<Sprite> icoSprite = new List<Sprite>();

	[SerializeField]
	private Transform tfTableInfo;

	private Text txtName;

	private Text txtCoin;

	private Text txtTestCoin;

	private Image imaIco;

	[SerializeField]
	private DNTG_DropdownList sptDropdownList;

	private Transform Content;

	private Transform ContentOldPos;

	private Transform ContentTagPos;

	private Transform scrollView;

	private Button btnLeft;

	private Button btnRight;

	public GameObject tablePre;

	public GameObject noHallTip;

	[HideInInspector]
	public List<GameObject> tableList = new List<GameObject>();

	public CircleScrollRect circleScrollRect;

	public List<RotateBtnInfo> selectBtnList = new List<RotateBtnInfo>();

	public GameObject objImgLoadGame;

	private Dictionary<string, object> hallInfo = new Dictionary<string, object>();

	private List<DNTG_TableInfo> tempTableList;

	private bool isOnEnter = true;

	private int tempSelectId = -1;

	private float contentanchoredPositionX;

	private RectTransform contentRectTransform;

	private BYSD_TwoContentSizeCtrl bYSD_TwoContentSize;

	public Text testText;

	private List<DNTG_FishDesk> allTableArr;

	private void Start()
	{
		gameInfo = DNTG_GameInfo.getInstance();
		tipManager = DNTG_TipManager.getInstance();
		gameInfo.UIScene = this;
		tipManager.InitTip();
		GetObjects();
		HideGameObjects();
		ZH2_GVars.isStartGame = false;
		if (gameInfo.currentState == DNTG_GameState.On_Game)
		{
			showTableInterface();
			gameInfo.User.SeatIndex = -1;
			gameInfo.currentState = DNTG_GameState.On_SelectTable;
		}
		else
		{
			gameInfo.currentState = DNTG_GameState.On_SelectRoom;
		}
		DNTG_LocalData.getInstance().applySetting();
		UpdateUserInfo();
		gameInfo.NetShouldBlocked = false;
	}

	private void OnEnable()
	{
		tempSelectId = -1;
		isOnEnter = true;
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

	public void UpdateTableConfig()
	{
		DNTG_TableInfo table = gameInfo.Table;
		int index = (gameInfo.TableList.Count > 10) ? Mathf.FloorToInt(10f / (float)gameInfo.TableList.Count * (float)gameInfo.TableIndex) : gameInfo.TableIndex;
		sptDropdownList.ChooseTable(index);
		if (gameInfo.currentState != DNTG_GameState.On_Game)
		{
			gameInfo.updateCurUsers(table.mNetSeats);
		}
	}

	public void UpdateTableList()
	{
		tempTableList = new List<DNTG_TableInfo>();
		for (int i = 0; i < gameInfo.TableList.Count; i++)
		{
			tempTableList.Add(gameInfo.TableList[i]);
		}
		int count = tempTableList.Count;
		sptDropdownList.ShowTableList(count);
		for (int j = 0; j < count; j++)
		{
			sptDropdownList.txtTableName[j].text = tempTableList[j].Name;
			sptDropdownList.txtTableInfo[j].text = $"({tempTableList[j].PersonCount}/4)";
			int index = j;
			sptDropdownList.btnChilds[j].onClick.RemoveAllListeners();
			sptDropdownList.btnChilds[j].onClick.AddListener(delegate
			{
				ClickTableListItem(index);
			});
		}
	}

	private void ClickTableListItem(int index)
	{
		UnityEngine.Debug.LogError("点击了桌子列表: " + index);
		if (isSomeonePlayAction)
		{
			UnityEngine.Debug.LogError("被返回了");
			return;
		}
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		try
		{
			gameInfo.TableIndex = index;
			gameInfo.Table = gameInfo.TableList[index];
			UpdateTableConfig();
			ClickBtnTable(allTableArr[index].seats, gameInfo.Table.Id);
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("点击桌子列表错误: " + arg);
			isSomeonePlayAction = false;
		}
	}

	public void UpdateTableListItem(int index)
	{
		try
		{
			sptDropdownList.txtTableName[index].text = gameInfo.TableList[index].Name;
			sptDropdownList.txtTableInfo[index].text = $"({gameInfo.TableList[index].PersonCount}/4)";
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
		UpdateTableList();
	}

	public void UpdateTableUsers()
	{
		List<DNTG_UserInfo> userList = gameInfo.UserList;
		if (userList == null)
		{
			return;
		}
		for (int i = 0; i < userList.Count; i++)
		{
			int icon = userList[i].Icon;
			int seatIndex = userList[i].SeatIndex;
			if (userList[i].UserAccount == gameInfo.User.UserAccount)
			{
				gameInfo.User.SeatIndex = seatIndex;
				gameInfo.User.GunValue = gameInfo.Table.MinGun;
			}
		}
	}

	public void UpdateUserInfo()
	{
		txtCoin.text = (gameInfo.IsSpecial ? (gameInfo.User.CoinCount % 10000).ToString() : gameInfo.User.CoinCount.ToString());
		txtTestCoin.text = (gameInfo.IsSpecial ? (gameInfo.User.TestCoinCount % 10000).ToString() : gameInfo.User.TestCoinCount.ToString());
		if (ZH2_GVars.hallInfo != null && hallInfo != ZH2_GVars.hallInfo)
		{
			UnityEngine.Debug.LogError("更新厅信息");
			ShowHall();
			hallInfo = ZH2_GVars.hallInfo;
		}
		int num = gameInfo.User.Icon + 1;
		if (num <= 0 || num >= icoSprite.Count)
		{
			num = 1;
		}
		imaIco.sprite = icoSprite[num];
	}

	private void GetObjects()
	{
		btnBG = base.transform.Find("ImgBG").GetComponent<Button>();
		Transform transform = base.transform.Find("Title");
		btnBack = transform.Find("BtnBack").GetComponent<Button>();
		txtName = transform.Find("TxtName").GetComponent<Text>();
		txtName.text = ZH2_GVars.ShowTip("大闹天宫", "TheMonKeyKing", "TheMonKeyKing", "Đại Náo Thiên Cung");
		txtCoin = transform.Find("Coin/TxtCoin").GetComponent<Text>();
		txtTestCoin = transform.Find("Coin/TxtTestCoin").GetComponent<Text>();
		imaIco = transform.Find("Head/Image").GetComponent<Image>();
		Transform transform2 = base.transform.Find("Tables");
		scrollView = transform2.Find("Scroll View");
		Content = scrollView.Find("Viewport/Content");
		ContentOldPos = scrollView.Find("Viewport/ContentOldPos");
		ContentTagPos = scrollView.Find("Viewport/ContentTagPos");
		btnBG.onClick.AddListener(delegate
		{
			ClickBtnBG(bChangeState: false);
		});
		btnBG.onClick.AddListener(delegate
		{
			ClickBtnBG(bChangeState: false);
		});
		btnBack.onClick.AddListener(ClickBtnBack);
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
	}

	private void HideGameObjects()
	{
		tfTableInfo.gameObject.SetActive(value: false);
		objImgLoadGame.SetActive(value: false);
	}

	public void EnterGame()
	{
		objImgLoadGame.SetActive(value: true);
		SceneManager.LoadSceneAsync("DNTG_GameScene");
	}

	public void ClickBtnBG(bool bChangeState)
	{
		if (!bChangeState)
		{
			if (sptDropdownList.isOpening)
			{
				sptDropdownList.ClickBtnArrow();
			}
		}
		else
		{
			sptDropdownList.ResetTableList();
		}
	}

	public void ClickBtnBack()
	{
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		DNTG_TipManager.getInstance().ClickBlack();
		ClickBtnBG(bChangeState: true);
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
				string text = testText.text;
				JsonData jsonData = JsonMapper.ToObject(text);
				All_NoticePanel.GetInstance().AddTip(jsonData);
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

	private void GoToRoom(int otherId = 0)
	{
		if (gameInfo.currentState != DNTG_GameState.On_SelectRoom && gameInfo.currentState == DNTG_GameState.On_SelectTable)
		{
			objImgLoadGame.SetActive(value: false);
			gameInfo.clearTableInfo();
			gameInfo.currentState = DNTG_GameState.On_SelectRoom;
			HideTable();
		}
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
		if ((bool)txtName && ZH2_GVars.isShowTingName)
		{
			txtName.text = selectBtnList[index].name;
		}
		GoToRoom();
		float Time = (!isOnEnter) ? 0.25f : 0.05f;
		isOnEnter = false;
		yield return new WaitForSeconds(Time);
		int id = selectBtnList[index].hallId;
		int roomId = selectBtnList[index].hallType;
		if (roomId <= 0)
		{
			txtCoin.gameObject.SetActive(value: false);
			txtTestCoin.gameObject.SetActive(value: true);
		}
		else
		{
			txtCoin.gameObject.SetActive(value: true);
			txtTestCoin.gameObject.SetActive(value: false);
		}
		tipManager.roomId = roomId;
		gameInfo.User.RoomId = roomId;
		DNTG_NetMngr.GetSingleton().MyCreateSocket.SendEnterHall(id);
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
		noHallTip.SetActive(value: false);
	}

	public void InItTable(DNTG_FishDesk[] tableArr)
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
		allTableArr = new List<DNTG_FishDesk>();
		if (tableArr == null)
		{
			SetBtnLeft(isInteractable: false);
			SetBtnRight(isInteractable: false);
			return;
		}
		for (int j = 0; j < tableArr.Length; j++)
		{
			allTableArr.Add(tableArr[j]);
		}
		for (int k = 0; k < allTableArr.Count; k++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(tablePre, Content);
			LongPressBtn3 component = gameObject.GetComponent<LongPressBtn3>();
			if (component == null)
			{
				component = gameObject.AddComponent<LongPressBtn3>();
			}
			tableList.Add(gameObject);
		}
		for (int l = 0; l < tableList.Count; l++)
		{
			int num = l;
			if (allTableArr[l] != null && allTableArr[l].seats != null && allTableArr[l].seats.Length > 0)
			{
				for (int m = 0; m < allTableArr[l].seats.Length; m++)
				{
					tableList[l].transform.Find("Ico/Image" + m).GetComponent<Image>().sprite = icoSprite[0];
				}
				for (int n = 0; n < allTableArr[l].seats.Length; n++)
				{
					if (!allTableArr[l].seats[n].isFree)
					{
						int num2 = allTableArr[l].seats[n].user.photoId;
						int num3 = allTableArr[l].seats[n].seatId - 1;
						if (num2 <= 0 || num2 >= icoSprite.Count)
						{
							num2 = 1;
						}
						tableList[l].transform.Find("Ico/Image" + num3).GetComponent<Image>().sprite = icoSprite[num2];
					}
				}
				tableList[l].transform.Find("Inifo/Name").GetComponent<Text>().text = allTableArr[l].name;
				tableList[l].transform.Find("Inifo/min/Text").GetComponent<Text>().text = allTableArr[l].minGunValue.ToString();
				tableList[l].transform.Find("Inifo/Max/Text").GetComponent<Text>().text = allTableArr[l].maxGunValue.ToString();


                Destroy(tableList[l].transform.Find("Inifo/min").GetComponent<Translation_Game>());
                Destroy(tableList[l].transform.Find("Inifo/Max").GetComponent<Translation_Game>());
                tableList[l].transform.Find("Inifo/min").GetComponent<Text>().text = ZH2_GVars.ShowTip("最小炮值:", "Min Bet:", "Min Bet:", "Tối thiểu:");
                tableList[l].transform.Find("Inifo/Max").GetComponent<Text>().text = ZH2_GVars.ShowTip("最大炮值:", "Max Bet:", "Max Bet:", "Tối đa:");
            }
			else
			{
				UnityEngine.Debug.LogError(l + " seats异常 " + ((allTableArr[l] == null) ? "为空" : "个数为0"));
			}
		}
		if (ZH2_GVars.isClickTableEnterGame)
		{
			for (int num4 = 0; num4 < tableList.Count; num4++)
			{
				int index = num4;
				tableList[num4].gameObject.GetComponent<Button>().onClick.AddListener(delegate
				{
					ClickBtnTable(allTableArr[index].seats, allTableArr[index].id);
				});
			}
		}
		else
		{
			for (int num5 = 0; num5 < tableList.Count; num5++)
			{
				int num6 = num5;
				tableList[num5].gameObject.GetComponent<LongPressBtn3>().onClickUp.AddListener(delegate
				{
				});
			}
		}
		for (int num7 = 0; num7 < tableList.Count; num7++)
		{
			int index2 = num7;
			for (int num8 = 0; num8 < 4; num8++)
			{
				int deskid = num8;
				tableList[index2].transform.Find("Ico/Image" + num8 + "/Btn").GetComponent<Button>().onClick.AddListener(delegate
				{
					ClickBtnArrow(allTableArr[index2].roomId, allTableArr[index2].id, deskid + 1);
				});
			}
		}
	}

	public void ClickBtnTable(DNTG_Seat[] netSeats, int deskNum)
	{
		UnityEngine.Debug.LogError("点击了桌子: " + deskNum);
		if (isSomeonePlayAction)
		{
			UnityEngine.Debug.LogError("被返回了");
			return;
		}
		if (gameInfo == null)
		{
			gameInfo = DNTG_GameInfo.getInstance();
		}
		if (gameInfo != null)
		{
			for (int i = 0; i < tempTableList.Count; i++)
			{
				if (tempTableList[i].Id == deskNum)
				{
					gameInfo.Table = tempTableList[i];
					UnityEngine.Debug.LogError("设置了桌子");
					break;
				}
			}
		}
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
		EnterSeat(netSeats);
	}

	public void EnterSeat(DNTG_Seat[] netSeats)
	{
		int num = 0;
		for (int i = 0; i < netSeats.Length; i++)
		{
			int num2 = i;
			if (netSeats[num2].isFree && num <= 0)
			{
				num = 1;
				ClickBtnArrow(netSeats[num2].seatId);
				UnityEngine.Debug.LogError("EnterSeat选择了: " + netSeats[num2].seatId + " 号位");
				break;
			}
		}
		if (num <= 0)
		{
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.SelectSeat_NotEmpty, 0, string.Empty);
		}
	}

	public void UpdateSeat(DNTG_Seat[] netSeats, int deskId)
	{
		if (!base.gameObject || allTableArr == null)
		{
			return;
		}
		for (int i = 0; i < allTableArr.Count; i++)
		{
			if (allTableArr[i].id != deskId)
			{
				continue;
			}
			UnityEngine.Debug.LogError("更新座位信息: " + JsonMapper.ToJson(netSeats));
			allTableArr[i].seats = netSeats;
			for (int j = 0; j < netSeats.Length; j++)
			{
				tableList[i].transform.Find("Ico/Image" + j).GetComponent<Image>().sprite = icoSprite[0];
			}
			for (int k = 0; k < netSeats.Length; k++)
			{
				if (!allTableArr[i].seats[k].isFree)
				{
					int num = allTableArr[i].seats[k].user.photoId;
					int num2 = allTableArr[i].seats[k].seatId - 1;
					if (num <= 0 || num >= icoSprite.Count)
					{
						num = 1;
					}
					tableList[i].transform.Find("Ico/Image" + num2).GetComponent<Image>().sprite = icoSprite[num];
				}
			}
		}
	}

	public void InSeat(DNTG_Seat[] netSeats)
	{
		int num = 0;
		int num2 = 0;
		while (true)
		{
			if (num2 < netSeats.Length)
			{
				if (netSeats[num2].isFree && num <= 0)
				{
					break;
				}
				num2++;
				continue;
			}
			return;
		}
		num = 1;
		int num3 = num2;
	}

	public void EnterRoom()
	{
		isSomeonePlayAction = true;
		gameInfo.currentState = DNTG_GameState.On_SelectTable;
		HideRoom();
		ShowTable(0.02f);
	}

	public void HideRoom()
	{
	}

	public void ShowRoom()
	{
	}

	public void ClickBtnLeftArrow()
	{
		TableMoveLeft();
		DNTG_GameInfo.getInstance().changeTable(0);
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
	}

	public void ClickBtnRightArrow()
	{
		TableMoveRight();
		DNTG_GameInfo.getInstance().changeTable(1);
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
	}

	public void TableMoveRight()
	{
		if (!isSomeonePlayAction)
		{
			isSomeonePlayAction = true;
			DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.changeTable);
		}
	}

	public void TableMoveLeft()
	{
		if (!isSomeonePlayAction)
		{
			isSomeonePlayAction = true;
			DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.changeTable);
		}
	}

	private void ResetTablePosition()
	{
		isSomeonePlayAction = false;
	}

	public void HideTable()
	{
		isSomeonePlayAction = true;
		tfTableInfo.gameObject.SetActive(value: false);
	}

	public void ShowTable(float delay)
	{
		DNTG_NetMngr.GetSingleton().MyCreateSocket.SendLeaveDesk(gameInfo.Table.Id);
		tfTableInfo.gameObject.SetActive(value: true);
		sptDropdownList.ResetTableList();
		UpdateTableConfig();
		ResetTablePosition();
		UpdateUserInfo();
	}

	private void ShowTableAndSeat(float delay)
	{
	}

	private void HideTableAndSeat()
	{
		isSomeonePlayAction = true;
	}

	private void showTableInterface()
	{
		tfTableInfo.gameObject.SetActive(value: true);
		sptDropdownList.ResetTableList();
		UpdateTableList();
		UpdateTableConfig();
	}

	private void ClickBtnArrow(int index)
	{
		ClickBtnBG(bChangeState: false);
		if (!isSomeonePlayAction)
		{
			UnityEngine.Debug.LogError("TableId: " + gameInfo.Table.Id + "  座位ID: " + index);
			DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
			DNTG_NetMngr.GetSingleton().MyCreateSocket.SendRequestSeat(gameInfo.User.RoomId, gameInfo.Table.Id, index);
		}
	}

	private void ClickBtnArrow(int roomId, int deskId, int index)
	{
		ClickBtnBG(bChangeState: false);
		if (!isSomeonePlayAction)
		{
			gameInfo.Table.Id = deskId;
			gameInfo.User.RoomId = roomId - 1;
			DNTG_GameInfo.getInstance().SelectTable(deskId);
			DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
			DNTG_NetMngr.GetSingleton().MyCreateSocket.SendRequestSeat(gameInfo.User.RoomId, gameInfo.Table.Id, index);
		}
	}

	private void ClickBtnPerson(int index)
	{
		if (!isSomeonePlayAction)
		{
			DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.common);
			if (index != gameInfo.User.SeatIndex - 1)
			{
				DNTG_NetMngr.GetSingleton().MyCreateSocket.SendRequestPlayerInfo(gameInfo.UserList[index].Id);
			}
		}
	}
}
