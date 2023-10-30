using DG.Tweening;
using JsonFx.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSE_LobbyViewController : MSE_MB_Singleton<MSE_LobbyViewController>
{
	private GameObject _goContainer;

	private GameObject _goRoomView;

	private GameObject _goDeskView;

	private MSE_HeadViewController _headViewController;

	private MSE_UserViewer _userViewer;

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

	private List<MSE_Desk> _deskList = new List<MSE_Desk>();

	private int _curRoomType;

	private bool _bCanClick;

	public int curDeskIndex;

	public bool _IsAning;

	private Vector3 _posRoomLeft;

	private Vector3 _posRoomRight;

	private string _curState;

	private bool _inputForbidden;

	private MSE_DeskWidget[] _deskWidgets = new MSE_DeskWidget[3];

	private Vector3 _deskOffset = Vector3.right * 12.8f;

	private float tempTime;

	public bool HasDesk => _deskList.Count > 0;

	private void Awake()
	{
		InitFinGame();
		base.transform.localScale = Vector3.one;
		if (MSE_MB_Singleton<MSE_LobbyViewController>._instance == null)
		{
			MSE_MB_Singleton<MSE_LobbyViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void InitFinGame()
	{
		_goContainer = base.gameObject;
		_goRoomView = base.transform.Find("Rooms").gameObject;
		_goDeskView = base.transform.Find("Desks").gameObject;
		_headViewController = base.transform.Find("Title").GetComponent<MSE_HeadViewController>();
		_userViewer = base.transform.Find("PersonInfoDialog").GetComponent<MSE_UserViewer>();
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
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !MSE_MB_Singleton<MSE_LobbyViewController>.GetInstance()._IsAning && !MSE_LockManager.IsLocked("Esc"))
		{
			UnityEngine.Debug.Log("ESC Btn Down In Lobby View");
			MSE_MB_Singleton<MSE_GameManager>.GetInstance().Handle_BtnReturn();
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
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().RegisterHandler("roomInfo", HandleNetMsg_RoomInfo);
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().RegisterHandler("enterDesk", HandleNetMsg_EnterDesk);
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().RegisterHandler("updateRoomInfo", HandleNetMsg_UpdateRoomInfo);
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().RegisterHandler("addExpeGoldAuto", HandleNetMsg_AddExpeGoldAuto);
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().RegisterHandler("expeGold", HandleNetMsg_ExpeGold);
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().RegisterHandler("playerInfo", HandleNetMsg_PlayerInfo);
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().RegisterHandler("updateDeskInfo", HandleNetMsg_UpdateDeskInfo);
		_goRoomView.SetActive(value: true);
		_goDeskView.SetActive(value: false);
		int num = (MSE_GVars.language == "en") ? 1 : 0;
		_goNoDeskHint.GetComponent<Image>().sprite = spiNoDesks[num];
		_goNoDeskHint.SetActive(value: false);
		_userViewer.gameObject.SetActive(value: false);
		_headViewController.Show();
		MSE_Drag_DeskController._instance.left = OnBtnLeftDesk_Click;
		MSE_Drag_DeskController._instance.right = OnBtnRightDesk_Click;
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
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().ChangeView(empty);
		_headViewController.UpdateView();
		_updateHeadInfo();
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	public void OnBtnRoom_Click(int roomType)
	{
		if (!MSE_LockManager.IsLocked("btn_room"))
		{
			MSE_SoundManager.Instance.PlayClickAudio();
			MSE_LockManager.Lock("btn_room");
			_curRoomType = roomType;
			Send_EnterRoom(roomType);
		}
	}

	public void OnBtnLeftDesk_Click()
	{
		if (!MSE_GVars.tryLockOnePoint && MSE_DeskPullDownListController.Instance.bMoveFinished)
		{
			_desksMoveToLeft();
		}
	}

	public void OnBtnRightDesk_Click()
	{
		if (!MSE_GVars.tryLockOnePoint && MSE_DeskPullDownListController.Instance.bMoveFinished)
		{
			_desksMoveToRight();
		}
	}

	public void OnBtnReturn_Click()
	{
		if (!MSE_GVars.tryLockOnePoint && _bCanClick)
		{
			_bCanClick = false;
			MSE_SoundManager.Instance.PlayClickAudio();
			MSE_MB_Singleton<MSE_RoomSelectionViewController>.GetInstance().Show();
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
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().Send("userService/enterRoom", args);
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "Send_EnterRoom");
	}

	public void Send_EnterDesk(int deskId)
	{
		object[] args = new object[1]
		{
			deskId
		};
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().Send("userService/enterDesk", args);
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "Send_EnterDesk");
	}

	public void Send_LeaveRoom()
	{
		object[] args = new object[1]
		{
			_curRoomType
		};
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().Send("userService/leaveRoom", args);
	}

	public void Send_GetUserAward()
	{
		object[] args = new object[0];
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().Send("userService/getUserAward", args);
	}

	public void Send_AddExpeGoldAuto()
	{
		object[] args = new object[0];
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().Send("userService/addExpeGoldAuto", args);
	}

	public void Send_PlayerInfo(int userId)
	{
		object[] args = new object[1]
		{
			userId
		};
		MSE_MB_Singleton<MSE_NetManager>.GetInstance().Send("userService/playerInfo", args);
	}

	public void HandleNetMsg_RoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(MSE_LogHelper.NetHandle("HandleNetMsg_RoomInfo"));
		if (MSE_GVars.curView == "MajorGame" || MSE_GVars.curView == "DiceGame")
		{
			UnityEngine.Debug.Log(MSE_LogHelper.Orange("游戏中，不处理"));
			return;
		}
		_curState = "DeskView";
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "Send_EnterRoom done");
		if (dictionary != null)
		{
			MSE_LockManager.UnLock("btn_room");
			UnityEngine.Debug.Log("isOverflow: " + dictionary["isOverflow"]);
			MSE_MB_Singleton<MSE_AlertDialog>.GetInstance().ShowDialog((MSE_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please exchange");
			return;
		}
		object[] array = args[0] as object[];
		List<MSE_Desk> list = new List<MSE_Desk>();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> data = array[i] as Dictionary<string, object>;
			list.Add(MSE_Desk.CreateWithDic(data));
		}
		_deskList = list;
		UnityEngine.Debug.Log("HandleNetMsg_RoomInfo> desks.length: " + _deskList.Count);
		_updateHeadInfo();
		_hideRoomViewAni();
		StartCoroutine(_showDeskViewAni());
	}

	public void HandleNetMsg_EnterDesk(object[] args)
	{
		UnityEngine.Debug.Log(MSE_LogHelper.NetHandle("HandleNetMsg_EnterDesk"));
		MSE_LockManager.UnLock("EnterDesk");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["success"];
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "Send_EnterDesk done");
		UnityEngine.Debug.LogError("flag: " + flag);
		if (flag)
		{
			Hide();
			UnityEngine.Debug.LogError("curDeskIndex: " + curDeskIndex);
			MSE_GVars.desk = _deskList[curDeskIndex];
			MSE_MB_Singleton<MSE_MajorGameController>.GetInstance().Show();
			MSE_MB_Singleton<MSE_MajorGameController>.GetInstance().InitGame();
			MSE_MB_Singleton<MSE_ScoreBank>.GetInstance().SetKeepScore(0);
			MSE_MB_Singleton<MSE_ScoreBank>.GetInstance().SetRate(MSE_GVars.desk.exchange);
			return;
		}
		switch ((int)dictionary["messageStatus"])
		{
		case 0:
			MSE_MB_Singleton<MSE_AlertDialog>.GetInstance().ShowDialog((MSE_GVars.language == "zh") ? "桌子不存在" : "Game table not exist");
			break;
		case 1:
			MSE_MB_Singleton<MSE_AlertDialog>.GetInstance().ShowDialog((MSE_GVars.language == "zh") ? "桌已满" : "Don't Have Free Table");
			break;
		case 2:
			MSE_MB_Singleton<MSE_AlertDialog>.GetInstance().ShowDialog((MSE_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge");
			break;
		case 3:
			MSE_MB_Singleton<MSE_AlertDialog>.GetInstance().ShowDialog((MSE_GVars.language == "zh") ? "体验币不足" : "Experience Coins insufficient");
			break;
		case 4:
			MSE_MB_Singleton<MSE_AlertDialog>.GetInstance().ShowDialog((MSE_GVars.language == "zh") ? "游戏币不足" : "Coins insufficient");
			break;
		default:
			UnityEngine.Debug.Log(string.Empty);
			break;
		}
	}

	public void HandleNetMsg_UpdateRoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(MSE_LogHelper.NetHandle("HandleNetMsg_UpdateRoomInfo"));
		if (MSE_GVars.curView == "LoadingView")
		{
			return;
		}
		object[] array = args[0] as object[];
		List<MSE_Desk> list = new List<MSE_Desk>();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> data = array[i] as Dictionary<string, object>;
			list.Add(MSE_Desk.CreateWithDic(data));
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
			MSE_DeskPullDownListController.Instance.UpdateDeskInList(_deskList);
		}
		else
		{
			UpdateAllDeskWidgets();
			MSE_DeskPullDownListController.Instance.UpdateDeskInList(_deskList);
		}
	}

	public void HandleNetMsg_AddExpeGoldAuto(object[] args)
	{
		UnityEngine.Debug.Log(JsonWriter.Serialize(args));
		MSE_MB_Singleton<MSE_AlertDialog>.GetInstance().ShowDialog("申请成功！请返回大厅重新游戏", showOkCancel: false, delegate
		{
			MSE_MB_Singleton<MSE_GameManager>.GetInstance().QuitToHallGame();
		});
	}

	public void HandleNetMsg_ExpeGold(object[] args)
	{
		UnityEngine.Debug.Log(MSE_LogHelper.NetHandle("HandleNetMsg_ExpeGold"));
		if ((bool)args[0])
		{
			MSE_GVars.user.expeGold = 10000;
			_headViewController.UpdateExpeAndGold();
		}
		else
		{
			UnityEngine.Debug.LogError("ExpeGold> should not be false");
		}
	}

	public void HandleNetMsg_PlayerInfo(object[] args)
	{
		UnityEngine.Debug.Log(MSE_LogHelper.NetHandle("HandleNetMsg_PlayerInfo"));
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
		UnityEngine.Debug.Log(MSE_LogHelper.NetHandle("HandleNetMsg_UpdateDeskInfo"));
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
		MSE_GVars.curView = "DeskSelectionView";
		MSE_LockManager.UnLock("btn_room");
		yield return null;
	}

	private MSE_DeskWidget _createDeskWidget()
	{
		UnityEngine.Debug.Log("_createDeskWidget");
		GameObject gameObject = Object.Instantiate(_goDeskWidget, _goDeskWidget.transform.position, _goDeskWidget.transform.rotation);
		gameObject.transform.SetParent(_goDeskWidget.transform.parent);
		gameObject.transform.localScale = _goDeskWidget.transform.localScale;
		MSE_DeskWidget component = gameObject.GetComponent<MSE_DeskWidget>();
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
		MSE_DeskPullDownListController.Instance.bMoveFinished = false;
		MSE_SoundManager.Instance.PlayClickAudio();
		if (_deskList.Count != 0 && !_inputForbidden)
		{
			MSE_DeskPullDownListController.Instance.ShowListButtonHighBG(_bIsLeft: true, curDeskIndex);
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
				MSE_DeskPullDownListController.Instance.bMoveFinished = true;
				MSE_DeskPointsController._Instance.ShowDeskPoint2(_deskList.Count, curDeskIndex);
			};
		}
	}

	private void _desksMoveToRight()
	{
		MSE_DeskPullDownListController.Instance.bMoveFinished = false;
		MSE_SoundManager.Instance.PlayClickAudio();
		if (_deskList.Count != 0 && !_inputForbidden)
		{
			MSE_DeskPullDownListController.Instance.ShowListButtonHighBG(_bIsLeft: false, curDeskIndex);
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
				MSE_DeskPullDownListController.Instance.bMoveFinished = true;
				MSE_DeskPointsController._Instance.ShowDeskPoint2(_deskList.Count, curDeskIndex);
			};
		}
	}

	public void ForceMoveDesksToLeft(int newIndex)
	{
		MSE_DeskPullDownListController.Instance.bMoveFinished = false;
		MSE_SoundManager.Instance.PlayClickAudio();
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
				MSE_DeskPullDownListController.Instance.bMoveFinished = true;
				MSE_DeskPointsController._Instance.ShowDeskPoint2(_deskList.Count, curDeskIndex);
			};
			StartCoroutine(MSE_Utils.DelayCall(0.5f, delegate
			{
			}));
		}
	}

	public void ForceMoveDesksToRight(int newIndex)
	{
		MSE_DeskPullDownListController.Instance.bMoveFinished = false;
		MSE_SoundManager.Instance.PlayClickAudio();
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
				MSE_DeskPullDownListController.Instance.bMoveFinished = true;
				MSE_DeskPointsController._Instance.ShowDeskPoint2(_deskList.Count, curDeskIndex);
			};
		}
	}

	private void _swapDesks(int a, int b)
	{
		MSE_DeskWidget mSE_DeskWidget = _deskWidgets[a];
		_deskWidgets[a] = _deskWidgets[b];
		_deskWidgets[b] = mSE_DeskWidget;
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
		MSE_DeskPointsController._Instance.ShowDeskPoint2(_deskList.Count, curDeskIndex);
		MSE_DeskPullDownListController.Instance.ChangeListHeightByDeskNum(_deskList.Count, _deskList);
	}

	private void _onBtnDesk_Click()
	{
		MSE_SoundManager.Instance.PlayClickAudio();
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
			if (MSE_GVars.user.expeGold == 0 || MSE_GVars.user.expeGold < _deskList[curDeskIndex].minGold)
			{
				UnityEngine.Debug.Log($"expeGold: {MSE_GVars.user.expeGold}, _curDeskIndex: {curDeskIndex}, minGold: {_deskList[curDeskIndex].minGold}");
				MSE_MB_Singleton<MSE_AlertDialog>.GetInstance().ShowDialog("体验币不足，是否领取体验币？", showOkCancel: true, delegate
				{
					Send_AddExpeGoldAuto();
				});
			}
			else
			{
				MSE_LockManager.Lock("EnterDesk");
				Send_EnterDesk(_deskList[curDeskIndex].id);
				MSE_SoundManager.Instance.StopLobbyBGM();
			}
		}
		else if (MSE_GVars.user.gameGold == 0 || MSE_GVars.user.gameGold < _deskList[curDeskIndex].minGold)
		{
			UnityEngine.Debug.LogError($"gameGold: {MSE_GVars.user.gameGold}, _curDeskIndex: {curDeskIndex}, minGold: {_deskList[curDeskIndex].minGold}");
			MSE_MB_Singleton<MSE_AlertDialog>.GetInstance().ShowDialog("游戏币不足！");
		}
		else
		{
			MSE_LockManager.Lock("EnterDesk");
			Send_EnterDesk(_deskList[curDeskIndex].id);
			MSE_SoundManager.Instance.StopLobbyBGM();
		}
	}

	private void _updateHeadInfo()
	{
		string info = (MSE_GVars.language == "zh") ? "选择房间" : "SelectRoom";
		if (_curState == "DeskView")
		{
			if (_curRoomType == 1)
			{
				info = ((MSE_GVars.language == "zh") ? "新手练习厅" : "Training");
			}
			else if (_curRoomType == 2)
			{
				info = ((MSE_GVars.language == "zh") ? "欢乐竞技厅" : "Arena");
			}
		}
		_headViewController.SetInfo(info);
	}

	private void _hideRoomViewAni()
	{
		_IsAning = true;
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "_hideRoomViewAni");
		Sequence sequence = DOTween.Sequence();
		sequence.Join(_btnTrainningRoom.gameObject.transform.DOLocalMoveX(-935f, 1.5f));
		sequence.Join(_btnCompetitiveRoom.gameObject.transform.DOLocalMoveX(935f, 1.5f));
		Color white = Color.white;
		white.a = 0f;
		sequence.Join(_btnTrainningRoom.image.DOColor(white, 1f));
		sequence.Join(_btnCompetitiveRoom.image.DOColor(white, 1f));
		sequence.onComplete = delegate
		{
			MSE_MB_Singleton<MSE_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "_hideRoomViewAni done");
			_IsAning = false;
			_bCanClick = true;
		};
	}

	public void ResetRoomView()
	{
		UnityEngine.Debug.Log("ResetRoomView");
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "ResetRoomView");
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
			MSE_MB_Singleton<MSE_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "ResetRoomView_Over");
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
			MSE_MB_Singleton<MSE_DeskSelectionViewController>.GetInstance().Hide();
		};
	}
}
