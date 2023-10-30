using UnityEngine;
using UnityEngine.UI;

public class DP_ResultCtrl : MonoBehaviour
{
	[SerializeField]
	private DP_AnimalType spiAnimalTypes;

	[SerializeField]
	private DP_AnimalColor spiAnimalColors;

	private Text txtScore;

	private Text txtBlance;

	private Image imgAnimalType;

	private Image imgAnimalColor;

	private Text txtPower;

	private GameObject objBonus;

	private Text txtMacNum;

	public void Init()
	{
		txtScore = base.transform.Find("DefenBg/TxtDefen").GetComponent<Text>();
		txtBlance = base.transform.Find("JiesunBg/TxtJieSuan").GetComponent<Text>();
		txtPower = base.transform.Find("TxtBeilv").GetComponent<Text>();
		imgAnimalType = base.transform.Find("Result/Animal").GetComponent<Image>();
		imgAnimalColor = base.transform.Find("Result").GetComponent<Image>();
		objBonus = base.transform.Find("Bonus").gameObject;
		objBonus.SetActive(value: false);
		txtMacNum = objBonus.transform.Find("TxtMacNum").GetComponent<Text>();
	}

	public void ShowResult()
	{
		base.gameObject.SetActive(value: true);
	}

	public void HideResult()
	{
		objBonus.SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	public void SetScoreAndBlance(int score, int blance)
	{
		txtScore.text = "+" + score.ToString();
		txtBlance.text = ((blance < 0) ? blance.ToString() : ("+" + blance.ToString()));
	}

	public void SetAnimalResult(int animal)
	{
		imgAnimalType.sprite = spiAnimalTypes.spis[animal / 3];
		imgAnimalColor.sprite = spiAnimalColors.spis[animal % 3];
		txtPower.text = "X" + DP_GameData.power[animal];
	}

	public void SetBonus(int luckyMacNum)
	{
		txtMacNum.text = luckyMacNum.ToString();
		objBonus.SetActive(value: true);
	}
}
