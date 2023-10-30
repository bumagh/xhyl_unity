using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DNTG_ButtonScaAni : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public float value = 1.1f;

	private Vector3 initScale;

	private float pressDuration = 0.1f;

	private float releaseDuration = 0.1f;

	public Transform target;

	private Button btn;

	private Coroutine c;

	private void Awake()
	{
		if (target == null)
		{
			target = base.transform;
		}
		if (btn == null)
		{
			btn = GetComponent<Button>();
		}
		initScale = base.transform.localScale;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!(btn != null) || btn.interactable)
		{
			if (c != null)
			{
				StopCoroutine(c);
			}
			c = StartCoroutine(DOScale(initScale * value, pressDuration));
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!(btn != null) || btn.interactable)
		{
			if (c != null)
			{
				StopCoroutine(c);
			}
			c = StartCoroutine(DOScale(initScale, releaseDuration));
		}
	}

	private IEnumerator DOScale(Vector3 vecTarget, float duration)
	{
		while (duration >= 0f)
		{
			Vector3 vecStep = (vecTarget - base.transform.localScale) * Time.deltaTime / duration;
			duration -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
			base.transform.localScale += vecStep;
		}
	}
}
