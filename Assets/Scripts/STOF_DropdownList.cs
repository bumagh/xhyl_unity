using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STOF_DropdownList : MonoBehaviour
{
	public Sprite[] spiArrow;

	private Button btnArrow;

	private Button btnParent;

	private RectTransform rtContent;

	[HideInInspector]
	public List<Button> btnChilds = new List<Button>();

	private List<GameObject> objChoose = new List<GameObject>();

	[HideInInspector]
	public List<Text> txtTableName = new List<Text>();

	[HideInInspector]
	public List<Text> txtTableInfo = new List<Text>();

	private float offset = 50f;

	private int activeCount;

	private int chooseIndex = -1;

	private WaitForSeconds wait = new WaitForSeconds(0.02f);

	[HideInInspector]
	public bool isOpening
	{
		get;
		private set;
	}

	[HideInInspector]
	public bool isCanClick
	{
		get;
		set;
	}

	private void Awake()
	{
		isOpening = false;
		isCanClick = true;
		btnArrow = base.transform.Find("BtnArrow").GetComponent<Button>();
		rtContent = base.transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();
		for (int i = 0; i < 10; i++)
		{
			btnChilds.Add(rtContent.GetChild(i).GetComponent<Button>());
			objChoose.Add(rtContent.GetChild(i).Find("BgChoose").gameObject);
			txtTableName.Add(rtContent.GetChild(i).Find("TxtTableName").GetComponent<Text>());
			txtTableInfo.Add(rtContent.GetChild(i).Find("TxtTableInfo").GetComponent<Text>());
		}
		btnArrow.onClick.AddListener(ClickBtnArrow);
	}
    private void OnEnable()
    {
        transform.Find("ImgBg/TxtName").GetComponent<Text>().text = ZH2_GVars.ShowTip("桌列表名称", "Table List", "การพกพาขั้นต่ำ", "Danh sách bàn");
    }

    public void ClickBtnArrow()
	{
		if (isCanClick)
		{
			STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.common2);
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
		isCanClick = false;
		btnArrow.image.sprite = spiArrow[1];
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
		btnArrow.image.sprite = spiArrow[0];
		for (int i = activeCount - 1; i >= 0; i--)
		{
			btnChilds[i].transform.localPosition += Vector3.up * ((float)i * offset + offset);
			yield return wait;
		}
		isCanClick = true;
		isOpening = false;
	}

	public void ShowTableList(int tableCount)
	{
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

	public void ResetTableList()
	{
		isCanClick = true;
		isOpening = false;
		btnArrow.image.sprite = spiArrow[0];
		for (int i = 0; i < activeCount; i++)
		{
			btnChilds[i].transform.localPosition = Vector3.up * 25f + Vector3.right * 130f;
		}
	}

	public void ChooseTable(int index)
	{
		if (index != chooseIndex)
		{
			if (chooseIndex >= 0)
			{
				objChoose[chooseIndex].SetActive(value: false);
			}
			objChoose[index].SetActive(value: true);
			chooseIndex = index;
		}
	}
}
