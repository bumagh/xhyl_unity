using UnityEngine;
using UnityEngine.UI;

public class SettingPanelController : MonoBehaviour
{
	public GameObject about;

	[SerializeField]
	private ToggleController m_toggleCtrl_Sound;

	[SerializeField]
	private ToggleController m_toggleCtrl_BG;

	public Text VersionText;

	private Tween_SlowAction tween_SlowAction;

	private void Start()
	{
		m_toggleCtrl_Sound.onToggle = OnToggle_Sound;
		m_toggleCtrl_BG.onToggle = OnToggle_BG;
		Init();
		VersionText.text = "v" + Application.version;
		tween_SlowAction = GetComponent<Tween_SlowAction>();
	}

	public void OnBtnClick_ShowAbout()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		about.SetActive(value: true);
	}

	public void OnBtnClick_CloseAbout()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		about.SetActive(value: false);
	}

	public void OnBtnClick_CloseSetting()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		if ((object)tween_SlowAction != null)
		{
			tween_SlowAction.Hide(base.gameObject);
		}
	}

	public void OnToggle_Sound(bool newValue)
	{
		UnityEngine.Debug.Log("OnToggle_Sound: " + newValue);
		MB_Singleton<SoundManager>.Get().SetSilenceSound(newValue);
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
	}

	public void OnToggle_BG(bool newValue)
	{
		UnityEngine.Debug.Log("OnToggle_BG: " + newValue);
		MB_Singleton<SoundManager>.Get().SetSilenceBG(newValue);
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
	}

	public void OnToggle_TestUninstall(bool newValue)
	{
		UnityEngine.Debug.Log("OnToggle_TestUninstall: " + newValue);
		InnerGameManager.Get().debug_Uninstall = newValue;
		InnerGameManager.Get().Refresh_UninstallBtnState();
	}

	public void Init()
	{
		m_toggleCtrl_Sound.SetValue(!MB_Singleton<SoundManager>.Get().IsSilenceSound());
		m_toggleCtrl_BG.SetValue(!MB_Singleton<SoundManager>.Get().IsSilenceBG());
	}
}
