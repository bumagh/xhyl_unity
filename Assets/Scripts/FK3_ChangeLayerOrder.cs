using UnityEngine;

[ExecuteInEditMode]
public class FK3_ChangeLayerOrder : MonoBehaviour
{
	public int layerOrder;

	private void Awake()
	{
		Renderer component = GetComponent<Renderer>();
		component.sortingOrder = layerOrder;
	}

	private void Update()
	{
	}
}
