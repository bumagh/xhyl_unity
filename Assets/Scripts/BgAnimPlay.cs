using System.Collections;
using UnityEngine;

public class BgAnimPlay : MonoBehaviour
{
	private FrameAnimator frameAnimator;

	private void Start()
	{
		frameAnimator = GetComponent<FrameAnimator>();
		if (!(frameAnimator == null))
		{
			frameAnimator.enabled = true;
			StartCoroutine(WaitPlayAnim());
		}
	}

	private IEnumerator WaitPlayAnim()
	{
		yield return new WaitForSeconds(0.2f);
		if (frameAnimator != null)
		{
			frameAnimator.Loop = false;
		}
		while (true)
		{
			yield return new WaitForSeconds(5f);
			if (frameAnimator != null)
			{
				frameAnimator.enabled = true;
			}
			if (frameAnimator != null)
			{
				frameAnimator.Loop = true;
			}
			yield return new WaitForSeconds(0.2f);
			if (frameAnimator != null)
			{
				frameAnimator.Loop = false;
			}
		}
	}
}
