using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSYS_Audiomanger : MonoBehaviour
{
	public static JSYS_Audiomanger _instenc;

	public AudioClip[] LobbyBG;

	public AudioClip GameBG;

	public AudioClip winmusic;

	public AudioClip clickbuttom;

	public AudioClip dangshuangBG;

	public AudioClip newBG;

	public AudioClip kaiJiang;

	public List<AudioClip> pokerColor;

	public List<AudioClip> pokerNum;

	public List<AudioClip> playZhuangXian;

	public List<AudioClip> playTip;

	public List<AudioClip> playDanShuang;

	public List<AudioClip> playElb;

	public List<AudioClip> playTianDi;

	public AudioClip ZhuangZeng;

	public AudioClip XianZeng;

	public AudioClip ZhuangP;

	public AudioClip XianP;

	public AudioClip[] LongHuHe;

	public AudioClip[] zhuangnumber;

	public AudioClip[] xiannumber;

	public AudioClip[] Xwy_Win;

	private void Start()
	{
		_instenc = this;
	}

	public void ChangeBGMusic_KaiJiang()
	{
		AudioSource component = base.transform.GetComponent<AudioSource>();
		component.clip = kaiJiang;
		component.Play();
	}

	public void ChangeBGMusic()
	{
		AudioSource component = base.transform.GetComponent<AudioSource>();
		component.clip = newBG;
		component.Play();
	}

	public void playwinmusic()
	{
		AudioSource component = base.transform.GetChild(0).GetComponent<AudioSource>();
		component.clip = winmusic;
		component.Play();
	}

	public void playfromGo(GameObject musicGo)
	{
		if (musicGo != null)
		{
			musicGo.GetComponent<AudioSource>().Play();
		}
	}

	public void clickvoice()
	{
		AudioSource component = base.transform.GetChild(0).GetComponent<AudioSource>();
		component.clip = clickbuttom;
		component.Play();
	}

	public void OnClick(string value)
	{
		if (!(value == string.Empty))
		{
		}
	}

	public void PlayDanTiaoWin(int num)
	{
		StartCoroutine(PlayDanTiao(num));
	}

	private IEnumerator PlayDanTiao(int pokerNum_)
	{
		int color = pokerNum_ / 13;
		int num = pokerNum_ % 13;
		if (color != 4)
		{
			AudioSource colorAs = base.transform.GetChild(1).GetComponent<AudioSource>();
			colorAs.clip = pokerColor[color];
			colorAs.Play();
			yield return new WaitForSeconds(colorAs.clip.length);
			colorAs.clip = pokerNum[num];
			colorAs.Play();
		}
		else if (num == 0)
		{
			AudioSource component = base.transform.GetChild(1).GetComponent<AudioSource>();
			component.clip = pokerColor[13];
			component.Play();
		}
		else
		{
			AudioSource component2 = base.transform.GetChild(1).GetComponent<AudioSource>();
			component2.clip = pokerColor[14];
			component2.Play();
		}
	}

	public void PlayTip(int num)
	{
		AudioSource component = base.transform.GetChild(1).GetComponent<AudioSource>();
		component.clip = playTip[num];
		component.Play();
	}

	public void PlayDanShuangWin(int num)
	{
		AudioSource component = base.transform.GetChild(1).GetComponent<AudioSource>();
		component.clip = playDanShuang[num];
		component.Play();
	}

	public void PlayElbWin(int num)
	{
		AudioSource component = base.transform.GetChild(1).GetComponent<AudioSource>();
		component.clip = playElb[num];
		component.Play();
	}

	public void PlayZhuangXianWin(int num)
	{
		AudioSource component = base.transform.GetChild(1).GetComponent<AudioSource>();
		component.clip = playZhuangXian[num];
		component.Play();
	}

	public void PlayTianDiWin(int num)
	{
		AudioSource component = base.transform.GetChild(1).GetComponent<AudioSource>();
		component.clip = playTianDi[num];
		component.Play();
	}

	public void PlaySound(AudioClip clip)
	{
		AudioSource component = base.transform.GetChild(1).GetComponent<AudioSource>();
		component.clip = clip;
		component.Play();
	}

	public void playZhuangnumber(int num)
	{
		AudioSource component = base.transform.GetChild(1).GetComponent<AudioSource>();
		component.clip = zhuangnumber[num];
		component.Play();
	}

	public void playXiannumber(int num)
	{
		AudioSource component = base.transform.GetChild(1).GetComponent<AudioSource>();
		component.clip = xiannumber[num];
		component.Play();
	}
}
