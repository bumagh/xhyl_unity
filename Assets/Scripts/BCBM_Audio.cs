using System.Collections.Generic;
using UnityEngine;

public class BCBM_Audio : MonoBehaviour
{
	public static BCBM_Audio publicAudio;

	public AudioSource BgAudio;

	public AudioSource ButtonAudio;

	public AudioSource RingAudio;

	public AudioSource HongMAudio;

	public AudioClip audioClickClip;

	[Header("背景音乐资源")]
	public List<AudioClip> BgAudioList = new List<AudioClip>();

	public List<AudioClip> RingAudioList = new List<AudioClip>();

	public AudioClip HallClip;

	public AudioClip bg_Start;

	public AudioClip bg_Wait;

	public AudioClip[] startBetAndNedBet;

	public AudioClip hongMing;

	public AudioClip hongMingStart;

	public AudioClip chipClip;

	private void Awake()
	{
		publicAudio = this;
	}

	private void Start()
	{
	}

	public void PlayBg(int num, float pitch)
	{
		BgAudio.pitch = pitch;
		AudioSource bgAudio = BgAudio;
		object hallClip;
		switch (num)
		{
		case 0:
			hallClip = HallClip;
			break;
		case 1:
			hallClip = bg_Start;
			break;
		default:
			hallClip = bg_Wait;
			break;
		}
		bgAudio.clip = (AudioClip)hallClip;
		BgAudio.loop = true;
		BgAudio.Play();
	}

	public void PlayHongMIng()
	{
		HongMAudio.loop = false;
		HongMAudio.clip = hongMing;
		HongMAudio.Play();
	}

	public void PlayHongMIngStart()
	{
		HongMAudio.loop = false;
		HongMAudio.clip = hongMingStart;
		HongMAudio.Play();
	}

	public void PlaychipClip()
	{
		HongMAudio.loop = false;
		HongMAudio.clip = chipClip;
		HongMAudio.Play();
	}

	public void EndBet(bool isEndBet)
	{
		int num = isEndBet ? 1 : 0;
		ButtonAudio.loop = false;
		ButtonAudio.PlayOneShot(startBetAndNedBet[num]);
	}

	public void ButtonAudioMethon()
	{
		ButtonAudio.loop = false;
		ButtonAudio.PlayOneShot(audioClickClip);
	}

	public void AudiioMethon(int laue)
	{
		BgAudio.Stop();
		if (laue < RingAudioList.Count)
		{
			RingAudio.PlayOneShot(RingAudioList[laue]);
		}
		PlayHongMIng();
		BgAudio.Play();
	}

	public void BgAudioMethon(string Name)
	{
	}
}
