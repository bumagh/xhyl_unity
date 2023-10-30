using com.miracle9.game.bean;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DP_AnimalRecord : MonoBehaviour
{
	private Image[] imgs = new Image[9];

	private int[] animalTypes = new int[9];

	[SerializeField]
	private DP_AnimalSpis animalSpis;

	[HideInInspector]
	public bool bFirst;

	public void Init()
	{
		for (int i = 0; i < 9; i++)
		{
			imgs[i] = base.transform.Find($"Img{i}").GetComponent<Image>();
			imgs[i].gameObject.SetActive(value: false);
		}
		bFirst = true;
	}

	public void AnimalRecordAnim(DPDeskRecord[] records)
	{
		if (bFirst)
		{
			ShowAnimalRecord(records);
		}
		else
		{
			UpdateAnimalRecord(records[0]);
		}
	}

	private void ShowAnimalRecord(DPDeskRecord[] records)
	{
		bFirst = false;
		for (int i = 0; i < 9; i++)
		{
			if (i < records.Length)
			{
				animalTypes[i] = records[i].animalColor + records[i].animalType * 3;
			}
			else
			{
				animalTypes[i] = UnityEngine.Random.Range(0, 12);
			}
			imgs[i].sprite = animalSpis.spis[animalTypes[i]];
			imgs[i].gameObject.SetActive(value: true);
			imgs[i].transform.DOBlendableLocalRotateBy(Vector3.back * 180f, 0.1f).SetLoops(8, LoopType.Incremental);
		}
		base.transform.DOScale(0.7f, 0.8f).OnComplete(AfterRotate);
		base.transform.DOScale(0.7f, 1f).OnComplete(Reset);
	}

	private void UpdateAnimalRecord(DPDeskRecord record)
	{
		int num = record.animalColor + record.animalType * 3;
		for (int num2 = 8; num2 >= 1; num2--)
		{
			animalTypes[num2] = animalTypes[num2 - 1];
		}
		animalTypes[0] = num;
		imgs[0].sprite = animalSpis.spis[num];
		imgs[0].gameObject.SetActive(value: true);
		imgs[0].transform.DOBlendableLocalRotateBy(Vector3.back * 180f, 0.1f).SetLoops(8, LoopType.Incremental);
		base.transform.DOScale(0.7f, 0.8f).OnComplete(AfterRotate);
		base.transform.DOScale(0.7f, 1f).OnComplete(Reset);
	}

	private void AfterRotate()
	{
		for (int i = 0; i < 9; i++)
		{
			imgs[i].transform.DOBlendableLocalMoveBy(Vector3.right * 80f, 0.2f);
		}
	}

	private void Reset()
	{
		float num = -300f;
		imgs[0].gameObject.SetActive(value: false);
		imgs[0].transform.localPosition = Vector3.right * num + Vector3.up * -0.5f;
		for (int i = 1; i < 9; i++)
		{
			imgs[i].sprite = animalSpis.spis[animalTypes[i - 1]];
			num += 80f;
			imgs[i].transform.localPosition = Vector3.right * num + Vector3.up * -0.5f;
		}
	}
}
