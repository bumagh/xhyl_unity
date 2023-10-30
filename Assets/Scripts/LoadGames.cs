using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGames : MonoBehaviour
{
	public string loadName = string.Empty;

	private void Awake()
	{
		SceneManager.LoadSceneAsync(loadName);
	}
}
