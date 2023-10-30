using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class STWM_RulePicController : MonoBehaviour, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
{
	private float[] targetFloat = new float[4]
	{
		0f,
		0.33333f,
		0.66666f,
		1f
	};

	[SerializeField]
	private ScrollRect scrollRect;

	private float targetHoritalPosition;

	private bool isDrag;

	private int index;

	private float oldPosition;

	private float speed = 4f;

	[SerializeField]
	private Image[] imgDots;

	private Color color;

	[SerializeField]
	private Sprite[] spiRules;

	[SerializeField]
	private Image[] imgRules;

	private int language;

	private void Update()
	{
		if (!isDrag)
		{
			scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetHoritalPosition, Time.deltaTime * speed);
		}
		else
		{
			UnityEngine.Debug.Log(scrollRect.horizontalNormalizedPosition);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDrag = true;
		oldPosition = scrollRect.horizontalNormalizedPosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDrag = false;
		float horizontalNormalizedPosition = scrollRect.horizontalNormalizedPosition;
		if (oldPosition > horizontalNormalizedPosition)
		{
			index--;
			if (index < 0)
			{
				index = 0;
			}
			DotColorSet();
			targetHoritalPosition = targetFloat[index];
		}
		else if (oldPosition < horizontalNormalizedPosition)
		{
			index++;
			if (index > 3)
			{
				index = 3;
			}
			DotColorSet();
			targetHoritalPosition = targetFloat[index];
		}
	}

	public void PointButtonClick(int value)
	{
		targetHoritalPosition = targetFloat[value];
	}

	public void LeftButton()
	{
		STWM_SoundManager.Instance.PlayClickAudio();
		index--;
		if (index < 0)
		{
			index = 0;
		}
		DotColorSet();
		targetHoritalPosition = targetFloat[index];
	}

	public void RightButton()
	{
		STWM_SoundManager.Instance.PlayClickAudio();
		index++;
		if (index > 3)
		{
			index = 3;
		}
		DotColorSet();
		targetHoritalPosition = targetFloat[index];
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
		targetHoritalPosition = targetFloat[0];
		index = 0;
		color = imgDots[1].color;
		language = ((!(STWM_GVars.language == "zh")) ? 1 : 0);
		DotColorSet();
		ImgRulesSet();
		UnityEngine.Debug.Log("index: " + index);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void OnBtnBgClose_Click()
	{
		UnityEngine.Debug.Log("OnBtnBgClose_Click");
		Hide();
	}

	private void DotColorSet()
	{
		for (int i = 0; i < 4; i++)
		{
			imgDots[i].color = ((i != index) ? color : Color.white);
		}
	}

	private void ImgRulesSet()
	{
		for (int i = 0; i < 4; i++)
		{
			imgRules[i].sprite = spiRules[i + 4 * language];
		}
	}
}
