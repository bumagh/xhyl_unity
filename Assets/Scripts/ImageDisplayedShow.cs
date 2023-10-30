using UnityEngine;

public class ImageDisplayedShow : MonoBehaviour
{
	private int tempNum;

	private Transform parent;

	private void Awake()
	{
		parent = base.transform.parent;
	}

	private void Update()
	{
		if (parent != null && parent.childCount != 0 && parent.childCount != tempNum)
		{
			tempNum = parent.childCount;
			base.transform.SetSiblingIndex(parent.childCount);
		}
	}
}
