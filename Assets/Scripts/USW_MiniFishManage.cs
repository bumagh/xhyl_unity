using System;
using System.Collections.Generic;
using UnityEngine;

public class USW_MiniFishManage : MonoBehaviour
{
	private List<GameObject> miniFishLeftList = new List<GameObject>();

	private List<GameObject> miniFishRightList = new List<GameObject>();

	private List<GameObject> miniFishAllList = new List<GameObject>();

	private Transform miniFishLeft;

	private Transform miniFishRight;

	private void Awake()
	{
		Init();
	}

	private void Start()
	{
		InvokeRepeating("ActiveFishByPool", 0f, 3.5f);
	}

	private void Init()
	{
		miniFishLeft = base.transform.Find("MiniFishLeft");
		miniFishRight = base.transform.Find("MiniFishRight");
		miniFishRightList = new List<GameObject>();
		miniFishLeftList = new List<GameObject>();
		miniFishAllList = new List<GameObject>();
		for (int i = 0; i < miniFishLeft.transform.childCount; i++)
		{
			miniFishLeftList.Add(miniFishLeft.transform.GetChild(i).gameObject);
			miniFishRightList.Add(miniFishRight.transform.GetChild(i).gameObject);
			miniFishLeftList[i].gameObject.SetActive(value: false);
			miniFishRightList[i].gameObject.SetActive(value: false);
		}
		for (int j = 0; j < miniFishRightList.Count; j++)
		{
			miniFishAllList.Add(miniFishLeftList[j].gameObject);
			miniFishAllList.Add(miniFishRightList[j].gameObject);
		}
		miniFishAllList = RandomShuffle(miniFishAllList);
	}

	private void ActiveFishByPool()
	{
		for (int i = 0; i < UnityEngine.Random.Range(2, 5); i++)
		{
			miniFishAllList[UnityEngine.Random.Range(0, miniFishAllList.Count)].SetActive(GetPooledObject(miniFishAllList));
		}
	}

	private void Update()
	{
		if (miniFishAllList == null)
		{
			return;
		}
		for (int i = 0; i < miniFishAllList.Count; i++)
		{
			if (!miniFishAllList[i].gameObject.activeInHierarchy)
			{
				continue;
			}
			Vector3 localPosition = miniFishAllList[i].transform.localPosition;
			float x = localPosition.x;
			Vector3 localPosition2 = miniFishRight.localPosition;
			if (!(x > localPosition2.x * 2f))
			{
				Vector3 localPosition3 = miniFishAllList[i].transform.localPosition;
				float x2 = localPosition3.x;
				Vector3 localPosition4 = miniFishLeft.localPosition;
				if (!(x2 < localPosition4.x * 2f))
				{
					continue;
				}
			}
			miniFishAllList[i].gameObject.SetActive(value: false);
		}
	}

	public bool GetPooledObject(List<GameObject> pooledObject_List)
	{
		for (int i = 0; i < pooledObject_List.Count; i++)
		{
			if (!pooledObject_List[i].activeInHierarchy)
			{
				return true;
			}
		}
		return false;
	}

	public List<T> RandomShuffle<T>(List<T> original)
	{
		System.Random random = new System.Random();
		int num = 0;
		for (int i = 0; i < original.Count; i++)
		{
			num = random.Next(0, original.Count - 1);
			if (num != i)
			{
				T value = original[i];
				original[i] = original[num];
				original[num] = value;
			}
		}
		return original;
	}
}
