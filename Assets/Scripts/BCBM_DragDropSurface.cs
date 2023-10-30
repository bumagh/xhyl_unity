using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Surface")]
public class BCBM_DragDropSurface : MonoBehaviour
{
	public bool rotatePlacedObject;

	private void OnDrop(GameObject go)
	{
		BCBM_DragDropItem component = go.GetComponent<BCBM_DragDropItem>();
		if (component != null)
		{
			GameObject gameObject = NGUITools.AddChild(base.gameObject, component.prefab);
			Transform transform = gameObject.transform;
			transform.position = UICamera.lastHit.point;
			if (rotatePlacedObject)
			{
				transform.rotation = Quaternion.LookRotation(UICamera.lastHit.normal) * Quaternion.Euler(90f, 0f, 0f);
			}
			UnityEngine.Object.Destroy(go);
		}
	}
}
