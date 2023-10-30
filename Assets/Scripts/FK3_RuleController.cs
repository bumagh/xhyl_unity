using UnityEngine;
using UnityEngine.UI;

public class FK3_RuleController : MonoBehaviour
{
	[SerializeField]
	private RectTransform _rule;

	private void Awake()
	{
		base.transform.Find("grayBg").GetComponent<Button>().onClick.AddListener(CloseRule);
	}

	public void Init()
	{
		Move();
	}

	public void CloseRule()
	{
		_rule.gameObject.SetActive(value: false);
	}

	public void Move()
	{
	}
}
