using GameConfig;
using UnityEngine;
using UnityEngine.UI;

public class TF_DlgUserInfo : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spiPrivateChat;

	[SerializeField]
	private Sprite[] spiHonor;

	[SerializeField]
	private TF_UserIcon userIcon;

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

	private TF_GameInfo gameInfo;

	private TF_DlgChat sptDlgChat;

	private int language;

	private int userId;

	private void Awake()
	{
		gameInfo = TF_GameInfo.getInstance();
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
		sptDlgChat = base.transform.parent.Find("ChatDialog").GetComponent<TF_DlgChat>();
	}

	public void ClickBtnPrivateChat()
	{
		TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.common);
		base.gameObject.SetActive(value: false);
		sptDlgChat.SetPrivateChat(isPrivate: true, txtNickname.text, userId);
		sptDlgChat.ShowChat();
	}

	public void ShowUserInfo(TF_UserInfo user, int honor)
	{
		imgUserIcon.sprite = userIcon.spiIcon[user.Icon - 1];
		txtCoin.text = user.CoinCount.ToString();
		txtLevel.text = "Lv" + user.Level + "(" + TF_TitleName.names[user.Level - 1] + ")";
		if (honor < 1 || honor > 10)
		{
			txtHonor.text = ((language != 0) ? "Failed to enter the ranking" : "未上榜");
		}
		else
		{
			txtHonor.text = ((language != 0) ? ("ToadFishing：No." + honor.ToString()) : ("金蟾捕鱼：No." + honor.ToString()));
		}
		txtNickname.text = user.Name;
		userId = user.Id;
		base.gameObject.SetActive(value: true);
	}
}
