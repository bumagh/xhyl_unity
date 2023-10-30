using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class XLDT_ButtonScaAni : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public float value = 1.1f;

	private Vector3 initScale;

	private float pressDuration = 0.1f;

	private float releaseDuration = 0.1f;

	public Transform target;

	private Button btn;

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
			target.DOScale(initScale * value, pressDuration);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!(btn != null) || btn.interactable)
		{
			target.DOScale(initScale, releaseDuration);
		}
	}
}
