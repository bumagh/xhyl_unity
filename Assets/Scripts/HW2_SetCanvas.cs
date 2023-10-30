using UnityEngine;
using UnityEngine.UI;

public class HW2_SetCanvas : MonoBehaviour
{
	private CanvasScaler cs;

	private void Awake()
	{
		cs = base.transform.GetComponent<CanvasScaler>();
		if ((float)Screen.width / (float)Screen.height > 1.77777779f)
		{
			cs.matchWidthOrHeight = 1f;
		}
		else
		{
			cs.matchWidthOrHeight = 0f;
		}
	}

	private void OnEnable()
	{
		if ((float)Screen.width / (float)Screen.height > 1.77777779f)
		{
			cs.matchWidthOrHeight = 1f;
		}
		else
		{
			cs.matchWidthOrHeight = 0f;
		}
	}
}
