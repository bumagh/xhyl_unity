using System.Collections;
using UnityEngine;

public class PTM_SoundManager : MonoBehaviour
{
	public static PTM_SoundManager Instance;

	[HideInInspector]
	public AudioSource ads_Click;

	[HideInInspector]
	public AudioSource ads_LobbyBGM;

	[HideInInspector]
	public AudioSource ads_LobbyBGM1;

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

	public AudioClip diceShake;

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

	private Coroutine coroutine;

	private int temp;

	private int index = -1;

	private void Awake()
	{
		ads_Click = base.transform.Find("AS_Click").GetComponent<AudioSource>();
		ads_LobbyBGM = base.transform.Find("AS_LobbyBGM").GetComponent<AudioSource>();
		ads_LobbyBGM1 = ads_LobbyBGM.transform.Find("AS_LobbyBGM (1)").GetComponent<AudioSource>();
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
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
			}
			coroutine = StartCoroutine(PlayBao());
		}
	}

	private IEnumerator PlayBao()
	{
		while (true)
		{
			ads_LobbyBGM1.Play();
			yield return new WaitForSeconds(UnityEngine.Random.Range(5.5f, 8.5f));
			ads_LobbyBGM1.Stop();
		}
	}

	public void StopLobbyBGM()
	{
		ads_LobbyBGM.Stop();
		ads_LobbyBGM1.Stop();
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
	}

	public void PlayDiceBGM()
	{
		if (!ads_DiceBGM.isPlaying)
		{
			ads_DiceBGM.Play();
			ads_DiceBGM.loop = true;
		}
	}

	public void PlayShakeDiceAudio()
	{
		if (!ads_Dealer.isPlaying)
		{
			ads_Dealer.clip = diceShake;
			ads_Dealer.Play();
			StartCoroutine("Dealeryoho");
		}
	}

	public IEnumerator PlayNPCsDefualtAudio()
	{
		yield return new WaitForSeconds(1.5f);
		bShouldStop = false;
		StartCoroutine("GuysDefault");
	}

	public void PlayDiceResultAudio(int[] dices)
	{
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

	public void StopDealerAudio()
	{
		bShouldStop = true;
		StopCoroutine("GuysDefault");
		StopCoroutine("Dealeryoho");
		ads_Dealer.Stop();
	}

	public void StopGuysAudio()
	{
		ads_Guys.Stop();
		ads_Guys.loop = false;
		bShouldStop = true;
	}

	public void StopDiceBGM()
	{
		if (ads_DiceBGM.isPlaying)
		{
			ads_DiceBGM.Stop();
		}
	}

	private IEnumerator Dealeryoho()
	{
		yield return new WaitForSeconds(3f);
	}

	private IEnumerator GuysDefault()
	{
		int _iGuysWords = 1;
		do
		{
			if (ads_Guys.isPlaying || _iGuysWords < 4)
			{
			}
			yield return new WaitForSeconds(3f);
			if (!ads_Dealer.isPlaying)
			{
			}
			yield return new WaitForSeconds(3f);
		}
		while (!bShouldStop);
	}

	public void PlayMaryBGM()
	{
		if (!ads_MaryBGM.isPlaying)
		{
			ads_MaryBGM.clip = maryBgm;
			ads_MaryBGM.Play();
		}
	}

	public void StopMaryBGM()
	{
		if (ads_MaryBGM.isPlaying)
		{
			ads_MaryBGM.Stop();
		}
	}

	public void PlayRunAudio()
	{
		if (ads_Mary.isPlaying)
		{
		}
	}

	public void PlayMaryWinAudio()
	{
	}

	public void PlayMaryBigWinAudio()
	{
		if (ads_Mary.isPlaying)
		{
		}
	}

	public void StopMaryAudio()
	{
		if (ads_Mary.isPlaying)
		{
			ads_Mary.Stop();
			ads_Mary.loop = false;
		}
	}

	private IEnumerator PlayLaterAudio()
	{
		yield return new WaitForSeconds(3f);
	}

	public void PlayMaryBigWinWithoutLaterAudio()
	{
		if (ads_Mary.isPlaying)
		{
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
		if (isPlay && ads_MajorBGM != null)
		{
			ads_MajorBGM.Stop();
			ads_MajorBGM.Play();
		}
		else if (ads_MajorBGM != null)
		{
			ads_MajorBGM.Stop();
		}
	}

	public void PlayMajorGameAwardAudio(string _strType)
	{
		UnityEngine.Debug.Log("Type: " + _strType);
		if (_strType == "weapon")
		{
			PlayWeaponsFullAwardAudio();
		}
		else if (_strType == "Axe" || _strType == "Blade" || _strType == "Spear")
		{
			PlayFullAwardSameIcoAudio();
		}
		else if (_strType == "human")
		{
			PlayPeopleFullAwardAudio();
		}
		else if (_strType == "Lin" || _strType == "Lu" || _strType == "Song")
		{
			PlayFullAwardSameIcoAudio();
		}
		else if (_strType == "Sky" || _strType == "Hall")
		{
			PlayFullAwardSameIcoAudio();
		}
		else if (_strType == "dragon")
		{
			PlayDragonFullAwardAudio();
		}
	}

	public void PlayMajorGameAwardAudio(int index = 0)
	{
		UnityEngine.Debug.LogError(index);
		if (typeOfPlayEffect.Length > 0)
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

	public void PlayMajorGameAwardAudio(PTM_CellType celltype, int ImgNum = 0)
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
