using UnityEngine;
using UnityEngine.UI;

public class TwoContentSizeCtrl : MonoBehaviour
{
	private int curItemCount = -1;

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

	private void OnEnable()
	{
		curItemCount = -1;
		isInit = false;
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
		childCount = childCount / 2 + 1;
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
		UnityEngine.Debug.LogError("childCount: " + childCount + "  " + Vector2.right);
		Vector2 cellSize = glg.cellSize;
		cellSizeY = cellSize.x;
		Vector2 spacing = glg.spacing;
		spacingY = spacing.x;
		content.sizeDelta = Vector2.right * (cellSizeY + spacingY) * curItemCount;
	}
}
