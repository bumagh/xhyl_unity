using UnityEngine;
using UnityEngine.UI;

public class SPA_UserViewer : MonoBehaviour
{
	private Image _userIcon;

	private Text _textNickName;

	private Text _textLevelInfo;

	private Text _textCredit;

	private Text _textHonor;

	private bool _aniLock;

	private Button button;

	private void Awake()
	{
		_userIcon = base.transform.Find("ImgPerson").GetComponent<Image>();
		_textNickName = base.transform.Find("TxtName").GetComponent<Text>();
		_textLevelInfo = base.transform.Find("TxtLevel").GetComponent<Text>();
		_textCredit = base.transform.Find("TxtScore").GetComponent<Text>();
		_textHonor = base.transform.Find("TxtHonor").GetComponent<Text>();
		button = base.transform.GetComponent<Button>();
		button.onClick.AddListener(OnBtnBg_Click);
	}

	public void ShowViewer(string nickName, int level, int score, int photoId, int[] honor)
	{
		if (!_aniLock)
		{
			_aniLock = true;
			base.gameObject.SetActive(value: true);
			_initViewer(nickName, level, score, photoId, honor);
			_aniLock = false;
		}
	}

	private void _initViewer(string nickName, int level, int score, int photoId, int[] honor)
	{
		_textNickName.text = nickName;
		_textLevelInfo.text = $"Lv.{level}";
		_textCredit.text = score.ToString();
		string text = string.Empty;
		if (honor[2] != -1)
		{
			text += $"日：NO.{honor[2]}    ";
		}
		if (honor[1] != -1)
		{
			text += $"周：NO.{honor[1]}    ";
		}
		if (honor[0] != -1)
		{
			text += $"总：NO.{honor[0]}";
		}
		if (honor[0] + honor[1] + honor[2] == -3)
		{
			text = "未上榜";
		}
		_textHonor.text = text;
		_userIcon.sprite = SPA_MB_Singleton<SPA_HeadViewController>.GetInstance().GetUserIconSprites()[photoId - 1];
	}

	public void Hide()
	{
		if (!_aniLock)
		{
			_aniLock = true;
			base.gameObject.SetActive(value: false);
			_aniLock = false;
		}
	}

	public void OnBtnBg_Click()
	{
		Hide();
	}

	public bool IsShow()
	{
		return base.gameObject.activeSelf;
	}
}
