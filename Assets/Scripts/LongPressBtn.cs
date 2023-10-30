using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LongPressBtn : Selectable
{
	[Serializable]
	public class ButtonClickedEvent : UnityEvent
	{
	}

	private bool isExcute;

	[SerializeField]
	[FormerlySerializedAs("onClick")]
	private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

	[SerializeField]
	private ButtonClickedEvent m_OnClickUp = new ButtonClickedEvent();

	[SerializeField]
	private ButtonClickedEvent m_OnClickDown = new ButtonClickedEvent();

	private float startTime;

	public ButtonClickedEvent onClick
	{
		get
		{
			return m_OnClick;
		}
		set
		{
			m_OnClick = value;
		}
	}

	public ButtonClickedEvent onClickUp
	{
		get
		{
			return m_OnClickUp;
		}
		set
		{
			m_OnClickUp = value;
		}
	}

	public ButtonClickedEvent onClickDown
	{
		get
		{
			return m_OnClickDown;
		}
		set
		{
			m_OnClickDown = value;
		}
	}

	private void Update()
	{
		if (!isExcute)
		{
			startTime = 0f;
			return;
		}
		startTime += Time.deltaTime;
		if (!(startTime < 0.5f) && m_OnClick != null)
		{
			m_OnClick.Invoke();
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		isExcute = true;
		if (m_OnClickDown != null)
		{
			m_OnClickDown.Invoke();
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		isExcute = false;
		if (m_OnClickUp != null)
		{
			m_OnClickUp.Invoke();
		}
	}
}
