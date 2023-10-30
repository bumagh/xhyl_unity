using UnityEngine;

public class ToBeOne : MonoBehaviour
{
	private void OnEnable()
	{
		if (base.transform.localScale != Vector3.one)
		{
			base.transform.localScale = Vector3.one;
		}
	}
}
