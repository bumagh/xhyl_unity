using DG.Tweening;
using STDT_GameConfig;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_GameBtnsList : MonoBehaviour
{
	public Sprite[] spiBtnSet;

	public Sprite[] spiBtnChat;

	public Sprite[] spiBtnInOut;

	public Sprite[] spiBtnBack;

	private Transform tfArrow;

	private GameObject goMask;

	private Transform tfBtns;

	private bool bShow;

	private bool bCanClick = true;

	public Button btnBg;

	private Button btnBack;

	private Button btnSet;

	private Button btnInOut;

	private Button btnChat;

	private void Awake()
	{
		tfArrow = base.transform.Find("BtnArrow");
		goMask = base.transform.Find("Mask").gameObject;
		tfBtns = goMask.transform.Find("Btns");
		btnBack = tfBtns.Find("BtnBack").GetComponent<Button>();
		btnSet = tfBtns.Find("BtnSet").GetComponent<Button>();
		btnInOut = tfBtns.Find("BtnInOut").GetComponent<Button>();
		btnChat = tfBtns.Find("BtnChat").GetComponent<Button>();
		btnBg.onClick.AddListener(HideBtns);
		btnBack.onClick.AddListener(delegate
		{
			ClickBtns(btnBack);
		});
		btnSet.onClick.AddListener(delegate
		{
			ClickBtns(btnSet);
		});
		btnInOut.onClick.AddListener(delegate
		{
			ClickBtns(btnInOut);
		});
		btnChat.onClick.AddListener(delegate
		{
			ClickBtns(btnChat);
		});
	}

	private void Start()
	{
		int language = XLDT_GameInfo.getInstance().Language;
		btnBack.image.sprite = spiBtnBack[language];
		btnSet.image.sprite = spiBtnSet[language];
		btnInOut.image.sprite = spiBtnInOut[language];
		btnChat.image.sprite = spiBtnChat[language];
	}

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.Escape))
		{
			return;
		}
		if (XLDT_GameUIMngr.GetSingleton().mCurDlgType == XLDT_POP_DLG_TYPE.DLG_NONE)
		{
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
			if (bShow)
			{
				HideBtns();
			}
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.IsExitGame, 0, string.Empty);
		}
		else
		{
			XLDT_GameUIMngr.GetSingleton().OnDlgBlackClick();
		}
	}

	public void ShowBtns()
	{
		MonoBehaviour.print(XLDT_GameUIMngr.GetSingleton().mCurDlgType);
		if (XLDT_GameUIMngr.GetSingleton().mCurDlgType != XLDT_POP_DLG_TYPE.DLG_NONE)
		{
			XLDT_GameUIMngr.GetSingleton().OnDlgBlackClick();
		}
		bShow = !bShow;
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
		tfArrow.localEulerAngles = Vector3.back * 180f;
		btnBg.gameObject.SetActive(value: true);
		goMask.SetActive(value: true);
		tfBtns.DOLocalMoveY(-100f, 0.2f);
		tfBtns.DOLocalMoveY(50f, 0.1f).SetDelay(0.2f);
		tfBtns.DOLocalMoveY(0f, 0.05f).SetDelay(0.3f).OnComplete(delegate
		{
			bCanClick = true;
		});
	}

	public void HideBtns()
	{
		if (!(tfArrow.localEulerAngles == Vector3.zero))
		{
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
			bShow = !bShow;
			tfArrow.localEulerAngles = Vector3.zero;
			btnBg.gameObject.SetActive(value: false);
			tfBtns.DOLocalMoveY(50f, 0.05f);
			tfBtns.DOLocalMoveY(-100f, 0.1f).SetDelay(0.05f);
			tfBtns.DOLocalMoveY(380f, 0.2f).SetDelay(0.15f).OnComplete(delegate
			{
				bCanClick = true;
				goMask.SetActive(value: false);
			});
		}
	}

	public void ShowOrHide()
	{
		if (bCanClick)
		{
			bCanClick = false;
			if (bShow)
			{
				HideBtns();
			}
			else
			{
				ShowBtns();
			}
		}
	}

	private void ClickBtns(Button btn)
	{
		HideBtns();
		string name = btn.name;
		if (name == null)
		{
			return;
		}
		if (!(name == "BtnBack"))
		{
			if (!(name == "BtnChat"))
			{
				if (!(name == "BtnInOut"))
				{
					if (name == "BtnSet")
					{
						XLDT_GameUIMngr.GetSingleton().ShowSetDlg(isShow: true);
					}
				}
				else
				{
					XLDT_GameUIMngr.GetSingleton().ShowMoneyInOutDlg();
				}
			}
			else
			{
				XLDT_GameUIMngr.GetSingleton().ShowChatDlg();
			}
		}
		else
		{
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.IsExitGame, 0, string.Empty);
		}
	}
}
