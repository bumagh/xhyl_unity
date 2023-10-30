using UnityEngine;

public class JSYS_Fish : MonoBehaviour
{
	public float speed;

	private void Start()
	{
		speed = UnityEngine.Random.Range(1f, 2f);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		base.transform.Rotate(new Vector3(0f, 0f, UnityEngine.Random.Range(160f, 225f)));
		speed = UnityEngine.Random.Range(1f, 2f);
	}
}
