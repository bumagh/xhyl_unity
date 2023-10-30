using System.Collections;
using UnityEngine;

public class Hfh_AudioManager : Hfh_Singleton<Hfh_AudioManager>
{
	public AudioClip[] _BGMClip;

	public AudioClip _clickClip;

	public AudioClip _drawClip;

	public AudioClip[] _typeClips;

	public AudioClip _errorClip;

	public AudioClip _failClip;

	public AudioClip _sucClip;

	public AudioClip _bbWinClip;

	public AudioClip _blowClip;

	public AudioClip[] _scoreClip;

	private AudioSource _Sound;

	private AudioSource _Music;

	private AudioSource _Voice;

	public Coroutine _coPlayScore;

	private void Awake()
	{
		Hfh_Singleton<Hfh_AudioManager>.SetInstance(this);
		_Sound = base.transform.Find("Sound").GetComponent<AudioSource>();
		_Music = base.transform.Find("Music").GetComponent<AudioSource>();
		_Voice = base.transform.Find("Voice").GetComponent<AudioSource>();
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

	public void PlaySound_Hold()
	{
		_Sound.clip = _drawClip;
		_Sound.Play();
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
						if (!(type == "suc"))
						{
							if (!(type == "bwin"))
							{
								if (type == "blow")
								{
									_Sound.clip = _blowClip;
								}
							}
							else
							{
								_Sound.clip = _bbWinClip;
							}
						}
						else
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

	public void PlayVoice(int type)
	{
		switch (type)
		{
		case 11:
			_Voice.clip = _typeClips[0];
			break;
		case 10:
			_Voice.clip = _typeClips[1];
			break;
		case 9:
			_Voice.clip = _typeClips[2];
			break;
		case 8:
			_Voice.clip = _typeClips[3];
			break;
		case 7:
			_Voice.clip = _typeClips[4];
			break;
		case 6:
			_Voice.clip = _typeClips[5];
			break;
		case 5:
			_Voice.clip = _typeClips[6];
			break;
		case 4:
			_Voice.clip = _typeClips[7];
			break;
		case 3:
			_Voice.clip = _typeClips[8];
			break;
		case 2:
			_Voice.clip = _typeClips[9];
			break;
		case 1:
			_Voice.clip = _typeClips[10];
			break;
		case 0:
			_Voice.clip = _typeClips[11];
			break;
		}
		if (type >= 0)
		{
			_Voice.Play();
		}
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
