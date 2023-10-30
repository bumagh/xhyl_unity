using GameCommon;
using System.Collections.Generic;
using UnityEngine;

public class DK_MusicMngr : MonoBehaviour
{
	public enum MUSIC_GAME_MUSIC
	{
		GAME_BG0,
		GAME_BG1,
		GAME_BG2,
		GAME_BG3,
		GAME_BG_NUMBER
	}

	public enum GAME_SOUND
	{
		SOUND_LIZI_EXPLODE = 0,
		SOUND_SILVER_COIN = 1,
		SOUND_GOLD_COIN = 2,
		SOUND_SMALLFISH_CAUGHT = 3,
		SOUND_MIDDLEFISH_CAUGHT = 4,
		SOUND_BIGFISH_CAUGHT = 5,
		SOUND_SUPERBOMB = 6,
		SOUND_SCORE_PANEL = 7,
		SOUND_FISH_SOUND = 8,
		SOUND_CLEAR_SCENE = 24,
		SOUND_NORMAL_SHOOT = 25,
		SOUND_LIZI_SHOOT = 26,
		SOUND_NORMAL_BULLET_EXPLODE = 27,
		SOUND_NUMBER = 28
	}

	public static DK_MusicMngr G_MusicMngr;

	public AudioClip[] mGameSoundClip;

	public Transform _soundPrefab;

	private List<DK_SoundPlay> _soundList = new List<DK_SoundPlay>();

	public static DK_MusicMngr GetSingleton()
	{
		return G_MusicMngr;
	}

	private void Awake()
	{
		if (G_MusicMngr == null)
		{
			G_MusicMngr = this;
		}
	}

	public void PlayRandomWordsByFish(DK_FISH_TYPE fishType)
	{
		if (fishType >= DK_FISH_TYPE.Fish_Turtle && fishType <= DK_FISH_TYPE.Fish_NiuMoWang)
		{
			int soundType = UnityEngine.Random.Range(7, 23);
			PlayGameSound((GAME_SOUND)soundType);
		}
		else if (fishType >= DK_FISH_TYPE.Fish_Same_Shrimp && fishType <= DK_FISH_TYPE.Fish_Double_Kill)
		{
			int soundType2 = UnityEngine.Random.Range(7, 23);
			PlayGameSound((GAME_SOUND)soundType2);
		}
	}

	public void PlayFishCaught(DK_FISH_TYPE fishType)
	{
		int fishOODS = DK_EffectMngr.GetSingleton().GetFishOODS(fishType);
		if (fishOODS <= 9 && fishOODS >= 1)
		{
			PlayGameSound(GAME_SOUND.SOUND_SMALLFISH_CAUGHT);
		}
		else if (fishOODS <= 18 && fishOODS >= 10)
		{
			PlayGameSound(GAME_SOUND.SOUND_MIDDLEFISH_CAUGHT);
		}
		else if (fishOODS >= 20)
		{
			PlayGameSound(GAME_SOUND.SOUND_BIGFISH_CAUGHT);
		}
	}

	public Transform PlayGameSound(GAME_SOUND soundType, bool isLoop = false)
	{
		if (soundType >= GAME_SOUND.SOUND_NUMBER || soundType < GAME_SOUND.SOUND_LIZI_EXPLODE)
		{
			UnityEngine.Debug.Log("@MusicMngr PlayGameSound Error, with error soundType : " + (int)soundType);
			return null;
		}
		if (!DK_GameParameter.G_IsSoundOn)
		{
			return null;
		}
		if ((soundType == GAME_SOUND.SOUND_NORMAL_SHOOT || soundType == GAME_SOUND.SOUND_LIZI_SHOOT || soundType == GAME_SOUND.SOUND_NORMAL_BULLET_EXPLODE || soundType == GAME_SOUND.SOUND_LIZI_EXPLODE) && !DK_GameParameter.G_IsSoundFxOn)
		{
			return null;
		}
		DK_SoundPlay dK_SoundPlay;
		if (_soundList.Count >= 20)
		{
			dK_SoundPlay = _soundList[0];
			dK_SoundPlay.ObjDestroy();
		}
		dK_SoundPlay = DK_PoolManager.Pools["DKSoundPool"].Spawn(_soundPrefab).GetComponent<DK_SoundPlay>();
		dK_SoundPlay.Play(mGameSoundClip[(int)soundType], isLoop, DK_GameParameter.G_fGameMusicVolume);
		_soundList.Insert(_soundList.Count, dK_SoundPlay);
		return base.transform;
	}

	public void DestroySound(DK_SoundPlay sp)
	{
		_soundList.Remove(sp);
		DK_PoolManager.Pools["DKSoundPool"].Despawn(sp.transform);
	}

	public void SetMusicOnOff(bool isTurnOn)
	{
		DK_GameParameter.G_IsSoundOn = isTurnOn;
		DK_PoolManager.Pools["DKSoundPool"].DespawnAll();
		_soundList.Clear();
	}

	public void SetMusicFxOnOff(bool isTurnOn)
	{
		DK_GameParameter.G_IsSoundFxOn = isTurnOn;
	}

	private void _playSound(AudioSource audio, AudioClip clip, bool isLoop = false)
	{
		if (audio != null)
		{
			if (clip != null)
			{
				if (audio.isPlaying)
				{
					audio.Stop();
				}
				audio.loop = isLoop;
				audio.clip = clip;
				audio.volume = DK_GameParameter.G_fGameMusicVolume;
				audio.Play();
			}
			else
			{
				UnityEngine.Debug.Log("_playSound audio null!");
			}
		}
		else
		{
			UnityEngine.Debug.Log("_playSound clip null!");
		}
	}
}
