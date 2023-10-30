using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButton : Button
{
	public ButtonClickedEvent my_onLongPress;

	public ButtonClickedEvent my_onDoubleClick;

	private bool my_isStartPress;

	private float my_curPointDownTime;

	private float my_longPressTime = 2.5f;

	private bool my_longPressTrigger;

	public ButtonClickedEvent OnLongPress
	{
		get
		{
			return my_onLongPress;
		}
		set
		{
			my_onLongPress = value;
		}
	}

	public ButtonClickedEvent OnDoubleClick
	{
		get
		{
			return my_onDoubleClick;
		}
		set
		{
			my_onDoubleClick = value;
		}
	}

	protected MyButton()
	{
		my_onDoubleClick = new ButtonClickedEvent();
		my_onLongPress = new ButtonClickedEvent();
	}

	private void Update()
	{
		CheckIsLongPress();
	}

	private void CheckIsLongPress()
	{
		if (my_isStartPress && !my_longPressTrigger && Time.time > my_curPointDownTime + my_longPressTime)
		{
			my_longPressTrigger = true;
			my_isStartPress = false;
			if (my_onLongPress != null)
			{
				my_onLongPress.Invoke();
			}
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		my_curPointDownTime = Time.time;
		my_isStartPress = true;
		my_longPressTrigger = false;
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		my_isStartPress = false;
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		my_isStartPress = false;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (my_longPressTrigger)
		{
			return;
		}
		if (eventData.clickCount == 2)
		{
			if (my_onDoubleClick != null)
			{
				my_onDoubleClick.Invoke();
			}
		}
		else if (eventData.clickCount == 1)
		{
			base.onClick.Invoke();
		}
	}
}
