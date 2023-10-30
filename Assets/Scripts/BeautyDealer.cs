using System.Collections;
using UnityEngine;

public class BeautyDealer : MonoBehaviour
{
	private Animator animator;

	private MeshRenderer mRenderer;

	private Coroutine coPlayControl;

	private Coroutine coPlayGreet;

	private bool greeting;

	private void OnEnable()
	{
		if (animator == null)
		{
			animator = GetComponent<Animator>();
		}
		if (mRenderer == null)
		{
			mRenderer = GetComponent<MeshRenderer>();
		}
		greeting = false;
		if (coPlayControl != null)
		{
			StopCoroutine(coPlayControl);
		}
		coPlayControl = StartCoroutine(PlayControl2());
	}

	private IEnumerator PlayControl()
	{
		UnityEngine.Debug.Log("PlayControl");
		while (true)
		{
			int times = UnityEngine.Random.Range(2, 4);
			for (int i = 0; i < times; i++)
			{
				while (greeting)
				{
					yield return null;
				}
				yield return PlayState("Standby_1");
				yield return null;
			}
			while (greeting)
			{
				yield return null;
			}
			yield return PlayState("Standby_2");
		}
	}

	private IEnumerator PlayControl2()
	{
		UnityEngine.Debug.Log("PlayControl2");
		while (true)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 20f));
		}
	}

	private IEnumerator PlayState(string state)
	{
		animator.Play(string.Empty);
		animator.Play(state);
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
	}

	public void Greet()
	{
		if (coPlayGreet != null)
		{
			StopCoroutine(coPlayGreet);
		}
		coPlayGreet = StartCoroutine(PlayGreet());
	}

	private IEnumerator PlayGreet()
	{
		animator.SetTrigger("greet");
		yield return 2;
		animator.ResetTrigger("greet");
	}
}
