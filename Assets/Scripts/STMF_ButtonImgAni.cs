using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class STMF_ButtonImgAni : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	[SerializeField]
	private Sprite[] sprites;

	private Image image;

	private int language;

	private int index;

	public bool isSetNativeSize;

	private void Start()
	{
		image = base.transform.GetComponent<Image>();
		language = STMF_GameInfo.getInstance().Language;
		if (language > 1)
		{
			language = 1;
		}
		if (language < 0)
		{
			language = 0;
		}
		index = language * 2;
		image.sprite = sprites[index];
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		image.sprite = sprites[index + 1];
		if (isSetNativeSize)
		{
			image.SetNativeSize();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		image.sprite = sprites[index];
		if (isSetNativeSize)
		{
			image.SetNativeSize();
		}
	}
}
