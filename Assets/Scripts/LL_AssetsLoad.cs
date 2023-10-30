using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LL_AssetsLoad : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return SceneManager.LoadSceneAsync("MyBigLevel");
		UnityEngine.Debug.Log("Loading complete");
	}

	private void Update()
	{
	}
}
