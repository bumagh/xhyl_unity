using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_DropdownList : MonoBehaviour
{
	private Button btnArrow;

	private RectTransform rtContent;

	private GameObject ScrollView;

	[HideInInspector]
	public List<Button> btnChilds;

	[HideInInspector]
	public List<GameObject> objChoose;

	[HideInInspector]
	public List<Text> txtTableName;

	[HideInInspector]
	public List<Text> txtTableInfo;

	private float offset = 50f;

	private int activeCount;

	private WaitForSeconds wait = new WaitForSeconds(0.02f);

	private int chooseIndex;

	public bool isOpening
	{
		get;
		private set;
	}

	public bool isCanClick
	{
		get;
		set;
	}

	private void Awake()
	{
	}

	private void Cles()
	{
		btnChilds = new List<Button>();
		objChoose = new List<GameObject>();
		txtTableName = new List<Text>();
		txtTableInfo = new List<Text>();
	}

	public void Init()
	{
		Cles();
		isOpening = false;
		isCanClick = true;
		btnArrow = base.transform.Find("BtnArrow").GetComponent<Button>();
		ScrollView = base.transform.Find("Scroll View").gameObject;
		rtContent = base.transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();
		for (int i = 0; i < 10; i++)
		{
			btnChilds.Add(rtContent.GetChild(i).GetComponent<Button>());
			objChoose.Add(rtContent.GetChild(i).Find("BgChoose").gameObject);
			txtTableName.Add(rtContent.GetChild(i).Find("TxtTableName").GetComponent<Text>());
			txtTableInfo.Add(rtContent.GetChild(i).Find("TxtTableInfo").GetComponent<Text>());
		}
		btnArrow.onClick.AddListener(OnButtonClick);
	}

	private void OnButtonClick()
	{
		if (isCanClick)
		{
			if (!isOpening)
			{
				StartCoroutine(ShowChildMenu());
			}
			else
			{
				StartCoroutine(HideChildMenu());
			}
		}
	}

	private IEnumerator ShowChildMenu()
	{
		ScrollView.SetActive(value: true);
		isCanClick = false;
		for (int i = 0; i < activeCount; i++)
		{
			btnChilds[i].transform.localPosition -= Vector3.up * ((float)i * offset + offset);
			yield return wait;
		}
		isCanClick = true;
		isOpening = true;
	}

	private IEnumerator HideChildMenu()
	{
		isCanClick = false;
		for (int i = activeCount - 1; i >= 0; i--)
		{
			btnChilds[i].transform.localPosition += Vector3.up * ((float)i * offset + offset);
			yield return wait;
		}
		isCanClick = true;
		isOpening = false;
		ScrollView.SetActive(value: false);
	}

	public void ChooseTable(int index)
	{
		if (index != chooseIndex)
		{
			objChoose[chooseIndex].SetActive(value: false);
			objChoose[index].SetActive(value: true);
			chooseIndex = index;
		}
	}

	public void ResetTableList()
	{
		isCanClick = true;
		isOpening = false;
		for (int i = 0; i < activeCount; i++)
		{
			btnChilds[i].transform.localPosition = Vector3.up * 25f + Vector3.right * 130f;
		}
	}

	public void ShowTableList(int tableCount)
	{
		UnityEngine.Debug.LogError("ShowTableList >> tableCount: " + tableCount);
		Init();
		activeCount = tableCount;
		int count = btnChilds.Count;
		if (count < activeCount)
		{
			for (int i = count; i < activeCount; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(rtContent.GetChild(0).gameObject, rtContent);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = rtContent.GetChild(0).localPosition;
				btnChilds.Add(gameObject.GetComponent<Button>());
				objChoose.Add(gameObject.transform.Find("BgChoose").gameObject);
				txtTableName.Add(gameObject.transform.Find("TxtTableName").GetComponent<Text>());
				txtTableInfo.Add(gameObject.transform.Find("TxtTableInfo").GetComponent<Text>());
			}
		}
		for (int j = 0; j < btnChilds.Count; j++)
		{
			btnChilds[j].gameObject.SetActive(value: false);
		}
		for (int k = 0; k < activeCount; k++)
		{
			btnChilds[k].gameObject.SetActive(value: true);
		}
		rtContent.sizeDelta = Vector3.up * offset * activeCount;
	}
}
