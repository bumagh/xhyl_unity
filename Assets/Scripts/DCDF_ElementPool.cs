using System;
using System.Collections.Generic;
using UnityEngine;

public class DCDF_ElementPool : MonoBehaviour
{
	private static DCDF_ElementPool elementPool;

	public GameObject[] objElements = new GameObject[16];

	private Dictionary<int, Stack<GameObject>> dicElement = new Dictionary<int, Stack<GameObject>>();

	public static DCDF_ElementPool GetSingleton()
	{
		return elementPool;
	}

	private void Awake()
	{
		if (elementPool == null)
		{
			elementPool = this;
		}
		InitPool();
	}

	private void InitPool()
	{
		for (int i = 0; i < 11; i++)
		{
			Stack<GameObject> stack = new Stack<GameObject>();
			for (int j = 0; j < 13; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(objElements[i]);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localScale = Vector3.one;
				gameObject.name = i.ToString();
				gameObject.SetActive(value: false);
				stack.Push(gameObject);
			}
			dicElement.Add(i, stack);
		}
		for (int k = 11; k < 16; k++)
		{
			Stack<GameObject> stack2 = new Stack<GameObject>();
			for (int l = 0; l < 10; l++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(objElements[k]);
				gameObject2.transform.SetParent(base.transform);
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.name = k.ToString();
				gameObject2.SetActive(value: false);
				stack2.Push(gameObject2);
			}
			dicElement.Add(k, stack2);
		}
	}

	public GameObject GetElement(int i)
	{
		if (dicElement[i].Count <= 1)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(objElements[i]);
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = i.ToString();
			gameObject.SetActive(value: false);
			dicElement[i].Push(gameObject);
		}
		GameObject gameObject2 = dicElement[i].Pop();
		gameObject2.SetActive(value: true);
		return gameObject2;
	}

	public void GiveBackElement(GameObject obj)
	{
		if (!(obj == null))
		{
			int key = Convert.ToInt32(obj.name);
			obj.transform.SetParent(base.transform);
			obj.SetActive(value: false);
			dicElement[key].Push(obj);
		}
	}

	private void OnDestroy()
	{
		dicElement.Clear();
		elementPool = null;
	}
}
