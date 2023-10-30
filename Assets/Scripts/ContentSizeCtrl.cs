using UnityEngine;
using UnityEngine.UI;

public class ContentSizeCtrl : MonoBehaviour
{
	private int curItemCount;

	private bool isInit;

	private RectTransform content;

	private GridLayoutGroup glg;

	private float cellSizeY;

	private float spacingY;

	private int childCount;

	private void Start()
	{
		content = base.transform.GetComponent<RectTransform>();
		glg = base.transform.GetComponent<GridLayoutGroup>();
	}

	private void Update()
	{
		childCount = 0;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeSelf)
			{
				childCount++;
			}
		}
		if (!isInit)
		{
			isInit = true;
			curItemCount = childCount;
			SetSize();
		}
		else if (curItemCount != childCount)
		{
			curItemCount = childCount;
			SetSize();
		}
	}

	private void SetSize()
	{
		Vector2 cellSize = glg.cellSize;
		cellSizeY = cellSize.y;
		Vector2 spacing = glg.spacing;
		spacingY = spacing.y;
		content.sizeDelta = Vector2.up * (cellSizeY + spacingY) * curItemCount;
	}
}
