using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DP_LongPressOrClickEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler, IEventSystemHandler
{
	[Tooltip("How long must pointer be down on this object to trigger a long press")]
	public float durationThreshold = 1f;

	public UnityEvent onLongPress = new UnityEvent();

	public UnityEvent onClick = new UnityEvent();

	private bool isPointerDown;

	private float timePressStarted;

	private void Update()
	{
		if (isPointerDown)
		{
			timePressStarted += Time.deltaTime;
			if (timePressStarted >= durationThreshold)
			{
				timePressStarted = 0f;
				onLongPress.Invoke();
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		timePressStarted = 0f;
		isPointerDown = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isPointerDown = false;
		timePressStarted = 0f;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isPointerDown = false;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		onClick.Invoke();
	}
}
