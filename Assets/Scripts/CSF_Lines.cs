using UnityEngine;

public class CSF_Lines : MonoBehaviour
{
	[HideInInspector]
	public RectTransform rtfMask;

	[HideInInspector]
	public GameObject[] imgLines = new GameObject[9];

	private void Awake()
	{
		rtfMask = GetComponent<RectTransform>();
		for (int i = 0; i < 9; i++)
		{
			imgLines[i] = rtfMask.Find((i + 1).ToString()).gameObject;
		}
	}
}
