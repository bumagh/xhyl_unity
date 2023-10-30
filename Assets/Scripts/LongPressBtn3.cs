using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongPressBtn3 : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler, IScrollHandler, IPointerUpHandler, IEventSystemHandler
{
	private ScrollRect scrollRect;

	[SerializeField]
	private Button.ButtonClickedEvent m_OnClickUp = new Button.ButtonClickedEvent();

	public Button.ButtonClickedEvent onClickUp
	{
		get
		{
			return m_OnClickUp;
		}
		set
		{
			m_OnClickUp = value;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		UnityEngine.Debug.LogError("=====鼠标抬起===");
		GetRect();
		SetScale(isToBig: false);
	}

	private void GetRect()
	{
		if (scrollRect == null)
		{
			Transform root = base.transform.root;
			Transform transform = root.Find("Mask/Tables/Scroll View");
			if (transform == null)
			{
				transform = root.Find("Mask/Lobby/STWM_Tables/Scroll View");
			}
			if (transform == null)
			{
				transform = root.Find("Tables_LL/Scroll View");
			}
			if (transform == null)
			{
				transform = root.Find("Mask/Room/Tables/Scroll View");
			}
			if (transform != null)
			{
				scrollRect = transform.GetComponent<ScrollRect>();
			}
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		GetRect();
		SetScale();
		UnityEngine.Debug.LogError("=====开始拖动=====");
		scrollRect.OnBeginDrag(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		GetRect();
		SetScale();
		scrollRect.OnDrag(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		GetRect();
		SetScale(isToBig: false);
		UnityEngine.Debug.LogError("=====结束拖动=====");
		scrollRect.OnEndDrag(eventData);
	}

	public void OnScroll(PointerEventData eventData)
	{
		GetRect();
		scrollRect.OnScroll(eventData);
	}

	public void OnInitializePotentialDrag(PointerEventData eventData)
	{
		GetRect();
		SetScale();
		UnityEngine.Debug.LogError("=====点击 但没拖拽=====");
		scrollRect.OnInitializePotentialDrag(eventData);
	}

	private void SetScale(bool isToBig = true)
	{
		if (isToBig)
		{
			if (base.transform.localScale != Vector3.one * 1.1f)
			{
				base.transform.DOScale(Vector3.one * 1.1f, 0.1f);
			}
		}
		else if (base.transform.localScale != Vector3.one)
		{
			base.transform.DOScale(Vector3.one, 0.1f);
		}
	}
}
