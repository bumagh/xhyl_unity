using M__M.GameHall.Common;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class FK3_UserInfoShow : MonoBehaviour
	{
		private static FK3_UserInfoShow instance;

		[SerializeField]
		private Image _headIconImage;

		[SerializeField]
		public Text _userGameGlodText;

		[SerializeField]
		public Text _userExpText;

		[SerializeField]
		public Text _txtLogo;

		public static FK3_UserInfoShow Get()
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

		public void Init()
		{
			if (FK3_GVars.user.photoId >= FK3_MB_Singleton<FK3_GameUIController>.Get()._icon.Length || FK3_GVars.user.photoId <= 1)
			{
				FK3_GVars.user.photoId = 1;
			}
			try
			{
				_headIconImage.sprite = ((FK3_MB_Singleton<FK3_GameUIController>.Get()._icon.Length < 1) ? null : FK3_MB_Singleton<FK3_GameUIController>.Get()._icon[FK3_GVars.user.photoId]);
				_userGameGlodText.text = FK3_GVars.user.gameGold.ToString();
				_userExpText.text = FK3_GVars.user.expeGold.ToString();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}
	}
}
