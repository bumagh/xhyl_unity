using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SPA_BtnLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	private float Ping = 0.1f;

	private bool bPress;

	private float LastTime;

	public Action action;

	private void Update()
	{
		if (bPress && Time.time - LastTime > Ping)
		{
			LastTime = Time.time;
			if (action != null)
			{
				action();
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!bPress)
		{
			bPress = true;
			LastTime = Time.time;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (bPress)
		{
			bPress = false;
			LastTime = Time.time;
		}
	}
}
