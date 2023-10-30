using UnityEngine;

public class Fish : MonoBehaviour
{
	private Vector3 velocity = Vector3.zero;

	public float flapSpeed = 100f;

	public float forwardSpeed = 1f;

	private bool didFlap;

	private Animator animator;

	public bool dead;

	private float deathCooldown;

	public bool godMode;

	private void Start()
	{
		animator = base.transform.GetComponentInChildren<Animator>();
		if (animator == null)
		{
			UnityEngine.Debug.LogError("Didn't find animator!");
		}
	}

	private void Update()
	{
		if (dead)
		{
			deathCooldown -= Time.deltaTime;
			if (deathCooldown <= 0f && (UnityEngine.Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
			}
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
		{
			didFlap = true;
		}
	}

	private void FixedUpdate()
	{
		if (!dead)
		{
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * forwardSpeed);
			if (didFlap)
			{
				GetComponent<Rigidbody2D>().AddForce(Vector2.up * flapSpeed);
				animator.SetTrigger("DoFlap");
				didFlap = false;
			}
			Vector2 vector = GetComponent<Rigidbody2D>().velocity;
			if (vector.y > 0f)
			{
				base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				return;
			}
			Vector2 vector2 = GetComponent<Rigidbody2D>().velocity;
			float z = Mathf.Lerp(0f, -90f, (0f - vector2.y) / 3f);
			base.transform.rotation = Quaternion.Euler(0f, 0f, z);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!godMode)
		{
			animator.SetTrigger("Death");
			dead = true;
			deathCooldown = 0.5f;
		}
	}
}
