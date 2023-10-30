using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareCashOut : MonoBehaviour
{
	private Button btnCash;

	private Button btnPrev;

	private Button btnNext;

	private Text txtPage;

	private Text[] txtTotal = new Text[4];

	[SerializeField]
	private CashoutPanel cashoutPanel;

	[SerializeField]
	private CashoutRecordTable cashoutRecordTable;

	private List<CashoutRecordTable> listTable = new List<CashoutRecordTable>();

	private int curPage;

	private int numPage;

	private void Awake()
	{
		btnCash = base.transform.Find("BtnCashout").GetComponent<Button>();
		btnPrev = base.transform.Find("Page/BtnPrevPage").GetComponent<Button>();
		btnNext = base.transform.Find("Page/BtnNextPage").GetComponent<Button>();
		txtPage = base.transform.Find("Page/TxtPage").GetComponent<Text>();
		for (int i = 0; i < 4; i++)
		{
			txtTotal[i] = base.transform.Find("ImgCashout/Table0").GetChild(i).GetComponent<Text>();
		}
		btnCash.onClick.AddListener(ClickBtnCash);
		btnNext.onClick.AddListener(delegate
		{
			ClickBtnPage(1);
		});
		btnPrev.onClick.AddListener(delegate
		{
			ClickBtnPage(-1);
		});
	}

	public void InitTotal()
	{
		for (int i = 0; i < 4; i++)
		{
			txtTotal[i].text = string.Empty;
		}
	}

	public void InitRecord(int count)
	{
		curPage = 1;
		int num = count / 4;
		int num2 = count - num * 4;
		numPage = ((num <= 1) ? 1 : ((num2 != 0) ? (num + 1) : num));
		Transform parent = base.transform.Find("ImgCashRecord");
		listTable.Clear();
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = cashoutRecordTable.gameObject;
			CashoutRecordTable component = gameObject.GetComponent<CashoutRecordTable>();
			listTable.Add(component);
			if (i > 0)
			{
				Object.Instantiate(gameObject);
			}
			gameObject.transform.SetParent(parent);
			gameObject.transform.localPosition = Vector3.up * (18 - i % 4 * 47);
		}
		ShowTable();
	}

	private void ClickBtnCash()
	{
		cashoutPanel.gameObject.SetActive(value: true);
		cashoutPanel.Init();
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
