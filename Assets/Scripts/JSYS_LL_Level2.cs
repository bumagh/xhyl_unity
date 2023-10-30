using UnityEngine;

public class JSYS_LL_Level2 : MonoBehaviour
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
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
		if (GUI.Button(new Rect(10f, 100f, 50f, 50f), "Quit"))
		{
			Application.Quit();
		}
	}
}
