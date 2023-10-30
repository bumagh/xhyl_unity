using UnityEngine;
using UnityEngine.UI;

public class ImageSetNativeSize : MonoBehaviour
{
	private Image image;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	private void Update()
	{
		if (image != null)
		{
			image.SetNativeSize();
		}
	}
}
