using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DCDF_LobbyViewController : DCDF_MB_Singleton<DCDF_LobbyViewController>
{
	[SerializeField]
	private GameObject _goContainer;

	private int _curRoomType;

	private string _curState;

	private Text txtCoin;

	private TextMesh[,] txtPrizePools = new TextMesh[5, 4];

	private Button btnBack;

	private Button[] btnRooms = new Button[5];

	private List<DCDF_Desk> deskList = new List<DCDF_Desk>();

	private GameObject objLobby3D;

	private void Awake()
	{
		if (DCDF_MB_Singleton<DCDF_LobbyViewController>._instance == null)
		{
			DCDF_MB_Singleton<DCDF_LobbyViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		_init();
	}

	public void PreInit()
	{
		if (_goContainer == null)
		{
			_goContainer = base.gameObject;
		}
		btnBack = base.transform.Find("BtnBack").GetComponent<Button>();
		btnBack.onClick.AddListener(ClickBtnBack);
		txtCoin = base.transform.Find("TxtCoin").GetComponent<Text>();
		objLobby3D = GameObject.Find("Lobby3D");
		objLobby3D.SetActive(value: false);
		for (int i = 0; i < 5; i++)
		{
			btnRooms[i] = base.transform.Find("BtnRooms").GetChild(i).GetComponent<Button>();
			int index = i;
			btnRooms[index].onClick.AddListener(delegate
			{
				ClickBtnRoom(index);
			});
			Transform transform = objLobby3D.transform.Find($"PrizePool{i}");
			for (int j = 0; j < 4; j++)
			{
				txtPrizePools[i, j] = transform.Find($"Txt{j + 6}").GetComponent<TextMesh>();
			}
		}
	}

	private void _init()
	{
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("roomInfo", HandleNetMsg_RoomInfo);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("enterRoom", HandleNetMsg_EnterRoom);
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
		objLobby3D.SetActive(value: true);
		string empty = string.Empty;
		empty = "RoomSelectionView";
		DCDF_MB_Singleton<DCDF_GameManager>.GetInstance().ChangeView(empty);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
		objLobby3D.SetActive(value: false);
	}

	public void ClickBtnRoom(int roomType)
	{
		DCDF_SoundManager.Instance.PlayClickAudio();
		_curRoomType = roomType;
		Send_EnterRoom(roomType);
	}

	public void ClickBtnBack()
	{
		if (!DCDF_MySqlConnection.tryLockOnePoint)
		{
			DCDF_SoundManager.Instance.PlayClickAudio();
			DCDF_MB_Singleton<DCDF_GameManager>.GetInstance().QuitToHallGame();
		}
	}

	public void Send_EnterRoom(int roomType)
	{
		object[] args = new object[1]
		{
			roomType
		};
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().Send("userService/enterRoom", args);
	}

	public void HandleNetMsg_RoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_RoomInfo"));
		if (DCDF_MySqlConnection.curView == "MajorGame")
		{
			UnityEngine.Debug.Log(DCDF_LogHelper.Orange("游戏中，不处理"));
			return;
		}
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if (dictionary != null)
		{
			UnityEngine.Debug.Log("isOverflow: " + dictionary["isOverflow"]);
			DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog((DCDF_MySqlConnection.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please exchange");
			return;
		}
		object[] array = args[0] as object[];
		List<DCDF_Desk> list = new List<DCDF_Desk>();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> data = array[i] as Dictionary<string, object>;
			DCDF_Desk dCDF_Desk = DCDF_Desk.CreateWithDic(data);
			list.Add(dCDF_Desk);
			for (int j = 0; j < dCDF_Desk.prizePool.Length; j++)
			{
				txtPrizePools[i, j].text = dCDF_Desk.prizePool[j].ToString("#0.00");
			}
		}
		deskList = list;
	}

	public void HandleNetMsg_EnterRoom(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			Hide();
			DCDF_MySqlConnection.desk = deskList[_curRoomType];
			DCDF_MB_Singleton<DCDF_MajorGameController>.GetInstance().Show();
			DCDF_MB_Singleton<DCDF_MajorGameController>.GetInstance().InitGame();
			return;
		}
		switch ((int)dictionary["messageStatus"])
		{
		case 0:
			DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog("桌子不存在");
			break;
		case 1:
			DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog("桌已满");
			break;
		case 2:
			DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog("您的账户已爆机，请兑奖");
			break;
		case 3:
			DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog("体验币不足");
			break;
		case 4:
			DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog("游戏币不足");
			break;
		default:
			UnityEngine.Debug.Log(string.Empty);
			break;
		}
	}

	public void UpdateView()
	{
		txtCoin.text = DCDF_MySqlConnection.user.gameGold.ToString("#0.00");
	}
}
