using UnityEngine;

public class BCBM_SettingPanel : MonoBehaviour
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

	protected BCBM_UICheckbox mGameVoiceSet;

	protected BCBM_UICheckbox mButtonVoiceSet;

	protected BCBM_UICheckbox mPublicChatSet;

	protected BCBM_UICheckbox mPrivateChatSet;

	protected BCBM_UICheckbox mScreenSleepSet;

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
		mGameVoiceSet = base.transform.Find("IsGameVoice").GetComponent<BCBM_UICheckbox>();
		mButtonVoiceSet = base.transform.Find("IsButtonVoice").GetComponent<BCBM_UICheckbox>();
		mPublicChatSet = base.transform.Find("PublicChatSetting").GetComponent<BCBM_UICheckbox>();
		mPrivateChatSet = base.transform.Find("PrivateChatSetting").GetComponent<BCBM_UICheckbox>();
		mScreenSleepSet = base.transform.Find("ScreenSleepSetting").GetComponent<BCBM_UICheckbox>();
		initSetting();
		BCBM_LocalData.getInstance().applySetting();
		HideSetting();
	}

	private void Update()
	{
	}

	private void initSetting()
	{
		BCBM_MusicMngr.GetSingleton().SetGameMusicVolume(BCBM_GameInfo.getInstance().Setted.bIsGameVolum ? 1 : 0);
		BCBM_LuckyLion_SoundManager.GetSingleton().SetMusicVolume(BCBM_GameInfo.getInstance().Setted.bIsGameVolum ? 1 : 0);
		BCBM_LuckyLion_SoundManager.GetSingleton().SetSoundVolume(BCBM_GameInfo.getInstance().Setted.bIsButtonVolum ? 1 : 0);
		BCBM_SettedInfo setted = BCBM_GameInfo.getInstance().Setted;
		mGameVoiceSet.isChecked = setted.bIsGameVolum;
		mButtonVoiceSet.isChecked = setted.bIsButtonVolum;
		mPublicChatSet.isChecked = setted.bIsForbidPublicChat;
		mPrivateChatSet.isChecked = setted.bIsForbidPrivateChat;
		mScreenSleepSet.isChecked = setted.bIsScreenNeverSleep;
	}

	public void ShowSetting()
	{
		if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
		{
			BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound();
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
		if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
		{
			BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
		BCBM_SettedInfo setted = BCBM_GameInfo.getInstance().Setted;
		setted.bIsGameVolum = mGameVoiceSet.isChecked;
		setted.bIsButtonVolum = mButtonVoiceSet.isChecked;
		setted.bIsForbidPublicChat = mPublicChatSet.isChecked;
		setted.bIsForbidPrivateChat = mPrivateChatSet.isChecked;
		setted.bIsScreenNeverSleep = mScreenSleepSet.isChecked;
		BCBM_LocalData.getInstance().applySetting();
		BCBM_LocalData.getInstance().saveUserSetting();
		HideSetting();
	}

	public void _onClickOptionSetting(GameObject go)
	{
		if (go.name == "IsGameVoice")
		{
			BCBM_MusicMngr.GetSingleton().SetGameMusicVolume(mGameVoiceSet.isChecked ? 1 : 0);
			BCBM_LuckyLion_SoundManager.GetSingleton().SetMusicVolume(mGameVoiceSet.isChecked ? 1 : 0);
		}
		else if (go.name == "IsButtonVoice")
		{
			BCBM_LuckyLion_SoundManager.GetSingleton().SetSoundVolume(mButtonVoiceSet.isChecked ? 1 : 0);
		}
	}
}
