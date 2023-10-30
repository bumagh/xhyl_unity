using UnityEngine;

namespace M__M.HaiWang.Demo
{
	public class SettingController : MonoBehaviour
	{
		[SerializeField]
		private Transform _bgSoundTrans;

		[SerializeField]
		private Transform _screenBrightTrans;

		[SerializeField]
		private Transform _groupChatTrans;

		[SerializeField]
		private Transform _privateChatTrans;

		private void Awake()
		{
		}

		private void OnEnable()
		{
			if (PlayerPrefs.HasKey("BgSound"))
			{
				int @int = PlayerPrefs.GetInt("BgSound");
				SwitchControl(_bgSoundTrans, @int == 1);
				HW2_Singleton<SoundMgr>.Get().SetActive(@int == 1);
			}
			else
			{
				SwitchControl(_bgSoundTrans, isOn: true);
			}
			if (PlayerPrefs.HasKey("ScreenBright"))
			{
				int int2 = PlayerPrefs.GetInt("ScreenBright");
				SwitchControl(_screenBrightTrans, int2 == 1);
				Screen.sleepTimeout = ((int2 == 1) ? (-1) : (-2));
			}
			else
			{
				SwitchControl(_screenBrightTrans, isOn: true);
			}
			if (PlayerPrefs.HasKey("GroupChat"))
			{
				int int3 = PlayerPrefs.GetInt("GroupChat");
				SwitchControl(_groupChatTrans, int3 == 1);
				HW2_GVars.isShutChatGroup = (int3 == 1);
			}
			else
			{
				SwitchControl(_groupChatTrans, isOn: true);
			}
			if (PlayerPrefs.HasKey("PrivateChat"))
			{
				int int4 = PlayerPrefs.GetInt("PrivateChat");
				SwitchControl(_privateChatTrans, int4 == 1);
				HW2_GVars.isShutChatPrivate = (int4 == 1);
			}
			else
			{
				SwitchControl(_privateChatTrans, isOn: true);
			}
		}

		private void SwitchControl(Transform _trans, bool isOn)
		{
			GameObject gameObject = _trans.Find("switch/on").gameObject;
			GameObject gameObject2 = _trans.Find("switch/off").gameObject;
			gameObject.SetActive(isOn);
			gameObject2.SetActive(!isOn);
		}

		public void OnBtnClick_BgSound()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("设置界面滑动");
			HW2_Singleton<SoundMgr>.Get().SetVolume("设置界面滑动", 1f);
			GameObject gameObject = _bgSoundTrans.Find("switch/on").gameObject;
			GameObject gameObject2 = _bgSoundTrans.Find("switch/off").gameObject;
			gameObject.SetActive(!gameObject.activeSelf);
			gameObject2.SetActive(!gameObject2.activeSelf);
			if (gameObject2.activeSelf)
			{
				HW2_Singleton<SoundMgr>.Get().SetActive(bFlag: false);
				PlayerPrefs.SetInt("BgSound", 0);
			}
			else
			{
				HW2_Singleton<SoundMgr>.Get().SetActive(bFlag: true);
				PlayerPrefs.SetInt("BgSound", 1);
			}
		}

		public void OnBtnClick_ScreenBright()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("设置界面滑动");
			HW2_Singleton<SoundMgr>.Get().SetVolume("设置界面滑动", 1f);
			GameObject gameObject = _screenBrightTrans.Find("switch/on").gameObject;
			GameObject gameObject2 = _screenBrightTrans.Find("switch/off").gameObject;
			gameObject.SetActive(!gameObject.activeSelf);
			gameObject2.SetActive(!gameObject2.activeSelf);
			if (gameObject2.activeSelf)
			{
				Screen.sleepTimeout = -2;
				PlayerPrefs.SetInt("ScreenBright", 0);
			}
			else
			{
				Screen.sleepTimeout = -1;
				PlayerPrefs.SetInt("ScreenBright", 1);
			}
		}

		public void OnBtnClick_GroupChat()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("设置界面开关音效");
			HW2_Singleton<SoundMgr>.Get().SetVolume("设置界面开关音效", 1f);
			GameObject gameObject = _groupChatTrans.Find("switch/on").gameObject;
			GameObject gameObject2 = _groupChatTrans.Find("switch/off").gameObject;
			gameObject.SetActive(!gameObject.activeSelf);
			gameObject2.SetActive(!gameObject2.activeSelf);
			HW2_GVars.isShutChatGroup = gameObject.activeSelf;
			PlayerPrefs.SetInt("GroupChat", gameObject.activeSelf ? 1 : 0);
		}

		public void OnBtnClick_PrivateChat()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("设置界面开关音效");
			HW2_Singleton<SoundMgr>.Get().SetVolume("设置界面开关音效", 1f);
			GameObject gameObject = _privateChatTrans.Find("switch/on").gameObject;
			GameObject gameObject2 = _privateChatTrans.Find("switch/off").gameObject;
			gameObject.SetActive(!gameObject.activeSelf);
			gameObject2.SetActive(!gameObject2.activeSelf);
			HW2_GVars.isShutChatPrivate = gameObject.activeSelf;
			PlayerPrefs.SetInt("PrivateChat", gameObject.activeSelf ? 1 : 0);
		}

		public void BtnMusic()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("帮助设置");
			HW2_Singleton<SoundMgr>.Get().SetVolume("帮助设置", 1f);
		}

		public void BtnSure()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
