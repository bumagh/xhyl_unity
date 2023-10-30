using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dzb_LoadScene : Dzb_Singleton<Dzb_LoadScene>
{
	private float LoadValue;

	private GameObject LoadPanel;

	private void Awake()
	{
		Dzb_Singleton<Dzb_LoadScene>.SetInstance(this);
		LoadPanel = base.transform.Find("LoadPanel").gameObject;
		StartCoroutine(Loading_IE());
		Dzb_MySqlConnection.curView = "LoadingView";
	}

	private IEnumerator Loading_IE()
	{
		while (true)
		{
			LoadPanel.transform.Find("Slider/Fill").GetComponent<Image>().fillAmount = LoadValue;
			yield return new WaitForSeconds(0.1f);
			if (!Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsTest)
			{
				if (LoadValue <= 0.7f)
				{
					LoadValue += 0.01f;
					continue;
				}
				if (LoadValue < 0.8f)
				{
					if (Dzb_Singleton<Dzb_NetManager>.GetInstance().isConnected)
					{
						LoadValue += 0.01f;
					}
					continue;
				}
				if (LoadValue < 0.9f)
				{
					if (Dzb_Singleton<Dzb_NetManager>.GetInstance().isReady)
					{
						LoadValue += 0.01f;
					}
					continue;
				}
				if (!Dzb_Singleton<Dzb_NetManager>.GetInstance().isError)
				{
					LoadValue += 0.01f;
				}
				if (LoadValue >= 1f)
				{
					SceneManager.LoadSceneAsync("DzbHall");
				}
			}
			else
			{
				LoadValue += 0.01f;
				if (LoadValue >= 1f)
				{
					SceneManager.LoadSceneAsync("DzbHall");
				}
			}
		}
	}
}
