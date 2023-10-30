using UnityEngine;

public class ShadowByRender : MonoBehaviour
{
	private SpriteRenderer sp;

	private SpriteRenderer spParent;

	private void Start()
	{
		sp = base.gameObject.GetComponent<SpriteRenderer>();
		spParent = base.transform.parent.GetComponent<SpriteRenderer>();
		sp.sortingOrder = spParent.sortingOrder - 1;
	}

	private void Update()
	{
		if (sp.sprite != spParent.sprite)
		{
			sp.sprite = spParent.sprite;
		}
	}
}
