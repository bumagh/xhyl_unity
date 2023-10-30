using UnityEngine;

public class JSYS_LL_GameMsgMngr : MonoBehaviour
{
	public enum GAME_MSG_TYPE
	{
		MSG_UIBACK_BUTTON,
		MSG_UI_QUIT_CONFIRM
	}

	public static JSYS_LL_GameMsgMngr G_GameMsgMngr;

	private JSYS_LL_Subject _UI_BackBtn_Msg;

	private JSYS_LL_Subject _UI_quit_to_login_Msg;

	public static JSYS_LL_GameMsgMngr GetSingleton()
	{
		return G_GameMsgMngr;
	}

	public void QuitMsgAttach(JSYS_LL_Observer obs)
	{
		_UI_quit_to_login_Msg.Attach(obs);
	}

	public void BackBtnMsgAttach(JSYS_LL_Observer obs)
	{
		_UI_BackBtn_Msg.Attach(obs);
	}

	public void On_UIBackBtn_Press()
	{
		_UI_BackBtn_Msg.Notify();
	}

	public void On_UI_quit_to_login()
	{
		int num = 0;
		_UI_quit_to_login_Msg.Notify(num);
	}

	public void On_UI_quit_to_GameHall()
	{
		int num = 1;
		_UI_quit_to_login_Msg.Notify(num);
	}

	private void Awake()
	{
		if (G_GameMsgMngr == null)
		{
			G_GameMsgMngr = this;
		}
		_UI_BackBtn_Msg = new JSYS_LL_Subject(0);
		_UI_quit_to_login_Msg = new JSYS_LL_Subject(1);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
