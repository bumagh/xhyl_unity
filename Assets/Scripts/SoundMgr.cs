using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : HW2_Singleton<SoundMgr>
{
	protected bool _bEnabled;

	protected const int MAX_SOURCE = 20;

	protected AudioSource[] _arrSources;

	protected float _fDefaultVol = 0.5f;

	protected Dictionary<string, AudioClip> _audioClips;

	public GameObject soundMgrGmObj;

	public bool enabled
	{
		get
		{
			return _bEnabled;
		}
		set
		{
			_bEnabled = value;
			if (!_bEnabled)
			{
				StopAllSounds();
			}
		}
	}

	public void LoadSounds(AudioClip[] array)
	{
		foreach (AudioClip audioClip in array)
		{
			_audioClips.Add(audioClip.name, audioClip);
		}
	}

	public void UnloadAllSounds()
	{
		_audioClips.Clear();
		StopAllSounds();
		Resources.UnloadUnusedAssets();
	}

	public void SetVolume(int i, float newVol)
	{
		try
		{
			_arrSources[i].volume = newVol;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning(ex.Message);
		}
	}

	public void SetVolume(AudioClip c, float newVol)
	{
		for (int i = 0; i < _arrSources.Length; i++)
		{
			AudioSource audioSource = _arrSources[i];
			if (audioSource.clip == c)
			{
				audioSource.volume = newVol;
			}
		}
	}

	public void SetVolume(string soundName, float newVol)
	{
		AudioClip clip = GetClip(soundName);
		for (int i = 0; i < _arrSources.Length; i++)
		{
			AudioSource audioSource = _arrSources[i];
			if (audioSource.clip == clip)
			{
				audioSource.volume = newVol;
			}
		}
	}

	public int PlayClip(string soundName, bool loop = false)
	{
		AudioClip c = _audioClips[soundName];
		return PlayClip(c, loop);
	}

	public int PlayClip(string soundName, float vol, bool loop = false)
	{
		AudioClip c = _audioClips[soundName];
		return PlayClip(c, vol, loop);
	}

	public AudioClip GetClip(string soundName)
	{
		return _audioClips[soundName];
	}

	public int PlayClip(AudioClip c, bool loop = false)
	{
		if (!_bEnabled)
		{
			return -1;
		}
		for (int i = 0; i < _arrSources.Length; i++)
		{
			AudioSource audioSource = _arrSources[i];
			if (!audioSource.isPlaying)
			{
				audioSource.clip = c;
				audioSource.loop = loop;
				audioSource.Play();
				SetVolume(i, _fDefaultVol);
				return i;
			}
		}
		return -1;
	}

	public int PlayClip(AudioClip c, float vol, bool loop = false)
	{
		if (!_bEnabled)
		{
			return -1;
		}
		for (int i = 0; i < _arrSources.Length; i++)
		{
			AudioSource audioSource = _arrSources[i];
			if (!audioSource.isPlaying)
			{
				audioSource.clip = c;
				audioSource.loop = loop;
				audioSource.Play();
				SetVolume(i, vol);
				return i;
			}
		}
		return -1;
	}

	public int PlayClip(AudioClip c, int mask, bool loop)
	{
		if (!_bEnabled)
		{
			return -1;
		}
		for (int i = 0; i < _arrSources.Length; i++)
		{
			if ((mask & (1 << i)) > 0 && !_arrSources[i].isPlaying)
			{
				_arrSources[i].clip = c;
				_arrSources[i].loop = loop;
				_arrSources[i].Play();
				return i;
			}
		}
		return -1;
	}

	public void StopClip(AudioClip c)
	{
		AudioSource[] arrSources = _arrSources;
		foreach (AudioSource audioSource in arrSources)
		{
			if (audioSource != null && audioSource.clip == c && audioSource.isPlaying)
			{
				audioSource.Stop();
				audioSource.clip = null;
			}
		}
	}

	public void StopAllSounds()
	{
		for (int i = 0; i < _arrSources.Length; i++)
		{
			_arrSources[i].Stop();
			_arrSources[i].clip = null;
		}
	}

	public void StopChannel(int i)
	{
		_arrSources[i].Stop();
	}

	public void PauseChannel(int i)
	{
		if (_arrSources[i].isPlaying)
		{
			_arrSources[i].Pause();
		}
	}

	public void ResumeChannel(int i)
	{
		if (!_arrSources[i].isPlaying)
		{
			_arrSources[i].UnPause();
		}
	}

	private void _init(GameObject gameObject)
	{
		_bEnabled = true;
		if (gameObject.GetComponent<AudioListener>() == null)
		{
			gameObject.AddComponent<AudioListener>();
		}
		_arrSources = new AudioSource[20];
		for (int i = 0; i < _arrSources.Length; i++)
		{
			_arrSources[i] = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
		}
		_audioClips = new Dictionary<string, AudioClip>();
		soundMgrGmObj = gameObject;
	}

	public void ChangeValue(float newValue)
	{
		for (int i = 0; i < _arrSources.Length; i++)
		{
			SetVolume(i, newValue);
		}
	}

	public void SetActive(bool bFlag)
	{
		AudioListener.volume = (bFlag ? 1 : 0);
	}

	protected override void _onInit()
	{
		GameObject gameObject = new GameObject("SoundMgr");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		HW2_GVars.dontDestroyOnLoadList.Add(gameObject);
		_init(gameObject);
	}
}
