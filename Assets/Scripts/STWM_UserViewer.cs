using UnityEngine;
using UnityEngine.UI;

public class STWM_UserViewer : MonoBehaviour
{
	[SerializeField]
	private Image _userIcon;

	[SerializeField]
	private Text _textNickName;

	[SerializeField]
	private Text _textLevelInfo;

	[SerializeField]
	private Text _textCredit;

	[SerializeField]
	private Text _textHonor;

	private bool _aniLock;

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
		_userIcon.sprite = STWM_MB_Singleton<STWM_HeadViewController>.GetInstance().GetUserIconSprites()[photoId - 1];
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
