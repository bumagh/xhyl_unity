using GameCommon;
using System.Collections.Generic;
using UnityEngine;

public class DNTG_MusicMngr : MonoBehaviour
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

	public static DNTG_MusicMngr G_MusicMngr;

	public AudioClip[] mGameSoundClip;

	public Transform _soundPrefab;

	private List<DNTG_SoundPlay> _soundList = new List<DNTG_SoundPlay>();

	public static DNTG_MusicMngr GetSingleton()
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

	public void PlayRandomWordsByFish(DNTG_FISH_TYPE fishType)
	{
		if (fishType >= DNTG_FISH_TYPE.Fish_Turtle && fishType <= DNTG_FISH_TYPE.Fish_Boss)
		{
			int soundType = UnityEngine.Random.Range(7, 23);
			PlayGameSound((GAME_SOUND)soundType);
		}
		else if (fishType >= DNTG_FISH_TYPE.Fish_Same_Shrimp && fishType <= DNTG_FISH_TYPE.Fish_Turtle_Group)
		{
			int soundType2 = UnityEngine.Random.Range(7, 23);
			PlayGameSound((GAME_SOUND)soundType2);
		}
	}

	public void PlayFishCaught(DNTG_FISH_TYPE fishType)
	{
		int fishOODS = DNTG_EffectMngr.GetSingleton().GetFishOODS(fishType);
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
		if (!DNTG_GameParameter.G_IsSoundOn)
		{
			return null;
		}
		if ((soundType == GAME_SOUND.SOUND_NORMAL_SHOOT || soundType == GAME_SOUND.SOUND_LIZI_SHOOT || soundType == GAME_SOUND.SOUND_NORMAL_BULLET_EXPLODE || soundType == GAME_SOUND.SOUND_LIZI_EXPLODE) && !DNTG_GameParameter.G_IsSoundFxOn)
		{
			return null;
		}
		DNTG_SoundPlay dNTG_SoundPlay;
		if (_soundList.Count >= 20)
		{
			dNTG_SoundPlay = _soundList[0];
			dNTG_SoundPlay.ObjDestroy();
		}
		dNTG_SoundPlay = DNTG_PoolManager.Pools["DNTGSoundPool"].Spawn(_soundPrefab).GetComponent<DNTG_SoundPlay>();
		if (dNTG_SoundPlay != null)
		{
			dNTG_SoundPlay.Play(mGameSoundClip[(int)soundType], isLoop, DNTG_GameParameter.G_fGameMusicVolume);
			_soundList.Insert(_soundList.Count, dNTG_SoundPlay);
		}
		else
		{
			UnityEngine.Debug.LogError("=====sp为空=====");
		}
		return base.transform;
	}

	public void DestroySound(DNTG_SoundPlay sp)
	{
		_soundList.Remove(sp);
		DNTG_PoolManager.Pools["DNTGSoundPool"].Despawn(sp.transform);
	}

	public void SetMusicOnOff(bool isTurnOn)
	{
		DNTG_GameParameter.G_IsSoundOn = isTurnOn;
		DNTG_PoolManager.Pools["DNTGSoundPool"].DespawnAll();
		_soundList.Clear();
	}

	public void SetMusicFxOnOff(bool isTurnOn)
	{
		DNTG_GameParameter.G_IsSoundFxOn = isTurnOn;
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
				audio.volume = DNTG_GameParameter.G_fGameMusicVolume;
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
