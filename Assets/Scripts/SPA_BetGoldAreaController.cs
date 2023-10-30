using UnityEngine;
using UnityEngine.UI;

public class SPA_BetGoldAreaController : MonoBehaviour
{
	private Button _btnBetSmall;

	private Button _btnBetMiddle;

	private Button _btnBetBig;

	private void Awake()
	{
		_btnBetSmall = base.transform.Find("BtnXiao").GetComponent<Button>();
		_btnBetMiddle = base.transform.Find("BtnHe").GetComponent<Button>();
		_btnBetBig = base.transform.Find("BtnDa").GetComponent<Button>();
	}

	private void Start()
	{
		_btnBetSmall.onClick.AddListener(SPA_MB_Singleton<SPA_DiceGameController2>.GetInstance().OnBtnBetSmall_Click);
		_btnBetMiddle.onClick.AddListener(SPA_MB_Singleton<SPA_DiceGameController2>.GetInstance().OnBtnBetMiddle_Click);
		_btnBetBig.onClick.AddListener(SPA_MB_Singleton<SPA_DiceGameController2>.GetInstance().OnBtnBetBig_Click);
	}

	public void SetBetGoldButtonsEnable(bool isEnable)
	{
		_btnBetBig.enabled = isEnable;
		_btnBetSmall.enabled = isEnable;
		_btnBetMiddle.enabled = isEnable;
	}
}
