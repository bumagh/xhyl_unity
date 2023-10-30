using UnityEngine;
using UnityEngine.UI;

public class BZJX_SetCanvas : MonoBehaviour
{
	public static float Width;

	public static float Height;

	private void Awake()
	{
		CanvasScaler component = base.transform.GetComponent<CanvasScaler>();
		if ((float)Screen.width / (float)Screen.height > 1.77777779f)
		{
			component.matchWidthOrHeight = 1f;
			Width = (float)Screen.width / (float)Screen.height * 720f;
			Height = 720f;
		}
		else
		{
			component.matchWidthOrHeight = 0f;
			Width = (float)Screen.width / (float)Screen.height * 720f;
			Height = GetComponent<RectTransform>().rect.height;
		}
		Application.runInBackground = false;
	}
}
