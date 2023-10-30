using UnityEngine;
using UnityEngine.UI;

public class ContentSizeCtrl_WaterMargin : MonoBehaviour
{
	private int curItemCount;

	private bool isInit;

	public RectTransform content;

	private GridLayoutGroup glg;

	private float cellSizeY;

	private float spacingY;

	private float cellSizeX;

	private float spacingX;

	private int childCount;

	public Direction_WaterMargin direction;

	public int RowOrColumnNum = 1;

	private void Start()
	{
		content = base.transform.GetComponent<RectTransform>();
		glg = base.transform.GetComponent<GridLayoutGroup>();
	}

	private void OnEnable()
	{
		if (content == null)
		{
			content = base.transform.GetComponent<RectTransform>();
		}
		if (glg == null)
		{
			glg = base.transform.GetComponent<GridLayoutGroup>();
		}
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

	public void SetSize()
	{
		if (glg == null || content == null || curItemCount <= 0)
		{
			UnityEngine.Debug.LogError("设置错误 " + curItemCount);
			content.sizeDelta = Vector3.zero;
			return;
		}
		Vector2 cellSize = glg.cellSize;
		cellSizeY = cellSize.y;
		Vector2 spacing = glg.spacing;
		spacingY = spacing.y;
		Vector2 cellSize2 = glg.cellSize;
		cellSizeX = cellSize2.x;
		Vector2 spacing2 = glg.spacing;
		spacingX = spacing2.x;
		content.sizeDelta = Vector2.zero;
		switch (direction)
		{
		case Direction_WaterMargin.Y:
			content.sizeDelta = Vector2.up * (cellSizeY + spacingY) * Mathf.CeilToInt(curItemCount / RowOrColumnNum);
			break;
		case Direction_WaterMargin.X:
			content.sizeDelta = Vector2.right * (cellSizeX - spacingX) * Mathf.FloorToInt(curItemCount / RowOrColumnNum);
			break;
		case Direction_WaterMargin.X2:
			content.sizeDelta = Vector2.right * (cellSizeX + spacingX) * Mathf.FloorToInt(curItemCount / RowOrColumnNum);
			break;
		case Direction_WaterMargin.X3:
			content.sizeDelta = Vector2.right * (cellSizeX + spacingX) * curItemCount - Vector2.right * (cellSizeX + spacingX) * 3.9f;
			break;
		case Direction_WaterMargin.XJSYS:
			content.sizeDelta = Vector2.right * (cellSizeX + spacingX) * curItemCount - Vector2.right * (cellSizeX + spacingX) * 2.2f;
			break;
		case Direction_WaterMargin.ALL:
		{
			RectTransform rectTransform = content;
			Vector2 vector = Vector2.right * (cellSizeX + spacingX) * Mathf.CeilToInt(curItemCount / RowOrColumnNum);
			float x = vector.x;
			Vector2 vector2 = Vector2.up * (cellSizeY + spacingY) * Mathf.CeilToInt(curItemCount / RowOrColumnNum);
			rectTransform.sizeDelta = new Vector2(x, vector2.y);
			break;
		}
		}
	}
}
