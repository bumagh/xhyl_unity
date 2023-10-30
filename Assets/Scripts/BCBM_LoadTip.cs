using BCBM_UICommon;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_LoadTip : MonoBehaviour
{
	public enum tipType
	{
		None = -1,
		NetDown,
		LoginError,
		ServerStop,
		IdIsFrozen,
		OverFlow,
		ServerUpdate,
		UserIdDeleted,
		UserIdRepeative,
		UserPwdChanged,
		LoseTheServer,
		Custom
	}

	public Button ensureButton;

	public Text mTipContent;

	private int language = -1;

	private static BCBM_LoadTip instance;

	public tipType currentType = tipType.None;

	public static BCBM_LoadTip getInstance()
	{
		return instance;
	}

	private void Start()
	{
		language = BCBM_GameInfo.getInstance().Language;
		instance = this;
		ensureButton.onClick.AddListener(clickOk);
		base.gameObject.SetActive(value: false);
	}

	public void showTip(tipType type, string msg = "")
	{
		if (msg != string.Empty)
		{
			mTipContent.text = msg;
		}
		else
		{
			switch (type)
			{
			case tipType.NetDown:
				mTipContent.text = BCBM_TipContent.contents[language][0];
				break;
			case tipType.LoginError:
				mTipContent.text = BCBM_TipContent.contents[language][12];
				break;
			case tipType.ServerStop:
				mTipContent.text = BCBM_TipContent.contents[language][10];
				break;
			case tipType.IdIsFrozen:
				mTipContent.text = BCBM_TipContent.contents[language][11];
				break;
			case tipType.OverFlow:
				mTipContent.text = BCBM_TipContent.contents[language][2];
				break;
			case tipType.UserIdDeleted:
				mTipContent.text = BCBM_TipContent.contents[language][13];
				break;
			case tipType.UserIdRepeative:
				mTipContent.text = BCBM_TipContent.contents[language][14];
				break;
			case tipType.UserPwdChanged:
				mTipContent.text = BCBM_TipContent.contents[language][15];
				break;
			case tipType.LoseTheServer:
				mTipContent.text = BCBM_TipContent.contents[language][1];
				break;
			}
		}
		base.transform.localScale = Vector3.one;
		base.gameObject.SetActive(value: true);
		currentType = type;
	}

	public void clickOk()
	{
		base.gameObject.SetActive(value: false);
		if (currentType != tipType.Custom)
		{
			BCBM_MySqlConnection.isStartedFromGame = false;
			UnityEngine.Object.Destroy(BCBM_GameInfo.getInstance().LoadScene.gameObject);
			BCBM_GameInfo.ClearGameInfo();
			UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
			AsyncOperation asyncOperation = Application.LoadLevelAsync(0);
			asyncOperation.allowSceneActivation = true;
		}
	}
}
