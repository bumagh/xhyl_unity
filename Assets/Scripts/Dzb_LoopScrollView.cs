using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class Dzb_LoopScrollView : MonoBehaviour
{
	private List<GameObject> goList;

	private Queue<GameObject> freeGoQueue;

	private Dictionary<GameObject, int> goIndexDic;

	private int startIndex;

	private int maxCount;

	private int createCount;

	private int cacheCount;

	private const int invalidStartIndex = -1;

	private ScrollRect scrollrect;

	private RectTransform Content;

	public GameObject prefabGo;

	private Action<GameObject, int> updateCellCB;

	private Vector2 scrollRectSize;

	private Vector2 cellSize;

	private float cellPadding;

	private int dataCount;

	private int oldCount;

	private bool IsInit;

	public int SetcacheCount
	{
		get
		{
			return cacheCount;
		}
		set
		{
			cacheCount = value;
		}
	}

	public void Init(int _count, Action<GameObject, int> _updateCellCB)
	{
		if (!IsInit)
		{
			goList = new List<GameObject>();
			freeGoQueue = new Queue<GameObject>();
			goIndexDic = new Dictionary<GameObject, int>();
			scrollrect = GetComponent<ScrollRect>();
			Content = scrollrect.content;
			scrollRectSize = scrollrect.GetComponent<RectTransform>().sizeDelta;
			float width = scrollrect.GetComponent<RectTransform>().rect.width;
			float height = scrollrect.GetComponent<RectTransform>().rect.height;
			if (scrollrect.horizontal)
			{
				cellSize = width / (float)cacheCount * Vector2.right + height * Vector2.up;
			}
			else
			{
				cellSize = width * Vector2.right + height / (float)cacheCount * Vector2.up;
			}
			UnityEngine.Debug.Log(cellSize);
			prefabGo.GetComponent<RectTransform>().sizeDelta = cellSize;
			dataCount = _count;
			startIndex = 0;
			maxCount = GetMaxCount();
			createCount = 0;
			updateCellCB = _updateCellCB;
			if (scrollrect.horizontal)
			{
				Content.anchorMin = new Vector2(0f, 0f);
				Content.anchorMax = new Vector2(0f, 1f);
			}
			else
			{
				Content.anchorMin = new Vector2(0f, 1f);
				Content.anchorMax = new Vector2(1f, 1f);
			}
			scrollrect.onValueChanged.RemoveAllListeners();
			scrollrect.onValueChanged.AddListener(OnValueChanged);
			ResetSize();
			IsInit = true;
		}
	}

	public void ResetSize()
	{
		Content.sizeDelta = GetContentSize();
		for (int num = goList.Count - 1; num >= 0; num--)
		{
			GameObject go = goList[num];
			RecycleItem(go);
		}
		createCount = cacheCount + 1;
		for (int i = 0; i < createCount; i++)
		{
			UseItem(i);
		}
		startIndex = -1;
		Content.anchoredPosition = Vector3.zero;
		OnValueChanged(Vector2.zero);
	}

	public void UpdateList(int _count)
	{
		dataCount = _count;
		Content.sizeDelta = GetContentSize();
		if (_count > cacheCount)
		{
			for (int i = 0; i < Content.childCount; i++)
			{
				Content.GetChild(i).gameObject.SetActive(value: true);
			}
			for (int j = 0; j < goList.Count; j++)
			{
				GameObject gameObject = goList[j];
				int arg = goIndexDic[gameObject];
				updateCellCB(gameObject, arg);
			}
		}
		else if (_count == cacheCount)
		{
			for (int k = 0; k < Content.childCount; k++)
			{
				if (int.Parse(Content.GetChild(k).name) < _count)
				{
					Content.GetChild(k).gameObject.SetActive(value: true);
				}
				else
				{
					Content.GetChild(k).gameObject.SetActive(value: false);
				}
			}
			for (int l = 0; l < goList.Count; l++)
			{
				GameObject gameObject2 = goList[l];
				int arg2 = goIndexDic[gameObject2];
				updateCellCB(gameObject2, arg2);
			}
		}
		else if (_count < oldCount)
		{
			ResetSize();
		}
		else
		{
			for (int m = 0; m < Content.childCount; m++)
			{
				if (int.Parse(Content.GetChild(m).name) < _count)
				{
					Content.GetChild(m).gameObject.SetActive(value: true);
				}
				else
				{
					Content.GetChild(m).gameObject.SetActive(value: false);
				}
			}
			for (int n = 0; n < goList.Count; n++)
			{
				GameObject gameObject3 = goList[n];
				int arg3 = goIndexDic[gameObject3];
				updateCellCB(gameObject3, arg3);
			}
		}
		oldCount = _count;
	}

	private int GetMaxCount()
	{
		if (scrollrect.horizontal)
		{
			return Mathf.CeilToInt(scrollRectSize.x / (cellSize.x + cellPadding)) + cacheCount;
		}
		return Mathf.CeilToInt(scrollRectSize.y / (cellSize.y + cellPadding)) + cacheCount;
	}

	private Vector2 GetContentSize()
	{
		if (scrollrect.horizontal)
		{
			float x = cellSize.x * (float)dataCount + cellPadding * (float)(dataCount - 1);
			Vector2 sizeDelta = Content.sizeDelta;
			return new Vector2(x, sizeDelta.y);
		}
		Vector2 sizeDelta2 = Content.sizeDelta;
		return new Vector2(sizeDelta2.x, cellSize.y * (float)dataCount + cellPadding * (float)(dataCount - 1));
	}

	private void OnValueChanged(Vector2 vec)
	{
		int num = GetStartIndex();
		if (num <= -1)
		{
			num = 0;
		}
		if (startIndex == num || num <= -1)
		{
			return;
		}
		startIndex = num;
		for (int num2 = goList.Count - 1; num2 >= 0; num2--)
		{
			GameObject gameObject = goList[num2];
			int num3 = goIndexDic[gameObject];
			if (num3 < startIndex || num3 > startIndex + createCount - 1)
			{
				RecycleItem(gameObject);
			}
		}
		for (int i = startIndex; i < startIndex + createCount && i < dataCount; i++)
		{
			bool flag = false;
			for (int j = 0; j < goList.Count; j++)
			{
				GameObject key = goList[j];
				int num4 = goIndexDic[key];
				if (num4 == i)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				UseItem(i);
			}
		}
	}

	private int GetStartIndex()
	{
		if (scrollrect.horizontal)
		{
			Vector2 anchoredPosition = Content.anchoredPosition;
			return Mathf.FloorToInt((0f - anchoredPosition.x) / (cellSize.x + cellPadding));
		}
		Vector2 anchoredPosition2 = Content.anchoredPosition;
		return Mathf.FloorToInt(anchoredPosition2.y / (cellSize.y + cellPadding));
	}

	private Vector3 GetPosition(int index)
	{
		if (scrollrect.horizontal)
		{
			return new Vector3((float)index * (cellSize.x + cellPadding), 0f, 0f);
		}
		return new Vector3(0f, (float)index * (0f - (cellSize.y + cellPadding)), 0f);
	}

	private void UseItem(int index)
	{
		GameObject gameObject;
		if (freeGoQueue.Count > 0)
		{
			gameObject = freeGoQueue.Dequeue();
			goIndexDic[gameObject] = index;
		}
		else
		{
			gameObject = CreateItem();
			goIndexDic.Add(gameObject, index);
		}
		if (index < dataCount)
		{
			gameObject.SetActive(value: true);
		}
		else
		{
			gameObject.SetActive(value: false);
		}
		goList.Add(gameObject);
		gameObject.transform.localPosition = GetPosition(index);
		gameObject.transform.localScale = Vector3.one;
		updateCellCB(gameObject, index);
	}

	private GameObject CreateItem()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefabGo);
		gameObject.transform.SetParent(Content.transform);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.pivot = new Vector2(0f, 1f);
		component.anchorMin = new Vector2(0f, 1f);
		component.anchorMax = new Vector2(0f, 1f);
		return gameObject;
	}

	private void RecycleItem(GameObject go)
	{
		go.SetActive(value: false);
		goList.Remove(go);
		freeGoQueue.Enqueue(go);
		goIndexDic[go] = -1;
	}
}
