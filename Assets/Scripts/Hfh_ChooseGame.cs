using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hfh_ChooseGame : Hfh_Singleton<Hfh_ChooseGame>
{
	public Button ReturnBtn;

	public Hfh_LoopScrollView _loopScrollView;

	public GameObject _Content;

	public Sprite[] GameSprite;

	private bool IsFrist = true;

	private void Awake()
	{
		Hfh_Singleton<Hfh_ChooseGame>.SetInstance(this);
		ReturnBtn = base.transform.Find("ReturnBtn").GetComponent<Button>();
		_loopScrollView = base.transform.Find("ScrollView").GetComponent<Hfh_LoopScrollView>();
		_Content = base.transform.Find("ScrollView/Viewport/Content").gameObject;
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
			Hfh_Singleton<Hfh_AlertDialog>.GetInstance().ShowDialog("是否退出游戏？", showOkCancel: true, delegate
			{
				Hfh_Singleton<Hfh_GameInfo>.GetInstance().PrepareQuitGame();
				Hfh_Singleton<Hfh_GameInfo>.GetInstance().QuitToHallLogin();
				SceneManager.LoadSceneAsync(0);
			});
		});
	}

	private void InitRoomCell()
	{
		_loopScrollView.SetcacheCount = 4;
		_loopScrollView.Init(0, UpdateCell);
		_loopScrollView.UpdateList(Hfh_GVars.roomList.Count);
		for (int i = 0; i < _Content.transform.childCount; i++)
		{
			GameObject obj = _Content.transform.GetChild(i).GetChild(0).gameObject;
			_Content.transform.GetChild(i).Find("Click").GetComponent<Button>()
				.onClick.AddListener(delegate
				{
					Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
					Hfh_Singleton<Hfh_GameInfo>.GetInstance().RoomId = int.Parse(obj.name);
					for (int j = 0; j < Hfh_GVars.roomList.Count; j++)
					{
						if (Hfh_GVars.roomList[j].roomId == Hfh_Singleton<Hfh_GameInfo>.GetInstance().RoomId)
						{
							Hfh_GVars.room = Hfh_GVars.roomList[j];
							break;
						}
					}
					Hfh_SendMsgManager.Send_EnterRoom(int.Parse(obj.name));
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
				_loopScrollView.UpdateList(Hfh_GVars.roomList.Count);
			}
		}

		private void UpdateCell(GameObject obj, int num)
		{
			obj.name = num.ToString();
			if (Hfh_GVars.roomList.Count > num)
			{
				obj.transform.GetChild(0).name = Hfh_GVars.roomList[num].roomId.ToString();
				obj.transform.GetChild(0).GetComponent<Image>().sprite = GameSprite[Hfh_GVars.roomList[num].RoomType - 1];
				obj.transform.Find("Rate").GetComponent<Text>().text = Hfh_GVars.roomList[num].MinBet + "-" + Hfh_GVars.roomList[num].MaxBet;
				obj.transform.Find("Count").GetComponent<Text>().text = Hfh_GVars.roomList[num].PeopleNum + "/" + Hfh_GVars.roomList[num].PeopleCount;
				obj.transform.Find("Limit").GetComponent<Text>().text = Hfh_GVars.roomList[num].NeedCoin.ToString();
			}
		}
	}
