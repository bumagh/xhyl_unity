using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BCBM_CanVasCor : MonoBehaviour
{
	public BCBM_AppUIMngr uIMngr;

	public Button btnBlack;

	private void Awake()
	{
		CanvasScaler component = base.transform.GetComponent<CanvasScaler>();
		if ((float)Screen.width / (float)Screen.height > 1.77777779f)
		{
			component.matchWidthOrHeight = 1f;
		}
		else
		{
			component.matchWidthOrHeight = 0f;
		}
		Application.runInBackground = true;
		btnBlack = base.transform.Find("Title/BtnBack").GetComponent<Button>();
		btnBlack.onClick.AddListener(dropOutButton);
	}

	private void OnEnable()
	{
		uIMngr.OnEnable1();
		BCBM_NetMngr.isInLoading = false;
		BCBM_Audio.publicAudio.PlayBg(0, 1f);
	}

	public void dropOutButton()
	{
		UnityEngine.Debug.LogError("=======4");
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		ZH2_GVars.isStartedFromGame = true;
		GameObject gameObject = GameObject.Find("netMngr");
		GameObject gameObject2 = GameObject.Find("GameMngr");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		else
		{
			UnityEngine.Debug.LogError("====netMngr===为空");
		}
		if (gameObject2 != null)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		else
		{
			UnityEngine.Debug.LogError("====gameMngr===为空");
		}
		AssetBundleManager.GetInstance().UnloadAB("BCBM");
		SceneManager.LoadScene("MainScene");
	}
}
