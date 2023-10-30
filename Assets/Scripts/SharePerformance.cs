using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharePerformance : MonoBehaviour
{
	private Button btnPrev;

	private Button btnNext;

	private Text txtPage;

	[SerializeField]
	private ShareWeekPerformance swp;

	[SerializeField]
	private ShareDayPerformance shareDayPerformance;

	private List<ShareDayPerformance> listTable = new List<ShareDayPerformance>();

	private int curPage;

	private int numPage;

	private void Awake()
	{
	}

	public void InitWeek(string[] strs)
	{
		swp.Init(strs);
	}

	public void InitDay(int count)
	{
		curPage = 1;
		int num = count / 7;
		int num2 = count - num * 7;
		numPage = ((num <= 1) ? 1 : ((num2 != 0) ? (num + 1) : num));
		Transform parent = base.transform.Find("ImgDayDetail");
		listTable.Clear();
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = shareDayPerformance.gameObject;
			ShareDayPerformance component = gameObject.GetComponent<ShareDayPerformance>();
			listTable.Add(component);
			if (i > 0)
			{
				Object.Instantiate(gameObject);
			}
			gameObject.transform.SetParent(parent);
			gameObject.transform.localPosition = Vector3.up * (80f - (float)(i % 7) * 45f);
		}
		ShowTable();
	}

	private void ClickBtnPage(int i)
	{
		if ((i != -1 || curPage != 1) && (i != 1 || curPage != numPage))
		{
			ShowTable();
		}
	}

	private void ShowTable()
	{
		txtPage.text = $"第{curPage}/{numPage}页";
		int num = curPage * 7;
		int num2 = (curPage != numPage) ? (num + 7) : listTable.Count;
		for (int i = num; i < num2; i++)
		{
			listTable[i].gameObject.SetActive(value: true);
		}
	}
}
