using DG.Tweening;
using STDT_GameConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_UserCtrl : MonoBehaviour
{
	public XLDT_UserPhoto uPhoto;

	public Sprite[] spiImgBg;

	public Sprite[] spiImgLit;

	private Text txtTableId;

	private Text txtNickname;

	private Text txtChat;

	private Image imgBg;

	private Image imgPhoto;

	private Image imgLit;

	private Image imgChat;

	private Button btn;

	private bool mBubleEnd = true;

	public Queue<string> queChat;

	private string mUserName = string.Empty;

	private WaitForSeconds wait;

	private bool mIsShowScore;

	private int mScore;

	protected int _index;

	public int Index
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
			if (!txtTableId)
			{
				txtTableId = base.transform.Find("TxtTableId").GetComponent<Text>();
			}
			txtTableId.text = (_index + 1).ToString();
		}
	}

	private void Awake()
	{
		queChat = new Queue<string>();
		txtTableId = base.transform.Find("TxtTableId").GetComponent<Text>();
		txtNickname = base.transform.Find("TxtNickname").GetComponent<Text>();
		txtChat = base.transform.Find("Chat/ImgChat/Mask1/TxtChat").GetComponent<Text>();
		imgBg = base.transform.Find("ImgBg").GetComponent<Image>();
		imgPhoto = base.transform.Find("ImgPhoto").GetComponent<Image>();
		imgLit = base.transform.Find("ImgLit").GetComponent<Image>();
		imgChat = base.transform.Find("Chat/ImgChat").GetComponent<Image>();
		btn = base.transform.Find("Btn").GetComponent<Button>();
		btn.onClick.AddListener(OnUserClick);
		wait = new WaitForSeconds(0.1f);
	}

	private void Update()
	{
		_updateChatWords();
	}

	public void Hide()
	{
		imgBg.sprite = spiImgBg[0];
		for (int i = 1; i < 6; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(value: false);
		}
		mUserName = string.Empty;
		playNameLit(isShow: false);
	}

	public void Show(int icon, string name, bool isPlayer = false)
	{
		mUserName = name;
		imgBg.sprite = spiImgBg[1];
		if (mIsShowScore)
		{
			txtNickname.text = mScore.ToString();
		}
		else
		{
			txtNickname.text = ZH2_GVars.GetBreviaryName(name);
		}
		if (icon - 1 < 0 || icon - 1 >= uPhoto.spiPhoto.Length)
		{
			UnityEngine.Debug.LogError("头像id异常! " + icon);
			icon = uPhoto.spiPhoto.Length;
		}
		imgPhoto.sprite = uPhoto.spiPhoto[icon - 1];
		for (int i = 1; i < 6; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(value: true);
		}
	}

	public void ShowWinScore(bool isShow, int score = 0)
	{
		if (isShow)
		{
			mIsShowScore = true;
			mScore = score;
			txtNickname.text = XLDT_DanTiaoCommon.ChangeNumber(score);
			playNameLit(isShow: true);
			if (score > 0)
			{
				txtNickname.color = Color.green;
			}
			else if (score == 0)
			{
				txtNickname.color = Color.white;
			}
			else
			{
				txtNickname.color = Color.red;
			}
		}
		else
		{
			if (mIsShowScore)
			{
				playNameLit(isShow: true);
			}
			else
			{
				playNameLit(isShow: false);
			}
			mScore = 0;
			mIsShowScore = false;
			txtNickname.text = ZH2_GVars.GetBreviaryName(mUserName);
			txtNickname.color = Color.white;
		}
	}

	public void ShowChat(string words)
	{
		queChat.Enqueue(words);
	}

	private void _updateChatWords()
	{
		if (mBubleEnd && queChat.Count > 0)
		{
			mBubleEnd = false;
			imgChat.gameObject.SetActive(value: true);
			string text = queChat.Dequeue();
			txtChat.text = text;
			Vector2 sizeDelta = txtChat.rectTransform.sizeDelta;
			float y = sizeDelta.y;
			float num = 53f;
			float num2 = (y + num) / 2f;
			txtChat.transform.localPosition = Vector3.down * num2;
			float num3 = 30f;
			txtChat.transform.DOLocalMoveY(num2, num2 * 2f / num3).OnComplete(OnWordsEnd);
		}
	}

	public void OnWordsEnd()
	{
		mBubleEnd = true;
		imgChat.gameObject.SetActive(value: false);
	}

	public void OnUserClick()
	{
		if (XLDT_GameInfo.getInstance().User.SeatIndex - 1 != Index)
		{
			XLDT_NetMain.GetSingleton().MyCreateSocket.SendPlayerInfo(XLDT_GameInfo.getInstance().UserList[Index].Id);
		}
	}

	public void playNameLit(bool isShow)
	{
		if (isShow)
		{
			StopCoroutine("_nameLitAnimUpdate");
			StartCoroutine("_nameLitAnimUpdate");
		}
		else
		{
			StopCoroutine("_nameLitAnimUpdate");
			imgLit.enabled = false;
		}
	}

	private IEnumerator _nameLitAnimUpdate()
	{
		imgLit.enabled = true;
		for (int i = 0; i < 4; i++)
		{
			imgLit.sprite = spiImgLit[i];
			yield return wait;
		}
		imgLit.enabled = false;
	}
}
