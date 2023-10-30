using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PHG_RulePicController : MonoBehaviour, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
{
	private List<float> targetFloat = new List<float>();

	private ScrollRect scrollRect;

	private float targetHoritalPosition;

	private bool isDrag;

	private int index;

	private float oldPosition;

	private float speed = 6f;

	private Image[] imgDots;

	private Color color;

	[SerializeField]
	private Sprite[] spiRules;

	private Image[] imgRules;

	private int language;

	private int tempNum;

	private Button BtnBack;

	private Button BtnLeft;

	private Button BtnRight;

	private void Awake()
	{
		BtnBack = base.transform.Find("BtnBack").GetComponent<Button>();
		BtnBack.onClick.AddListener(OnBtnBgClose_Click);
		BtnLeft = base.transform.Find("BtnLeft").GetComponent<Button>();
		BtnLeft.onClick.AddListener(LeftButton);
		BtnRight = base.transform.Find("BtnRight").GetComponent<Button>();
		BtnRight.onClick.AddListener(RightButton);
		scrollRect = base.transform.GetComponent<ScrollRect>();
		tempNum = spiRules.Length / 2;
		speed = tempNum;
		imgDots = new Image[tempNum];
		imgRules = new Image[tempNum];
		targetFloat = new List<float>();
		for (int i = 0; i < tempNum; i++)
		{
			float item = 1f / (float)(tempNum - 1) * (float)i;
			if (i == tempNum - 1)
			{
				item = 1f;
			}
			targetFloat.Add(item);
		}
		for (int j = 0; j < base.transform.Find("Viewport/Content").childCount; j++)
		{
			imgDots[j] = base.transform.Find("Dots/ImgDot" + j).GetComponent<Image>();
			imgRules[j] = base.transform.Find("Viewport/Content/Image" + j).GetComponent<Image>();
			if (j >= tempNum)
			{
				imgDots[j].gameObject.SetActive(value: false);
				imgRules[j].gameObject.SetActive(value: false);
			}
		}
	}

	private void Update()
	{
		if (!isDrag)
		{
			scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetHoritalPosition, 0.02f * speed);
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
				index = tempNum - 1;
			}
			DotColorSet();
			targetHoritalPosition = targetFloat[index];
		}
		else if (oldPosition < horizontalNormalizedPosition)
		{
			index++;
			if (index >= tempNum)
			{
				index = 0;
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
		PHG_SoundManager.Instance.PlayClickAudio();
		index--;
		if (index < 0)
		{
			index = tempNum - 1;
		}
		DotColorSet();
		targetHoritalPosition = targetFloat[index];
	}

	public void RightButton()
	{
		PHG_SoundManager.Instance.PlayClickAudio();
		index++;
		if (index >= tempNum)
		{
			index = 0;
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
		language = ((!(PHG_GVars.language == "zh")) ? 1 : 0);
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
		for (int i = 0; i < imgDots.Length; i++)
		{
			imgDots[i].color = ((i != index) ? color : Color.white);
		}
	}

	private void ImgRulesSet()
	{
		for (int i = 0; i < tempNum; i++)
		{
			imgRules[i].sprite = spiRules[i + tempNum * language];
		}
	}
}
