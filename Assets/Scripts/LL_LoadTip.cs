using LL_UICommon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LL_LoadTip : MonoBehaviour
{
	public enum tipType
	{
		None = -1,
		NetDown,
		LoginError,
		ServerStop,
		IdIsFrozen,
		OverFlow,
		UserIdDeleted,
		Custom
	}

	public Button ensureButton;

	public Text mTipContent;

	private int language = -1;

	private static LL_LoadTip instance;

	public tipType currentType = tipType.None;

	public static LL_LoadTip getInstance()
	{
		return instance;
	}

	private void Start()
	{
		language = LL_GameInfo.getInstance().Language;
		instance = this;
		ensureButton.onClick.AddListener(clickOk);
		base.gameObject.SetActive(value: false);
	}

	public void showTip(tipType type, string msg = "")
	{
		UnityEngine.Debug.LogError("进来了");
		if (msg != string.Empty)
		{
			mTipContent.text = msg;
		}
		else
		{
			switch (type)
			{
			case tipType.NetDown:
				mTipContent.text = LL_TipContent.contents[language][0];
				break;
			case tipType.LoginError:
				mTipContent.text = LL_TipContent.contents[language][12];
				break;
			case tipType.ServerStop:
				mTipContent.text = LL_TipContent.contents[language][10];
				break;
			case tipType.IdIsFrozen:
				mTipContent.text = LL_TipContent.contents[language][11];
				break;
			case tipType.OverFlow:
				mTipContent.text = LL_TipContent.contents[language][2];
				break;
			default:
				mTipContent.text = LL_TipContent.contents[language][13];
				break;
			}
		}
		base.gameObject.SetActive(value: true);
		base.transform.localScale = Vector3.one;
		currentType = type;
	}

	public void clickOk()
	{
		base.gameObject.SetActive(value: false);
		if (currentType != tipType.Custom)
		{
			ZH2_GVars.isStartedFromGame = false;
			UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
			AssetBundleManager.GetInstance().UnloadAB("LuckyLion");
			SceneManager.LoadScene("MainScene");
		}
	}
}
