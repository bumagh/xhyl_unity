using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSYS_AnimationScene : MonoBehaviour
{
	public static JSYS_AnimationScene publicAnimationScene;

	[Header("燕子动画链表")]
	public List<Sprite> sprites_3 = new List<Sprite>();

	[Header("鸽子动画链表")]
	public List<Sprite> sprites_4 = new List<Sprite>();

	[Header("孔雀动画链表")]
	public List<Sprite> sprites_5 = new List<Sprite>();

	[Header("老鹰动画链表")]
	public List<Sprite> sprites_6 = new List<Sprite>();

	[Header("狮子动画链表")]
	public List<Sprite> sprites_7 = new List<Sprite>();

	[Header("熊猫动画链表")]
	public List<Sprite> sprites_8 = new List<Sprite>();

	[Header("猴子动画链表")]
	public List<Sprite> sprites_9 = new List<Sprite>();

	[Header("兔子动画链表")]
	public List<Sprite> sprites_10 = new List<Sprite>();

	[Header("动画组件链表")]
	public List<Image> images = new List<Image>();

	[Header("背景组件")]
	public Image Bg;

	[Header("背景图片资源链表[0走兽][1飞禽][2海洋]")]
	public List<Sprite> BgSprite = new List<Sprite>();

	[Header("动画场景[0默认场景][1金鲨场景][2银鲨场景]")]
	public List<GameObject> BgAudio = new List<GameObject>();

	[Header("倍率[0各位][1十位]")]
	public List<GameObject> Beilulait = new List<GameObject>();

	public List<Sprite> BeiLUSprite = new List<Sprite>();

	private bool PanDuan;

	private void Awake()
	{
		publicAnimationScene = this;
	}

	public IEnumerator AnimatonMethon(string Name)
	{
		PanDuan = false;
		int Laue = 0;
		Invoke("GuanBi", 5f);
		foreach (GameObject item in BgAudio)
		{
			item.SetActive(value: false);
		}
		switch (Name)
		{
		case "金鲨":
			BgAudio[1].SetActive(value: true);
			break;
		case "银鲨":
			BgAudio[2].SetActive(value: true);
			break;
		case "兔子":
			BgAudio[0].SetActive(value: true);
			Bg.sprite = BgSprite[0];
			BeiLu(4);
			while (true)
			{
				for (int num2 = 0; num2 < images.Count; num2++)
				{
					images[num2].sprite = sprites_10[Laue];
				}
				Laue = (Laue + 1) % sprites_10.Count;
				if (PanDuan)
				{
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
			break;
		case "猴子":
			BgAudio[0].SetActive(value: true);
			Bg.sprite = BgSprite[0];
			BeiLu(5);
			while (true)
			{
				for (int m = 0; m < images.Count; m++)
				{
					images[m].sprite = sprites_9[Laue];
				}
				Laue = (Laue + 1) % sprites_9.Count;
				if (PanDuan)
				{
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
			break;
		case "熊猫":
			BgAudio[0].SetActive(value: true);
			Bg.sprite = BgSprite[0];
			BeiLu(6);
			while (true)
			{
				for (int j = 0; j < images.Count; j++)
				{
					images[j].sprite = sprites_8[Laue];
				}
				Laue = (Laue + 1) % sprites_8.Count;
				if (PanDuan)
				{
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
			break;
		case "狮子":
			BgAudio[0].SetActive(value: true);
			Bg.sprite = BgSprite[0];
			BeiLu(7);
			while (true)
			{
				for (int num = 0; num < images.Count; num++)
				{
					images[num].sprite = sprites_7[Laue];
				}
				Laue = (Laue + 1) % sprites_7.Count;
				if (PanDuan)
				{
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
			break;
		case "老鹰":
			BgAudio[0].SetActive(value: true);
			Bg.sprite = BgSprite[1];
			BeiLu(3);
			while (true)
			{
				for (int n = 0; n < images.Count; n++)
				{
					images[n].sprite = sprites_6[Laue];
				}
				Laue = (Laue + 1) % sprites_6.Count;
				if (PanDuan)
				{
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
			break;
		case "孔雀":
			BgAudio[0].SetActive(value: true);
			Bg.sprite = BgSprite[1];
			BeiLu(2);
			while (true)
			{
				for (int l = 0; l < images.Count; l++)
				{
					images[l].sprite = sprites_5[Laue];
				}
				Laue = (Laue + 1) % sprites_5.Count;
				if (PanDuan)
				{
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
			break;
		case "鸽子":
			BgAudio[0].SetActive(value: true);
			Bg.sprite = BgSprite[1];
			BeiLu(1);
			while (true)
			{
				for (int k = 0; k < images.Count; k++)
				{
					images[k].sprite = sprites_4[Laue];
				}
				Laue = (Laue + 1) % sprites_4.Count;
				if (PanDuan)
				{
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
			break;
		case "燕子":
			BgAudio[0].SetActive(value: true);
			Bg.sprite = BgSprite[1];
			BeiLu(0);
			while (true)
			{
				for (int i = 0; i < images.Count; i++)
				{
					images[i].sprite = sprites_3[Laue];
				}
				Laue = (Laue + 1) % sprites_3.Count;
				if (PanDuan)
				{
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
			break;
		}
		yield return new WaitForSeconds(1f);
	}

	private void GuanBi()
	{
		PanDuan = true;
	}

	public void BeiLu(int index)
	{
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			UnityEngine.Debug.LogError("倍率表: " + JsonMapper.ToJson(JSYS_LL_GameInfo.getInstance().BeiLv));
		}
		int num = JSYS_LL_GameInfo.getInstance().BeiLv[index];
		UnityEngine.Debug.LogError("=====显示倍率: " + num);
		string text = num.ToString();
		int num2 = num / 10;
		num -= num2 * 10;
		int num3 = num;
		num -= num3;
		switch (text.Length)
		{
		case 2:
			Beilulait[1].SetActive(value: true);
			Beilulait[1].GetComponent<Image>().sprite = BeiLUSprite[num3];
			Beilulait[0].GetComponent<Image>().sprite = BeiLUSprite[num2];
			break;
		case 1:
			Beilulait[1].SetActive(value: false);
			Beilulait[0].GetComponent<Image>().sprite = BeiLUSprite[num3];
			break;
		}
	}
}
