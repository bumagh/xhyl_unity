using UnityEngine;

public class JSYS_LL_Animatedalpha : MonoBehaviour
{
	public float alpha = 1f;

	private UIWidget mWidget;

	private UIPanel mPanel;

	private void Awake()
	{
		mWidget = GetComponent<UIWidget>();
		mPanel = GetComponent<UIPanel>();
		Update();
	}

	private void Update()
	{
		if (mWidget != null)
		{
			mWidget.alpha = alpha;
		}
		if (mPanel != null)
		{
			mPanel.alpha = alpha;
		}
	}
}
