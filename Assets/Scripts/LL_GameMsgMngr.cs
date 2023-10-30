using UnityEngine;

public class LL_GameMsgMngr : MonoBehaviour
{
	public enum GAME_MSG_TYPE
	{
		MSG_UIBACK_BUTTON,
		MSG_UI_QUIT_CONFIRM
	}

	public static LL_GameMsgMngr G_GameMsgMngr;

	private LL_Subject _UI_BackBtn_Msg;

	private LL_Subject _UI_quit_to_login_Msg;

	public static LL_GameMsgMngr GetSingleton()
	{
		return G_GameMsgMngr;
	}

	public void QuitMsgAttach(LL_Observer obs)
	{
		_UI_quit_to_login_Msg.Attach(obs);
	}

	public void BackBtnMsgAttach(LL_Observer obs)
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
		_UI_BackBtn_Msg = new LL_Subject(0);
		_UI_quit_to_login_Msg = new LL_Subject(1);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
