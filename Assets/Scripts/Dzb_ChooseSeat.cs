using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dzb_ChooseSeat : Dzb_Singleton<Dzb_ChooseSeat>
{
	public Button ReturnBtn;

	public Button QuickStartBtn;

	public Dzb_LoopScrollView _loopScrollView;

	public GameObject _Content;

	private bool IsFrist = true;

	private void Awake()
	{
		Dzb_Singleton<Dzb_ChooseSeat>.SetInstance(this);
		ReturnBtn = base.transform.Find("ReturnBtn").GetComponent<Button>();
		QuickStartBtn = base.transform.Find("QuickStartBtn").GetComponent<Button>();
		_loopScrollView = base.transform.Find("ScrollView").GetComponent<Dzb_LoopScrollView>();
		_Content = base.transform.Find("ScrollView/Viewport/Content").gameObject;
		base.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		if (Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsTest)
		{
			_loopScrollView.SetcacheCount = 2;
			_loopScrollView.Init(0, UpdateCell);
			_loopScrollView.UpdateList(5);
			for (int i = 0; i < _Content.transform.childCount; i++)
			{
				for (int j = 0; j < _Content.transform.GetChild(i).childCount; j++)
				{
					string name = _Content.transform.GetChild(i).GetChild(j).name;
					_Content.transform.GetChild(i).GetChild(j).GetComponent<Button>()
						.onClick.AddListener(delegate
						{
							UnityEngine.Debug.Log("按下" + name);
							if (Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsTest)
							{
								SceneManager.LoadSceneAsync("DzbGame");
							}
							else
							{
								Dzb_Singleton<Dzb_GameInfo>.GetInstance().DeskId = int.Parse(name);
								Dzb_SendMsgManager.Send_EnterDesk(int.Parse(name), 0);
							}
						});
					}
				}
			}
			AddListener();
		}

		private void AddListener()
		{
			ReturnBtn.onClick.AddListener(delegate
			{
				Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
				if (!Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsTest)
				{
					Dzb_SendMsgManager.Send_RoomInfo();
					Dzb_Singleton<Dzb_ChooseGame>.GetInstance().gameObject.SetActive(value: true);
					base.gameObject.SetActive(value: false);
					Dzb_MySqlConnection.room = null;
					Dzb_Singleton<Dzb_UIScene>.GetInstance().UpdateScore();
				}
				else
				{
					Dzb_Singleton<Dzb_ChooseGame>.GetInstance().gameObject.SetActive(value: true);
					base.gameObject.SetActive(value: false);
				}
			});
		}

		private void InitRoomCell()
		{
			_loopScrollView.SetcacheCount = 2;
			_loopScrollView.Init(0, UpdateCell);
			if (Dzb_MySqlConnection.seatList.Count % 9 > 0)
			{
				_loopScrollView.UpdateList(Dzb_MySqlConnection.seatList.Count / 9 + 1);
			}
			else
			{
				_loopScrollView.UpdateList(Dzb_MySqlConnection.seatList.Count / 9);
			}
			for (int i = 0; i < _Content.transform.childCount; i++)
			{
				for (int j = 0; j < _Content.transform.GetChild(i).childCount; j++)
				{
					GameObject obj = _Content.transform.GetChild(i).GetChild(j).gameObject;
					_Content.transform.GetChild(i).GetChild(j).GetComponent<Button>()
						.onClick.AddListener(delegate
						{
							Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
							Dzb_Singleton<Dzb_GameInfo>.GetInstance().DeskId = int.Parse(obj.name);
							for (int k = 0; k < Dzb_MySqlConnection.seatList.Count; k++)
							{
								if (Dzb_MySqlConnection.seatList[k].id == Dzb_Singleton<Dzb_GameInfo>.GetInstance().DeskId)
								{
									Dzb_MySqlConnection.seat = Dzb_MySqlConnection.seatList[k];
									break;
								}
							}
							Dzb_SendMsgManager.Send_EnterDesk(Dzb_Singleton<Dzb_GameInfo>.GetInstance().DeskId, 0);
							Dzb_Singleton<Dzb_AlertDialogText>.GetInstance().ShowDialogText("正在进入房间，请稍后...", 9999f);
						});
					}
				}
			}

			public void UpdateSeatCell()
			{
				if (IsFrist)
				{
					InitRoomCell();
					IsFrist = false;
				}
				else if (Dzb_MySqlConnection.seatList.Count % 9 > 0)
				{
					_loopScrollView.UpdateList(Dzb_MySqlConnection.seatList.Count / 9 + 1);
				}
				else
				{
					_loopScrollView.UpdateList(Dzb_MySqlConnection.seatList.Count / 9);
				}
			}

			private void UpdateCell(GameObject obj, int num)
			{
				obj.name = num.ToString();
				for (int i = 0; i < obj.transform.childCount; i++)
				{
					if (num * 9 + i < Dzb_MySqlConnection.seatList.Count)
					{
						obj.transform.GetChild(i).name = Dzb_MySqlConnection.seatList[num * 9 + i].id.ToString();
						obj.transform.GetChild(i).Find("Text").GetComponent<Text>()
							.text = Dzb_MySqlConnection.seatList[num * 9 + i].id.ToString();
							if (Dzb_MySqlConnection.seatList[num * 9 + i].isKeepTable)
							{
								if (Dzb_MySqlConnection.seatList[num * 9 + i].playerid == Dzb_MySqlConnection.user.id)
								{
									obj.transform.GetChild(i).Find("MyLock").gameObject.SetActive(value: true);
									obj.transform.GetChild(i).Find("Lock").gameObject.SetActive(value: false);
								}
								else
								{
									obj.transform.GetChild(i).Find("MyLock").gameObject.SetActive(value: false);
									obj.transform.GetChild(i).Find("Lock").gameObject.SetActive(value: true);
								}
								obj.transform.GetChild(i).Find("Image").gameObject.SetActive(value: false);
								obj.transform.GetChild(i).Find("Time").gameObject.SetActive(value: true);
								obj.transform.GetChild(i).Find("Time").GetComponent<Dzb_Timer>()
									.Activate(Dzb_MySqlConnection.seatList[num * 9 + i]);
							}
							else
							{
								obj.transform.GetChild(i).Find("MyLock").gameObject.SetActive(value: false);
								obj.transform.GetChild(i).Find("Lock").gameObject.SetActive(value: false);
								obj.transform.GetChild(i).Find("Time").gameObject.SetActive(value: false);
								if (Dzb_MySqlConnection.seatList[num * 9 + i].playerid != -1)
								{
									obj.transform.GetChild(i).Find("Image").gameObject.SetActive(value: true);
								}
								else
								{
									obj.transform.GetChild(i).Find("Image").gameObject.SetActive(value: false);
								}
							}
							obj.transform.GetChild(i).gameObject.SetActive(value: true);
						}
						else
						{
							obj.transform.GetChild(i).gameObject.SetActive(value: false);
						}
					}
				}
			}
