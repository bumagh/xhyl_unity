using UnityEngine;

public class BottomMover : MonoBehaviour
{
	private Rigidbody2D player;

	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
		if (gameObject == null)
		{
			UnityEngine.Debug.LogError("Couldn't find an object with tag 'Player'!");
		}
		else
		{
			player = gameObject.GetComponent<Rigidbody2D>();
		}
	}

	private void FixedUpdate()
	{
		Vector2 velocity = player.velocity;
		float d = velocity.x * 0.75f;
		base.transform.position = base.transform.position + Vector3.right * d * Time.deltaTime;
	}
}
