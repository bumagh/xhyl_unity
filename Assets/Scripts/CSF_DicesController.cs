using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CSF_DicesController : MonoBehaviour
{
	private Image imgPlate;

	private Image imgSmallDice1;

	private Image imgSmallDice2;

	private Image imgBigDice1;

	private Image imgBigDice2;

	private Transform tfBigDice;

	[SerializeField]
	private Sprite[] spiBigDices = new Sprite[6];

	[SerializeField]
	private Sprite[] spiSmallDices = new Sprite[6];

	private void Awake()
	{
		imgPlate = base.transform.parent.Find("ImgPlate").GetComponent<Image>();
		imgSmallDice1 = base.transform.parent.Find("ImgSmallDice1").GetComponent<Image>();
		imgSmallDice2 = base.transform.parent.Find("ImgSmallDice2").GetComponent<Image>();
		tfBigDice = base.transform.parent.Find("BigDice");
		imgBigDice1 = tfBigDice.Find("ImgBigDice1").GetComponent<Image>();
		imgBigDice2 = tfBigDice.Find("ImgBigDice2").GetComponent<Image>();
	}

	public void Init()
	{
		HideSmallDices();
		HideBigDices();
	}

	public void SetSmallDices(int dice1, int dice2)
	{
		imgSmallDice1.sprite = spiSmallDices[dice1 - 1];
		imgSmallDice2.sprite = spiSmallDices[dice2 - 1];
		ShowSmallDices();
	}

	public void ShowSmallDices()
	{
		imgPlate.gameObject.SetActive(value: true);
		imgSmallDice1.gameObject.SetActive(value: true);
		imgSmallDice2.gameObject.SetActive(value: true);
	}

	public void HideSmallDices()
	{
		imgPlate.gameObject.SetActive(value: false);
		imgSmallDice1.gameObject.SetActive(value: false);
		imgSmallDice2.gameObject.SetActive(value: false);
	}

	public IEnumerator WaitAndShowSmallDices(float waitTime, int dice1, int dice2)
	{
		yield return new WaitForSeconds(waitTime);
		SetSmallDices(dice1, dice2);
		yield return null;
	}

	public void ShowBigDices(int dice1, int dice2, CSF_BetDiceType type)
	{
		tfBigDice.gameObject.SetActive(value: true);
		imgBigDice1.sprite = spiBigDices[dice1 - 1];
		imgBigDice2.sprite = spiBigDices[dice2 - 1];
		switch (type)
		{
		case CSF_BetDiceType.Small:
			tfBigDice.localPosition = Vector3.left * 330f + Vector3.down * 150f;
			break;
		case CSF_BetDiceType.Middle:
			tfBigDice.localPosition = Vector3.down * 150f;
			break;
		case CSF_BetDiceType.Big:
			tfBigDice.localPosition = Vector3.right * 330f + Vector3.down * 150f;
			break;
		}
	}

	public IEnumerator WaitAndShowBigDices(float waitTime, int dice1, int dice2, CSF_BetDiceType type)
	{
		yield return new WaitForSeconds(waitTime);
		ShowBigDices(dice1, dice2, type);
		yield return null;
	}

	public void HideBigDices()
	{
		tfBigDice.gameObject.SetActive(value: false);
	}
}
