using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GirlPanelManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
{
	private float[] targetFloat = new float[2]
	{
		0f,
		1f
	};

	[SerializeField]
	private ScrollRect scrollRect;

	private float targetHoritalPosition;

	private bool isDrag;

	private int index;

	private float oldPosition;

	private float speed = 20f;

	private float timer;

	private void Update()
	{
		if (!isDrag)
		{
			timer += Time.deltaTime;
			scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetHoritalPosition, Time.deltaTime * speed);
			if (timer >= 5f)
			{
				timer = 0f;
				index = 1 - index;
				targetHoritalPosition = targetFloat[index];
			}
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDrag = true;
		timer = 0f;
		oldPosition = scrollRect.horizontalNormalizedPosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDrag = false;
		float horizontalNormalizedPosition = scrollRect.horizontalNormalizedPosition;
		if (oldPosition > horizontalNormalizedPosition)
		{
			index--;
			if (index < 0)
			{
				index = 0;
			}
			targetHoritalPosition = targetFloat[index];
		}
		else if (oldPosition < horizontalNormalizedPosition)
		{
			index++;
			if (index > 1)
			{
				index = 1;
			}
			targetHoritalPosition = targetFloat[index];
		}
	}
}
