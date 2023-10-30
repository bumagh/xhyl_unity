using UnityEngine;
using UnityEngine.UI;

public class Hfh_ChooseSeat : Hfh_Singleton<Hfh_ChooseSeat>
{
	public Button ReturnBtn;

	public Button QuickStartBtn;

	public Hfh_LoopScrollView _loopScrollView;

	public GameObject _Content;

	private bool IsFrist = true;

	private void Awake()
	{
		Hfh_Singleton<Hfh_ChooseSeat>.SetInstance(this);
		ReturnBtn = base.transform.Find("ReturnBtn").GetComponent<Button>();
		QuickStartBtn = base.transform.Find("QuickStartBtn").GetComponent<Button>();
		_loopScrollView = base.transform.Find("ScrollView").GetComponent<Hfh_LoopScrollView>();
		_Content = base.transform.Find("ScrollView/Viewport/Content").gameObject;
		base.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		AddListener();
	}

	private void AddListener()
	{
		ReturnBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			Hfh_SendMsgManager.Send_RoomInfo();
			Hfh_Singleton<Hfh_ChooseGame>.GetInstance().gameObject.SetActive(value: true);
			base.gameObject.SetActive(value: false);
			Hfh_GVars.room = null;
		});
	}

	private void InitRoomCell()
	{
		_loopScrollView.SetcacheCount = 2;
		_loopScrollView.Init(0, UpdateCell);
		if (Hfh_GVars.seatList.Count % 11 > 0)
		{
			_loopScrollView.UpdateList(Hfh_GVars.seatList.Count / 11 + 1);
		}
		else
		{
			_loopScrollView.UpdateList(Hfh_GVars.seatList.Count / 11);
		}
		for (int i = 0; i < _Content.transform.childCount; i++)
		{
			for (int j = 0; j < _Content.transform.GetChild(i).childCount; j++)
			{
				GameObject obj = _Content.transform.GetChild(i).GetChild(j).gameObject;
				_Content.transform.GetChild(i).GetChild(j).GetComponent<Button>()
					.onClick.AddListener(delegate
					{
						Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
						Hfh_Singleton<Hfh_GameInfo>.GetInstance().DeskId = int.Parse(obj.name);
						for (int k = 0; k < Hfh_GVars.seatList.Count; k++)
						{
							if (Hfh_GVars.seatList[k].id == Hfh_Singleton<Hfh_GameInfo>.GetInstance().DeskId)
							{
								Hfh_GVars.seat = Hfh_GVars.seatList[k];
								break;
							}
						}
						Hfh_SendMsgManager.Send_EnterDesk(Hfh_Singleton<Hfh_GameInfo>.GetInstance().DeskId, 0);
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
			else if (Hfh_GVars.seatList.Count % 11 > 0)
			{
				_loopScrollView.UpdateList(Hfh_GVars.seatList.Count / 11 + 1);
			}
			else
			{
				_loopScrollView.UpdateList(Hfh_GVars.seatList.Count / 11);
			}
		}

		private void UpdateCell(GameObject obj, int num)
		{
			obj.name = num.ToString();
			for (int i = 0; i < obj.transform.childCount; i++)
			{
				if (num * 11 + i < Hfh_GVars.seatList.Count)
				{
					obj.transform.GetChild(i).name = Hfh_GVars.seatList[num * 11 + i].id.ToString();
					obj.transform.GetChild(i).Find("Text").GetComponent<Text>()
						.text = Hfh_GVars.seatList[num * 11 + i].id.ToString();
						if (Hfh_GVars.seatList[num * 11 + i].isKeepTable)
						{
							if (Hfh_GVars.seatList[num * 11 + i].playerid == Hfh_GVars.user.id)
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
							obj.transform.GetChild(i).Find("Time").GetComponent<Hfh_Timer>()
								.Activate(Hfh_GVars.seatList[num * 9 + i]);
						}
						else
						{
							obj.transform.GetChild(i).Find("MyLock").gameObject.SetActive(value: false);
							obj.transform.GetChild(i).Find("Lock").gameObject.SetActive(value: false);
							obj.transform.GetChild(i).Find("Time").gameObject.SetActive(value: false);
							if (Hfh_GVars.seatList[num * 11 + i].playerid != -1)
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
