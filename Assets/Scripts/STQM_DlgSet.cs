using UnityEngine;
using UnityEngine.UI;

public class STQM_DlgSet : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spiImgSet;

	private Image imgSet;

	private Toggle togMusic;

	private Toggle togSound;

	private Toggle togGroupChat;

	private Toggle togPrivateChat;

	private Toggle togScreen;

	private Button btnConfirm;

	private STQM_GameInfo gameInfo;

	private int language;

	private void Awake()
	{
		gameInfo = STQM_GameInfo.getInstance();
		language = gameInfo.Language;
		GetAndInitCompenent();
	}

	private void GetAndInitCompenent()
	{
		imgSet = base.transform.Find("ImgSet").GetComponent<Image>();
		imgSet.sprite = spiImgSet[language];
		togMusic = imgSet.transform.Find("TogMusic").GetComponent<Toggle>();
		togSound = imgSet.transform.Find("TogSound").GetComponent<Toggle>();
		togGroupChat = imgSet.transform.Find("TogGroupChat").GetComponent<Toggle>();
		togPrivateChat = imgSet.transform.Find("TogPrivateChat").GetComponent<Toggle>();
		togScreen = imgSet.transform.Find("TogScreen").GetComponent<Toggle>();
		btnConfirm = base.transform.Find("BtnConfirm").GetComponent<Button>();
		btnConfirm.onClick.AddListener(ClickBtnConfirm);
	}

	public void ShowSet()
	{
		STQM_SettedInfo setted = gameInfo.Setted;
		togMusic.isOn = setted.bIsGameVolum;
		togSound.isOn = setted.bIsButtonVolum;
		togGroupChat.isOn = setted.bIsForbidPublicChat;
		togPrivateChat.isOn = setted.bIsForbidPrivateChat;
		togScreen.isOn = setted.bIsScreenNeverSleep;
		base.gameObject.SetActive(value: true);
	}

	private void ClickBtnConfirm()
	{
		SaveUserSetting();
	}

	public void SaveUserSetting()
	{
		STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.common);
		STQM_SettedInfo setted = gameInfo.Setted;
		setted.bIsGameVolum = togMusic.isOn;
		setted.bIsButtonVolum = togSound.isOn;
		setted.bIsForbidPublicChat = togGroupChat.isOn;
		setted.bIsForbidPrivateChat = togPrivateChat.isOn;
		setted.bIsScreenNeverSleep = togScreen.isOn;
		STQM_LocalData.getInstance().applySetting();
		STQM_LocalData.getInstance().saveUserSetting();
		base.gameObject.SetActive(value: false);
	}
}
