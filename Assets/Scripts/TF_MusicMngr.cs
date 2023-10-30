using GameCommon;
using System.Collections.Generic;
using UnityEngine;

public class TF_MusicMngr : MonoBehaviour
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

	public static TF_MusicMngr G_MusicMngr;

	public AudioClip[] mGameSoundClip;

	public Transform _soundPrefab;

	private List<TF_SoundPlay> _soundList = new List<TF_SoundPlay>();

	public static TF_MusicMngr GetSingleton()
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

	public void PlayRandomWordsByFish(TF_FISH_TYPE fishType)
	{
		if (fishType >= TF_FISH_TYPE.Fish_Turtle && fishType <= TF_FISH_TYPE.Fish_Toad)
		{
			int soundType = UnityEngine.Random.Range(7, 23);
			PlayGameSound((GAME_SOUND)soundType);
		}
		else if (fishType >= TF_FISH_TYPE.Fish_Same_Shrimp && fishType <= TF_FISH_TYPE.Fish_Turtle_Group)
		{
			int soundType2 = UnityEngine.Random.Range(7, 23);
			PlayGameSound((GAME_SOUND)soundType2);
		}
	}

	public void PlayFishCaught(TF_FISH_TYPE fishType)
	{
		int fishOODS = TF_EffectMngr.GetSingleton().GetFishOODS(fishType);
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
		if (!TF_GameParameter.G_IsSoundOn)
		{
			return null;
		}
		if ((soundType == GAME_SOUND.SOUND_NORMAL_SHOOT || soundType == GAME_SOUND.SOUND_LIZI_SHOOT || soundType == GAME_SOUND.SOUND_NORMAL_BULLET_EXPLODE || soundType == GAME_SOUND.SOUND_LIZI_EXPLODE) && !TF_GameParameter.G_IsSoundFxOn)
		{
			return null;
		}
		TF_SoundPlay tF_SoundPlay;
		if (_soundList.Count >= 20)
		{
			tF_SoundPlay = _soundList[0];
			tF_SoundPlay.ObjDestroy();
		}
		tF_SoundPlay = TF_PoolManager.Pools["TFSoundPool"].Spawn(_soundPrefab).GetComponent<TF_SoundPlay>();
		tF_SoundPlay.Play(mGameSoundClip[(int)soundType], isLoop, TF_GameParameter.G_fGameMusicVolume);
		_soundList.Insert(_soundList.Count, tF_SoundPlay);
		return base.transform;
	}

	public void DestroySound(TF_SoundPlay sp)
	{
		_soundList.Remove(sp);
		TF_PoolManager.Pools["TFSoundPool"].Despawn(sp.transform);
	}

	public void SetMusicOnOff(bool isTurnOn)
	{
		TF_GameParameter.G_IsSoundOn = isTurnOn;
		TF_PoolManager.Pools["TFSoundPool"].DespawnAll();
		_soundList.Clear();
	}

	public void SetMusicFxOnOff(bool isTurnOn)
	{
		TF_GameParameter.G_IsSoundFxOn = isTurnOn;
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
				audio.volume = TF_GameParameter.G_fGameMusicVolume;
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
