using UnityEngine;

public class ScorePoint : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player")
		{
			Score.AddPoint();
		}
	}
}
