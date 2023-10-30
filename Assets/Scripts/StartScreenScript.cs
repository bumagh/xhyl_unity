using UnityEngine;

public class StartScreenScript : MonoBehaviour
{
	private static bool sawOnce;

	private void Start()
	{
		if (!sawOnce)
		{
			GetComponent<SpriteRenderer>().enabled = true;
			Time.timeScale = 0f;
		}
		sawOnce = true;
	}

	private void Update()
	{
		if (Time.timeScale == 0f && (UnityEngine.Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
		{
			Time.timeScale = 1f;
			GetComponent<SpriteRenderer>().enabled = false;
			sawOnce = false;
		}
	}
}
