using DG.Tweening;
using JsonFx.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTM_LobbyViewController : PTM_MB_Singleton<PTM_LobbyViewController>
{
	private GameObject _goContainer;

	private GameObject _goRoomView;

	private GameObject _goDeskView;

	private PTM_HeadViewController _headViewController;

	private PTM_UserViewer _userViewer;

	private Button _btnTrainningRoom;

	private Button _btnCompetitiveRoom;

	[SerializeField]
	private Sprite[] spiTrainingRooms;

	[SerializeField]
	private Sprite[] spiCompetitiveRooms;

	private GameObject _goDeskWidget;

	private GameObject _goNoDeskHint;

	[SerializeField]
	private Sprite[] spiNoDesks;

	private GameObject _goDesk;

	private List<PTM_Desk> _deskList = new List<PTM_Desk>();

	private int _curRoomType;

	private bool _bCanClick;

	public int curDeskIndex;

	public bool _IsAning;

	private Vector3 _posRoomLeft;

	private Vector3 _posRoomRight;

	private string _curState;

	private bool _inputForbidden;

	private PTM_DeskWidget[] _deskWidgets = new PTM_DeskWidget[3];

	private Vector3 _deskOffset = Vector3.right * 12.8f;

	private float tempTime;

	public bool HasDesk => _deskList.Count > 0;

	private void Awake()
	{
		InitFinGame();
		base.transform.localScale = Vector3.one;
		if (PTM_MB_Singleton<PTM_LobbyViewController>._instance == null)
		{
			PTM_MB_Singleton<PTM_LobbyViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void InitFinGame()
	{
		_goContainer = base.gameObject;
		_goRoomView = base.transform.Find("Rooms").gameObject;
		_goDeskView = base.transform.Find("Desks").gameObject;
		_headViewController = base.transform.Find("Title").GetComponent<PTM_HeadViewController>();
		_userViewer = base.transform.Find("PersonInfoDialog").GetComponent<PTM_UserViewer>();
		_btnTrainningRoom = _goRoomView.transform.Find("BtnRoom0").GetComponent<Button>();
		_btnCompetitiveRoom = _goRoomView.transform.Find("BtnRoom1").GetComponent<Button>();
		_goDeskWidget = _goDeskView.transform.Find("Desk").gameObject;
		_goNoDeskHint = base.transform.Find("NoDeskHint").gameObject;
		_goDesk = _goDeskView.transform.Find("Desk").gameObject;
	}

	private void Start()
	{
		_init();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !PTM_MB_Singleton<PTM_LobbyViewController>.GetInstance()._IsAning && !PTM_LockManager.IsLocked("Esc"))
		{
			UnityEngine.Debug.Log("ESC Btn Down In Lobby View");
			PTM_MB_Singleton<PTM_GameManager>.GetInstance().Handle_BtnReturn();
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
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("roomInfo", HandleNetMsg_RoomInfo);
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("enterDesk", HandleNetMsg_EnterDesk);
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("updateRoomInfo", HandleNetMsg_UpdateRoomInfo);
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("addExpeGoldAuto", HandleNetMsg_AddExpeGoldAuto);
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("expeGold", HandleNetMsg_ExpeGold);
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("playerInfo", HandleNetMsg_PlayerInfo);
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("updateDeskInfo", HandleNetMsg_UpdateDeskInfo);
		_goRoomView.SetActive(value: true);
		_goDeskView.SetActive(value: false);
		int num = (PTM_GVars.language == "en") ? 1 : 0;
		_goNoDeskHint.GetComponent<Image>().sprite = spiNoDesks[num];
		_goNoDeskHint.SetActive(value: false);
		_userViewer.gameObject.SetActive(value: false);
		_headViewController.Show();
		PTM_Drag_DeskController._instance.left = OnBtnLeftDesk_Click;
		PTM_Drag_DeskController._instance.right = OnBtnRightDesk_Click;
		_posRoomLeft = _btnTrainningRoom.transform.localPosition;
		_posRoomRight = _btnCompetitiveRoom.transform.localPosition;
		_btnTrainningRoom.GetComponent<Image>().sprite = spiTrainingRooms[num];
		_btnCompetitiveRoom.GetComponent<Image>().sprite = spiCompetitiveRooms[num];
		_prepareDesks();
		_curState = "RoomView";
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
		string empty = string.Empty;
		empty = ((!_goDeskView.activeSelf && !_goNoDeskHint.activeSelf) ? "RoomSelectionView" : "DeskSelectionView");
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().ChangeView(empty);
		_headViewController.UpdateView();
		_updateHeadInfo();
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	public void OnBtnRoom_Click(int roomType)
	{
		if (!PTM_LockManager.IsLocked("btn_room"))
		{
			PTM_SoundManager.Instance.PlayClickAudio();
			PTM_LockManager.Lock("btn_room");
			_curRoomType = roomType;
			Send_EnterRoom(roomType);
		}
	}

	public void OnBtnLeftDesk_Click()
	{
		if (!PTM_GVars.tryLockOnePoint && PTM_DeskPullDownListController.Instance.bMoveFinished)
		{
			_desksMoveToLeft();
		}
	}

	public void OnBtnRightDesk_Click()
	{
		if (!PTM_GVars.tryLockOnePoint && PTM_DeskPullDownListController.Instance.bMoveFinished)
		{
			_desksMoveToRight();
		}
	}

	public void OnBtnReturn_Click()
	{
		if (!PTM_GVars.tryLockOnePoint && _bCanClick)
		{
			_bCanClick = false;
			PTM_SoundManager.Instance.PlayClickAudio();
			PTM_MB_Singleton<PTM_RoomSelectionViewController>.GetInstance().Show();
			ResetRoomView();
			Send_LeaveRoom();
			_goNoDeskHint.SetActive(value: false);
		}
	}

	public void Send_EnterRoom(int roomType)
	{
		object[] args = new object[1]
		{
			roomType
		};
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().Send("userService/enterRoom", args);
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "Send_EnterRoom");
	}

	public void Send_EnterDesk(int deskId)
	{
		object[] args = new object[1]
		{
			deskId
		};
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().Send("userService/enterDesk", args);
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "Send_EnterDesk");
	}

	public void Send_LeaveRoom()
	{
		object[] args = new object[1]
		{
			_curRoomType
		};
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().Send("userService/leaveRoom", args);
	}

	public void Send_GetUserAward()
	{
		object[] args = new object[0];
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().Send("userService/getUserAward", args);
	}

	public void Send_AddExpeGoldAuto()
	{
		object[] args = new object[0];
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().Send("userService/addExpeGoldAuto", args);
	}

	public void Send_PlayerInfo(int userId)
	{
		object[] args = new object[1]
		{
			userId
		};
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().Send("userService/playerInfo", args);
	}

	public void HandleNetMsg_RoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(PTM_LogHelper.NetHandle("HandleNetMsg_RoomInfo"));
		if (PTM_GVars.curView == "MajorGame" || PTM_GVars.curView == "DiceGame")
		{
			UnityEngine.Debug.Log(PTM_LogHelper.Orange("游戏中，不处理"));
			return;
		}
		_curState = "DeskView";
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "Send_EnterRoom done");
		if (dictionary != null)
		{
			PTM_LockManager.UnLock("btn_room");
			UnityEngine.Debug.Log("isOverflow: " + dictionary["isOverflow"]);
			PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog((PTM_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please exchange");
			return;
		}
		object[] array = args[0] as object[];
		List<PTM_Desk> list = new List<PTM_Desk>();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> data = array[i] as Dictionary<string, object>;
			list.Add(PTM_Desk.CreateWithDic(data));
		}
		_deskList = list;
		UnityEngine.Debug.Log("HandleNetMsg_RoomInfo> desks.length: " + _deskList.Count);
		_updateHeadInfo();
		_hideRoomViewAni();
		StartCoroutine(_showDeskViewAni());
	}

	public void HandleNetMsg_EnterDesk(object[] args)
	{
		UnityEngine.Debug.Log(PTM_LogHelper.NetHandle("HandleNetMsg_EnterDesk"));
		PTM_LockManager.UnLock("EnterDesk");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["success"];
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "Send_EnterDesk done");
		if (flag)
		{
			Hide();
			PTM_GVars.desk = _deskList[curDeskIndex];
			PTM_MB_Singleton<PTM_MajorGameController>.GetInstance().Show();
			PTM_MB_Singleton<PTM_MajorGameController>.GetInstance().InitGame();
			PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().SetKeepScore(0);
			PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().SetRate(PTM_GVars.desk.exchange);
			return;
		}
		switch ((int)dictionary["messageStatus"])
		{
		case 0:
			PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog((PTM_GVars.language == "zh") ? "桌子不存在" : "Game table not exist");
			break;
		case 1:
			PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog((PTM_GVars.language == "zh") ? "桌已满" : "Don't Have Free Table");
			break;
		case 2:
			PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog((PTM_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge");
			break;
		case 3:
			PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog((PTM_GVars.language == "zh") ? "体验币不足" : "Experience Coins insufficient");
			break;
		case 4:
			PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog((PTM_GVars.language == "zh") ? "游戏币不足" : "Coins insufficient");
			break;
		default:
			UnityEngine.Debug.Log(string.Empty);
			break;
		}
	}

	public void HandleNetMsg_UpdateRoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(PTM_LogHelper.NetHandle("HandleNetMsg_UpdateRoomInfo"));
		if (PTM_GVars.curView == "LoadingView")
		{
			return;
		}
		object[] array = args[0] as object[];
		List<PTM_Desk> list = new List<PTM_Desk>();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> data = array[i] as Dictionary<string, object>;
			list.Add(PTM_Desk.CreateWithDic(data));
		}
		if (array.Length == 0)
		{
			_goDeskView.SetActive(value: false);
			_goNoDeskHint.SetActive(value: true);
			curDeskIndex = 0;
			_deskList.Clear();
			return;
		}
		if (_deskList.Count == 0)
		{
			_goDeskView.SetActive(value: true);
			_goNoDeskHint.SetActive(value: false);
			curDeskIndex = 0;
		}
		int count = _deskList.Count;
		_deskList = list;
		if (count != list.Count)
		{
			UpdateAllDeskWidgets();
			PTM_DeskPullDownListController.Instance.UpdateDeskInList(_deskList);
		}
		else
		{
			UpdateAllDeskWidgets();
			PTM_DeskPullDownListController.Instance.UpdateDeskInList(_deskList);
		}
	}

	public void HandleNetMsg_AddExpeGoldAuto(object[] args)
	{
		UnityEngine.Debug.Log(JsonWriter.Serialize(args));
		PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog("申请成功！请返回大厅重新游戏", showOkCancel: false, delegate
		{
			PTM_MB_Singleton<PTM_GameManager>.GetInstance().QuitToHallGame();
		});
	}

	public void HandleNetMsg_ExpeGold(object[] args)
	{
		UnityEngine.Debug.Log(PTM_LogHelper.NetHandle("HandleNetMsg_ExpeGold"));
		if ((bool)args[0])
		{
			PTM_GVars.user.expeGold = 10000;
			_headViewController.UpdateExpeAndGold();
		}
		else
		{
			UnityEngine.Debug.LogError("ExpeGold> should not be false");
		}
	}

	public void HandleNetMsg_PlayerInfo(object[] args)
	{
		UnityEngine.Debug.Log(PTM_LogHelper.NetHandle("HandleNetMsg_PlayerInfo"));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int[] honor = (int[])dictionary["honor"];
		Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
		string nickName = dictionary2["nickname"] as string;
		int level = (int)dictionary2["level"];
		int score = (_curRoomType == 1) ? ((int)dictionary2["expeScore"]) : ((int)dictionary2["gameScore"]);
		int photoId = (int)dictionary2["photoId"];
		_userViewer.ShowViewer(nickName, level, score, photoId, honor);
	}

	public void HandleNetMsg_UpdateDeskInfo(object[] args)
	{
		UnityEngine.Debug.Log(PTM_LogHelper.NetHandle("HandleNetMsg_UpdateDeskInfo"));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		Dictionary<string, object> dictionary2 = dictionary["desk"] as Dictionary<string, object>;
		int num = (int)dictionary["kind"];
	}

	private IEnumerator _showDeskViewAni()
	{
		yield return new WaitForSeconds(0.5f);
		if (_deskList.Count > 0)
		{
			_goDeskView.SetActive(value: true);
			if (_deskWidgets[2] == null)
			{
				_goDesk.transform.localScale = Vector3.one * 0.4f;
				Sequence sequence = DOTween.Sequence();
				sequence.Join(_goDesk.transform.DOScale(Vector3.one, 0.5f));
				sequence.onComplete = delegate
				{
					_prepareDesks();
					curDeskIndex = 0;
					UpdateAllDeskWidgets();
				};
			}
			else
			{
				for (int i = 0; i < _deskWidgets.Length; i++)
				{
					_deskWidgets[i].transform.localScale = Vector3.one * 0.4f;
					Sequence sequence2 = DOTween.Sequence();
					sequence2.Join(_deskWidgets[i].transform.DOScale(Vector3.one, 0.5f));
					sequence2.onComplete = delegate
					{
						_prepareDesks();
						curDeskIndex = 0;
						UpdateAllDeskWidgets();
					};
				}
			}
		}
		else
		{
			_goDeskView.SetActive(value: false);
			_goNoDeskHint.SetActive(value: true);
			curDeskIndex = 0;
		}
		PTM_GVars.curView = "DeskSelectionView";
		PTM_LockManager.UnLock("btn_room");
		yield return null;
	}

	private PTM_DeskWidget _createDeskWidget()
	{
		UnityEngine.Debug.Log("_createDeskWidget");
		GameObject gameObject = Object.Instantiate(_goDeskWidget, _goDeskWidget.transform.position, _goDeskWidget.transform.rotation);
		gameObject.transform.SetParent(_goDeskWidget.transform.parent);
		gameObject.transform.localScale = _goDeskWidget.transform.localScale;
		PTM_DeskWidget component = gameObject.GetComponent<PTM_DeskWidget>();
		component.InitPlayerInDesk(string.Empty, string.Empty);
		return component;
	}

	private void _prepareDesks()
	{
		if (!(_deskWidgets[1] != null))
		{
			_deskWidgets[1] = _createDeskWidget();
			_deskWidgets[0] = _createDeskWidget();
			_deskWidgets[0].transform.Translate(-_deskOffset);
			_deskWidgets[2] = _createDeskWidget();
			_deskWidgets[2].transform.Translate(_deskOffset);
			_deskWidgets[0].onDeskClickAction = _onBtnDesk_Click;
			_deskWidgets[1].onDeskClickAction = _onBtnDesk_Click;
			_deskWidgets[2].onDeskClickAction = _onBtnDesk_Click;
			_updateDeskWidgetsName();
			_goDeskWidget.SetActive(value: false);
		}
	}

	private void _desksMoveToLeft()
	{
		PTM_DeskPullDownListController.Instance.bMoveFinished = false;
		PTM_SoundManager.Instance.PlayClickAudio();
		if (_deskList.Count != 0 && !_inputForbidden)
		{
			PTM_DeskPullDownListController.Instance.ShowListButtonHighBG(_bIsLeft: true, curDeskIndex);
			_deskWidgets[2].transform.position = _deskWidgets[0].transform.position;
			Sequence sequence = DOTween.Sequence();
			Tweener t = _deskWidgets[1].transform.DOMoveX(_deskOffset.x, 0.45f);
			sequence.Join(_deskWidgets[0].transform.DOMoveX(0f, 0.45f));
			sequence.Join(t);
			_inputForbidden = true;
			sequence.onComplete = delegate
			{
				_swapDesks(1, 0);
				_swapDesks(0, 2);
				curDeskIndex = (curDeskIndex + _deskList.Count - 1) % _deskList.Count;
				_updateSideDeskWidgets();
				_inputForbidden = false;
				PTM_DeskPullDownListController.Instance.bMoveFinished = true;
				PTM_DeskPointsController._Instance.ShowDeskPoint2(_deskList.Count, curDeskIndex);
			};
		}
	}

	private void _desksMoveToRight()
	{
		PTM_DeskPullDownListController.Instance.bMoveFinished = false;
		PTM_SoundManager.Instance.PlayClickAudio();
		if (_deskList.Count != 0 && !_inputForbidden)
		{
			PTM_DeskPullDownListController.Instance.ShowListButtonHighBG(_bIsLeft: false, curDeskIndex);
			_deskWidgets[0].transform.position = _deskWidgets[2].transform.position;
			Sequence sequence = DOTween.Sequence();
			Tweener t = _deskWidgets[1].transform.DOMoveX(0f - _deskOffset.x, 0.45f);
			sequence.Join(_deskWidgets[2].transform.DOMoveX(0f, 0.45f));
			sequence.Join(t);
			_inputForbidden = true;
			sequence.onComplete = delegate
			{
				_swapDesks(1, 2);
				_swapDesks(2, 0);
				curDeskIndex = (curDeskIndex + 1) % _deskList.Count;
				_updateSideDeskWidgets();
				_inputForbidden = false;
				PTM_DeskPullDownListController.Instance.bMoveFinished = true;
				PTM_DeskPointsController._Instance.ShowDeskPoint2(_deskList.Count, curDeskIndex);
			};
		}
	}

	public void ForceMoveDesksToLeft(int newIndex)
	{
		PTM_DeskPullDownListController.Instance.bMoveFinished = false;
		PTM_SoundManager.Instance.PlayClickAudio();
		curDeskIndex = newIndex;
		_deskWidgets[0].InitDesk(_deskList[curDeskIndex]);
		if (!_inputForbidden)
		{
			_deskWidgets[2].transform.position = _deskWidgets[0].transform.position;
			Sequence sequence = DOTween.Sequence();
			sequence.Join(_deskWidgets[1].transform.DOMoveX(_deskOffset.x, 0.52f));
			sequence.Join(_deskWidgets[0].transform.DOMoveX(0f, 0.52f));
			_inputForbidden = true;
			sequence.onComplete = delegate
			{
				_swapDesks(1, 0);
				_swapDesks(0, 2);
				_updateSideDeskWidgets();
				_inputForbidden = false;
				PTM_DeskPullDownListController.Instance.bMoveFinished = true;
				PTM_DeskPointsController._Instance.ShowDeskPoint2(_deskList.Count, curDeskIndex);
			};
			StartCoroutine(PTM_Utils.DelayCall(0.5f, delegate
			{
			}));
		}
	}

	public void ForceMoveDesksToRight(int newIndex)
	{
		PTM_DeskPullDownListController.Instance.bMoveFinished = false;
		PTM_SoundManager.Instance.PlayClickAudio();
		curDeskIndex = newIndex;
		_deskWidgets[2].InitDesk(_deskList[curDeskIndex]);
		if (!_inputForbidden)
		{
			_deskWidgets[0].transform.position = _deskWidgets[2].transform.position;
			Sequence sequence = DOTween.Sequence();
			Tweener t = _deskWidgets[1].transform.DOMoveX(0f - _deskOffset.x, 0.52f);
			sequence.Join(_deskWidgets[2].transform.DOMoveX(0f, 0.52f));
			sequence.Join(t);
			_inputForbidden = true;
			sequence.onComplete = delegate
			{
				_swapDesks(1, 2);
				_swapDesks(2, 0);
				_updateSideDeskWidgets();
				_inputForbidden = false;
				PTM_DeskPullDownListController.Instance.bMoveFinished = true;
				PTM_DeskPointsController._Instance.ShowDeskPoint2(_deskList.Count, curDeskIndex);
			};
		}
	}

	private void _swapDesks(int a, int b)
	{
		PTM_DeskWidget pTM_DeskWidget = _deskWidgets[a];
		_deskWidgets[a] = _deskWidgets[b];
		_deskWidgets[b] = pTM_DeskWidget;
	}

	private void _updateSideDeskWidgets()
	{
		int count = _deskList.Count;
		_deskWidgets[0].InitDesk(_deskList[(curDeskIndex + count - 1) % count]);
		_deskWidgets[2].InitDesk(_deskList[(curDeskIndex + 1) % count]);
		_updateDeskWidgetsName();
	}

	private void _updateCurrentDeskWidget()
	{
		_deskWidgets[1].InitDesk(_deskList[curDeskIndex]);
	}

	private void _updateDeskWidgetsName()
	{
		_deskWidgets[0].gameObject.name = "desk left";
		_deskWidgets[1].gameObject.name = "desk center";
		_deskWidgets[2].gameObject.name = "desk right";
	}

	public void UpdateAllDeskWidgets()
	{
		int count = _deskList.Count;
		if (count != 0 && count > 0)
		{
			if (curDeskIndex >= count)
			{
				UnityEngine.Debug.Log($"_curDeskIndex: {curDeskIndex}, deskCount: {count}");
				curDeskIndex = 0;
			}
			_prepareDesks();
			_deskWidgets[1].InitDesk(_deskList[curDeskIndex]);
			_deskWidgets[0].InitDesk(_deskList[(curDeskIndex + count - 1) % count]);
			_deskWidgets[2].InitDesk(_deskList[(curDeskIndex + 1) % count]);
		}
		PTM_DeskPointsController._Instance.ShowDeskPoint2(_deskList.Count, curDeskIndex);
		PTM_DeskPullDownListController.Instance.ChangeListHeightByDeskNum(_deskList.Count, _deskList);
	}

	private void _onBtnDesk_Click()
	{
		PTM_SoundManager.Instance.PlayClickAudio();
		UnityEngine.Debug.Log("_onBtnDesk_Click");
		if (_userViewer.IsShow())
		{
			return;
		}
		if (_deskList[curDeskIndex].userId > 0)
		{
			Send_PlayerInfo(_deskList[curDeskIndex].userId);
		}
		else if (_curRoomType == 1)
		{
			if (PTM_GVars.user.expeGold == 0 || PTM_GVars.user.expeGold < _deskList[curDeskIndex].minGold)
			{
				UnityEngine.Debug.Log($"expeGold: {PTM_GVars.user.expeGold}, _curDeskIndex: {curDeskIndex}, minGold: {_deskList[curDeskIndex].minGold}");
				PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog("体验币不足，是否领取体验币？", showOkCancel: true, delegate
				{
					Send_AddExpeGoldAuto();
				});
			}
			else
			{
				PTM_LockManager.Lock("EnterDesk");
				Send_EnterDesk(_deskList[curDeskIndex].id);
				PTM_SoundManager.Instance.StopLobbyBGM();
			}
		}
		else if (PTM_GVars.user.gameGold == 0 || PTM_GVars.user.gameGold < _deskList[curDeskIndex].minGold)
		{
			UnityEngine.Debug.LogError($"gameGold: {PTM_GVars.user.gameGold}, _curDeskIndex: {curDeskIndex}, minGold: {_deskList[curDeskIndex].minGold}");
			PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog("游戏币不足！");
		}
		else
		{
			PTM_LockManager.Lock("EnterDesk");
			Send_EnterDesk(_deskList[curDeskIndex].id);
			PTM_SoundManager.Instance.StopLobbyBGM();
		}
	}

	private void _updateHeadInfo()
	{
		string info = (PTM_GVars.language == "zh") ? "选择房间" : "SelectRoom";
		if (_curState == "DeskView")
		{
			if (_curRoomType == 1)
			{
				info = ((PTM_GVars.language == "zh") ? "新手练习厅" : "Training");
			}
			else if (_curRoomType == 2)
			{
				info = ((PTM_GVars.language == "zh") ? "欢乐竞技厅" : "Arena");
			}
		}
		_headViewController.SetInfo(info);
	}

	private void _hideRoomViewAni()
	{
		_IsAning = true;
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "_hideRoomViewAni");
		Sequence sequence = DOTween.Sequence();
		sequence.Join(_btnTrainningRoom.gameObject.transform.DOLocalMoveX(-935f, 1.5f));
		sequence.Join(_btnCompetitiveRoom.gameObject.transform.DOLocalMoveX(935f, 1.5f));
		Color white = Color.white;
		white.a = 0f;
		sequence.Join(_btnTrainningRoom.image.DOColor(white, 1f));
		sequence.Join(_btnCompetitiveRoom.image.DOColor(white, 1f));
		sequence.onComplete = delegate
		{
			PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "_hideRoomViewAni done");
			_IsAning = false;
			_bCanClick = true;
		};
	}

	public void ResetRoomView()
	{
		UnityEngine.Debug.Log("ResetRoomView");
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "ResetRoomView");
		_userViewer.gameObject.SetActive(value: false);
		Sequence sequence = DOTween.Sequence();
		sequence.Join(_btnTrainningRoom.gameObject.transform.DOLocalMove(_posRoomLeft, 0.6f));
		sequence.Join(_btnCompetitiveRoom.gameObject.transform.DOLocalMove(_posRoomRight, 0.6f));
		sequence.Join(_btnTrainningRoom.image.DOColor(Color.white, 0.8f));
		sequence.Join(_btnCompetitiveRoom.image.DOColor(Color.white, 0.8f));
		sequence.onComplete = delegate
		{
			_curState = "RoomView";
			_bCanClick = false;
			_updateHeadInfo();
			PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "ResetRoomView_Over");
		};
	}

	public void HideDeskViewAni()
	{
		Sequence sequence = DOTween.Sequence();
		for (int i = 0; i < _deskWidgets.Length; i++)
		{
			sequence.Join(_deskWidgets[i].transform.DOScale(new Vector3(0.2f, 0.2f, 1f), 0.6f));
		}
		sequence.onComplete = delegate
		{
			PTM_MB_Singleton<PTM_DeskSelectionViewController>.GetInstance().Hide();
		};
	}
}
