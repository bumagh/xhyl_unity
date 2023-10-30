using UnityEngine;
using UnityEngine.UI;

public class XLDT_SetCanvas : MonoBehaviour
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
			Width = GetComponent<RectTransform>().rect.width;
			Height = GetComponent<RectTransform>().rect.height;
		}
		Application.runInBackground = true;
	}

	public void setViewport()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		float num3 = Screen.width;
		float num4 = Screen.height;
		float x = 0f;
		float y = 0f;
		float num5 = 1f;
		float num6 = 1f;
		if (num3 / num4 > num / num2)
		{
			num5 = 1f;
			num6 = num4 * num / num3 / num2;
			y = (1f - num6) / 2f;
		}
		else if (num3 / num4 < num / num2)
		{
			num6 = 1f;
			num5 = num3 * num2 / num4 / num;
			x = (1f - num5) / 2f;
		}
		Camera[] allCameras = Camera.allCameras;
		foreach (Camera camera in allCameras)
		{
			camera.rect = new Rect(x, y, num5, num6);
		}
	}
}
