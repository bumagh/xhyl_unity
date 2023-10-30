using UnityEngine;

public class BgLooper : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		UnityEngine.Debug.Log("Triggered: " + collider.name);
		Vector2 size = ((BoxCollider2D)collider).size;
		float x = size.x;
		Vector3 position = collider.transform.position;
		if (collider.tag == "Pipe")
		{
			position.x += 23.38f;
		}
		else
		{
			position.x += 8.1642f;
		}
		collider.transform.position = position;
	}
}
