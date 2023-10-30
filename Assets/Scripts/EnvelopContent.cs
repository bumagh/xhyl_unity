using UnityEngine;

[AddComponentMenu("NGUI/Examples/Envelop Content")]
[RequireComponent(typeof(UIWidget))]
public class EnvelopContent : MonoBehaviour
{
	public Transform targetRoot;

	public int padLeft;

	public int padRight;

	public int padBottom;

	public int padTop;

	private bool mStarted;

	private void Start()
	{
		mStarted = true;
		Execute();
	}

	private void OnEnable()
	{
		if (mStarted)
		{
			Execute();
		}
	}

	[ContextMenu("Execute")]
	public void Execute()
	{
		if (targetRoot == base.transform)
		{
			UnityEngine.Debug.LogError("Target Root object cannot be the same object that has Envelop Content. Make it a sibling instead.", this);
			return;
		}
		if (NGUITools.IsChild(targetRoot, base.transform))
		{
			UnityEngine.Debug.LogError("Target Root object should not be a parent of Envelop Content. Make it a sibling instead.", this);
			return;
		}
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.transform.parent, targetRoot, considerInactive: false);
		Vector3 min = bounds.min;
		float num = min.x + (float)padLeft;
		Vector3 min2 = bounds.min;
		float num2 = min2.y + (float)padBottom;
		Vector3 max = bounds.max;
		float num3 = max.x + (float)padRight;
		Vector3 max2 = bounds.max;
		float num4 = max2.y + (float)padTop;
		UIWidget component = GetComponent<UIWidget>();
		component.SetRect(num, num2, num3 - num, num4 - num2);
		BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
	}
}
