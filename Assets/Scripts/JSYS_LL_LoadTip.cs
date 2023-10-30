using JSYS_LL_UICommon;
using UnityEngine;
using UnityEngine.UI;

public class JSYS_LL_LoadTip : MonoBehaviour
{
	public enum tipType
	{
		None = -1,
		NetDown,
		LoginError,
		ServerStop,
		IdIsFrozen,
		OverFlow
	}

	private Button ensureButton;

	private Text mTipContent;

	private int language = -1;

	private static JSYS_LL_LoadTip instance;

	public tipType currentType = tipType.None;

	public static JSYS_LL_LoadTip getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
	}

	private void Start()
	{
		language = JSYS_LL_GameInfo.getInstance().Language;
		instance = this;
		mTipContent = base.transform.Find("Text").GetComponent<Text>();
		ensureButton = base.transform.Find("EnsureBtn").GetComponent<Button>();
		ensureButton.onClick.AddListener(clickOk);
		base.gameObject.SetActive(value: false);
		base.transform.localScale = Vector3.one;
	}

	private void Update()
	{
	}

	public void showTip(tipType type)
	{
		switch (type)
		{
		case tipType.NetDown:
			mTipContent.text = JSYS_LL_TipContent.contents[language][0];
			break;
		case tipType.LoginError:
			mTipContent.text = JSYS_LL_TipContent.contents[language][12];
			break;
		case tipType.ServerStop:
			mTipContent.text = JSYS_LL_TipContent.contents[language][10];
			break;
		case tipType.IdIsFrozen:
			mTipContent.text = JSYS_LL_TipContent.contents[language][11];
			break;
		case tipType.OverFlow:
			mTipContent.text = JSYS_LL_TipContent.contents[language][2];
			break;
		}
		base.gameObject.SetActive(value: true);
		currentType = type;
	}

	public void clickOk()
	{
		base.gameObject.SetActive(value: false);
		JSYS_GVars.isStartedFromGame = false;
		JSYS_LL_GameInfo.ClearGameInfo();
		UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
		AsyncOperation asyncOperation = Application.LoadLevelAsync(0);
		asyncOperation.allowSceneActivation = true;
	}
}
