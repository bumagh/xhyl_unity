using UnityEngine;

public class CameraTracksPlayer : MonoBehaviour
{
	private Transform player;

	private float offsetX;

	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
		if (gameObject == null)
		{
			UnityEngine.Debug.LogError("Couldn't find an object with tag 'Player'!");
			return;
		}
		player = gameObject.transform;
		Vector3 position = base.transform.position;
		float x = position.x;
		Vector3 position2 = player.position;
		offsetX = x - position2.x;
	}

	private void Update()
	{
		if (player != null)
		{
			Vector3 position = base.transform.position;
			Vector3 position2 = player.position;
			position.x = position2.x + offsetX;
			base.transform.position = position;
		}
	}
}
