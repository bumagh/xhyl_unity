using System.Collections;
using UnityEngine;

public class Dzb_AudioManager : Dzb_Singleton<Dzb_AudioManager>
{
	public AudioClip[] _BGMClip;

	public AudioClip _clickClip;

	public AudioClip _errorClip;

	public AudioClip _failClip;

	public AudioClip _sucClip;

	public AudioClip[] _Hold_1;

	public AudioClip[] _Hold_2;

	public AudioClip[] _scoreClip;

	private AudioSource _Sound;

	private AudioSource _Music;

	public Coroutine _coPlayScore;

	private void Awake()
	{
		Dzb_Singleton<Dzb_AudioManager>.SetInstance(this);
		_Sound = base.transform.Find("Sound").GetComponent<AudioSource>();
		_Music = base.transform.Find("Music").GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey(DZb_MySlider.SliderType.SoundVolume.ToString()))
		{
			Dzb_GameInfo.SoundVolume = PlayerPrefs.GetFloat(DZb_MySlider.SliderType.SoundVolume.ToString());
		}
		else
		{
			Dzb_GameInfo.SoundVolume = 1f;
		}
		if (PlayerPrefs.HasKey(DZb_MySlider.SliderType.MusicVolume.ToString()))
		{
			Dzb_GameInfo.MusicVolume = PlayerPrefs.GetFloat(DZb_MySlider.SliderType.MusicVolume.ToString());
		}
		else
		{
			Dzb_GameInfo.MusicVolume = 1f;
		}
		UpdateSoundVolume(Dzb_GameInfo.SoundVolume);
		UpdateMusicVolume(Dzb_GameInfo.MusicVolume);
	}

	public void UpdateSoundVolume(float value)
	{
		_Sound.volume = value;
	}

	public void UpdateMusicVolume(float value)
	{
		_Music.volume = value;
	}

	public void PlayMusic(int num)
	{
		if (num < _BGMClip.Length)
		{
			_Music.clip = _BGMClip[num];
			_Music.Play();
		}
	}

	public void PlaySound(string type)
	{
		if (type != null)
		{
			if (!(type == "click"))
			{
				if (!(type == "error"))
				{
					if (!(type == "fail"))
					{
						if (type == "suc")
						{
							_Sound.clip = _sucClip;
						}
					}
					else
					{
						_Sound.clip = _failClip;
					}
				}
				else
				{
					_Sound.clip = _errorClip;
				}
			}
			else
			{
				_Sound.clip = _clickClip;
			}
		}
		_Sound.Play();
	}

	public void PlaySound_Hold1(int num)
	{
		_Sound.clip = _Hold_1[num];
		_Sound.Play();
	}

	public void PlaySound_Hold2(int num)
	{
		_Sound.clip = _Hold_2[num];
		_Sound.Play();
	}

	public void PlaySound_Score()
	{
		if (_coPlayScore != null)
		{
			StopCoroutine(_coPlayScore);
			_coPlayScore = null;
		}
		_coPlayScore = StartCoroutine(PlayScore_IE());
	}

	public void StopSound_Score()
	{
		if (_coPlayScore != null)
		{
			StopCoroutine(_coPlayScore);
			_coPlayScore = null;
		}
	}

	private IEnumerator PlayScore_IE()
	{
		int _num = 0;
		while (true)
		{
			if (!_Sound.isPlaying)
			{
				_Sound.clip = _scoreClip[_num];
				_Sound.Play();
				_num++;
				if (_num >= _scoreClip.Length)
				{
					_num = 0;
				}
			}
			yield return new WaitForSeconds(0.02f);
		}
	}
}
