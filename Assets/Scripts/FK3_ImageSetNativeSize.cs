using UnityEngine;
using UnityEngine.UI;

public class FK3_ImageSetNativeSize : MonoBehaviour
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
