using DG.Tweening;
using GameConfig;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TF_UIScene : MonoBehaviour
{
	private Button btnBG;

	private Button btnBack;

	private bool isSomeonePlayAction;

	private TF_GameInfo gameInfo;

	private TF_TipManager tipManager;

	[SerializeField]
	private Transform tfTableInfo;

	private Text txtName;

	private Text txtCoin;

	private Text txtTestCoin;

	private Text txtNickname;

	private Text txtTableName;

	private Text txtMinCarry;

	private Text txtMaxGun;

	private Text txtMinGun;

	private Text txtScorePerCoin;

	private Text txtValueConfig;

	private Text txtCoinConfig;

	private Text txtMinGunConfig;

	private Text txtMaxGunConfig;

	private Button btnRoom0;

	private Button btnRoom1;

	private Button btnLeftArrow;

	private Button btnRightArrow;

	private Transform tfLeftTable;

	private Transform tfRightTable;

	private float[] pX = new float[3]
	{
		0f,
		-1100f,
		1100f
	};

	private float pY = -50f;

	[SerializeField]
	private TF_DotsAnimCtrl sptDotsAnimCtrl;

	[SerializeField]
	private TF_DropdownList sptDropdownList;

	[SerializeField]
	private GameObject objTableWithSeat;

	private Image[] imgPersons = new Image[4];

	private Button[] btnPerson = new Button[4];

	private Button[] btnArrow = new Button[4];

	[SerializeField]
	private TF_UserPic userPic;

	private GameObject objImgLoadGame;

	private Text txtOtherName;

	private Text txtOtherCoin;

	private Text txtOtherLevel;

	private Text txtOtherHonor;

	private Image imgOtherIcon;

	[SerializeField]
	private TF_UserIcon userIcon;

	[SerializeField]
	private GameObject objPersonInfoDialog;

	private void Start()
	{
		gameInfo = TF_GameInfo.getInstance();
		tipManager = TF_TipManager.getInstance();
		gameInfo.UIScene = this;
		tipManager.InitTip();
		GetObjects();
		InitTableConfig();
		HideGameObjects();
		ZH2_GVars.isStartGame = false;
		if (gameInfo.currentState == TF_GameState.On_Game)
		{
			showTableInterface();
			gameInfo.User.SeatIndex = -1;
			gameInfo.currentState = TF_GameState.On_SelectTable;
		}
		else
		{
			gameInfo.currentState = TF_GameState.On_SelectRoom;
		}
		TF_LocalData.getInstance().applySetting();
		UpdateUserInfo();
		gameInfo.NetShouldBlocked = false;
	}

	public void SetIndicator()
	{
		int count = (gameInfo.TableList.Count > 10) ? 10 : gameInfo.TableList.Count;
		sptDotsAnimCtrl.ShowDots(count);
	}

	private void InitTableConfig()
	{
		int language = gameInfo.Language;
		txtValueConfig.text = ((language != 0) ? "Ticket Value" : "一币分值");
		txtCoinConfig.text = ((language != 0) ? "Min Tickets" : "最小携带");
		txtMinGunConfig.text = ((language != 0) ? "Min GunType" : "最小炮值");
		txtMaxGunConfig.text = ((language != 0) ? "Max GunType" : "最大炮值");
	}

	public void UpdateTableConfig()
	{
		UnityEngine.Debug.Log("UpdateTableConfig");
		TF_TableInfo table = gameInfo.Table;
		txtTableName.text = table.Name;
		txtMinCarry.text = table.MinCarry.ToString();
		txtMinGun.text = table.MinGun.ToString();
		txtMaxGun.text = table.MaxGun.ToString();
		txtScorePerCoin.text = table.ScorePerCoin.ToString();
		if (gameInfo.Language == 1)
		{
			txtValueConfig.text = ((gameInfo.User.RoomId == 1) ? "Ticket Value" : "Coin Value");
			txtCoinConfig.text = ((gameInfo.User.RoomId == 1) ? "Min Tickets" : "Min Coins");
		}
		int index = (gameInfo.TableList.Count > 10) ? Mathf.FloorToInt(10f / (float)gameInfo.TableList.Count * (float)gameInfo.TableIndex) : gameInfo.TableIndex;
		sptDotsAnimCtrl.ChooseTable(index);
		sptDropdownList.ChooseTable(index);
		if (gameInfo.currentState != TF_GameState.On_Game)
		{
			gameInfo.updateCurUsers(table.mNetSeats);
		}
	}

	public void UpdateTableList()
	{
		UnityEngine.Debug.Log("UpdateTableList");
		int count = gameInfo.TableList.Count;
		sptDropdownList.ShowTableList(count);
		for (int i = 0; i < count; i++)
		{
			sptDropdownList.txtTableName[i].text = gameInfo.TableList[i].Name;
			sptDropdownList.txtTableInfo[i].text = $"({gameInfo.TableList[i].PersonCount}/4)";
			int index = i;
			sptDropdownList.btnChilds[i].onClick.RemoveAllListeners();
			sptDropdownList.btnChilds[i].onClick.AddListener(delegate
			{
				ClickTableListItem(index);
			});
		}
	}

	private void ClickTableListItem(int index)
	{
		if (isSomeonePlayAction)
		{
			return;
		}
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
		if (index != gameInfo.TableIndex)
		{
			if (index > gameInfo.TableIndex)
			{
				TableMoveLeft();
			}
			else
			{
				TableMoveRight();
			}
			gameInfo.TableIndex = index;
			gameInfo.Table = gameInfo.TableList[index];
			UpdateTableConfig();
		}
	}

	public void UpdateTableListItem(int index)
	{
		sptDropdownList.txtTableName[index].text = gameInfo.TableList[index].Name;
		sptDropdownList.txtTableInfo[index].text = $"({gameInfo.TableList[index].PersonCount}/4)";
	}

	public void UpdateTableUsers()
	{
		List<TF_UserInfo> userList = gameInfo.UserList;
		if (userList == null)
		{
			return;
		}
		for (int i = 0; i < userList.Count; i++)
		{
			int icon = userList[i].Icon;
			int seatIndex = userList[i].SeatIndex;
			if (!userList[i].IsExist)
			{
				btnArrow[i].gameObject.SetActive(value: true);
				imgPersons[i].gameObject.SetActive(value: false);
				btnPerson[i].gameObject.SetActive(value: false);
			}
			else
			{
				imgPersons[i].sprite = userPic.spiPic[(icon - 1) * 4 + seatIndex - 1];
				imgPersons[i].SetNativeSize();
				imgPersons[i].transform.localPosition = TF_Person.positions[icon - 1][seatIndex - 1];
				imgPersons[i].gameObject.SetActive(value: true);
				btnPerson[i].gameObject.SetActive(value: true);
				btnArrow[i].gameObject.SetActive(value: false);
			}
			if (userList[i].UserAccount == gameInfo.User.UserAccount)
			{
				gameInfo.User.SeatIndex = seatIndex;
				gameInfo.User.GunValue = gameInfo.Table.MinGun;
			}
		}
	}

	public void UpdateUserInfo()
	{
		UnityEngine.Debug.Log("UpdateUserInfo");
		if (gameInfo.currentState == TF_GameState.On_SelectRoom)
		{
			txtName.text = ((gameInfo.Language == 0) ? "金蟾捕鱼" : "ToadFishing");
		}
		else if (gameInfo.currentState == TF_GameState.On_SelectTable)
		{
			if (gameInfo.User.RoomId == 1)
			{
				txtName.text = ((gameInfo.Language == 0) ? "练习厅" : "Training");
			}
			else
			{
				txtName.text = ((gameInfo.Language == 0) ? "竞技厅" : "Arena");
			}
		}
		else
		{
			txtName.text = gameInfo.Table.Name;
		}
		txtCoin.text = (gameInfo.IsSpecial ? (gameInfo.User.CoinCount % 10000).ToString() : gameInfo.User.CoinCount.ToString());
		txtTestCoin.text = (gameInfo.IsSpecial ? (gameInfo.User.TestCoinCount % 10000).ToString() : gameInfo.User.TestCoinCount.ToString());
		txtNickname.text = gameInfo.User.Name;
	}

	private void GetObjects()
	{
		btnBG = base.transform.Find("ImgBG").GetComponent<Button>();
		Transform transform = base.transform.Find("Title");
		btnBack = transform.Find("BtnBack").GetComponent<Button>();
		txtName = transform.Find("TxtName").GetComponent<Text>();
		txtCoin = transform.Find("TxtCoin").GetComponent<Text>();
		txtTestCoin = transform.Find("TxtTestCoin").GetComponent<Text>();
		txtNickname = transform.Find("TxtNickname").GetComponent<Text>();
		Transform transform2 = tfTableInfo.Find("Info");
		txtTableName = transform2.Find("TxtRoomName").GetComponent<Text>();
		txtMinCarry = transform2.Find("TxtMinTicketsValue").GetComponent<Text>();
		txtMaxGun = transform2.Find("TxtMaxGunTypeValue").GetComponent<Text>();
		txtMinGun = transform2.Find("TxtMinGunTypeValue").GetComponent<Text>();
		txtScorePerCoin = transform2.Find("TxtTicketValueValue").GetComponent<Text>();
		txtValueConfig = transform2.Find("TxtTicketValue").GetComponent<Text>();
		txtCoinConfig = transform2.Find("TxtMinTickets").GetComponent<Text>();
		txtMinGunConfig = transform2.Find("TxtMinGunType").GetComponent<Text>();
		txtMaxGunConfig = transform2.Find("TxtMaxGunType").GetComponent<Text>();
		objImgLoadGame = objTableWithSeat.transform.Find("ImgLoadGame").gameObject;
		Transform transform3 = objTableWithSeat.transform.Find("Persons");
		Transform transform4 = objTableWithSeat.transform.Find("Arrows");
		for (int i = 0; i < 4; i++)
		{
			imgPersons[i] = transform3.Find($"ImgPerson{i}").GetComponent<Image>();
			btnPerson[i] = transform3.Find($"BtnPerson{i}").GetComponent<Button>();
			btnArrow[i] = transform4.Find($"BtnArrow{i}").GetComponent<Button>();
			int index = i;
			btnPerson[i].onClick.AddListener(delegate
			{
				ClickBtnPerson(index);
			});
			btnArrow[i].onClick.AddListener(delegate
			{
				ClickBtnArrow(index);
			});
		}
		txtOtherCoin = objPersonInfoDialog.transform.Find("TxtCoin").GetComponent<Text>();
		txtOtherHonor = objPersonInfoDialog.transform.Find("TxtHonor").GetComponent<Text>();
		txtOtherName = objPersonInfoDialog.transform.Find("TxtNickname").GetComponent<Text>();
		imgOtherIcon = objPersonInfoDialog.transform.Find("ImgIcon").GetComponent<Image>();
		txtOtherLevel = objPersonInfoDialog.transform.Find("TxtLevel").GetComponent<Text>();
		btnRoom0 = base.transform.Find("Rooms/BtnTrain").GetComponent<Button>();
		btnRoom1 = base.transform.Find("Rooms/BtnArena").GetComponent<Button>();
		Transform transform5 = tfTableInfo.Find("Arrows");
		btnLeftArrow = transform5.Find("BtnLeft").GetComponent<Button>();
		btnRightArrow = transform5.Find("BtnRight").GetComponent<Button>();
		Transform transform6 = base.transform.Find("Tables");
		tfLeftTable = transform6.Find("ImgTableLeft");
		tfRightTable = transform6.Find("ImgTableRight");
		btnBG.onClick.AddListener(delegate
		{
			ClickBtnBG(bChangeState: false);
		});
		btnBack.onClick.AddListener(ClickBtnBack);
		btnRoom0.onClick.AddListener(delegate
		{
			ClickBtnRoom(0);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			ClickBtnRoom(1);
		});
		btnLeftArrow.onClick.AddListener(ClickBtnLeftArrow);
		btnRightArrow.onClick.AddListener(ClickBtnRightArrow);
	}

	private void HideGameObjects()
	{
		tfTableInfo.gameObject.SetActive(value: false);
		objImgLoadGame.SetActive(value: false);
		objPersonInfoDialog.SetActive(value: false);
	}

	public void EnterGame()
	{
		objImgLoadGame.SetActive(value: true);
		SceneManager.LoadSceneAsync("STTF_GameScene");
	}

	public void ShowPersonInfo(TF_UserInfo user, int honor)
	{
		if (TF_TipManager.getInstance().mTipType == TF_TipType.NoneTip)
		{
			imgOtherIcon.sprite = userIcon.spiIcon[user.Icon - 1];
			txtOtherCoin.text = user.CoinCount + string.Empty;
			txtOtherLevel.text = "Lv." + user.Level + "(" + TF_TitleName.names[user.Level - 1] + ")";
			if (TF_GameInfo.getInstance().Language == 1)
			{
				txtOtherHonor.text = ((honor >= 1 && honor <= 10) ? ("ToadFishing：No." + honor) : "Failed to enter the ranking");
			}
			else
			{
				txtOtherHonor.text = ((honor >= 1 && honor <= 10) ? ("金蟾捕鱼：No." + honor) : "未上榜");
			}
			txtOtherName.text = user.Name;
			objPersonInfoDialog.SetActive(value: true);
		}
	}

	public void ClickBtnBG(bool bChangeState)
	{
		if (objPersonInfoDialog.activeSelf)
		{
			objPersonInfoDialog.SetActive(value: false);
		}
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
		if (!isSomeonePlayAction)
		{
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
			ClickBtnBG(bChangeState: true);
			if (gameInfo.currentState == TF_GameState.On_SelectTable)
			{
				TF_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(gameInfo.User.RoomId);
				gameInfo.clearTableInfo();
				gameInfo.currentState = TF_GameState.On_SelectRoom;
				HideTable();
				ShowRoom();
				isSomeonePlayAction = true;
			}
			else if (gameInfo.currentState == TF_GameState.On_SelectRoom)
			{
				TF_TipManager.getInstance().ShowTip(TF_TipType.IsExitApplication);
			}
		}
	}

	private void ClickBtnRoom(int index)
	{
		if (!isSomeonePlayAction)
		{
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
			TF_NetMngr.GetSingleton().MyCreateSocket.SendEnterRoom(index);
		}
	}

	public void EnterRoom()
	{
		isSomeonePlayAction = true;
		gameInfo.currentState = TF_GameState.On_SelectTable;
		HideRoom();
		ShowTable(0.02f);
	}

	public void HideRoom()
	{
		btnRoom0.transform.DOLocalMoveX(-1000f, 0.5f);
		btnRoom1.transform.DOLocalMoveX(1150f, 0.5f);
	}

	public void ShowRoom()
	{
		btnRoom0.transform.DOLocalMoveX(-200f, 0.5f);
		btnRoom1.transform.DOLocalMoveX(315f, 0.5f).OnComplete(delegate
		{
			isSomeonePlayAction = false;
		});
	}

	public void ClickBtnLeftArrow()
	{
		TableMoveLeft();
		TF_GameInfo.getInstance().changeTable(0);
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
	}

	public void ClickBtnRightArrow()
	{
		TableMoveRight();
		TF_GameInfo.getInstance().changeTable(1);
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
	}

	public void TableMoveRight()
	{
		if (!isSomeonePlayAction)
		{
			isSomeonePlayAction = true;
			objTableWithSeat.transform.DOLocalMoveX(pX[2], 0.2f).SetEase(Ease.Linear);
			tfLeftTable.DOLocalMoveX(pX[0], 0.2f).SetEase(Ease.Linear).OnComplete(ResetTablePosition);
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.changeTable);
		}
	}

	public void TableMoveLeft()
	{
		if (!isSomeonePlayAction)
		{
			isSomeonePlayAction = true;
			objTableWithSeat.transform.DOLocalMoveX(pX[1], 0.2f).SetEase(Ease.Linear);
			tfRightTable.DOLocalMoveX(pX[0], 0.2f).SetEase(Ease.Linear).OnComplete(ResetTablePosition);
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.changeTable);
		}
	}

	private void ResetTablePosition()
	{
		objTableWithSeat.transform.localPosition = Vector3.zero;
		tfLeftTable.localPosition = Vector3.up * pY + Vector3.right * pX[1];
		tfRightTable.localPosition = Vector3.up * pY + Vector3.right * pX[2];
		isSomeonePlayAction = false;
	}

	public void HideTable()
	{
		isSomeonePlayAction = true;
		tfTableInfo.gameObject.SetActive(value: false);
		objTableWithSeat.transform.DOLocalMoveY(-100f, 1f);
		objTableWithSeat.transform.DOScale(0f, 1f).OnComplete(delegate
		{
			UpdateUserInfo();
		});
	}

	public void ShowTable(float delay)
	{
		objTableWithSeat.transform.DOMoveZ(0.1f, delay).OnComplete(delegate
		{
			objTableWithSeat.transform.DOLocalMoveY(0f, 1f);
			objTableWithSeat.transform.DOScale(1f, 1f).OnComplete(delegate
			{
				tfTableInfo.gameObject.SetActive(value: true);
				sptDropdownList.ResetTableList();
				UpdateTableConfig();
				ResetTablePosition();
				UpdateUserInfo();
			});
		});
	}

	private void ShowTableAndSeat(float delay)
	{
		objTableWithSeat.transform.DOMoveZ(0.1f, delay).OnComplete(delegate
		{
			objTableWithSeat.transform.DOLocalMoveY(0f, 0.5f);
			objTableWithSeat.transform.DOScale(1f, 0.5f).OnComplete(delegate
			{
				isSomeonePlayAction = false;
			});
		});
	}

	private void HideTableAndSeat()
	{
		isSomeonePlayAction = true;
		objTableWithSeat.transform.DOLocalMoveY(-100f, 1f);
		objTableWithSeat.transform.DOScale(0f, 1f);
	}

	private void showTableInterface()
	{
		objTableWithSeat.transform.localPosition = Vector3.zero;
		objTableWithSeat.transform.localScale = Vector3.one;
		btnRoom0.transform.localPosition = Vector3.left * 1000f + Vector3.down * 100f;
		btnRoom1.transform.localPosition = Vector3.right * 1150f + Vector3.down * 100f;
		tfTableInfo.gameObject.SetActive(value: true);
		sptDropdownList.ResetTableList();
		SetIndicator();
		UpdateTableList();
		UpdateTableConfig();
	}

	private void ClickBtnArrow(int index)
	{
		ClickBtnBG(bChangeState: false);
		if (!isSomeonePlayAction)
		{
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
			TF_NetMngr.GetSingleton().MyCreateSocket.SendRequestSeat(gameInfo.Table.Id, index + 1);
		}
	}

	private void ClickBtnPerson(int index)
	{
		if (!isSomeonePlayAction)
		{
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
			if (index != gameInfo.User.SeatIndex - 1)
			{
				TF_NetMngr.GetSingleton().MyCreateSocket.SendRequestPlayerInfo(gameInfo.UserList[index].Id);
			}
		}
	}
}
