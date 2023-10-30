using UnityEngine;
using UnityEngine.EventSystems;

public class TF_TableAnimCtrl : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
{
	private float posX;

	public TF_UIScene sptUiScene;

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
			TF_GameInfo.getInstance().changeTable(1);
			return;
		}
		Vector2 position3 = eventData.position;
		if (position3.y < posX)
		{
			sptUiScene.TableMoveLeft();
			TF_GameInfo.getInstance().changeTable(0);
		}
	}
}
