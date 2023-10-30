using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPosAni : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public float pressDuration = 0.07f;

	public float releaseDuration = 0.05f;

	private RectTransform target;

	public Vector3 m_posOffset = new Vector3(0f, -5f, 0f);

	private Vector3 m_posOrigin;

	private Button btn;

	private void Awake()
	{
		if (target == null)
		{
			target = GetComponent<RectTransform>();
		}
		btn = GetComponent<Button>();
		m_posOrigin = base.transform.localPosition;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!(btn != null) || btn.interactable)
		{
			target.DOLocalMove(m_posOrigin + m_posOffset, pressDuration);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!(btn != null) || btn.interactable)
		{
			target.DOLocalMove(m_posOrigin, releaseDuration);
		}
	}
}
