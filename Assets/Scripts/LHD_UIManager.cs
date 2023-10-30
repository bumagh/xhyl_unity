using UnityEngine;
using UnityEngine.UI;

public class LHD_UIManager : MonoBehaviour
{
	public LHD_LoadScene loadscene;

	public LHD_HallScene hallscene;

	public LHD_GameScene gamescene;

	public static LHD_UIManager instance;

	private void Awake()
	{
		instance = this;
		loadscene = base.transform.Find("Load").GetComponent<LHD_LoadScene>();
		hallscene = base.transform.Find("Hall").GetComponent<LHD_HallScene>();
		gamescene = base.transform.Find("Game").GetComponent<LHD_GameScene>();
		ShowLoad();
		if (All_TipCanvas.GetInstance() == null)
		{
		}
		CanvasScaler component = base.transform.GetComponent<CanvasScaler>();
		if (component == null)
		{
			UnityEngine.Debug.LogError("====cs为空=====");
			return;
		}
		if ((float)Screen.width / (float)Screen.height > 1.77777779f)
		{
			component.matchWidthOrHeight = 1f;
		}
		else
		{
			component.matchWidthOrHeight = 0f;
		}
		Application.runInBackground = true;
	}

	public void ShowLoad()
	{
		loadscene.gameObject.SetActive(value: true);
		hallscene.gameObject.SetActive(value: false);
		gamescene.gameObject.SetActive(value: false);
	}

	public void ShowHall()
	{
		loadscene.gameObject.SetActive(value: false);
		hallscene.gameObject.SetActive(value: true);
		gamescene.gameObject.SetActive(value: false);
	}

	public void ShowGame()
	{
		loadscene.gameObject.SetActive(value: false);
		hallscene.gameObject.SetActive(value: false);
		gamescene.gameObject.SetActive(value: true);
	}
}
