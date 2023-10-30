using UnityEngine;

public class demoGUI : MonoBehaviour
{
	public GameObject[] explosions;

	private void OnGUI()
	{
		for (int i = 0; i < explosions.Length / 2; i++)
		{
			GUI.color = new Color(1f, 0.75f, 0.5f);
			if (GUI.Button(new Rect(10f, 10 + i * 30, 100f, 20f), explosions[i].name))
			{
				Object.Instantiate(explosions[i], new Vector3(0f, 2f, 0f), Quaternion.identity);
			}
		}
		int num = 0;
		for (int j = explosions.Length / 2; j < explosions.Length; j++)
		{
			GUI.color = new Color(1f, 0.75f, 0.5f);
			if (GUI.Button(new Rect(Screen.width - 120, 10 + num * 30, 100f, 20f), explosions[j].name))
			{
				Object.Instantiate(explosions[j], new Vector3(0f, 2f, 0f), Quaternion.identity);
			}
			num++;
		}
	}
}
