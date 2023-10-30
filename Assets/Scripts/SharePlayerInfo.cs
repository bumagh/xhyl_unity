using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharePlayerInfo : MonoBehaviour
{
	private Button btnFind;

	private Button btnPrev;

	private Button btnNext;

	private InputField inputField;

	private Text txtInputField;

	private Text txtPage;

	[SerializeField]
	private SharePlayerTable sharePlayerTable;

	private List<SharePlayerTable> listTable = new List<SharePlayerTable>();

	private List<string> listID = new List<string>();

	private int curPage;

	private int numPage;

	private WaitForSeconds wait = new WaitForSeconds(0.05f);

	private WaitForSeconds intv = new WaitForSeconds(0.1f);

	private void Awake()
	{
		btnFind = base.transform.Find("Find/BtnFind").GetComponent<Button>();
		btnPrev = base.transform.Find("Page/BtnPrevPage").GetComponent<Button>();
		btnNext = base.transform.Find("Page/BtnNextPage").GetComponent<Button>();
		inputField = base.transform.Find("Find/InputField").GetComponent<InputField>();
		txtInputField = inputField.transform.Find("Text").GetComponent<Text>();
		txtPage = base.transform.Find("Page/TxtPage").GetComponent<Text>();
		btnFind.onClick.AddListener(ClickBtnFind);
		btnNext.onClick.AddListener(delegate
		{
			ClickBtnPage(1);
		});
		btnPrev.onClick.AddListener(delegate
		{
			ClickBtnPage(-1);
		});
	}

	public void Init(int count)
	{
		txtInputField.text = string.Empty;
		curPage = 1;
		int num = count / 7;
		int num2 = count - num * 7;
		numPage = ((num <= 1) ? 1 : ((num2 != 0) ? (num + 1) : num));
		Transform parent = base.transform.Find("ImgTable");
		listTable.Clear();
		listID.Clear();
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = sharePlayerTable.gameObject;
			SharePlayerTable component = gameObject.GetComponent<SharePlayerTable>();
			listID.Add(string.Empty);
			listTable.Add(component);
			if (i > 0)
			{
				Object.Instantiate(gameObject);
			}
			gameObject.transform.SetParent(parent);
			gameObject.transform.localPosition = Vector3.down * 65f * (i % 7 + 1);
		}
		ShowTable();
	}

	private void ClickBtnFind()
	{
		string text = txtInputField.text;
		if (listID.Contains(text))
		{
			int num = listID.IndexOf(text);
			curPage = ((num / 7 < 1) ? 1 : (num / 7));
			ShowTable();
			StartCoroutine(TableAnim(num));
		}
		else
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog("请先输入正确的玩家的ID", showOkCancel: true);
		}
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

	private IEnumerator TableAnim(int i)
	{
		yield return wait;
		listTable[i].gameObject.SetActive(value: false);
		yield return intv;
		listTable[i].gameObject.SetActive(value: true);
	}
}
