using UnityEngine;

public class UnderLine : MonoBehaviour
{
	private RectTransform rt;

	private RectTransform line;

	private float curWidth;

	private bool isInitUnderline;

	private void Start()
	{
		rt = base.transform.GetComponent<RectTransform>();
		line = base.transform.Find("Image").GetComponent<RectTransform>();
	}

	private void Update()
	{
		Vector2 sizeDelta = rt.sizeDelta;
		if (sizeDelta.x > 0f && !isInitUnderline)
		{
			isInitUnderline = true;
			Vector2 sizeDelta2 = rt.sizeDelta;
			curWidth = sizeDelta2.x;
			RectTransform rectTransform = line;
			float x = curWidth;
			Vector2 sizeDelta3 = line.sizeDelta;
			rectTransform.sizeDelta = new Vector2(x, sizeDelta3.y);
		}
		if (isInitUnderline)
		{
			float num = curWidth;
			Vector2 sizeDelta4 = rt.sizeDelta;
			if (num != sizeDelta4.x)
			{
				Vector2 sizeDelta5 = rt.sizeDelta;
				curWidth = sizeDelta5.x;
				RectTransform rectTransform2 = line;
				float x2 = curWidth;
				Vector2 sizeDelta6 = line.sizeDelta;
				rectTransform2.sizeDelta = new Vector2(x2, sizeDelta6.y);
			}
		}
	}
}
