using DG.Tweening;
using STDT_GameConfig;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_DlgSetting : MonoBehaviour
{
	public Sprite[] spiBtnConfirm;

	private Toggle isGameVolum;

	private Toggle isButtonVolum;

	private Toggle isForbidPublicChat;

	private Toggle isForbidPrivateChat;

	private Toggle isScreenNeverSleep;

	public XLDT_DlgBase sptDlgBase;

	private void Start()
	{
		InitDlg();
		ini();
	}

	private void ini()
	{
		if (XLDT_GameInfo.getInstance().Setted == null)
		{
			XLDT_GameInfo.getInstance().Setted = XLDT_localData.getInstance().ReadUserSetting();
		}
		isGameVolum.isOn = XLDT_GameInfo.getInstance().Setted.bIsGameVolum;
		isButtonVolum.isOn = XLDT_GameInfo.getInstance().Setted.bIsButtonVolum;
		isForbidPublicChat.isOn = XLDT_GameInfo.getInstance().Setted.bIsForbidPublicChat;
		isForbidPrivateChat.isOn = XLDT_GameInfo.getInstance().Setted.bIsForbidPrivateChat;
		isScreenNeverSleep.isOn = XLDT_GameInfo.getInstance().Setted.bIsScreenNeverSleep;
		XLDT_localData.getInstance().applySetting();
	}

	public void SaveUserSetting()
	{
		XLDT_GameInfo.getInstance().Setted.bIsGameVolum = isGameVolum.isOn;
		XLDT_GameInfo.getInstance().Setted.bIsButtonVolum = isButtonVolum.isOn;
		XLDT_GameInfo.getInstance().Setted.bIsForbidPrivateChat = isForbidPrivateChat.isOn;
		XLDT_GameInfo.getInstance().Setted.bIsForbidPublicChat = isForbidPublicChat.isOn;
		XLDT_GameInfo.getInstance().Setted.bIsScreenNeverSleep = isScreenNeverSleep.isOn;
		XLDT_localData.getInstance().applySetting();
		XLDT_localData.getInstance().saveUserSetting();
	}

	private void InitDlg()
	{
		Transform[] array = new Transform[5];
		for (int i = 0; i < 5; i++)
		{
			array[i] = base.transform.Find($"SetItem{i}/ImgBg");
		}
		isGameVolum = array[0].Find("Toggle").GetComponent<Toggle>();
		isButtonVolum = array[1].Find("Toggle").GetComponent<Toggle>();
		isForbidPrivateChat = array[2].Find("Toggle").GetComponent<Toggle>();
		isForbidPublicChat = array[3].Find("Toggle").GetComponent<Toggle>();
		isScreenNeverSleep = array[4].Find("Toggle").GetComponent<Toggle>();
		array[0].Find("Text").GetComponent<Text>().text = XLDT_Localization.Get("SetMusic");
		array[1].Find("Text").GetComponent<Text>().text = XLDT_Localization.Get("SetBtnSoundFx");
		array[2].Find("Text").GetComponent<Text>().text = XLDT_Localization.Get("SetFobidAllChat");
		array[3].Find("Text").GetComponent<Text>().text = XLDT_Localization.Get("SetFobidPrivateChat");
		array[4].Find("Text").GetComponent<Text>().text = XLDT_Localization.Get("SetScreenLight");
		Button component = base.transform.Find("BtnConfirm").GetComponent<Button>();
		component.GetComponent<Image>().sprite = spiBtnConfirm[XLDT_Localization.language];
		component.onClick.AddListener(ClickBtnConfirm);
	}

	public void OnBtnClick(GameObject go)
	{
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		SaveUserSetting();
	}

	public void ClickBtnConfirm()
	{
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		SaveUserSetting();
		XLDT_GameUIMngr.GetSingleton().ShowSetDlg(isShow: false);
	}

	public void OnToggleClick(Transform tf)
	{
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		SaveUserSetting();
		tf.DOScale(1.2f, 0.04f);
		tf.DOScale(1f, 0.04f).SetDelay(0.04f);
	}

	public void OnBlackClick()
	{
		SendMessageUpwards("OnDlgBlackClick", XLDT_POP_DLG_TYPE.DLG_SETTING);
	}
}
