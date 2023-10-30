using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MB_Singleton<SoundManager>
{
	[SerializeField]
	private AudioSource m_sourceTemplate;

	[SerializeField]
	private List<AudioSource> m_sourcePool;

	[SerializeField]
	private AudioSource m_sourceBG;

	[SerializeField]
	[Header("背景音乐")]
	private GameSound[] m_clipsBG;

	[SerializeField]
	[Header("通用点击声")]
	private GameSound m_clipCommonClick;

	[Header("弹框提示声")]
	[SerializeField]
	private GameSound m_clipAlertHint;

	[Header("加减按钮点击声")]
	[SerializeField]
	private GameSound m_clipClickPlusMinus;

	[SerializeField]
	[Header("删除邮件")]
	private GameSound m_clipMailDelete;

	[SerializeField]
	[Header("阅读邮件")]
	private GameSound m_clipMailRead;

	[Header("收到新邮件")]
	[SerializeField]
	private GameSound m_clipMailReceive;

	[SerializeField]
	[Header("选择邮件")]
	private GameSound m_clipMailSelect;

	[Header("获得金币")]
	[SerializeField]
	private GameSound m_clipGoldCoinGain;

	[Header("登录")]
	[SerializeField]
	private GameSound m_clipLogin;

	[SerializeField]
	[Header("绑定手机")]
	private GameSound m_clipBindPhone;

	[SerializeField]
	[Header("推广")]
	private GameSound m_clipShare;

	[Header("邮件")]
	[SerializeField]
	private GameSound m_clipMail;

	[Header("活动")]
	[SerializeField]
	private GameSound m_clipActivity;

	[Header("排行榜")]
	[SerializeField]
	private GameSound m_clipRank;

	[SerializeField]
	[Header("保险柜")]
	private GameSound m_clipSafebox;

	[SerializeField]
	[Header("兑换")]
	private GameSound m_clipExchange;

	[Header("充值")]
	[SerializeField]
	private GameSound m_clipRecharge;

	[SerializeField]
	[Header("复制")]
	private GameSound m_clipCopy;

	private bool m_silenceSound;

	private bool m_silenceBG;

	private string m_strSoundKey = "MuteSound";

	private string m_strBGKey = "MuteBG";

	private void Awake()
	{
		if (MB_Singleton<SoundManager>._instance == null)
		{
			MB_Singleton<SoundManager>.SetInstance(this);
			m_sourcePool = new List<AudioSource>();
		}
	}

	private void Start()
	{
		Init();
	}

	public void OnClosePanel()
	{
		PlaySound(SoundType.Common_Click);
	}

	public void PlaySound(SoundType soundType)
	{
		switch (soundType)
		{
		case SoundType.None:
		case SoundType.Login:
		case SoundType.BindPhone:
		case SoundType.Share:
		case SoundType.Mail:
		case SoundType.Activity:
		case SoundType.Rank:
		case SoundType.Safebox:
		case SoundType.Exchange:
		case SoundType.Recharge:
		case SoundType.Copy:
			break;
		case SoundType.Common_Click:
			SourcePlayClip(GetSource(), m_clipCommonClick.clip, m_silenceSound ? 0f : m_clipCommonClick.volume);
			break;
		case SoundType.Alert_Hint:
			SourcePlayClip(GetSource(), m_clipAlertHint.clip, m_silenceSound ? 0f : m_clipAlertHint.volume);
			break;
		case SoundType.Click_PlusMinus:
			SourcePlayClip(GetSource(), m_clipClickPlusMinus.clip, m_silenceSound ? 0f : m_clipClickPlusMinus.volume);
			break;
		case SoundType.Mail_Delete:
			SourcePlayClip(GetSource(), m_clipMailDelete.clip, m_silenceSound ? 0f : m_clipMailDelete.volume);
			break;
		case SoundType.Mail_Read:
			SourcePlayClip(GetSource(), m_clipMailRead.clip, m_silenceSound ? 0f : m_clipMailRead.volume);
			break;
		case SoundType.Mail_Receive:
			SourcePlayClip(GetSource(), m_clipMailReceive.clip, m_silenceSound ? 0f : m_clipMailReceive.volume);
			break;
		case SoundType.Mail_Select:
			SourcePlayClip(GetSource(), m_clipMailSelect.clip, m_silenceSound ? 0f : m_clipMailSelect.volume);
			break;
		case SoundType.GoldCoin_Gain:
			SourcePlayClip(GetSource(), m_clipGoldCoinGain.clip, m_silenceSound ? 0f : m_clipGoldCoinGain.volume);
			break;
		default:
			PlaySoundBG(soundType);
			break;
		}
	}

	private AudioSource GetSource()
	{
		foreach (AudioSource item in m_sourcePool)
		{
			if (!item.isPlaying)
			{
				return item;
			}
		}
		return CreateSource();
	}

	private AudioSource CreateSource()
	{
		AudioSource audioSource = Object.Instantiate(m_sourceTemplate);
		audioSource.transform.SetParent(m_sourceTemplate.transform.parent);
		audioSource.name = "Source";
		m_sourcePool.Add(audioSource);
		return audioSource;
	}

	private void PlaySoundBG(SoundType soundType)
	{
		GameSound gameSound = null;
		switch (soundType)
		{
		case SoundType.BG_First:
			gameSound = m_clipsBG[0];
			break;
		case SoundType.BG_Second:
			gameSound = m_clipsBG[1];
			break;
		case SoundType.BG_Third:
			gameSound = m_clipsBG[2];
			break;
		case SoundType.BG_Four:
			gameSound = m_clipsBG[3];
			break;
		case SoundType.BG_Random:
			gameSound = m_clipsBG[Random.Range(0, 4)];
			break;
		}
		if (!(gameSound == null))
		{
			SourcePlayClip(m_sourceBG, gameSound.clip, gameSound.volume, loop: true);
		}
	}

	private void SourcePlayClip(AudioSource source, AudioClip clip, float volume, bool loop = false)
	{
		source.clip = clip;
		source.loop = loop;
		source.volume = volume;
		source.Play();
	}

	public void SetSilenceSound(bool value)
	{
		m_silenceSound = value;
		if (value)
		{
			PlayerPrefs.SetInt(m_strSoundKey, 1);
		}
		else
		{
			PlayerPrefs.DeleteKey(m_strSoundKey);
		}
		PlayerPrefs.Save();
	}

	public bool IsSilenceSound()
	{
		return m_silenceSound;
	}

	public void SetSilenceBG(bool value)
	{
		m_silenceBG = value;
		m_sourceBG.mute = value;
		if (value)
		{
			PlayerPrefs.SetInt(m_strBGKey, 1);
		}
		else
		{
			PlayerPrefs.DeleteKey(m_strBGKey);
		}
		PlayerPrefs.Save();
	}

	public bool IsSilenceBG()
	{
		return m_silenceBG;
	}

	public void Init()
	{
		if (PlayerPrefs.HasKey(m_strSoundKey))
		{
			m_silenceSound = true;
		}
		if (PlayerPrefs.HasKey(m_strBGKey))
		{
			m_silenceBG = true;
			m_sourceBG.mute = true;
		}
	}
}
