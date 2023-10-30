using UnityEngine;

[RequireComponent(typeof(UIWidget))]
[ExecuteInEditMode]
public class BCBM_Animatedcolor : MonoBehaviour
{
	public Color color = Color.white;

	private UIWidget mWidget;

	private void Awake()
	{
		mWidget = GetComponent<UIWidget>();
	}

	private void Update()
	{
		mWidget.color = color;
	}
}
