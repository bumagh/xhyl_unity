using UnityEngine;
using UnityEngine.UI;

public class DK_SetCanvas : MonoBehaviour
{
	public static float Width;

	public static float Height;

	public static DK_SetCanvas instance;

	private void Awake()
	{
		instance = this;
		CanvasScaler component = base.transform.GetComponent<CanvasScaler>();
		if ((float)Screen.width / (float)Screen.height > 1.77777779f)
		{
			component.matchWidthOrHeight = 1f;
			Width = (float)Screen.width / (float)Screen.height * 720f;
			Height = 720f;
		}
		else
		{
			Width = (float)Screen.width / (float)Screen.height * 720f;
			Height = GetComponent<RectTransform>().rect.height;
			component.matchWidthOrHeight = 0f;
		}
		Application.runInBackground = true;
	}

	public static float GetCanvasWidth()
	{
		return instance.GetComponent<RectTransform>().rect.width;
	}

	public static float GetCanvasHeight()
	{
		return instance.GetComponent<RectTransform>().rect.height;
	}
}
