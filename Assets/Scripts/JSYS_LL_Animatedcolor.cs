using UnityEngine;

[RequireComponent(typeof(UIWidget))]
[ExecuteInEditMode]
public class JSYS_LL_Animatedcolor : MonoBehaviour
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
