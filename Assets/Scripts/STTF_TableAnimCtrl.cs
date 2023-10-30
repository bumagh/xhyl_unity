using UnityEngine;
using UnityEngine.EventSystems;

public class STTF_TableAnimCtrl : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
{
	private float posX;

	public STTF_UIScene sptUiScene;

	public void OnPointerDown(PointerEventData eventData)
	{
		Vector2 position = eventData.position;
		posX = position.x;
		sptUiScene.ClickBtnBG(bChangeState: false);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Vector2 position = eventData.position;
		if (position.x == posX)
		{
			return;
		}
		Vector2 position2 = eventData.position;
		if (position2.x > posX)
		{
			sptUiScene.TableMoveRight();
			STTF_GameInfo.getInstance().changeTable(1);
			return;
		}
		Vector2 position3 = eventData.position;
		if (position3.y < posX)
		{
			sptUiScene.TableMoveLeft();
			STTF_GameInfo.getInstance().changeTable(0);
		}
	}
}
