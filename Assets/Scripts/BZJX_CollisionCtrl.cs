using UnityEngine;

public class BZJX_CollisionCtrl : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (base.transform.parent.gameObject.activeSelf)
		{
			SendMessageUpwards("OnOtherCollision", other);
		}
	}
}
