using UnityEngine;
using UnityEngine.UI;

public class Hfh_UIScene : Hfh_Singleton<Hfh_UIScene>
{
	public Image Head;

	public Text NickName;

	public Text Gold;

	public Text Diamond;

	public GameObject ChooseGamePanel;

	public GameObject ChooseSeatPanel;

	private void Awake()
	{
		Hfh_Singleton<Hfh_UIScene>.SetInstance(this);
	}

	private void Start()
	{
		Head = base.transform.Find("Player/Picture").GetComponent<Image>();
		NickName = base.transform.Find("Player/Name/Text").GetComponent<Text>();
		Gold = base.transform.Find("Gold/Text").GetComponent<Text>();
		Diamond = base.transform.Find("Diamond/Text").GetComponent<Text>();
		ChooseGamePanel = base.transform.Find("ChooseGame").gameObject;
		ChooseSeatPanel = base.transform.Find("ChooseSeat").gameObject;
		Init();
	}

	private void Init()
	{
		ChooseGamePanel.SetActive(value: true);
		ChooseSeatPanel.SetActive(value: false);
		if (Hfh_GVars.user != null)
		{
			NickName.text = Hfh_GVars.user.nickname;
			if (Hfh_GVars.room != null)
			{
				if (Hfh_GVars.room.RoomType == 4)
				{
					Diamond.text = Hfh_GVars.user.expeGold.ToString();
					Gold.text = Hfh_GVars.user.expeScore.ToString();
				}
				else
				{
					Diamond.text = Hfh_GVars.user.gameGold.ToString();
					Gold.text = Hfh_GVars.user.gameScore.ToString();
				}
			}
			else
			{
				Diamond.text = Hfh_GVars.user.gameGold.ToString();
				Gold.text = Hfh_GVars.user.gameScore.ToString();
			}
		}
		if (Hfh_Singleton<Hfh_GameInfo>.GetInstance()._IsQuitGame)
		{
			Hfh_Singleton<Hfh_GameInfo>.GetInstance()._IsQuitGame = false;
			Hfh_SendMsgManager.Send_EnterRoom(Hfh_GVars.room.roomId);
		}
		else
		{
			Hfh_SendMsgManager.Send_RoomInfo();
		}
	}

	public void UpdateScore()
	{
		if (Hfh_GVars.room != null)
		{
			if (Hfh_GVars.room.RoomType == 4)
			{
				Diamond.text = Hfh_GVars.user.expeGold.ToString();
				Gold.text = Hfh_GVars.user.expeScore.ToString();
			}
			else
			{
				Diamond.text = Hfh_GVars.user.gameGold.ToString();
				Gold.text = Hfh_GVars.user.gameScore.ToString();
			}
		}
		else
		{
			Diamond.text = Hfh_GVars.user.gameGold.ToString();
			Gold.text = Hfh_GVars.user.gameScore.ToString();
		}
	}
}
