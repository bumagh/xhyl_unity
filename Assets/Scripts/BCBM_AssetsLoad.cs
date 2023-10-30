using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BCBM_AssetsLoad : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return SceneManager.LoadSceneAsync("MyBigLevel");
	}

	private void Update()
	{
	}
}
