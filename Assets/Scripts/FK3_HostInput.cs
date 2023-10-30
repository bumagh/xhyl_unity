using UnityEngine.EventSystems;

public class FK3_HostInput : EventTrigger
{
	private static FK3_HostInput _instance;

	public bool debugAllowInput = true;

	private bool m_touching;

	public static FK3_HostInput Get()
	{
		return _instance;
	}

	private void Awake()
	{
		_instance = this;
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		m_touching = true;
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		m_touching = false;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	public bool IsTouching()
	{
		return debugAllowInput && m_touching;
	}
}
