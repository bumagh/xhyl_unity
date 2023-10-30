using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DK_ButtonImgAni : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	[SerializeField]
	private Sprite[] sprites;

	private Image image;

	private int language;

	private int index;

	private void Start()
	{
		image = base.transform.GetComponent<Image>();
		index = language * 2;
		image.sprite = sprites[index];
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		image.sprite = sprites[index + 1];
		image.SetNativeSize();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		image.sprite = sprites[index];
		image.SetNativeSize();
	}
}
