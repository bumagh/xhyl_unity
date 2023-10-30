using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hfh_LoadScene : Hfh_Singleton<Hfh_LoadScene>
{
	private float LoadValue;

	private GameObject LoadPanel;

	private void Awake()
	{
		Hfh_Singleton<Hfh_LoadScene>.SetInstance(this);
		LoadPanel = base.transform.Find("LoadPanel").gameObject;
		StartCoroutine(Loading_IE());
		Hfh_GVars.curView = "LoadingView";
	}

	private IEnumerator Loading_IE()
	{
		while (true)
		{
			LoadPanel.transform.Find("Slider/Fill").GetComponent<Image>().fillAmount = LoadValue;
			yield return new WaitForSeconds(0.1f);
			if (LoadValue <= 0.7f)
			{
				LoadValue += 0.01f;
				continue;
			}
			if (LoadValue < 0.8f)
			{
				if (Hfh_Singleton<Hfh_NetManager>.GetInstance().isConnected)
				{
					LoadValue += 0.01f;
				}
				continue;
			}
			if (LoadValue < 0.9f)
			{
				if (Hfh_Singleton<Hfh_NetManager>.GetInstance().isReady)
				{
					LoadValue += 0.01f;
				}
				continue;
			}
			if (!Hfh_Singleton<Hfh_NetManager>.GetInstance().isError)
			{
				LoadValue += 0.01f;
			}
			if (LoadValue >= 1f)
			{
				SceneManager.LoadSceneAsync("HfhHall");
			}
		}
	}
}
