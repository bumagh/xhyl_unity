using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
	private Transform tfScroll;

	private int indexRoll;

	private float speed = 14f;

	private float progress;

	private float progress1;

	private bool bRoll;

	private float[] cellScrollPos = new float[9]
	{
		0.02f,
		0.78f,
		0.56f,
		0.67f,
		0.23f,
		0.9f,
		0.34f,
		0.12f,
		0.44f
	};

	private float[] posScroll = new float[9]
	{
		15f,
		805f,
		565f,
		685f,
		230f,
		925f,
		345f,
		125f,
		455f
	};

	private List<int> listA = new List<int>
	{
		2
	};

	private List<int> listB = new List<int>
	{
		1,
		2,
		3
	};

	private List<int> listC = new List<int>
	{
		1,
		2,
		3,
		5
	};

	private List<List<int>> list = new List<List<int>>();

	private int[,] arr = new int[5, 3];

	private List<int[]> lists = new List<int[]>();

	private int count;

	private void Start()
	{
	}

	private bool IsContainsAll(List<int> ListA, List<int> ListB)
	{
		return ListB.All((int b) => ListA.Any((int a) => a == b));
	}

	private bool Contains(List<List<int>> list1, List<int> list2)
	{
		for (int i = 0; i < list1.Count; i++)
		{
			for (int j = 0; j < list2.Count && list1[i].Contains(list2[j]); j++)
			{
				if (list1[i].Contains(list2[j]) && j == list2.Count - 1)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void Loop()
	{
		if (bRoll)
		{
			tfScroll.localPosition = Vector3.up * progress;
			progress -= Time.deltaTime * speed * 100f;
		}
	}

	private IEnumerator NormalRoll()
	{
		bRoll = true;
		NormalResetElements();
		float acc2 = 40f;
		speed = 0f;
		float accTime = 0.5f;
		float timer2 = 0f;
		while (timer2 < accTime)
		{
			speed += acc2 * Time.deltaTime;
			timer2 += Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(2.2f);
		float acc = 10f;
		float timer = 0f;
		while (progress >= 140f)
		{
			speed -= acc * Time.deltaTime;
			timer += Time.deltaTime;
			yield return null;
		}
		bRoll = false;
		tfScroll.DOLocalMoveY(125f, 0.2f);
		tfScroll.DOLocalMoveY(140f, 0.2f).SetDelay(0.2f);
	}

	private IEnumerator SpeedUpRoll()
	{
		bRoll = true;
		SpeedUpResetElements();
		float acc2 = 40f;
		speed = 0f;
		float accTime = 0.5f;
		float timer2 = 0f;
		while (timer2 < accTime)
		{
			speed += acc2 * Time.deltaTime;
			timer2 += Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(1.2f);
		float acc = 10f;
		float timer = 0f;
		while (progress >= 140f)
		{
			speed -= acc * Time.deltaTime;
			timer += Time.deltaTime;
			yield return null;
		}
		bRoll = false;
		tfScroll.DOLocalMoveY(125f, 0.2f);
		tfScroll.DOLocalMoveY(140f, 0.2f).SetDelay(0.2f);
	}

	private void NormalResetElements()
	{
		for (int i = 3; i < tfScroll.childCount; i++)
		{
			DCDF_ElementPool.GetSingleton().GiveBackElement(tfScroll.GetChild(i).gameObject);
		}
		for (int num = tfScroll.childCount - 1; num >= 0; num--)
		{
			tfScroll.GetChild(num).localPosition = Vector3.up * (-num - 36 - indexRoll * 2) * 140f;
		}
		for (int j = 3; j < 36 + indexRoll * 2; j++)
		{
			int i2 = UnityEngine.Random.Range(0, 11);
			GameObject element = DCDF_ElementPool.GetSingleton().GetElement(i2);
			element.transform.SetParent(tfScroll);
			element.transform.SetAsFirstSibling();
			element.transform.localScale = Vector3.one;
			element.transform.localPosition = Vector3.up * -j * 140f;
		}
		for (int num2 = 2; num2 >= 0; num2--)
		{
			int i3 = UnityEngine.Random.Range(0, 11);
			GameObject element2 = DCDF_ElementPool.GetSingleton().GetElement(i3);
			element2.transform.SetParent(tfScroll);
			element2.transform.SetAsFirstSibling();
			element2.transform.localScale = Vector3.one;
			element2.transform.localPosition = Vector3.up * -num2 * 140f;
		}
		tfScroll.localPosition = Vector3.up * (37 + indexRoll * 2) * 140f;
		progress = (float)(37 + indexRoll * 2) * 140f;
	}

	private void SpeedUpResetElements()
	{
		for (int i = 3; i < tfScroll.childCount; i++)
		{
			DCDF_ElementPool.GetSingleton().GiveBackElement(tfScroll.GetChild(i).gameObject);
		}
		for (int num = tfScroll.childCount - 1; num >= 0; num--)
		{
			tfScroll.GetChild(num).localPosition = Vector3.up * (-num - 24) * 140f;
		}
		for (int j = 3; j < 24; j++)
		{
			int i2 = UnityEngine.Random.Range(0, 11);
			GameObject element = DCDF_ElementPool.GetSingleton().GetElement(i2);
			element.transform.SetParent(tfScroll);
			element.transform.SetAsFirstSibling();
			element.transform.localScale = Vector3.one;
			element.transform.localPosition = Vector3.up * -j * 140f;
		}
		for (int num2 = 2; num2 >= 0; num2--)
		{
			int i3 = UnityEngine.Random.Range(0, 11);
			GameObject element2 = DCDF_ElementPool.GetSingleton().GetElement(i3);
			element2.transform.SetParent(tfScroll);
			element2.transform.SetAsFirstSibling();
			element2.transform.localScale = Vector3.one;
			element2.transform.localPosition = Vector3.up * -num2 * 140f;
		}
		tfScroll.localPosition = Vector3.up * 25f * 140f;
		progress = 3500f;
	}

	private void InitArr()
	{
		int num = 1;
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				arr[i, j] = num;
				num++;
			}
		}
	}

	private void GetLists()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					for (int l = 0; l < 3; l++)
					{
						for (int m = 0; m < 3; m++)
						{
							int[] item = new int[5]
							{
								arr[0, i],
								arr[1, j],
								arr[2, k],
								arr[3, l],
								arr[4, m]
							};
							lists.Add(item);
						}
					}
				}
			}
		}
	}

	private void Fun(int n, int depth)
	{
		for (int i = 0; i < n; i++)
		{
			if (depth == 1)
			{
				int[] array = new int[5];
			}
			else
			{
				Fun(n, depth - 1);
			}
		}
	}

	private void PrintLists()
	{
		for (int i = 0; i < lists.Count; i++)
		{
			string text = null;
			for (int j = 0; j < 5; j++)
			{
				text = text + lists[i][j] + ",";
			}
			MonoBehaviour.print(text);
		}
	}
}
