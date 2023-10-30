using JSYS_LL_GameCommon;
using System.Collections;
using UnityEngine;

public class JSYS_LL_MusicMngr : MonoBehaviour
{
	public enum MUSIC_GAME_MUSIC
	{
		GAME_BG = 0,
		GAME_ONE_ANIMAL_PRIZE_BEGIN = 2,
		GAME_ALL_PRIZE_BEGIN = 11,
		GAME_ALL_PRIZE_END = 12,
		GAME_ONE_ANIMAL_END = 13,
		GAME_LUCKYPRIZE_BEGIN = 14
	}

	public enum MUSIC_SCENE_MUSIC
	{
		SCENE_FLIP_PLATE,
		SCENE_RESET_COLOR,
		SCENE_SPIN_ANIMAL
	}

	public enum MUSIC_EFFECT_MUSIC
	{
		EFFECT_ALL_PRIZE_PRE,
		EFFECT_All_LIGHTNING,
		EFFECT_ALL_SONGDENG,
		EFFECT_FIREWORKS,
		Effect_ALL_LIGHTING_X2_FLY,
		Effect_ALL_LIGHTING_X2_FLYEND,
		Effect_BONUS_COINS
	}

	public enum MUSIC_BROADCAST_MUSIC
	{
		BROADCAST_NO_1,
		BROADCAST_NO_2,
		BROADCAST_NO_3,
		BROADCAST_NO_4,
		BROADCAST_NO_5,
		BROADCAST_NO_6,
		BROADCAST_NO_7,
		BROADCAST_NO_8,
		LUCKY_PRIZE_TIMES,
		LUCKY_PRIZE_LIGHTNING,
		LUCKY_PRIZE_BONUS,
		ALL_CAIJIN,
		ALL_LIGHTING,
		ALL_SONGDENG,
		ALL_DASANYUAN_LION,
		ALL_DASANYUAN_PANDA,
		ALL_DASANYUAN_MONKEY,
		ALL_DASANYUAN_RABBIT,
		ALL_DASIXI_RED,
		ALL_DASIXI_GREEN,
		ALL_DASIXI_YELLOW,
		NORMAL_RED_LION,
		NORMAL_RED_PANDA,
		NORMAL_RED_MONKEY,
		NORMAL_RED_RABBIT,
		NORMAL_GREEN_LION,
		NORMAL_GREEN_PANDA,
		NORMAL_GREEN_MONKEY,
		NORMAL_GREEN_RABBIT,
		NORMAL_YELLOW_LION,
		NORMAL_YELLOW_PANDA,
		NORMAL_YELLOW_MONKEY,
		NORMAL_YELLOW_RABBIT,
		lUCKY_RED_LION,
		lUCKY_RED_PANDA,
		lUCKY_RED_MONKEY,
		lUCKY_RED_RABBIT,
		lUCKY_GREEN_LION,
		lUCKY_GREEN_PANDA,
		lUCKY_GREEN_MONKEY,
		lUCKY_GREEN_RABBIT,
		lUCKY_YELLOW_LION,
		lUCKY_YELLOW_PANDA,
		lUCKY_YELLOW_MONKEY,
		lUCKY_YELLOW_RABBIT
	}

	public enum MUSIC_UI
	{
		UI_COUNTDOWN0,
		UI_COUNTDONW1,
		UI_ZHUANG,
		UI_XIAN,
		UI_HE
	}

	public static JSYS_LL_MusicMngr G_MusicMngr;

	public AudioSource mGameAudio;

	public AudioClip[] mGameMusicClip;

	private int _nGameMusic_OneAnimal_BgIndex;

	private int _nGameMusic_LuckyPrize_BgIndex;

	public AudioSource mSceneAudio;

	public AudioClip[] mSceneClip;

	public AudioSource mEffectAudio;

	public AudioClip[] mEffectClip;

	public AudioSource mAnimalAudio;

	public AudioClip[] mAnimalClip;

	public AudioSource mBroadcastAudio;

	public AudioClip[] mBroadcastClip;

	private int _nLuckyNum = 1;

	private int _nLuckyTyp;

	public AudioSource mUIAudio;

	public AudioClip[] mGameUIClip;

	private ArrayList _AllAudioSrc = new ArrayList();

	private int test = 1;

	private int test_typ;

	private int testAnimal;

	private int testColor;

	public static JSYS_LL_MusicMngr GetSingleton()
	{
		return G_MusicMngr;
	}

	private void Awake()
	{
		if (G_MusicMngr == null)
		{
			G_MusicMngr = this;
		}
		_AllAudioSrc.Add(mGameAudio);
		_AllAudioSrc.Add(mSceneAudio);
		_AllAudioSrc.Add(mAnimalAudio);
		_AllAudioSrc.Add(mEffectAudio);
		_AllAudioSrc.Add(mBroadcastAudio);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void PlayGameCountDown(bool isZero)
	{
		if (isZero)
		{
			_playSound(mUIAudio, mGameUIClip[1]);
		}
		else
		{
			_playSound(mUIAudio, mGameUIClip[0]);
		}
	}

	public void PlayUISound(MUSIC_UI typ)
	{
		_playSound(mUIAudio, mGameUIClip[(int)typ]);
	}

	public void PlayGameMusic(MUSIC_GAME_MUSIC typ)
	{
		switch (typ)
		{
		case MUSIC_GAME_MUSIC.GAME_ONE_ANIMAL_PRIZE_BEGIN:
			_nGameMusic_OneAnimal_BgIndex = _nGameMusic_OneAnimal_BgIndex;
			_playSound(mGameAudio, mGameMusicClip[(int)(typ + _nGameMusic_OneAnimal_BgIndex)], isLoop: true);
			break;
		case MUSIC_GAME_MUSIC.GAME_LUCKYPRIZE_BEGIN:
			_nGameMusic_LuckyPrize_BgIndex = (_nGameMusic_LuckyPrize_BgIndex + 1) % 3;
			UnityEngine.Debug.Log((int)(_nGameMusic_LuckyPrize_BgIndex + typ));
			_playSound(mGameAudio, mGameMusicClip[(int)(typ + _nGameMusic_LuckyPrize_BgIndex)], isLoop: true);
			break;
		default:
			_playSound(mGameAudio, mGameMusicClip[(int)typ], isLoop: true);
			break;
		}
	}

	public void PlayAnimalWin(AnimalType typ)
	{
		_playSound(mAnimalAudio, mAnimalClip[(int)typ]);
	}

	public void PlayAnimalOut()
	{
		_playSound(mAnimalAudio, mAnimalClip[4]);
	}

	public void PlayEffectSound(MUSIC_EFFECT_MUSIC typ)
	{
		if (typ == MUSIC_EFFECT_MUSIC.EFFECT_FIREWORKS || typ == MUSIC_EFFECT_MUSIC.Effect_BONUS_COINS)
		{
			_playSound(mEffectAudio, mEffectClip[(int)typ], isLoop: true);
		}
		else
		{
			_playSound(mEffectAudio, mEffectClip[(int)typ]);
		}
	}

	public void PlaySceneSound(MUSIC_SCENE_MUSIC typ)
	{
		_playSound(mSceneAudio, mSceneClip[(int)typ]);
	}

	public void PlayLuckyPrizeSound(int nLuckyNum, int nLuckyTyp)
	{
		if (_nLuckyNum <= 8 && _nLuckyNum >= 1)
		{
			_nLuckyNum = nLuckyNum;
			if (_nLuckyTyp <= 2 && _nLuckyTyp >= 0)
			{
				_nLuckyTyp = nLuckyTyp;
				StopCoroutine("_playPrize");
				StartCoroutine("_playPrize");
			}
		}
	}

	private IEnumerator _playPrize()
	{
		if (_nLuckyNum <= 8 && _nLuckyNum >= 1)
		{
			_playSound(mBroadcastAudio, mBroadcastClip[_nLuckyNum - 1]);
			yield return new WaitForSeconds(1.7f);
			_playSound(mBroadcastAudio, mBroadcastClip[_nLuckyTyp + 8]);
		}
	}

	public void PlayResultSound(MUSIC_BROADCAST_MUSIC typ)
	{
		_playSound(mBroadcastAudio, mBroadcastClip[(int)typ]);
	}

	public void PlayNormalAnimalResult(int nColor, int nType)
	{
		int num = nColor * 4 + nType;
		_playSound(mBroadcastAudio, mBroadcastClip[num + 21]);
	}

	public void PlayLuckyAnimalResult(int nColor, int nType)
	{
		int num = nColor * 4 + nType;
		_playSound(mBroadcastAudio, mBroadcastClip[num + 33]);
	}

	public void SetGameMusicVolume(float fVolume)
	{
		if (fVolume <= 1f && fVolume >= 0f)
		{
			for (int i = 0; i < _AllAudioSrc.Count; i++)
			{
				AudioSource audioSource = (AudioSource)_AllAudioSrc[i];
				audioSource.volume = PlayerPrefs.GetFloat("setBGVolume");
			}
			JSYS_LL_Parameter.G_fGameMusicVolume = fVolume;
		}
	}

	public void SetMusicPlay(bool isTurnOn)
	{
		JSYS_LL_Parameter.G_IsSoundOn = isTurnOn;
	}

	public void Reset()
	{
		StopCoroutine("_playPrize");
		for (int i = 0; i < _AllAudioSrc.Count; i++)
		{
			AudioSource audioSource = (AudioSource)_AllAudioSrc[i];
			if (audioSource.isPlaying)
			{
				audioSource.Stop();
			}
		}
		if (mUIAudio.isPlaying)
		{
			mUIAudio.Stop();
		}
	}

	public void ResetWithoutUI()
	{
		StopCoroutine("_playPrize");
		for (int i = 0; i < _AllAudioSrc.Count; i++)
		{
			AudioSource audioSource = (AudioSource)_AllAudioSrc[i];
			if (audioSource.isPlaying)
			{
				audioSource.Stop();
			}
		}
	}

	private void _playSound(AudioSource audio, AudioClip clip, bool isLoop = false)
	{
		if (audio != null && clip != null)
		{
			if (audio.isPlaying)
			{
				audio.Stop();
			}
			audio.loop = isLoop;
			audio.clip = clip;
			audio.volume = PlayerPrefs.GetFloat("setSoundVolume");
			audio.Play();
		}
	}
}
