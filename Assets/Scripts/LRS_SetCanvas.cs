using UnityEngine;
using UnityEngine.UI;

public class LRS_SetCanvas : MonoBehaviour
{
	private void Awake()
	{
		CanvasScaler component = base.transform.GetComponent<CanvasScaler>();
		if ((float)Screen.width / (float)Screen.height > 1.77777779f)
		{
			component.matchWidthOrHeight = 1f;
		}
		else
		{
			component.matchWidthOrHeight = 0f;
		}
	}
}
