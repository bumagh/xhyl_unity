using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JSYS_LL_AssetsLoad : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return SceneManager.LoadSceneAsync("MyBigLevel");
	}

	private void Update()
	{
	}
}
