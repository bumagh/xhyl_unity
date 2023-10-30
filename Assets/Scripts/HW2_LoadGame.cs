using UnityEngine;
using UnityEngine.SceneManagement;

public class HW2_LoadGame : MonoBehaviour
{
	private void Awake()
	{
		SceneManager.LoadSceneAsync("FK3_Load");
	}
}
