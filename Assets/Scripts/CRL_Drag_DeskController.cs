using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CRL_Drag_DeskController : EventTrigger
{
	public static CRL_Drag_DeskController _instance;

	private float _fStartX;

	private float _fEndX;

	public Action right;

	public Action left;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		Vector3 mousePosition = UnityEngine.Input.mousePosition;
		_fStartX = mousePosition.x;
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		Vector3 mousePosition = UnityEngine.Input.mousePosition;
		_fEndX = mousePosition.x;
		JugeDragDirection(_fStartX, _fEndX);
	}

	public void JugeDragDirection(float startx, float endx)
	{
		if (startx - endx > 25f)
		{
			right();
		}
		else if (startx - endx < -25f)
		{
			left();
		}
		else if (startx == endx && CRL_MySqlConnection.curView == "DeskSelectionView")
		{
			CRL_DeskPullDownListController.Instance.RollUpListByClickBG();
		}
	}
}
