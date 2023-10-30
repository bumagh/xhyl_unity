using UnityEngine;

public class ESP_SoundManager : MonoBehaviour
{
	public static ESP_SoundManager Instance;

	[HideInInspector]
	public AudioSource ads_Click;

	[HideInInspector]
	public AudioSource ads_LobbyBGM;

	[HideInInspector]
	public AudioSource ads_Dealer;

	[HideInInspector]
	public AudioSource ads_Guys;

	[HideInInspector]
	public AudioSource ads_DiceBGM;

	[HideInInspector]
	public AudioSource ads_MaryBGM;

	[HideInInspector]
	public AudioSource ads_Mary;

	[HideInInspector]
	public AudioSource ads_Major;

	[HideInInspector]
	public AudioSource ads_MajorBGM;

	[HideInInspector]
	public AudioSource ads_NumberRoll;

	public static bool bShouldStop;

	public AudioClip diceBaoji;

	public AudioClip diceLoose;

	public AudioClip diceWin;

	public AudioClip maryBgm;

	public AudioClip haveChange;

	public AudioClip enterMary;

	public AudioClip fullDragon;

	public AudioClip fullPersons;

	public AudioClip fullSame;

	public AudioClip fullWeapons;

	public AudioClip fullAward;

	public AudioClip numRoll;

	public AudioClip numRollEnd;

	public AudioClip roll;

	public AudioClip roll1;

	public AudioClip[] lines;

	public AudioClip[] typeOfPlayEffect;

	public AudioClip[] stopPlayEffect;

	private string _strAward;

	private bool isPlayDice;

	private int temp;

	private int index = -1;

	private void Awake()
	{
		ads_Click = base.transform.Find("AS_Click").GetComponent<AudioSource>();
		ads_LobbyBGM = base.transform.Find("AS_LobbyBGM").GetComponent<AudioSource>();
		ads_Dealer = base.transform.Find("AS_Dealer").GetComponent<AudioSource>();
		ads_Guys = base.transform.Find("AS_Guys").GetComponent<AudioSource>();
		ads_DiceBGM = base.transform.Find("AS_DiceBGM").GetComponent<AudioSource>();
		ads_MaryBGM = base.transform.Find("AS_MaryBGM").GetComponent<AudioSource>();
		ads_Mary = base.transform.Find("AS_Mary").GetComponent<AudioSource>();
		ads_Major = base.transform.Find("AS_Major").GetComponent<AudioSource>();
		ads_MajorBGM = base.transform.Find("AS_MajorBGM").GetComponent<AudioSource>();
		ads_NumberRoll = base.transform.Find("AS_NumberRoll").GetComponent<AudioSource>();
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public void PlayClickAudio()
	{
		ads_Click.Play();
	}

	public void PlayLobbyBGM()
	{
		if (!ads_LobbyBGM.isPlaying)
		{
			ads_LobbyBGM.Play();
			ads_LobbyBGM.loop = true;
		}
	}

	public void StopLobbyBGM()
	{
		ads_LobbyBGM.Stop();
	}

	public void PlayDiceBGM()
	{
		ads_DiceBGM.Play();
		ads_DiceBGM.loop = true;
		isPlayDice = true;
	}

	public void PlayDiceWinAudio()
	{
		ads_Dealer.clip = diceWin;
		ads_Dealer.Play();
	}

	public void PlayDiceLooseAudio()
	{
		ads_Dealer.clip = diceLoose;
		ads_Dealer.Play();
	}

	public void PlayDiceOverFlowAudio()
	{
		if (!ads_Guys.isPlaying)
		{
			ads_Guys.clip = diceBaoji;
			ads_Guys.Play();
			ads_Guys.loop = true;
		}
	}

	public void StopGuysAudio()
	{
		ads_Guys.Stop();
		ads_Guys.loop = false;
		bShouldStop = true;
	}

	public void StopDiceBGM()
	{
		ads_DiceBGM.Stop();
		isPlayDice = false;
	}

	public void PlayMaryBGM()
	{
		PlayMajorBGMAndio(isPlay: false);
		ads_MaryBGM.clip = maryBgm;
		ads_MaryBGM.Play();
		ads_MaryBGM.loop = true;
	}

	public void StopMaryBGM()
	{
		PlayMajorBGMAndio(isPlay: true);
		ads_MaryBGM.Stop();
		ads_MaryBGM.loop = false;
	}

	public void StopMaryAudio()
	{
		if (ads_Mary.isPlaying)
		{
			ads_Mary.Stop();
			ads_Mary.loop = false;
		}
	}

	public void StopMajorAudio()
	{
		UnityEngine.Debug.Log("StopMajorAudio");
		if (ads_Major.isPlaying)
		{
			ads_Major.Stop();
		}
	}

	public void PlayMajorRollAudio()
	{
		ads_Major.clip = roll;
		ads_Major.Play();
	}

	public void PlayMajorRollEndAudio()
	{
		if (ads_Major.clip == roll && ads_Major.isPlaying)
		{
			ads_Major.clip = roll1;
			ads_Major.Play();
		}
	}

	public void PlayDrawLineAudio(int lineindex)
	{
		if (lines.Length > 0)
		{
			if (lineindex >= lines.Length)
			{
				lineindex = 0;
			}
			ads_Major.clip = lines[lineindex];
			ads_Major.Play();
		}
	}

	public void PlayStopAndio()
	{
		if (stopPlayEffect.Length <= 0)
		{
			return;
		}
		temp++;
		if (temp >= 3)
		{
			temp = 0;
			index++;
			if (index >= stopPlayEffect.Length)
			{
				index = 0;
			}
			ads_Major.clip = stopPlayEffect[index];
			ads_Major.Play();
		}
	}

	public void PlayMajorBGMAndio(bool isPlay)
	{
		if (!(ads_MajorBGM == null))
		{
			if (isPlay)
			{
				ads_MajorBGM.Stop();
				ads_MajorBGM.Play();
				ads_MajorBGM.loop = true;
			}
			else
			{
				ads_MajorBGM.Stop();
				ads_MajorBGM.loop = false;
			}
		}
	}

	public void PlayMajorGameAwardAudio(int index = 0)
	{
		if (typeOfPlayEffect.Length > 0 && !isPlayDice)
		{
			ads_Mary.Stop();
			if (typeOfPlayEffect.Length > index)
			{
				ads_Mary.clip = typeOfPlayEffect[index];
				ads_Mary.Play();
			}
			else if (typeOfPlayEffect.Length > 0)
			{
				ads_Mary.clip = typeOfPlayEffect[typeOfPlayEffect.Length - 1];
				ads_Mary.Play();
			}
			else
			{
				UnityEngine.Debug.LogError("索引超出长度");
			}
		}
	}

	public void StopMajorGameAwardAudio()
	{
		ads_Mary.Stop();
	}

	public void PlayMajorGameAwardAudio(ESP_CellType celltype, int ImgNum = 0)
	{
		if (typeOfPlayEffect.Length > 0)
		{
			if (typeOfPlayEffect.Length >= (int)celltype && typeOfPlayEffect[(int)(celltype - 1)] != null)
			{
				ads_Major.clip = typeOfPlayEffect[(int)(celltype - 1)];
				ads_Major.Play();
			}
			else
			{
				UnityEngine.Debug.LogError("索引超出长度");
			}
		}
	}

	public void PlayPeopleFullAwardAudio(bool isChange = false)
	{
		ads_Major.clip = (isChange ? haveChange : fullPersons);
		ads_Major.Play();
	}

	public void PlayWeaponsFullAwardAudio()
	{
		ads_Major.clip = fullWeapons;
		ads_Major.Play();
	}

	public void PlayDragonFullAwardAudio()
	{
		ads_Major.clip = fullDragon;
		ads_Major.Play();
	}

	public void PlayTiTianAndZhongYiTangFullAudio()
	{
		ads_Major.clip = fullAward;
		ads_Major.Play();
	}

	public void PlayFullAwardSameIcoAudio()
	{
		ads_Major.clip = fullSame;
		ads_Major.Play();
	}

	public void PlayEnterMaryAudio()
	{
		ads_Major.clip = enterMary;
		ads_Major.Play();
	}

	public void NumberRollAudio()
	{
		ads_NumberRoll.clip = numRoll;
		ads_NumberRoll.Play();
		ads_NumberRoll.loop = true;
	}

	public void NumberRollEndAudio()
	{
		ads_NumberRoll.clip = numRollEnd;
		ads_NumberRoll.Play();
		ads_NumberRoll.loop = false;
	}

	public void StopNumberRollAudio()
	{
		if (ads_NumberRoll != null && ads_NumberRoll.isPlaying)
		{
			ads_NumberRoll.Stop();
		}
	}

	public void SetAudioSourceVolum(AudioSource sorce, int vol)
	{
		if (sorce != null)
		{
			sorce.volume = vol;
		}
	}
}
