using UnityEngine;
using UnityEngine.UI;

public class ContentSizeCtrl_Campaign : MonoBehaviour
{
	private RectTransform content;

	private Text text;

	private void Start()
	{
		content = base.transform.GetComponent<RectTransform>();
	}

	private void Update()
	{
	}
}
