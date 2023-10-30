using System.Collections.Generic;
using UnityEngine;

public class LHD_AudioManger : MonoBehaviour
{
	public enum AudioType
	{
		BG,
		LongWin,
		HuWin,
		HeWin,
		CardShow,
		EnterBet,
		StartBet
	}

	public static LHD_AudioManger instance;

	public List<AudioClip> Audiolist;

	private Dictionary<string, AudioClip> AudiolistDic = new Dictionary<string, AudioClip>();

	private AudioSource AudioplayBG;

	private List<AudioSource> Audioplay;

	public AudioType audioType;

	public int playindex;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		Audioplay = new List<AudioSource>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Audioplay.Add(base.transform.GetChild(i).GetComponent<AudioSource>());
		}
		AudioplayBG = GetComponent<AudioSource>();
		foreach (AudioClip item in Audiolist)
		{
			AudiolistDic.Add(item.name, item);
		}
		SetValue(isBgMic: true, PlayerPrefs.GetFloat("LHDOpenBG", 1f));
		SetValue(isBgMic: false, PlayerPrefs.GetFloat("LHDOpenEff", 1f));
		PlayAudio(AudioType.BG, isloop: true);
	}

	public void PlayAudio(AudioType audioType, bool isloop = false)
	{
		AudioClip audio = GetAudio(audioType);
		if (audio != null)
		{
			if (audioType == AudioType.BG)
			{
				AudioplayBG.Stop();
				AudioplayBG.clip = audio;
				AudioplayBG.Play();
				AudioplayBG.loop = isloop;
				return;
			}
			if (playindex >= Audioplay.Count)
			{
				playindex = 0;
			}
			Audioplay[playindex].clip = audio;
			Audioplay[playindex].Play();
			Audioplay[playindex].loop = isloop;
			playindex = (playindex + 1) % Audioplay.Count;
		}
		else
		{
			UnityEngine.Debug.LogError("没有找到 " + audioType);
		}
	}

	public void SetValue(bool isBgMic, float value)
	{
		if (isBgMic)
		{
			AudioplayBG.volume = value;
			return;
		}
		for (int i = 0; i < Audioplay.Count; i++)
		{
			Audioplay[i].volume = value;
		}
	}

	private AudioClip GetAudio(AudioType audioType)
	{
		string key = string.Empty;
		switch (audioType)
		{
		case AudioType.BG:
			key = "LH_Bgm";
			break;
		case AudioType.LongWin:
			key = "LH_Long";
			break;
		case AudioType.HuWin:
			key = "LH_Hu";
			break;
		case AudioType.HeWin:
			key = "LH_He";
			break;
		case AudioType.CardShow:
			key = "LH_CardShowDown";
			break;
		case AudioType.EnterBet:
			key = "LH_xiazhuyinxiao";
			break;
		case AudioType.StartBet:
			key = "LH_Bet";
			break;
		}
		AudioClip result = null;
		if (AudiolistDic.ContainsKey(key))
		{
			result = AudiolistDic[key];
		}
		return result;
	}
}
