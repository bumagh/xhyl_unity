using com.miracle9.game.bean;
using UnityEngine;
using UnityEngine.UI;

public class DP_RecordCtrl : MonoBehaviour
{
	private Button btnClose;

	private Button btnRank;

	private Text txtRank;

	private Button btnRecord;

	private Text txtRecord;

	private bool bRank = true;

	private Color colTxt = new Color(0.8f, 0.8f, 0.8f, 1f);

	private Color colBtn = new Color(1f, 1f, 1f, 0f);

	private float[] height = new float[2];

	private GameObject objRank;

	private DP_RankItem rankItem;

	private GameObject objRecord;

	private GameObject[] objRecordLines = new GameObject[5];

	private DP_RecordItems[] recordItems = new DP_RecordItems[36];

	private RectTransform tfContent;

	[SerializeField]
	private Sprite[] spiSpecial = new Sprite[2];

	[SerializeField]
	private DP_AnimalColor spiAnimalColor;

	[SerializeField]
	private DP_AnimalType spiAnimalType;

	public void Init()
	{
		btnClose = base.transform.Find("BtnClose").GetComponent<Button>();
		btnClose.onClick.AddListener(ClickBtnClose);
		btnRank = base.transform.Find("BtnRank").GetComponent<Button>();
		txtRank = btnRank.transform.GetChild(0).GetComponent<Text>();
		btnRecord = base.transform.Find("BtnRecord").GetComponent<Button>();
		txtRecord = btnRecord.transform.GetChild(0).GetComponent<Text>();
		tfContent = base.transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();
		objRank = tfContent.Find("ObjRank").gameObject;
		rankItem = objRank.transform.Find("RankItem").GetComponent<DP_RankItem>();
		objRecord = tfContent.Find("ObjRecord").gameObject;
		for (int i = 0; i < 5; i++)
		{
			objRecordLines[i] = objRecord.transform.Find($"Line{i}").gameObject;
		}
		for (int j = 0; j < 36; j++)
		{
			recordItems[j] = objRecord.transform.Find($"RecordItem{j}").GetComponent<DP_RecordItems>();
		}
	}

	private void Reset()
	{
		bRank = false;
		ShowRecord(bRank);
	}

	private void ShowRecord(bool bRank)
	{
		btnRank.image.color = ((!bRank) ? colBtn : Color.white);
		txtRank.color = ((!bRank) ? colTxt : Color.white);
		btnRecord.image.color = (bRank ? colBtn : Color.white);
		txtRecord.color = (bRank ? colTxt : Color.white);
		objRank.SetActive(bRank);
		objRecord.SetActive(!bRank);
		float d = height[(!bRank) ? 1 : 0];
		tfContent.sizeDelta = Vector2.up * d;
	}

	private void ClickBtnClose()
	{
		DP_SoundManager.GetSingleton().playButtonSound();
		Reset();
		base.gameObject.SetActive(value: false);
	}

	private void ClickBtnRank()
	{
		if (!bRank)
		{
			bRank = true;
			ShowRecord(bRank);
		}
	}

	private void ClickBtnRecord()
	{
		if (bRank)
		{
			bRank = false;
			ShowRecord(bRank);
		}
	}

	public void SetRank()
	{
		height[0] = 100f;
	}

	public void SetRecord(DPDeskRecord[] records)
	{
		int num = records.Length;
		int num2 = (num - 1) / 6;
		height[1] = (float)(num2 + 1) * 140f;
		for (int i = 0; i < 5; i++)
		{
			objRecordLines[i].SetActive(i <= num2);
		}
		for (int j = 0; j < 36; j++)
		{
			recordItems[j].gameObject.SetActive(j <= num);
			if (j <= num)
			{
				recordItems[j].imgAnimalColor.sprite = spiAnimalColor.spis[records[j].animalColor];
				recordItems[j].imgAnimalType.sprite = spiAnimalType.spis[records[j].animalType];
				if (records[j].awardType == 0)
				{
					recordItems[j].imgSpecial.gameObject.SetActive(value: false);
					continue;
				}
				recordItems[j].imgSpecial.sprite = spiSpecial[records[j].awardType - 1];
				recordItems[j].imgSpecial.gameObject.SetActive(value: true);
			}
		}
	}
}
