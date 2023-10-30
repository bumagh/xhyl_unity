using UnityEngine;

public class BCBM_GameMsgMngr : MonoBehaviour
{
	public enum GAME_MSG_TYPE
	{
		MSG_UIBACK_BUTTON,
		MSG_UI_QUIT_CONFIRM
	}

	public static BCBM_GameMsgMngr G_GameMsgMngr;

	private BCBM_Subject _UI_BackBtn_Msg;

	private BCBM_Subject _UI_quit_to_login_Msg;

	public static BCBM_GameMsgMngr GetSingleton()
	{
		return G_GameMsgMngr;
	}

	public void QuitMsgAttach(BCBM_Observer obs)
	{
		_UI_quit_to_login_Msg.Attach(obs);
	}

	public void BackBtnMsgAttach(BCBM_Observer obs)
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
		_UI_BackBtn_Msg = new BCBM_Subject(0);
		_UI_quit_to_login_Msg = new BCBM_Subject(1);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
