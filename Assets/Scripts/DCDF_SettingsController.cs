using UnityEngine;
using UnityEngine.UI;

public class DCDF_SettingsController : DCDF_MB_Singleton<DCDF_SettingsController>
{
	private GameObject _goContainer;

	private Slider sliMusic;

	private Slider sliSound;

	private Button btnMusicSub;

	private Button btnMusicMul;

	private Button btnSoundSub;

	private Button btnSoundMul;

	private Button btnNoticeOn;

	private Button btnNoticeOff;

	private GameObject objOnOff0;

	private GameObject objOnOff1;

	private float fMusic;

	private float fSound;

	private int iNotice;

	private void Awake()
	{
		if (DCDF_MB_Singleton<DCDF_SettingsController>._instance == null)
		{
			DCDF_MB_Singleton<DCDF_SettingsController>.SetInstance(this);
			PreInit();
		}
	}

	private void OnEnable()
	{
		Init();
	}

	public void PreInit()
	{
		if (_goContainer == null)
		{
			_goContainer = base.gameObject;
			Transform transform = base.transform.Find("Music");
			sliMusic = transform.Find("Slider").GetComponent<Slider>();
			btnMusicSub = transform.Find("BtnSub").GetComponent<Button>();
			btnMusicMul = transform.Find("BtnMul").GetComponent<Button>();
			Transform transform2 = base.transform.Find("Sound");
			sliSound = transform2.Find("Slider").GetComponent<Slider>();
			btnSoundSub = transform2.Find("BtnSub").GetComponent<Button>();
			btnSoundMul = transform2.Find("BtnMul").GetComponent<Button>();
			btnNoticeOn = base.transform.Find("BtnOn").GetComponent<Button>();
			btnNoticeOff = base.transform.Find("BtnOff").GetComponent<Button>();
			objOnOff0 = base.transform.Find("OnOff0").gameObject;
			objOnOff1 = base.transform.Find("OnOff1").gameObject;
			sliMusic.onValueChanged.AddListener(delegate
			{
				MusicVolValueChange();
			});
			sliSound.onValueChanged.AddListener(delegate
			{
				SoundVolValueChange();
			});
			btnMusicMul.onClick.AddListener(delegate
			{
				ClickBtnChange(sliMusic, 1f);
			});
			btnMusicSub.onClick.AddListener(delegate
			{
				ClickBtnChange(sliMusic, -1f);
			});
			btnSoundMul.onClick.AddListener(delegate
			{
				ClickBtnChange(sliSound, 1f);
			});
			btnSoundSub.onClick.AddListener(delegate
			{
				ClickBtnChange(sliSound, -1f);
			});
			btnNoticeOn.onClick.AddListener(ClickBtnNoticeOn);
			btnNoticeOff.onClick.AddListener(ClickBtnNoticeOff);
		}
	}

	public void Init()
	{
		InitMusicPlayerpef();
		InitSoundPlayerpef();
		InitNoticePlayerpef();
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	public void InitMusicPlayerpef()
	{
		if (PlayerPrefs.HasKey("fMusic"))
		{
			fMusic = PlayerPrefs.GetInt("fMusic");
		}
		else
		{
			fMusic = 1f;
		}
		sliMusic.value = fMusic * 10f;
		DCDF_SoundManager.Instance.SetAudioSourceVolum(DCDF_SoundManager.Instance.ads_BGM, fMusic);
	}

	public void InitSoundPlayerpef()
	{
		if (PlayerPrefs.HasKey("fSound"))
		{
			fSound = PlayerPrefs.GetInt("fSound");
		}
		else
		{
			fSound = 1f;
		}
		sliSound.value = fSound * 10f;
		DCDF_SoundManager.Instance.SetAudioSourceVolum(DCDF_SoundManager.Instance.ads_Click, fSound);
		DCDF_SoundManager.Instance.SetAudioSourceVolum(DCDF_SoundManager.Instance.ads_Major, fSound);
		DCDF_SoundManager.Instance.SetAudioSourceVolum(DCDF_SoundManager.Instance.ads_Result, fSound);
	}

	public void InitNoticePlayerpef()
	{
		if (PlayerPrefs.HasKey("iNotice"))
		{
			iNotice = PlayerPrefs.GetInt("iNotice");
		}
		else
		{
			iNotice = 1;
		}
		if (iNotice == 0)
		{
			objOnOff0.SetActive(value: false);
			objOnOff1.SetActive(value: true);
			DCDF_MySqlConnection.bNoticeOff = true;
		}
		else if (iNotice == 1)
		{
			objOnOff0.SetActive(value: true);
			objOnOff1.SetActive(value: false);
			DCDF_MySqlConnection.bNoticeOff = false;
		}
	}

	public void ClickBtnChange(Slider sli, float i)
	{
		float value = sli.value;
		value += i;
		if (!(value > 10f) && !(value < 0f))
		{
			sli.value = value;
		}
	}

	public void MusicVolValueChange()
	{
		fMusic = sliMusic.value / 10f;
		DCDF_SoundManager.Instance.SetAudioSourceVolum(DCDF_SoundManager.Instance.ads_BGM, fMusic);
		PlayerPrefs.SetFloat("fMusic", fMusic);
		PlayerPrefs.Save();
	}

	public void SoundVolValueChange()
	{
		fSound = sliSound.value / 10f;
		DCDF_SoundManager.Instance.SetAudioSourceVolum(DCDF_SoundManager.Instance.ads_Click, fSound);
		DCDF_SoundManager.Instance.SetAudioSourceVolum(DCDF_SoundManager.Instance.ads_Major, fSound);
		DCDF_SoundManager.Instance.SetAudioSourceVolum(DCDF_SoundManager.Instance.ads_Result, fSound);
		PlayerPrefs.SetFloat("fSound", fSound);
		PlayerPrefs.Save();
	}

	public void ClickBtnNoticeOn()
	{
		if (!DCDF_MySqlConnection.tryLockOnePoint && DCDF_MySqlConnection.bNoticeOff)
		{
			DCDF_SoundManager.Instance.PlayClickAudio();
			DCDF_MySqlConnection.bNoticeOff = false;
			PlayerPrefs.SetInt("iNotice", 1);
			PlayerPrefs.Save();
		}
	}

	public void ClickBtnNoticeOff()
	{
		if (!DCDF_MySqlConnection.tryLockOnePoint && !DCDF_MySqlConnection.bNoticeOff)
		{
			DCDF_SoundManager.Instance.PlayClickAudio();
			DCDF_MySqlConnection.bNoticeOff = true;
			DCDF_MB_Singleton<DCDF_NoticeController>.GetInstance().Hide();
			PlayerPrefs.SetInt("iNotice", 0);
			PlayerPrefs.Save();
		}
	}
}
