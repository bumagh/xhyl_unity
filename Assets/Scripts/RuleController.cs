using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RuleController : MonoBehaviour
{
	[SerializeField]
	private RectTransform _rule;

	private float[] _pageArray;

	[SerializeField]
	private GameObject _helpContent;

	[SerializeField]
	private Toggle[] _toggleArray;

	private int centerIndex;

	private float lastPosX;

	private float currentPosX;

	public float speed = 0.01f;

	private void Awake()
	{
		_pageArray = new float[3]
		{
			0f,
			-890f,
			-1760f
		};
		Init();
		base.transform.Find("grayBg").GetComponent<Button>().onClick.AddListener(CloseRule);
	}

	private void Update()
	{
		if (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began)
		{
			Vector2 position = UnityEngine.Input.GetTouch(0).position;
			lastPosX = position.x;
		}
		if (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			Vector2 deltaPosition = UnityEngine.Input.GetTouch(0).deltaPosition;
			float num = deltaPosition.x;
			if (centerIndex == 0)
			{
				num = Mathf.Min(30f, num);
				Vector3 localPosition = _helpContent.transform.localPosition;
				if (localPosition.x < 40f)
				{
					_helpContent.transform.Translate(num * speed, 0f, 0f);
				}
			}
			if (centerIndex == 1)
			{
				_helpContent.transform.Translate(num * speed, 0f, 0f);
			}
			if (centerIndex == 2)
			{
				num = Mathf.Max(-30f, num);
				Vector3 localPosition2 = _helpContent.transform.localPosition;
				if (localPosition2.x > -1800f)
				{
					_helpContent.transform.Translate(num * speed, 0f, 0f);
				}
			}
		}
		if (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Canceled)
		{
			Vector2 position2 = UnityEngine.Input.GetTouch(0).position;
			currentPosX = position2.x;
			if (currentPosX - lastPosX > 100f)
			{
				if (centerIndex > 0)
				{
					centerIndex--;
				}
			}
			else if (lastPosX - currentPosX > 100f && centerIndex < 2)
			{
				centerIndex++;
			}
			Move();
		}
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			lastPosX = mousePosition.x;
		}
		if (UnityEngine.Input.touchCount > 0)
		{
		}
		if (!Input.GetMouseButtonUp(0))
		{
			return;
		}
		Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
		currentPosX = mousePosition2.x;
		if (currentPosX > lastPosX)
		{
			if (centerIndex > 0)
			{
				centerIndex--;
			}
		}
		else if (currentPosX < lastPosX && centerIndex < 2)
		{
			centerIndex++;
		}
		Move();
	}

	public void Init()
	{
		centerIndex = 0;
		_toggleArray[0].isOn = true;
		Move();
	}

	public void CloseRule()
	{
		Init();
		_rule.gameObject.SetActive(value: false);
	}

	public void Move()
	{
		_helpContent.transform.DOLocalMoveX(_pageArray[centerIndex], 0.45f);
		_toggleArray[centerIndex].isOn = true;
	}
}
