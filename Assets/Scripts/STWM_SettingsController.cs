using UnityEngine;
using UnityEngine.UI;

public class STWM_SettingsController : STWM_MB_Singleton<STWM_SettingsController>
{
	[SerializeField]
	private Sprite[] sprites;

	[SerializeField]
	private Sprite[] sprites_en;

	private GameObject _goContainer;

	private Image _img_BGMOpen;

	private Image _img_BGMClose;

	private Image _img_ClickAudioOpen;

	private Image _img_ClickAudioClose;

	private Image _img_ScreenOpen;

	private Image _img_ScreenClose;

	private Text txtBGM;

	private Text txtBTS;

	private Text txtSUL;

	private Text txtBtnConfirm;

	private int language;

	private int _iBGM;

	private int _iClickAudio;

	private int _iScreenOn;

	private void Awake()
	{
		_goContainer = base.gameObject;
		base.transform.localScale = Vector3.one;
		if (STWM_MB_Singleton<STWM_SettingsController>._instance == null)
		{
			STWM_MB_Singleton<STWM_SettingsController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		Init();
	}

	public void PreInit()
	{
		if (_goContainer == null)
		{
			_goContainer = base.gameObject;
		}
		_img_BGMOpen = base.transform.Find("BtnBGMOpen").GetComponent<Image>();
		_img_BGMClose = base.transform.Find("BtnBGMClose").GetComponent<Image>();
		_img_ClickAudioOpen = base.transform.Find("BtnBTSOpen").GetComponent<Image>();
		_img_ClickAudioClose = base.transform.Find("BtnBTSClose").GetComponent<Image>();
		_img_ScreenOpen = base.transform.Find("BtnSULOpen").GetComponent<Image>();
		_img_ScreenClose = base.transform.Find("BtnSULClose").GetComponent<Image>();
		txtBGM = base.transform.Find("TxtBGM").GetComponent<Text>();
		txtBTS = base.transform.Find("TxtBTS").GetComponent<Text>();
		txtSUL = base.transform.Find("TxtSUL").GetComponent<Text>();
		txtBtnConfirm = base.transform.Find("BtnConfirm/Text").GetComponent<Text>();
	}

	public void Init()
	{
		language = ((STWM_GVars.language == "en") ? 1 : 0);
		InitBGMPlayerpef();
		InitClickPlayerpef();
		InitScreenAlwaysOnPlayerpef();
		txtBGM.text = ((language != 0) ? "BG  Music" : "背景音乐");
		txtBTS.text = ((language != 0) ? "Sound Effect" : "按钮音效");
		txtSUL.text = ((language != 0) ? "Screen  On" : "屏幕常亮");
		txtBtnConfirm.text = ((language != 0) ? "Confirm" : "确定");
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	public void InitBGMPlayerpef()
	{
		if (PlayerPrefs.HasKey("BGM"))
		{
			_iBGM = PlayerPrefs.GetInt("BGM");
		}
		else
		{
			_iBGM = 1;
		}
		if (_iBGM == 0)
		{
			if (language == 0)
			{
				_img_BGMOpen.sprite = sprites[1];
				_img_BGMClose.sprite = sprites[2];
			}
			else
			{
				_img_BGMOpen.sprite = sprites_en[1];
				_img_BGMClose.sprite = sprites_en[2];
			}
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_LobbyBGM, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_DiceBGM, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_MaryBGM, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Dealer, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Guys, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Major, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Mary, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_NumberRoll, 0);
		}
		else if (_iBGM == 1)
		{
			if (language == 0)
			{
				_img_BGMOpen.sprite = sprites[0];
				_img_BGMClose.sprite = sprites[3];
			}
			else
			{
				_img_BGMOpen.sprite = sprites_en[0];
				_img_BGMClose.sprite = sprites_en[3];
			}
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_LobbyBGM, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_DiceBGM, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_MaryBGM, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Dealer, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Guys, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Major, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Mary, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_NumberRoll, 1);
		}
	}

	public void InitClickPlayerpef()
	{
		if (PlayerPrefs.HasKey("Click"))
		{
			_iClickAudio = PlayerPrefs.GetInt("Click");
		}
		else
		{
			_iClickAudio = 1;
		}
		if (_iClickAudio == 0)
		{
			if (language == 0)
			{
				_img_ClickAudioOpen.sprite = sprites[1];
				_img_ClickAudioClose.sprite = sprites[2];
			}
			else
			{
				_img_ClickAudioOpen.sprite = sprites_en[1];
				_img_ClickAudioClose.sprite = sprites_en[2];
			}
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Click, 0);
		}
		else if (_iClickAudio == 1)
		{
			if (language == 0)
			{
				_img_ClickAudioOpen.sprite = sprites[0];
				_img_ClickAudioClose.sprite = sprites[3];
			}
			else
			{
				_img_ClickAudioOpen.sprite = sprites_en[0];
				_img_ClickAudioClose.sprite = sprites_en[3];
			}
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Click, 1);
		}
	}

	public void InitScreenAlwaysOnPlayerpef()
	{
		if (PlayerPrefs.HasKey("Screen"))
		{
			_iScreenOn = PlayerPrefs.GetInt("Screen");
		}
		else
		{
			_iScreenOn = 1;
		}
		if (_iScreenOn == 0)
		{
			if (language == 0)
			{
				_img_ScreenOpen.sprite = sprites[1];
				_img_ScreenClose.sprite = sprites[2];
			}
			else
			{
				_img_ScreenOpen.sprite = sprites_en[1];
				_img_ScreenClose.sprite = sprites_en[2];
			}
			Screen.sleepTimeout = -2;
		}
		else if (_iScreenOn == 1)
		{
			if (language == 0)
			{
				_img_ScreenOpen.sprite = sprites[0];
				_img_ScreenClose.sprite = sprites[3];
			}
			else
			{
				_img_ScreenOpen.sprite = sprites_en[0];
				_img_ScreenClose.sprite = sprites_en[3];
			}
			Screen.sleepTimeout = -1;
		}
	}

	public void OnCloseOrMakeSureBtnDown()
	{
		if (!STWM_GVars.tryLockOnePoint)
		{
			Hide();
			STWM_SoundManager.Instance.PlayClickAudio();
		}
	}

	public void OnBGMOpenBtnDown()
	{
		if (!STWM_GVars.tryLockOnePoint)
		{
			if (language == 0)
			{
				_img_BGMOpen.sprite = sprites[0];
				_img_BGMClose.sprite = sprites[3];
			}
			else
			{
				_img_BGMOpen.sprite = sprites_en[0];
				_img_BGMClose.sprite = sprites_en[3];
			}
			STWM_SoundManager.Instance.PlayClickAudio();
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_LobbyBGM, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_DiceBGM, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_MaryBGM, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Dealer, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Guys, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Major, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Mary, 1);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_NumberRoll, 1);
			PlayerPrefs.SetInt("BGM", 1);
			PlayerPrefs.Save();
		}
	}

	public void OnBGMCloseBtnDown()
	{
		if (!STWM_GVars.tryLockOnePoint)
		{
			if (language == 0)
			{
				_img_BGMOpen.sprite = sprites[1];
				_img_BGMClose.sprite = sprites[2];
			}
			else
			{
				_img_BGMOpen.sprite = sprites_en[1];
				_img_BGMClose.sprite = sprites_en[2];
			}
			STWM_SoundManager.Instance.PlayClickAudio();
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_LobbyBGM, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_DiceBGM, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_MaryBGM, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Dealer, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Guys, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Major, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Mary, 0);
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_NumberRoll, 0);
			PlayerPrefs.SetInt("BGM", 0);
			PlayerPrefs.Save();
		}
	}

	public void OnClickAudioOpenBtnDown()
	{
		if (!STWM_GVars.tryLockOnePoint)
		{
			if (language == 0)
			{
				_img_ClickAudioOpen.sprite = sprites[0];
				_img_ClickAudioClose.sprite = sprites[3];
			}
			else
			{
				_img_ClickAudioOpen.sprite = sprites_en[0];
				_img_ClickAudioClose.sprite = sprites_en[3];
			}
			STWM_SoundManager.Instance.PlayClickAudio();
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Click, 1);
			PlayerPrefs.SetInt("Click", 1);
			PlayerPrefs.Save();
		}
	}

	public void OnClickAudioCloseBtnDown()
	{
		if (!STWM_GVars.tryLockOnePoint)
		{
			if (language == 0)
			{
				_img_ClickAudioOpen.sprite = sprites[1];
				_img_ClickAudioClose.sprite = sprites[2];
			}
			else
			{
				_img_ClickAudioOpen.sprite = sprites_en[1];
				_img_ClickAudioClose.sprite = sprites_en[2];
			}
			STWM_SoundManager.Instance.PlayClickAudio();
			STWM_SoundManager.Instance.SetAudioSourceVolum(STWM_SoundManager.Instance.ads_Click, 0);
			PlayerPrefs.SetInt("Click", 0);
			PlayerPrefs.Save();
		}
	}

	public void OnClickScreenAlwaysOnbtnDown()
	{
		if (!STWM_GVars.tryLockOnePoint)
		{
			if (language == 0)
			{
				_img_ScreenOpen.sprite = sprites[0];
				_img_ScreenClose.sprite = sprites[3];
			}
			else
			{
				_img_ScreenOpen.sprite = sprites_en[0];
				_img_ScreenClose.sprite = sprites_en[3];
			}
			STWM_SoundManager.Instance.PlayClickAudio();
			Screen.sleepTimeout = -1;
			PlayerPrefs.SetInt("Screen", 1);
			PlayerPrefs.Save();
		}
	}

	public void OnClickScreenAlawaysOnClosebtnDown()
	{
		if (!STWM_GVars.tryLockOnePoint)
		{
			if (language == 0)
			{
				_img_ScreenOpen.sprite = sprites[1];
				_img_ScreenClose.sprite = sprites[2];
			}
			else
			{
				_img_ScreenOpen.sprite = sprites_en[1];
				_img_ScreenClose.sprite = sprites_en[2];
			}
			STWM_SoundManager.Instance.PlayClickAudio();
			Screen.sleepTimeout = -2;
			PlayerPrefs.SetInt("Screen", 0);
			PlayerPrefs.Save();
		}
	}
}
