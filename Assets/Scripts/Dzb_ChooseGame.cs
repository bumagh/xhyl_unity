using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dzb_ChooseGame : Dzb_Singleton<Dzb_ChooseGame>
{
	public Button ReturnBtn;

	public Dzb_LoopScrollView _loopScrollView;

	public GameObject _Content;

	public Sprite[] GameSprite;

	private bool IsFrist = true;

	private void Awake()
	{
		Dzb_Singleton<Dzb_ChooseGame>.SetInstance(this);
		ReturnBtn = base.transform.Find("ReturnBtn").GetComponent<Button>();
		_loopScrollView = base.transform.Find("ScrollView").GetComponent<Dzb_LoopScrollView>();
		_Content = base.transform.Find("ScrollView/Viewport/Content").gameObject;
	}

	private void Start()
	{
		if (Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsTest)
		{
			_loopScrollView.SetcacheCount = 4;
			_loopScrollView.Init(0, UpdateCell);
			_loopScrollView.UpdateList(5);
			for (int i = 0; i < _Content.transform.childCount; i++)
			{
				string name = _Content.transform.GetChild(i).name;
				_Content.transform.GetChild(i).Find("Click").GetComponent<Button>()
					.onClick.AddListener(delegate
					{
						if (Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsTest)
						{
							Dzb_Singleton<Dzb_ChooseSeat>.GetInstance().gameObject.SetActive(value: true);
							base.gameObject.SetActive(value: false);
						}
						else
						{
							Dzb_Singleton<Dzb_GameInfo>.GetInstance().RoomId = int.Parse(name);
							Dzb_SendMsgManager.Send_EnterRoom(int.Parse(name));
						}
					});
				}
			}
			AddListener();
		}

		private void Update()
		{
			if (!Input.GetKeyDown(KeyCode.A))
			{
			}
		}

		private void AddListener()
		{
			ReturnBtn.onClick.AddListener(delegate
			{
				Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
				Dzb_Singleton<Dzb_AlertDialog>.GetInstance().ShowDialog("是否退出游戏？", showOkCancel: true, delegate
				{
					if (!Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsTest)
					{
						Dzb_Singleton<Dzb_GameInfo>.GetInstance().PrepareQuitGame();
						Dzb_Singleton<Dzb_GameInfo>.GetInstance().QuitToHallLogin();
						SceneManager.LoadSceneAsync(0);
					}
					else
					{
						SceneManager.LoadSceneAsync(0);
					}
				});
			});
		}

		private void InitRoomCell()
		{
			_loopScrollView.SetcacheCount = 4;
			_loopScrollView.Init(0, UpdateCell);
			_loopScrollView.UpdateList(Dzb_MySqlConnection.roomList.Count);
			for (int i = 0; i < _Content.transform.childCount; i++)
			{
				GameObject obj = _Content.transform.GetChild(i).GetChild(0).gameObject;
				_Content.transform.GetChild(i).Find("Click").GetComponent<Button>()
					.onClick.AddListener(delegate
					{
						Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
						Dzb_Singleton<Dzb_GameInfo>.GetInstance().RoomId = int.Parse(obj.name);
						for (int j = 0; j < Dzb_MySqlConnection.roomList.Count; j++)
						{
							if (Dzb_MySqlConnection.roomList[j].roomId == Dzb_Singleton<Dzb_GameInfo>.GetInstance().RoomId)
							{
								Dzb_MySqlConnection.room = Dzb_MySqlConnection.roomList[j];
								break;
							}
						}
						Dzb_SendMsgManager.Send_EnterRoom(int.Parse(obj.name));
					});
				}
			}

			public void UpdateRoomCell()
			{
				if (IsFrist)
				{
					InitRoomCell();
					IsFrist = false;
				}
				else
				{
					_loopScrollView.UpdateList(Dzb_MySqlConnection.roomList.Count);
				}
			}

			private void UpdateCell(GameObject obj, int num)
			{
				obj.name = num.ToString();
				if (Dzb_MySqlConnection.roomList.Count > num)
				{
					obj.transform.GetChild(0).name = Dzb_MySqlConnection.roomList[num].roomId.ToString();
					obj.transform.GetChild(0).GetComponent<Image>().sprite = GameSprite[Dzb_MySqlConnection.roomList[num].RoomType - 1];
					obj.transform.Find("Rate").GetComponent<Text>().text = Dzb_MySqlConnection.roomList[num].MinBet + "-" + Dzb_MySqlConnection.roomList[num].MaxBet;
					obj.transform.Find("Count").GetComponent<Text>().text = Dzb_MySqlConnection.roomList[num].PeopleNum + "/" + Dzb_MySqlConnection.roomList[num].PeopleCount;
					obj.transform.Find("Limit").GetComponent<Text>().text = Dzb_MySqlConnection.roomList[num].NeedCoin.ToString();
				}
			}
		}
