using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DP_BtnAnimCtrl : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
{
	[HideInInspector]
	public Button btn;

	private Image img;

	private Text txt;

	private void Awake()
	{
		btn = base.transform.GetComponent<Button>();
		img = btn.transform.GetChild(0).GetComponent<Image>();
		txt = img.transform.GetChild(0).GetComponent<Text>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		img.transform.DOBlendableLocalMoveBy(Vector3.up * -8f, 0.2f);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		img.transform.DOBlendableLocalMoveBy(Vector3.up * 8f, 0.2f);
	}

	public void SetBtnActive(bool bActive)
	{
		Color color = (!bActive) ? Color.grey : Color.white;
		btn.interactable = bActive;
		btn.image.color = color;
		img.color = color;
		txt.color = color;
	}
}
