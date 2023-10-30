using M__M.GameHall.Common;
using M__M.HaiWang.UIDefine;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class HW2_UserInfoShow : MonoBehaviour
	{
		private static HW2_UserInfoShow instance;

		[SerializeField]
		private Image _headIconImage;

		[SerializeField]
		private Text _userGameGlodText;

		[SerializeField]
		public Text _txtLogo;

		public static HW2_UserInfoShow Get()
		{
			return instance;
		}

		public void OnQuit()
		{
			instance = null;
		}

		private void Awake()
		{
			instance = this;
		}

		private void Update()
		{
		}

		public void Init()
		{
			if (HW2_GVars.user.photoId >= HW2_MB_Singleton<GameUIController>.Get()._icon.Length || HW2_GVars.user.photoId <= 1)
			{
				HW2_GVars.user.photoId = 1;
			}
			UnityEngine.Debug.LogError("photoId: " + HW2_GVars.user.photoId + "  Length: " + HW2_MB_Singleton<GameUIController>.Get()._icon.Length);
			try
			{
				_headIconImage.sprite = ((HW2_MB_Singleton<GameUIController>.Get()._icon.Length < 1) ? null : HW2_MB_Singleton<GameUIController>.Get()._icon[HW2_GVars.user.photoId]);
				_userGameGlodText.text = HW2_GVars.user.gameGold.ToString();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (HW2_GVars.m_curState == Demo_UI_State.RoomSelection)
			{
			}
			if (HW2_GVars.m_curState == Demo_UI_State.DeskSelection)
			{
			}
			UnityEngine.Debug.Log($"Gold:{HW2_GVars.user.gameGold},Expe:{HW2_GVars.user.expeGold}");
		}
	}
}
