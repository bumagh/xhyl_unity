using STDT_GameConfig;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_DlgPersonInfo : MonoBehaviour
{
	public XLDT_UserPhoto uPhoto;

	public Sprite[] spiImgNickname;

	public Sprite[] spiImgLevel;

	public Sprite[] spiImgScore;

	public Sprite[] spiImgHonor;

	public Sprite[] spiBtnPrivate;

	[HideInInspector]
	public Text txtNickname;

	[HideInInspector]
	public Text txtLevel;

	[HideInInspector]
	public Text txtScore;

	[HideInInspector]
	public Text txtHonour;

	[HideInInspector]
	public Button btnPrivate;

	[HideInInspector]
	public Image imgPhoto;

	private int mUserID;

	private void Awake()
	{
		InitDlg();
	}

	private void Start()
	{
		int language = XLDT_Localization.language;
		btnPrivate.GetComponent<Image>().sprite = spiBtnPrivate[language];
		base.transform.Find("ImgNicknameLable").GetComponent<Image>().sprite = spiImgNickname[language];
		base.transform.Find("ImgScoreLable").GetComponent<Image>().sprite = spiImgScore[language];
		base.transform.Find("ImgLevelLable").GetComponent<Image>().sprite = spiImgLevel[language];
		base.transform.Find("ImgHonorBg").GetComponent<Image>().sprite = spiImgHonor[language];
	}

	private void InitDlg()
	{
		txtNickname = base.transform.Find("TxtNickname").GetComponent<Text>();
		txtLevel = base.transform.Find("TxtLevel").GetComponent<Text>();
		txtScore = base.transform.Find("TxtScore").GetComponent<Text>();
		txtHonour = base.transform.Find("TxtHonor").GetComponent<Text>();
		btnPrivate = base.transform.Find("BtnPrivate").GetComponent<Button>();
		imgPhoto = base.transform.Find("ImgPhoto").GetComponent<Image>();
		btnPrivate.onClick.AddListener(ClickBtnPrivate);
	}

	public void OnBlackClick()
	{
		SendMessageUpwards("OnDlgBlackClick", XLDT_POP_DLG_TYPE.DLG_PERSON_INFO);
	}

	public void ClickBtnPrivate()
	{
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		XLDT_GameUIMngr.GetSingleton().ShowPersonInfoDlg(isShow: false, 0, string.Empty, 0, 0, 0, string.Empty);
		XLDT_GameUIMngr.GetSingleton().ShowChatDlg(isShow: true, isPrivateChat: true, txtNickname.text, mUserID);
	}

	public void ShowPersonInfo(int userId, string nickName, int nPhotoIndex, int nLevel, int nGameCoin, string honour)
	{
		if (nPhotoIndex > 4 || nPhotoIndex < 1)
		{
			nPhotoIndex = 1;
		}
		if (nLevel >= XLDT_TitleName.names.Length || nLevel < 0)
		{
			nLevel = 0;
		}
		btnPrivate.gameObject.SetActive(value: true);
		mUserID = userId;
		txtNickname.text = nickName;
		imgPhoto.sprite = uPhoto.spiPhoto[nPhotoIndex - 1];
		txtLevel.text = XLDT_TitleName.names[nLevel] + "(LV." + (nLevel + 1).ToString() + ")";
		txtScore.text = XLDT_DanTiaoCommon.ChangeNumber(nGameCoin);
		txtHonour.text = honour;
	}
}
