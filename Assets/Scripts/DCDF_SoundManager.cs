using UnityEngine;

public class DCDF_SoundManager : MonoBehaviour
{
	public static DCDF_SoundManager Instance;

	public AudioSource ads_Click;

	public AudioSource ads_BGM;

	public AudioSource ads_Major;

	public AudioSource ads_Result;

	public AudioSource ads_Gold;

	public AudioClip roll;

	public AudioClip roll1;

	public AudioClip rollEnd;

	public AudioClip win;

	public AudioClip lose;

	public AudioClip jackpot;

	public AudioClip explode;

	public AudioClip turn;

	public AudioClip gold;

	public AudioClip normalBGM;

	public AudioClip freeGameBGM;

	public AudioClip openBox;

	public AudioClip laugh;

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

	public void PlayNormalBGM()
	{
		ads_BGM.clip = normalBGM;
		ads_BGM.Play();
		ads_BGM.loop = true;
	}

	public void PlayFreeGameBGM()
	{
		ads_BGM.clip = freeGameBGM;
		ads_BGM.Play();
		ads_BGM.loop = true;
	}

	public void StopBGM()
	{
		ads_BGM.Stop();
	}

	public void StopMajorAudio()
	{
		UnityEngine.Debug.Log("StopMajorAudio");
		if (ads_Major.isPlaying)
		{
			ads_Major.Stop();
		}
	}

	public void PlayNormalRollAudio()
	{
		ads_Major.clip = roll;
		ads_Major.Play();
	}

	public void PlaySpeedUpRollAudio()
	{
		ads_Major.clip = roll1;
		ads_Major.Play();
	}

	public void PlayRollEndAudio()
	{
		ads_Major.clip = rollEnd;
		ads_Major.Play();
	}

	public void PlayWinAudio()
	{
		ads_Result.clip = win;
		ads_Result.Play();
	}

	public void PlayLoseAudio()
	{
		ads_Result.clip = lose;
		ads_Result.Play();
	}

	public void PlayJackpotAudio()
	{
		ads_Major.clip = jackpot;
		ads_Major.Play();
	}

	public void PlayLaughAudio()
	{
		ads_Major.clip = laugh;
		ads_Major.Play();
	}

	public void PlayExplodeAudio()
	{
		ads_Major.clip = explode;
		ads_Major.Play();
	}

	public void PlayOpenBoxAudio()
	{
		ads_Major.clip = turn;
		ads_Major.Play();
	}

	public void PlayGoldAudio()
	{
		ads_Major.clip = gold;
		ads_Major.Play();
	}

	public void PlayGoldIncreaseAudio()
	{
		ads_Gold.clip = gold;
		ads_Gold.loop = true;
		ads_Gold.Play();
	}

	public void StopGoldIncreaseAudio()
	{
		ads_Gold.Stop();
	}

	public void SetAudioSourceVolum(AudioSource sorce, float vol)
	{
		if (sorce != null)
		{
			sorce.volume = vol;
		}
	}
}
