using UnityEngine;

public class LL_SettingPanel : MonoBehaviour
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

	protected LL_UICheckbox mGameVoiceSet;

	protected LL_UICheckbox mButtonVoiceSet;

	protected LL_UICheckbox mPublicChatSet;

	protected LL_UICheckbox mPrivateChatSet;

	protected LL_UICheckbox mScreenSleepSet;

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
		mGameVoiceSet = base.transform.Find("IsGameVoice").GetComponent<LL_UICheckbox>();
		mButtonVoiceSet = base.transform.Find("IsButtonVoice").GetComponent<LL_UICheckbox>();
		mPublicChatSet = base.transform.Find("PublicChatSetting").GetComponent<LL_UICheckbox>();
		mPrivateChatSet = base.transform.Find("PrivateChatSetting").GetComponent<LL_UICheckbox>();
		mScreenSleepSet = base.transform.Find("ScreenSleepSetting").GetComponent<LL_UICheckbox>();
		initSetting();
		LL_LocalData.getInstance().applySetting();
		HideSetting();
	}

	private void Update()
	{
	}

	private void initSetting()
	{
		LL_MusicMngr.GetSingleton().SetGameMusicVolume(LL_GameInfo.getInstance().Setted.bIsGameVolum ? 1 : 0);
		LL_LuckyLion_SoundManager.GetSingleton().SetMusicVolume(LL_GameInfo.getInstance().Setted.bIsGameVolum ? 1 : 0);
		LL_LuckyLion_SoundManager.GetSingleton().SetSoundVolume(LL_GameInfo.getInstance().Setted.bIsButtonVolum ? 1 : 0);
		LL_SettedInfo setted = LL_GameInfo.getInstance().Setted;
		mGameVoiceSet.isChecked = setted.bIsGameVolum;
		mButtonVoiceSet.isChecked = setted.bIsButtonVolum;
		mPublicChatSet.isChecked = setted.bIsForbidPublicChat;
		mPrivateChatSet.isChecked = setted.bIsForbidPrivateChat;
		mScreenSleepSet.isChecked = setted.bIsScreenNeverSleep;
	}

	public void ShowSetting()
	{
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		LL_GameTipManager.GetSingleton().HideAllPopupPanel();
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
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		LL_SettedInfo setted = LL_GameInfo.getInstance().Setted;
		setted.bIsGameVolum = mGameVoiceSet.isChecked;
		setted.bIsButtonVolum = mButtonVoiceSet.isChecked;
		setted.bIsForbidPublicChat = mPublicChatSet.isChecked;
		setted.bIsForbidPrivateChat = mPrivateChatSet.isChecked;
		setted.bIsScreenNeverSleep = mScreenSleepSet.isChecked;
		LL_LocalData.getInstance().applySetting();
		LL_LocalData.getInstance().saveUserSetting();
		HideSetting();
	}

	public void _onClickOptionSetting(GameObject go)
	{
		if (go.name == "IsGameVoice")
		{
			LL_MusicMngr.GetSingleton().SetGameMusicVolume(mGameVoiceSet.isChecked ? 1 : 0);
			LL_LuckyLion_SoundManager.GetSingleton().SetMusicVolume(mGameVoiceSet.isChecked ? 1 : 0);
		}
		else if (go.name == "IsButtonVoice")
		{
			LL_LuckyLion_SoundManager.GetSingleton().SetSoundVolume(mButtonVoiceSet.isChecked ? 1 : 0);
		}
	}
}
