using UnityEngine;
using UnityEngine.UI;

public class Dzb_UIScene : Dzb_Singleton<Dzb_UIScene>
{
	public Image Head;

	public Text NickName;

	public Text Gold;

	public Text Diamond;

	public GameObject ChooseGamePanel;

	public GameObject ChooseSeatPanel;

	private void Awake()
	{
		Dzb_Singleton<Dzb_UIScene>.SetInstance(this);
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
		if (Dzb_MySqlConnection.user != null)
		{
			NickName.text = Dzb_MySqlConnection.user.nickname;
			if (Dzb_MySqlConnection.room != null)
			{
				if (Dzb_MySqlConnection.room.RoomType == 4)
				{
					Diamond.text = Dzb_MySqlConnection.user.expeGold.ToString();
					Gold.text = Dzb_MySqlConnection.user.expeScore.ToString();
				}
				else
				{
					Diamond.text = Dzb_MySqlConnection.user.gameGold.ToString();
					Gold.text = Dzb_MySqlConnection.user.gameScore.ToString();
				}
			}
			else
			{
				Diamond.text = Dzb_MySqlConnection.user.gameGold.ToString();
				Gold.text = Dzb_MySqlConnection.user.gameScore.ToString();
			}
		}
		if (Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsQuitGame)
		{
			Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsQuitGame = false;
			Dzb_SendMsgManager.Send_EnterRoom(Dzb_MySqlConnection.room.roomId);
		}
		else
		{
			Dzb_SendMsgManager.Send_RoomInfo();
		}
	}

	public void UpdateScore()
	{
		if (Dzb_MySqlConnection.room != null)
		{
			if (Dzb_MySqlConnection.room.RoomType == 4)
			{
				Diamond.text = Dzb_MySqlConnection.user.expeGold.ToString();
				Gold.text = Dzb_MySqlConnection.user.expeScore.ToString();
			}
			else
			{
				Diamond.text = Dzb_MySqlConnection.user.gameGold.ToString();
				Gold.text = Dzb_MySqlConnection.user.gameScore.ToString();
			}
		}
		else
		{
			Diamond.text = Dzb_MySqlConnection.user.gameGold.ToString();
			Gold.text = Dzb_MySqlConnection.user.gameScore.ToString();
		}
	}
}
