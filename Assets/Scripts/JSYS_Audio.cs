using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSYS_Audio : MonoBehaviour
{
	public static JSYS_Audio publicAudio;

	public AudioSource BgAudio;

	public AudioSource ButtonAudio;

	public AudioSource RingAudio;

	[Header("背景音乐资源")]
	public List<AudioClip> BgAudioList = new List<AudioClip>();

	[Header("动物音效资源链表[0金鲨鱼][1银鲨鱼][2燕子][3鸽子][4孔雀][5老鹰][6狮子][7熊猫][8猴子][9兔子]")]
	public List<AudioClip> RingAudioList = new List<AudioClip>();

	public AudioClip HY;

	private void Awake()
	{
		publicAudio = this;
	}

	private void Start()
	{
		ButtonAudio.PlayOneShot(HY);
	}

	public void ButtonAudioMethon(AudioClip audioClip)
	{
		ButtonAudio.PlayOneShot(audioClip);
	}

	public IEnumerator AudiioMethon(string laue)
	{
		switch (laue)
		{
		case "金鲨":
			RingAudio.PlayOneShot(RingAudioList[0]);
			break;
		case "银鲨":
			RingAudio.PlayOneShot(RingAudioList[1]);
			break;
		case "狮子":
			RingAudio.PlayOneShot(RingAudioList[6]);
			break;
		case "老鹰":
			RingAudio.PlayOneShot(RingAudioList[5]);
			break;
		case "熊猫":
			RingAudio.PlayOneShot(RingAudioList[7]);
			break;
		case "孔雀":
			RingAudio.PlayOneShot(RingAudioList[4]);
			break;
		case "猴子":
			RingAudio.PlayOneShot(RingAudioList[8]);
			break;
		case "鸽子":
			RingAudio.PlayOneShot(RingAudioList[3]);
			break;
		case "兔子":
			RingAudio.PlayOneShot(RingAudioList[9]);
			break;
		case "燕子":
			RingAudio.PlayOneShot(RingAudioList[2]);
			break;
		}
		yield return null;
	}

	public void BgAudioMethon(string Name)
	{
		if (Name == null)
		{
			return;
		}
		if (!(Name == "开始旋转"))
		{
			if (Name == "停止旋转")
			{
				BgAudio.pitch = 1f;
				BgAudio.clip = BgAudioList[0];
				BgAudio.loop = true;
				BgAudio.Play();
			}
			return;
		}
		BgAudio.Stop();
		if (RingAudio.isPlaying)
		{
			RingAudio.pitch = 1.4f;
			return;
		}
		RingAudio.pitch = 1f;
		RingAudio.clip = BgAudioList[1];
		RingAudio.loop = false;
		RingAudio.Play();
	}
}
