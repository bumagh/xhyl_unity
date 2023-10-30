using UnityEngine;
using UnityEngine.UI;

public class UIDepth : MonoBehaviour
{
	public int order;

	[Header("该组件是否需要包含点击事件")]
	public bool isHaveButton = true;

	private void OnEnable()
	{
		if (isHaveButton)
		{
			GraphicRaycaster component = GetComponent<GraphicRaycaster>();
			if (component == null)
			{
				component = base.gameObject.AddComponent<GraphicRaycaster>();
			}
		}
		Canvas canvas = GetComponent<Canvas>();
		if (canvas == null)
		{
			canvas = base.gameObject.AddComponent<Canvas>();
		}
		canvas.overrideSorting = true;
		canvas.sortingOrder = order;
	}

	private void Start()
	{
		if (isHaveButton)
		{
			GraphicRaycaster component = GetComponent<GraphicRaycaster>();
			if (component == null)
			{
				component = base.gameObject.AddComponent<GraphicRaycaster>();
			}
		}
		Canvas canvas = GetComponent<Canvas>();
		if (canvas == null)
		{
			canvas = base.gameObject.AddComponent<Canvas>();
		}
		canvas.overrideSorting = true;
		canvas.sortingOrder = order;
	}
}
