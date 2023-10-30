using UnityEngine;

public class PTM_Lines : MonoBehaviour
{
	[HideInInspector]
	public RectTransform rtfMask;

	[HideInInspector]
	public GameObject[] imgLines;

	private void Awake()
	{
		imgLines = new GameObject[base.transform.childCount];
		rtfMask = GetComponent<RectTransform>();
		for (int i = 0; i < 15; i++)
		{
			imgLines[i] = rtfMask.Find((i + 1).ToString()).gameObject;
		}
	}
}
