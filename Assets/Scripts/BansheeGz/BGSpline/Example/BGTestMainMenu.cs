using UnityEngine;
using UnityEngine.SceneManagement;

namespace BansheeGz.BGSpline.Example
{
	public class BGTestMainMenu : MonoBehaviour
	{
		public static bool Inited;

		private void Start()
		{
			Inited = true;
		}

		public void LoadScene(string scene)
		{
			SceneManager.LoadScene(scene);
		}
	}
}
