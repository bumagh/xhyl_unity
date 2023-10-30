using GameConfig;
using UnityEngine;
using UnityEngine.UI;

public class BZJX_DlgUserInfo : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spiPrivateChat;

	[SerializeField]
	private Sprite[] spiHonor;

	[SerializeField]
	private BZJX_UserIcon userIcon;

	private Text txtNameLabel;

	private Text txtNickname;

	private Text txtLevelLabel;

	private Text txtLevel;

	private Text txtCoinLabel;

	private Text txtCoin;

	private Text txtHonor;

	private Image imgUserIcon;

	private Image imgHonor;

	private Button btnPrivateChat;

	private BZJX_GameInfo gameInfo;

	private BZJX_DlgChat sptDlgChat;

	private int language;

	private int userId;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		gameInfo = BZJX_GameInfo.getInstance();
		language = gameInfo.Language;
		GetAndInitCompenent();
	}

	private void GetAndInitCompenent()
	{
		txtNameLabel = base.transform.Find("TxtNameLabel").GetComponent<Text>();
		txtNameLabel.text = ((language != 0) ? "Nickname:" : "昵称：");
		txtNickname = base.transform.Find("TxtNickname").GetComponent<Text>();
		txtLevelLabel = base.transform.Find("TxtLevelLabel").GetComponent<Text>();
		txtLevelLabel.text = ((language != 0) ? "Level:" : "等级：");
		txtLevel = base.transform.Find("TxtLevel").GetComponent<Text>();
		txtCoinLabel = base.transform.Find("TxtCoinLabel").GetComponent<Text>();
		txtCoinLabel.text = ((language != 0) ? "Coins" : "游戏币：");
		txtCoin = base.transform.Find("TxtCoin").GetComponent<Text>();
		txtHonor = base.transform.Find("TxtHonor").GetComponent<Text>();
		imgUserIcon = base.transform.Find("ImgIcon").GetComponent<Image>();
		imgHonor = base.transform.Find("ImgHonor").GetComponent<Image>();
		imgHonor.sprite = spiHonor[language];
		imgHonor.SetNativeSize();
		imgHonor.transform.localScale = ((language != 0) ? (Vector3.one * 0.9f) : (Vector3.one * 0.85f));
		btnPrivateChat = base.transform.Find("BtnPrivateChat").GetComponent<Button>();
		btnPrivateChat.image.sprite = spiPrivateChat[language];
	}

	public void ClickBtnPrivateChat()
	{
		BZJX_SoundManage.getInstance().playButtonMusic(BZJX_ButtonMusicType.common);
		base.gameObject.SetActive(value: false);
		sptDlgChat.SetPrivateChat(isPrivate: true, txtNickname.text, userId);
		sptDlgChat.ShowChat();
	}

	public void ShowUserInfo(BZJX_UserInfo user, int honor)
	{
		base.gameObject.SetActive(value: true);
		imgUserIcon.sprite = userIcon.spiIcon[user.Icon - 1];
		txtCoin.text = user.CoinCount.ToString();
		txtLevel.text = "Lv" + user.Level + "(" + BZJX_TitleName.names[user.Level - 1] + ")";
		if (honor < 1 || honor > 10)
		{
			txtHonor.text = ((language != 0) ? "Failed to enter the ranking" : "未上榜");
		}
		else
		{
			txtHonor.text = ((language != 0) ? ("LiKui Fish：No." + honor.ToString()) : ("李逵劈鱼：No." + honor.ToString()));
		}
		txtNickname.text = user.Name;
		userId = user.Id;
	}
}
