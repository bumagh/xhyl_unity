using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LKB_BetGoldAreaController : MonoBehaviour
{
	private Image _imgBetSmall;

	private Image _imgBetBig;

	private Image _imgBetMiddle;

	private Text _textBetNum;

	private Image _imgBetGold;

	private GameObject _doubleGold;

	private GameObject _quadrupleGold;

	private GameObject _manyGold;

	private Transform _goldLayer;

	private Vector3 vecSmall = new Vector3(-360f, -150f, 0f);

	private Vector3 vecMid = new Vector3(0f, -150f, 0f);

	private Vector3 vecBig = new Vector3(360f, -150f, 0f);

	private Button _btnBetSmall;

	private Button _btnBetMiddle;

	private Button _btnBetBig;

	private void Awake()
	{
		_imgBetSmall = base.transform.Find("ImgXiao").GetComponent<Image>();
		_imgBetBig = base.transform.Find("ImgDa").GetComponent<Image>();
		_imgBetMiddle = base.transform.Find("ImgHe").GetComponent<Image>();
		_textBetNum = base.transform.Find("ImgHe").GetComponent<Text>();
		_goldLayer = base.transform.Find("Bet");
		_textBetNum = _goldLayer.Find("TxtScore").GetComponent<Text>();
		_imgBetGold = _goldLayer.Find("ImgGold").GetComponent<Image>();
		_doubleGold = _goldLayer.Find("ImgTwoGold").gameObject;
		_quadrupleGold = _goldLayer.Find("ImgFourGold").gameObject;
		_manyGold = _goldLayer.Find("ImgManyGold").gameObject;
		_btnBetSmall = base.transform.Find("BtnXiao").GetComponent<Button>();
		_btnBetMiddle = base.transform.Find("BtnHe").GetComponent<Button>();
		_btnBetBig = base.transform.Find("BtnDa").GetComponent<Button>();
	}

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		SetBetNumEnable(isEnable: false);
		SetGoldLayerEnable(isEnable: true);
		SetAllBetImagesEnable(isEnable: false);
		SetBetGoldEnable(isEnable: false);
		SetBetWinGoldEnable(2, isEnable: false);
		SetBetWinGoldEnable(4, isEnable: false);
		SetBetWinGoldEnable(6, isEnable: false);
	}

	public IEnumerator AllBetImagesBlinkAni()
	{
		UnityEngine.Debug.Log("AllBetImagesBlinkAni");
		SetAllBetImagesEnable(isEnable: true);
		yield return new WaitForSeconds(0.5f);
		SetAllBetImagesEnable(isEnable: false);
		yield return new WaitForSeconds(0.5f);
		SetAllBetImagesEnable(isEnable: true);
		yield return new WaitForSeconds(0.5f);
		SetAllBetImagesEnable(isEnable: false);
	}

	public IEnumerator SingleBetImageBlinkAni(LKB_BetDiceType type)
	{
		Image target = _imgBetSmall;
		switch (type)
		{
		case LKB_BetDiceType.Big:
			target = _imgBetBig;
			break;
		case LKB_BetDiceType.Middle:
			target = _imgBetMiddle;
			break;
		}
		yield return new WaitForSeconds(0.5f);
		target.enabled = true;
		yield return new WaitForSeconds(0.5f);
		target.enabled = false;
		yield return new WaitForSeconds(0.5f);
		target.enabled = true;
	}

	public IEnumerator GoldLayerBlinkAni()
	{
		SetGoldLayerEnable(isEnable: true);
		yield return new WaitForSeconds(0.5f);
		SetGoldLayerEnable(isEnable: false);
		yield return new WaitForSeconds(0.5f);
		SetGoldLayerEnable(isEnable: true);
		yield return new WaitForSeconds(0.5f);
		SetGoldLayerEnable(isEnable: false);
	}

	public void SetAllBetImagesEnable(bool isEnable)
	{
		_imgBetSmall.enabled = isEnable;
		_imgBetBig.enabled = isEnable;
		_imgBetMiddle.enabled = isEnable;
	}

	public void SetSingleBetImage(LKB_BetDiceType type, bool isEnable = true)
	{
		Image image = _imgBetSmall;
		switch (type)
		{
		case LKB_BetDiceType.Big:
			image = _imgBetBig;
			break;
		case LKB_BetDiceType.Middle:
			image = _imgBetMiddle;
			break;
		}
		image.enabled = isEnable;
	}

	public void ShowBet(LKB_BetDiceType type, int betNum)
	{
		SetGoldLayerEnable(isEnable: true);
		_imgBetGold.gameObject.SetActive(value: true);
		_textBetNum.gameObject.SetActive(value: true);
		_textBetNum.text = betNum.ToString();
		_adjustGoldLayerPosition(type);
	}

	public void SetBetNum(int newNum)
	{
		_textBetNum.text = newNum.ToString();
	}

	public void SetBetNumEnable(bool isEnable)
	{
		_textBetNum.gameObject.SetActive(isEnable);
	}

	public void SetBetGoldEnable(bool isEnable)
	{
		_imgBetGold.gameObject.SetActive(isEnable);
	}

	public void SetBetWinGoldEnable(int winRate, bool isEnable)
	{
		switch (winRate)
		{
		case 2:
			_doubleGold.SetActive(isEnable);
			break;
		case 4:
			_quadrupleGold.SetActive(isEnable);
			break;
		case 6:
			_manyGold.SetActive(isEnable);
			break;
		}
	}

	public void SetGoldLayerEnable(bool isEnable)
	{
		_goldLayer.gameObject.SetActive(isEnable);
	}

	public void SetBetGoldButtonsEnable(bool isEnable)
	{
		_btnBetBig.enabled = isEnable;
		_btnBetSmall.enabled = isEnable;
		_btnBetMiddle.enabled = isEnable;
	}

	private void _adjustGoldLayerPosition(LKB_BetDiceType type)
	{
		switch (type)
		{
		case LKB_BetDiceType.Small:
			_goldLayer.localPosition = vecSmall;
			break;
		case LKB_BetDiceType.Middle:
			_goldLayer.localPosition = vecMid;
			break;
		case LKB_BetDiceType.Big:
			_goldLayer.localPosition = vecBig;
			break;
		}
	}
}
