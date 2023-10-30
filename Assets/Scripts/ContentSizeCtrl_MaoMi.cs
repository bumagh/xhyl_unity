using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContentSizeCtrl_MaoMi : MonoBehaviour
{
	private int curItemCount;

	private bool isInit;

	private RectTransform content;

	private VerticalLayoutGroup glg_V;

	private HorizontalLayoutGroup glg_H;

	private float spacing;

	private int childCount;

	public Direction_MaoMi direction;

	private Coroutine waitSetSize;

	private void OnEnable()
	{
		content = base.transform.GetComponent<RectTransform>();
		glg_V = base.transform.GetComponent<VerticalLayoutGroup>();
		glg_H = base.transform.GetComponent<HorizontalLayoutGroup>();
		curItemCount = -1;
		if (waitSetSize != null)
		{
			StopCoroutine(waitSetSize);
		}
		waitSetSize = StartCoroutine(WaitSetSize());
	}

	private IEnumerator WaitSetSize()
	{
		while (true)
		{
			SetSize();
			yield return new WaitForSeconds(0.2f);
		}
	}

	public void SetSize()
	{
		if (content == null)
		{
			return;
		}
		if (direction == Direction_MaoMi.X)
		{
			if (glg_H == null)
			{
				return;
			}
			spacing = glg_H.spacing;
			content.sizeDelta = Vector2.zero;
			curItemCount = 0;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				if (base.transform.GetChild(i).gameObject.activeInHierarchy)
				{
					curItemCount++;
					RectTransform rectTransform = content;
					Vector2 sizeDelta = rectTransform.sizeDelta;
					Vector2 sizeDelta2 = base.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta;
					float x = sizeDelta2.x;
					Vector2 sizeDelta3 = content.sizeDelta;
					rectTransform.sizeDelta = sizeDelta + new Vector2(x, sizeDelta3.y);
				}
			}
			RectTransform rectTransform2 = content;
			Vector2 sizeDelta4 = content.sizeDelta;
			float x2 = Mathf.Abs(sizeDelta4.x);
			Vector2 sizeDelta5 = content.sizeDelta;
			rectTransform2.sizeDelta = new Vector2(x2, sizeDelta5.y);
		}
		else
		{
			if (glg_V == null)
			{
				return;
			}
			spacing = glg_V.spacing;
			content.sizeDelta = Vector2.zero;
			curItemCount = 0;
			for (int j = 0; j < base.transform.childCount; j++)
			{
				if (base.transform.GetChild(j).gameObject.activeInHierarchy)
				{
					curItemCount++;
					RectTransform rectTransform3 = content;
					Vector2 sizeDelta6 = rectTransform3.sizeDelta;
					Vector2 sizeDelta7 = content.sizeDelta;
					float x3 = sizeDelta7.x;
					Vector2 sizeDelta8 = base.transform.GetChild(j).GetComponent<RectTransform>().sizeDelta;
					rectTransform3.sizeDelta = sizeDelta6 + new Vector2(x3, sizeDelta8.y);
				}
			}
			RectTransform rectTransform4 = content;
			Vector2 sizeDelta9 = content.sizeDelta;
			float x4 = sizeDelta9.x;
			Vector2 sizeDelta10 = content.sizeDelta;
			rectTransform4.sizeDelta = new Vector2(x4, sizeDelta10.y + spacing * (float)curItemCount);
		}
	}
}
