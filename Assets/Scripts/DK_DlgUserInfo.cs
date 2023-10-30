using GameConfig;
using UnityEngine;
using UnityEngine.UI;

public class DK_DlgUserInfo : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spiPrivateChat;

	[SerializeField]
	private Sprite[] spiHonor;

	[SerializeField]
	private DK_UserIcon userIcon;

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

	private DK_GameInfo gameInfo;

	private DK_DlgChat sptDlgChat;

	private int language;

	private int userId;

	private void Awake()
	{
		gameInfo = DK_GameInfo.getInstance();
		userIcon = gameInfo.UIScene.userIcon;
		language = gameInfo.Language;
		GetAndInitCompenent();
		base.transform.localScale = Vector3.one;
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
		DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.common);
		base.gameObject.SetActive(value: false);
		sptDlgChat.SetPrivateChat(isPrivate: true, txtNickname.text, userId);
		sptDlgChat.ShowChat();
	}

	public void ShowUserInfo(DK_UserInfo user, int honor)
	{
		base.gameObject.SetActive(value: true);
		int num = user.Icon - 1;
		if (num >= userIcon.spiIcon.Length || num < 0)
		{
			num = 0;
			user.Icon = 1;
		}
		imgUserIcon.sprite = userIcon.spiIcon[num];
		txtCoin.text = user.CoinCount.ToString();
		int num2 = user.Level - 1;
		if (num2 >= STMF_TitleName.names.Length || num2 < 0)
		{
			num2 = (user.Level = 0);
		}
		txtLevel.text = "Lv" + user.Level + "(" + DK_TitleName.names[num2] + ")";
		if (honor < 1 || honor > 10)
		{
			txtHonor.text = ((language != 0) ? "Failed to enter the ranking" : "未上榜");
		}
		else
		{
			txtHonor.text = ((language != 0) ? ("DemonKing：No." + honor.ToString()) : ("牛魔王：No." + honor.ToString()));
		}
		txtNickname.text = user.Name;
		userId = user.Id;
	}
}
