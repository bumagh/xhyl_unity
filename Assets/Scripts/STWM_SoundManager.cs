using System.Collections;
using UnityEngine;

public class STWM_SoundManager : MonoBehaviour
{
	public static STWM_SoundManager Instance;

	public AudioSource ads_Click;

	public AudioSource ads_LobbyBGM;

	public AudioSource ads_Dealer;

	public AudioSource ads_Guys;

	public AudioSource ads_DiceBGM;

	public AudioSource ads_MaryBGM;

	public AudioSource ads_Mary;

	public AudioSource ads_Major;

	public AudioSource ads_NumberRoll;

	public static bool bShouldStop;

	public AudioClip[] diceResults;

	public AudioClip[] diceNpcSays;

	public AudioClip diceBaoji;

	public AudioClip diceLoose;

	public AudioClip[] diceMakers;

	public AudioClip diceShake;

	public AudioClip diceWin;

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

	public AudioClip[] songJiang;

	public AudioClip[] zhongYiTang;

	public AudioClip[] tiTianXingDao;

	public AudioClip[] linChong;

	public AudioClip[] jinDao;

	public AudioClip[] tieFu;

	public AudioClip[] yinQiang;

	public AudioClip[] luZhiShen;

	public AudioClip maryBgm;

	public AudioClip bigWinFront;

	public AudioClip bigWinLater;

	public AudioClip maryRun;

	public AudioClip maryWin;

	public AudioClip[] maryRuns;

	private string _strAward;

	private void Awake()
	{
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
		ads_Dealer.clip = diceResults[dices[0] + dices[1] - 2];
		ads_Dealer.Play();
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
		ads_Dealer.clip = diceMakers[0];
		ads_Dealer.Play();
	}

	private IEnumerator GuysDefault()
	{
		int _iDealerWords = 1;
		int _iGuysWords = 1;
		do
		{
			if (!ads_Guys.isPlaying)
			{
				if (_iGuysWords < 4)
				{
					int _iwordsindex2 = UnityEngine.Random.Range(0, 5);
					ads_Guys.clip = diceNpcSays[_iwordsindex2];
					ads_Guys.Play();
					_iGuysWords++;
				}
				else
				{
					int _iwordsindex2 = UnityEngine.Random.Range(5, 9);
					ads_Guys.clip = diceNpcSays[_iwordsindex2];
					ads_Guys.Play();
				}
			}
			yield return new WaitForSeconds(3f);
			if (!ads_Dealer.isPlaying)
			{
				ads_Dealer.clip = diceMakers[_iDealerWords];
				ads_Dealer.Play();
				_iDealerWords++;
				if (_iDealerWords > 2)
				{
					_iDealerWords = 1;
				}
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
		if (!ads_Mary.isPlaying)
		{
			ads_Mary.clip = maryRun;
			ads_Mary.Play();
		}
	}

	public void PlayConstantSpeedAudio()
	{
		ads_Mary.clip = maryRuns[0];
		ads_Mary.Play();
	}

	public void PlayReduceSpeedAudio(int index)
	{
		ads_Mary.clip = maryRuns[index];
		ads_Mary.Play();
	}

	public void PlayMaryWinAudio()
	{
		ads_Mary.clip = maryWin;
		ads_Mary.Play();
	}

	public void PlayMaryBigWinAudio()
	{
		if (!ads_Mary.isPlaying)
		{
			ads_Mary.clip = bigWinFront;
			ads_Mary.Play();
			StartCoroutine(PlayLaterAudio());
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
		ads_Mary.clip = bigWinLater;
		ads_Mary.Play();
		ads_Mary.loop = true;
	}

	public void PlayMaryBigWinWithoutLaterAudio()
	{
		if (!ads_Mary.isPlaying)
		{
			ads_Mary.clip = bigWinFront;
			ads_Mary.Play();
		}
	}

	public void StopMajorAudio()
	{
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
		ads_Major.clip = lines[lineindex];
		ads_Major.Play();
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

	public void PlayMajorGameAwardAudio(STWM_CellType celltype, int ImgNum)
	{
		AudioClip[] array = null;
		switch (celltype)
		{
		case STWM_CellType.Blade:
			array = jinDao;
			break;
		case STWM_CellType.Axe:
			array = tieFu;
			break;
		case STWM_CellType.Spear:
			array = yinQiang;
			break;
		case STWM_CellType.Song:
			array = songJiang;
			break;
		case STWM_CellType.Lu:
			array = luZhiShen;
			break;
		case STWM_CellType.Lin:
			array = linChong;
			break;
		case STWM_CellType.Sky:
			array = tiTianXingDao;
			break;
		case STWM_CellType.Hall:
			array = zhongYiTang;
			break;
		case STWM_CellType.Dragon:
			array = null;
			PlayDragonFullAwardAudio();
			break;
		}
		if (array != null)
		{
			ads_Major.clip = array[ImgNum - 3];
			ads_Major.Play();
		}
	}

	public void PlayPeopleFullAwardAudio()
	{
		ads_Major.clip = fullPersons;
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
