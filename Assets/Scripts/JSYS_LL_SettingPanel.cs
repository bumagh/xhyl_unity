using UnityEngine;

public class JSYS_LL_SettingPanel : MonoBehaviour
{
	protected UIPanel mSettingPanel;

	protected Collider mBackgroundCol;

	protected Collider mSettingBgCol;

	protected Collider mEnsureBtnCol;

	protected Collider mGameMusicVolumeCol;

	protected Collider mButtonMusicVolumeCol;

	protected Collider mPublicChatSetCol;

	protected Collider mPrivateChatSetCol;

	protected Collider mScreenSleepSetCol;

	protected JSYS_LL_UICheckbox mGameVoiceSet;

	protected JSYS_LL_UICheckbox mButtonVoiceSet;

	protected JSYS_LL_UICheckbox mPublicChatSet;

	protected JSYS_LL_UICheckbox mPrivateChatSet;

	protected JSYS_LL_UICheckbox mScreenSleepSet;

	private void Start()
	{
		mSettingPanel = base.transform.GetComponent<UIPanel>();
		mBackgroundCol = base.transform.Find("Background").GetComponent<Collider>();
		mSettingBgCol = base.transform.Find("SettingBg").GetComponent<Collider>();
		mEnsureBtnCol = base.transform.Find("EnsureBtn").GetComponent<Collider>();
		mGameMusicVolumeCol = base.transform.Find("IsGameVoice").GetComponent<Collider>();
		mButtonMusicVolumeCol = base.transform.Find("IsButtonVoice").GetComponent<Collider>();
		mPublicChatSetCol = base.transform.Find("PublicChatSetting").GetComponent<Collider>();
		mPrivateChatSetCol = base.transform.Find("PrivateChatSetting").GetComponent<Collider>();
		mScreenSleepSetCol = base.transform.Find("ScreenSleepSetting").GetComponent<Collider>();
		mGameVoiceSet = base.transform.Find("IsGameVoice").GetComponent<JSYS_LL_UICheckbox>();
		mButtonVoiceSet = base.transform.Find("IsButtonVoice").GetComponent<JSYS_LL_UICheckbox>();
		mPublicChatSet = base.transform.Find("PublicChatSetting").GetComponent<JSYS_LL_UICheckbox>();
		mPrivateChatSet = base.transform.Find("PrivateChatSetting").GetComponent<JSYS_LL_UICheckbox>();
		mScreenSleepSet = base.transform.Find("ScreenSleepSetting").GetComponent<JSYS_LL_UICheckbox>();
		initSetting();
		JSYS_LL_LocalData.getInstance().applySetting();
		HideSetting();
	}

	private void Update()
	{
	}

	private void initSetting()
	{
		JSYS_LL_MusicMngr.GetSingleton().SetGameMusicVolume(JSYS_LL_GameInfo.getInstance().Setted.bIsGameVolum ? 1 : 0);
		JSYS_LL_LuckyLion_SoundManager.GetSingleton().SetMusicVolume(JSYS_LL_GameInfo.getInstance().Setted.bIsGameVolum ? 1 : 0);
		JSYS_LL_LuckyLion_SoundManager.GetSingleton().SetSoundVolume(JSYS_LL_GameInfo.getInstance().Setted.bIsButtonVolum ? 1 : 0);
		JSYS_LL_SettedInfo setted = JSYS_LL_GameInfo.getInstance().Setted;
		mGameVoiceSet.isChecked = setted.bIsGameVolum;
		mButtonVoiceSet.isChecked = setted.bIsButtonVolum;
		mPublicChatSet.isChecked = setted.bIsForbidPublicChat;
		mPrivateChatSet.isChecked = setted.bIsForbidPrivateChat;
		mScreenSleepSet.isChecked = setted.bIsScreenNeverSleep;
	}

	public void ShowSetting()
	{
		if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
		{
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
		mSettingPanel.enabled = true;
		_setColliderActive(bIsActive: true);
	}

	public void HideSetting()
	{
		mSettingPanel.enabled = false;
		_setColliderActive(bIsActive: false);
	}

	public void SaveAndHideSetting()
	{
		_onClickEnsureButton();
		HideSetting();
	}

	protected void _setColliderActive(bool bIsActive)
	{
		mBackgroundCol.enabled = bIsActive;
		mSettingBgCol.enabled = bIsActive;
		mEnsureBtnCol.enabled = bIsActive;
		mPublicChatSetCol.enabled = bIsActive;
		mPrivateChatSetCol.enabled = bIsActive;
		mScreenSleepSetCol.enabled = bIsActive;
		mGameMusicVolumeCol.enabled = bIsActive;
		mButtonMusicVolumeCol.enabled = bIsActive;
	}

	protected void _onClickEnsureButton()
	{
		if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
		{
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
		JSYS_LL_SettedInfo setted = JSYS_LL_GameInfo.getInstance().Setted;
		setted.bIsGameVolum = mGameVoiceSet.isChecked;
		setted.bIsButtonVolum = mButtonVoiceSet.isChecked;
		setted.bIsForbidPublicChat = mPublicChatSet.isChecked;
		setted.bIsForbidPrivateChat = mPrivateChatSet.isChecked;
		setted.bIsScreenNeverSleep = mScreenSleepSet.isChecked;
		JSYS_LL_LocalData.getInstance().applySetting();
		JSYS_LL_LocalData.getInstance().saveUserSetting();
		HideSetting();
	}

	public void _onClickOptionSetting(GameObject go)
	{
		if (go.name == "IsGameVoice")
		{
			JSYS_LL_MusicMngr.GetSingleton().SetGameMusicVolume(mGameVoiceSet.isChecked ? 1 : 0);
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().SetMusicVolume(mGameVoiceSet.isChecked ? 1 : 0);
		}
		else if (go.name == "IsButtonVoice")
		{
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().SetSoundVolume(mButtonVoiceSet.isChecked ? 1 : 0);
		}
	}
}
