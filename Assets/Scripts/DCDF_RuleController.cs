using UnityEngine;
using UnityEngine.UI;

public class DCDF_RuleController : MonoBehaviour
{
	private Button btnIllustration;

	private Button btnExchange;

	private GameObject objButtons0;

	private GameObject objButtons1;

	private GameObject objIllustration;

	private GameObject objExchange;

	private RectTransform rtfContent;

	private int curIndex;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		btnIllustration = base.transform.Find("BtnIllustration").GetComponent<Button>();
		btnExchange = base.transform.Find("BtnExchange").GetComponent<Button>();
		objButtons0 = base.transform.Find("ObjButtons0").gameObject;
		objButtons1 = base.transform.Find("ObjButtons1").gameObject;
		rtfContent = base.transform.Find("ScrollView/Viewport/Content").GetComponent<RectTransform>();
		objIllustration = rtfContent.transform.Find("Image0").gameObject;
		objExchange = rtfContent.transform.Find("Image1").gameObject;
		btnIllustration.onClick.AddListener(ClickBtnIllustration);
		btnExchange.onClick.AddListener(ClickBtnExchange);
	}

	private void ClickBtnIllustration()
	{
		if (curIndex != 0)
		{
			curIndex = 0;
			DCDF_SoundManager.Instance.PlayClickAudio();
			objButtons0.SetActive(value: true);
			objButtons1.SetActive(value: false);
			objIllustration.SetActive(value: true);
			objExchange.SetActive(value: false);
			rtfContent.sizeDelta = Vector2.up * 1706f;
		}
	}

	private void ClickBtnExchange()
	{
		if (curIndex != 1)
		{
			curIndex = 1;
			DCDF_SoundManager.Instance.PlayClickAudio();
			objButtons0.SetActive(value: false);
			objButtons1.SetActive(value: true);
			objIllustration.SetActive(value: false);
			objExchange.SetActive(value: true);
			rtfContent.sizeDelta = Vector2.up * 551f;
		}
	}

	public void Show()
	{
		curIndex = 0;
		objButtons0.SetActive(value: true);
		objButtons1.SetActive(value: false);
		objIllustration.SetActive(value: true);
		objExchange.SetActive(value: false);
		rtfContent.sizeDelta = Vector2.up * 1706f;
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
