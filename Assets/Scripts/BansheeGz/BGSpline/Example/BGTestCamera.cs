using UnityEngine;
using UnityEngine.SceneManagement;

namespace BansheeGz.BGSpline.Example
{
	public class BGTestCamera : MonoBehaviour
	{
		private const int Speed = 100;

		private GUIStyle style;

		private void Update()
		{
			if (UnityEngine.Input.GetKey(KeyCode.A))
			{
				base.transform.RotateAround(Vector3.zero, Vector3.up, 100f * Time.deltaTime);
			}
			else if (UnityEngine.Input.GetKey(KeyCode.D))
			{
				base.transform.RotateAround(Vector3.zero, Vector3.up, -100f * Time.deltaTime);
			}
		}

		private void OnGUI()
		{
			if (style == null)
			{
				style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 24
				};
			}
			GUILayout.BeginHorizontal();
			GUILayout.Label("Use A and D to rotate camera", style);
			if (BGTestMainMenu.Inited && GUILayout.Button("To Main Menu"))
			{
				SceneManager.LoadScene("BGCurveMainMenu");
			}
			GUILayout.EndHorizontal();
		}
	}
}
