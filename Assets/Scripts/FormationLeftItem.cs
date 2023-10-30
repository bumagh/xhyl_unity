using UnityEngine;
using UnityEngine.EventSystems;

public class FormationLeftItem : CircleScrollRectItemBase, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
{
	public Transform tran_Select;

	public CircleScrollRect circleScrollRect;

	private bool isDraging;

	public override object GetContentListItem(int index)
	{
		return index;
	}

	public override int GetContentListLength()
	{
		return 5;
	}

	public override void UpdateContent(object data)
	{
	}

	public void UpdateShow(bool isShow)
	{
		tran_Select.gameObject.SetActive(isShow);
	}

	public void OnDrag(PointerEventData eventData)
	{
		circleScrollRect.OnDrag(eventData);
		isDraging = true;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		circleScrollRect.OnBeginDrag(eventData);
		isDraging = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		circleScrollRect.EndDrag(eventData);
		isDraging = false;
	}
}
