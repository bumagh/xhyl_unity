using M__M.HaiWang.GameDefine;
using M__M.HaiWang.UIDefine;
using System;
using UnityEngine;

namespace M__M.HaiWang.UI
{
	public class FK3_LobbyUI : MonoBehaviour
	{
		private FK3_Demo_UI_State m_curState = FK3_Demo_UI_State.StartupLoading;

		private FK3_LobbyContext m_context = new FK3_LobbyContext();

		private static FK3_LobbyUI s_instance;

		public event Action<FK3_LobbyContext> EventHandler_UI_Login_OnBtnLoginClick;

		public event Action<FK3_LobbyContext> EventHandler_UI_RoomSelection_OnRoomClick;

		public event Action<FK3_LobbyContext> EventHandler_UI_DeskSelection_OnSeatClick;

		public event Action<FK3_LobbyContext> EventHandler_UI_DeskSelection_OnBtnReturnClick;

		public static FK3_LobbyUI Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnGUI()
		{
			Display_StateHelper();
			Display_StateChangeTest();
			Display_UI();
		}

		private void Display_UI()
		{
			switch (m_curState)
			{
			case FK3_Demo_UI_State.DeskSelection:
				Display_UserInfo();
				Display_UI_DeskSelection();
				break;
			case FK3_Demo_UI_State.RoomSelection:
				Display_UserInfo();
				Display_UI_RoomSelection();
				break;
			case FK3_Demo_UI_State.Login:
				Display_UI_Login();
				break;
			}
		}

		private void Display_UI_Login()
		{
			int num = 580;
			int num2 = 325;
			int num3 = num2 * 3;
			int num4 = num + 20;
			int num5 = num3 + 20;
			GUI.Box(new Rect(Screen.width / 2 - num4 / 2, Screen.height / 2 - num5 / 2, num4, num5), string.Empty);
			GUILayout.BeginArea(new Rect(Screen.width / 2 - num / 2, Screen.height / 2 - num3 / 2, num, num3));
			GUILayout.BeginHorizontal();
			GUILayout.Label("账 号：");
			m_context.strAccount = GUILayout.TextField(m_context.strAccount, GUILayout.Width(300f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("密 码：");
			m_context.strPassword = GUILayout.PasswordField(m_context.strPassword, '*', GUILayout.Width(300f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("登陆"))
			{
				UnityEngine.Debug.Log("UI_Login:btn_Login clicked");
				FK3_SimpleUtil.SafeInvoke(delegate
				{
					if (this.EventHandler_UI_Login_OnBtnLoginClick != null)
					{
						this.EventHandler_UI_Login_OnBtnLoginClick(m_context);
					}
				});
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		private void Display_UI_RoomSelection()
		{
			int num = 140;
			int num2 = 25;
			int num3 = num2 * 2;
			int num4 = num + 20;
			int num5 = num3 + 20;
			GUI.Box(new Rect(Screen.width / 2 - num4 / 2, Screen.height / 2 - num3 / 2, num4, num5), "选择要进入的房间");
			GUILayout.BeginArea(new Rect(Screen.width / 2 - num / 2, Screen.height / 2 - num3 / 2, num, num3));
			GUILayout.Label(string.Empty);
			GUILayout.BeginHorizontal();
			string[] array = new string[2]
			{
				"练习厅",
				"竞技厅"
			};
			for (int i = 0; i < array.Length; i++)
			{
				int curRoomId = i + 1;
				string text = array[i];
				if (GUILayout.Button(text))
				{
					m_context.curRoomId = curRoomId;
					UnityEngine.Debug.LogError("========curRoomId=====: " + m_context.curRoomId);
					FK3_SimpleUtil.SafeInvoke(delegate
					{
						if (this.EventHandler_UI_RoomSelection_OnRoomClick != null)
						{
							this.EventHandler_UI_RoomSelection_OnRoomClick(m_context);
						}
					});
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		private void Display_UI_DeskSelection()
		{
			if (GUI.Button(new Rect(0f, 0f, 100f, 25f), "返回"))
			{
				FK3_SimpleUtil.SafeInvoke(delegate
				{
					if (this.EventHandler_UI_DeskSelection_OnBtnReturnClick != null)
					{
						this.EventHandler_UI_DeskSelection_OnBtnReturnClick(m_context);
					}
				});
			}
			int count = m_context.desks.Count;
			int num = 300;
			int num2 = 25;
			int num3 = num2 * (count + 2);
			int num4 = num + 20;
			int num5 = num3 + 20;
			GUI.Box(new Rect(Screen.width / 2 - num4 / 2, Screen.height / 2 - num5 / 2, num4, num5), "选择要进入的桌子和座位");
			GUILayout.BeginArea(new Rect(Screen.width / 2 - num / 2, Screen.height / 2 - num3 / 2, num, num3));
			GUILayout.Label(string.Empty);
			bool flag = false;
			for (int i = 0; i < m_context.desks.Count; i++)
			{
				GUILayout.BeginHorizontal();
				FK3_DeskInfo fK3_DeskInfo = m_context.desks[i];
				GUILayout.Label(fK3_DeskInfo.name);
				for (int j = 0; j < 4; j++)
				{
					int num6 = j + 1;
					FK3_SeatInfo seat = fK3_DeskInfo.GetSeat(num6);
					if (seat == null)
					{
						UnityEngine.Debug.LogError($"seat[id:{num6}] not valid");
						flag = true;
					}
					if (flag)
					{
						break;
					}
					if (seat.isUsed)
					{
						string empty = string.Empty;
						empty = "有人";
						GUILayout.Label(empty, "box");
					}
					else if (GUILayout.Button("空座"))
					{
						m_context.curDeskId = fK3_DeskInfo.id;
						m_context.curSeatId = seat.id;
						FK3_SimpleUtil.SafeInvoke(delegate
						{
							if (this.EventHandler_UI_DeskSelection_OnSeatClick != null)
							{
								this.EventHandler_UI_DeskSelection_OnSeatClick(m_context);
							}
						});
					}
				}
				GUILayout.EndHorizontal();
				if (flag)
				{
					break;
				}
			}
			GUILayout.EndArea();
		}

		private void Display_UserInfo()
		{
			int num = 300;
			int num2 = 25;
			string text = $"【{m_context.user.nickname}】: 体验币：【{m_context.user.expeGold}】，金币：【{m_context.user.gameGold}】";
			GUI.Box(new Rect(Screen.width / 2 - num / 2, 10f, num, num2), text);
		}

		private void Display_StateHelper()
		{
			string text = m_curState.ToString();
			int num = 100;
			int num2 = 25;
			int num3 = num2 * 2;
			GUI.BeginGroup(new Rect(Screen.width - num, 0f, num, num3));
			GUI.Box(new Rect(0f, 0f, num, num3), "当前UI");
			GUI.Label(new Rect(0f, num2, num, num2), text, "button");
			GUI.EndGroup();
		}

		private void Display_StateChangeTest()
		{
			int num = 100;
			int num2 = 25;
			int num3 = num2;
			if (GUI.Button(new Rect(Screen.width - num, Screen.height - num3 - 20, num, num3), "切换UI"))
			{
				switch (m_curState)
				{
				case FK3_Demo_UI_State.StartupLoading:
					Change_UI_State(FK3_Demo_UI_State.Login);
					break;
				case FK3_Demo_UI_State.Login:
					Change_UI_State(FK3_Demo_UI_State.RoomSelection);
					break;
				case FK3_Demo_UI_State.RoomSelection:
					Change_UI_State(FK3_Demo_UI_State.DeskSelection);
					break;
				case FK3_Demo_UI_State.DeskSelection:
					Change_UI_State(FK3_Demo_UI_State.InGame);
					break;
				case FK3_Demo_UI_State.InGame:
					Change_UI_State(FK3_Demo_UI_State.StartupLoading);
					break;
				}
			}
		}

		private void Display_XX()
		{
		}

		public void Change_UI_State(FK3_Demo_UI_State newState)
		{
			m_curState = newState;
		}

		public FK3_LobbyContext GetContext()
		{
			return m_context;
		}
	}
}
