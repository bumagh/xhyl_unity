using UnityEngine;

public class ScreenWHCtrl : MonoBehaviour
{
	private RectTransform rt;

	private void Awake()
	{
		rt = base.transform.GetComponent<RectTransform>();
		int width = 1280;
		int height = (int)((float)Screen.height * 1280f / (float)Screen.width);
		Screen.SetResolution(width, height, fullscreen: false);
		rt.sizeDelta = new Vector2(Screen.width, Screen.height - 720);
	}
}
