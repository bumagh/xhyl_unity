using UnityEngine;
using UnityEngine.SceneManagement;

public class BCBM_Level2 : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 50f, 50f), "Back"))
		{
			ZH2_GVars.isStartedFromGame = true;
			SceneManager.LoadScene("MainScene");
		}
		if (GUI.Button(new Rect(10f, 100f, 50f, 50f), "Quit"))
		{
			Application.Quit();
		}
	}
}
