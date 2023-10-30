using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DPR_RulePicController : MonoBehaviour, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
{
	private float[] targetFloat = new float[4]
	{
		0f,
		0.33333f,
		0.66666f,
		1f
	};

	private ScrollRect scrollRect;

	private float targetHoritalPosition;

	private bool isDrag;

	private int index;

	private float oldPosition;

	private float speed = 4f;

	private Image[] imgDots;

	private Color color;

	[SerializeField]
	private Sprite[] spiRules;

	private Image[] imgRules;

	private int language;

	public int tempNum;

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
		imgDots = new Image[tempNum];
		imgRules = new Image[tempNum];
		for (int i = 0; i < base.transform.Find("Viewport/Content").childCount; i++)
		{
			imgDots[i] = base.transform.Find("ImgDot" + i).GetComponent<Image>();
			imgRules[i] = base.transform.Find("Viewport/Content/Image" + i).GetComponent<Image>();
			if (i >= tempNum)
			{
				imgDots[i].gameObject.SetActive(value: false);
				imgRules[i].gameObject.SetActive(value: false);
			}
		}
	}

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
		DPR_SoundManager.Instance.PlayClickAudio();
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
		DPR_SoundManager.Instance.PlayClickAudio();
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
		language = ((!(DPR_MySqlConnection.language == "zh")) ? 1 : 0);
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
