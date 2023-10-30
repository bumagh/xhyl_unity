using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSYS_NumSpriteControl : MonoBehaviour
{
	public List<Sprite> num_Sprite;

	public static JSYS_NumSpriteControl Instances;

	private int childNum;

	public Transform imageParent;

	private void Awake()
	{
		Instances = this;
	}

	public void StopImage(int num)
	{
		if (childNum < 10 && imageParent.GetChild(childNum).GetComponent<JSYS_NumSprite>().isChoose)
		{
			imageParent.GetChild(childNum).GetComponent<JSYS_NumSprite>().isChoose = false;
			imageParent.GetChild(childNum).GetComponent<Image>().sprite = num_Sprite[num - 1];
			childNum++;
		}
	}

	public void StopImage(int[] num)
	{
		if (childNum == 0 && imageParent.GetChild(childNum).GetComponent<JSYS_NumSprite>().isChoose)
		{
			for (int i = 0; i < num.Length; i++)
			{
				imageParent.GetChild(childNum).GetComponent<JSYS_NumSprite>().isChoose = false;
				imageParent.GetChild(childNum).GetComponent<Image>().sprite = num_Sprite[num[i]];
				childNum++;
			}
		}
	}

	public void SetImage(int[] num)
	{
		childNum = 0;
		for (int i = 0; i < num.Length; i++)
		{
			imageParent.GetChild(childNum).GetComponent<Image>().sprite = num_Sprite[num[i] - 1];
			childNum++;
		}
	}

	public void StartImage()
	{
		JSYS_NumSprite[] componentsInChildren = imageParent.GetComponentsInChildren<JSYS_NumSprite>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].isChoose)
			{
				componentsInChildren[i].isChoose = true;
				componentsInChildren[i].StartCoroutine("ChooseSprite", 0.1f);
			}
		}
		childNum = 0;
	}
}
