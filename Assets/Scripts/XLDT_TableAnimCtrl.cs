using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class XLDT_TableAnimCtrl : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
{
	private float posX;

	public Transform tfCenterTable;

	public Transform tfLeftTable;

	public Transform tfRightTable;

	public Button btn;

	public XLDT_DropdownList sptDropdownList;

	public XLDT_DotsAnimCtrl sptDotsAnimCtrl;

	public void ClickBtnLeftArrow()
	{
		TableMoveLeft();
		SetTableIndex(1);
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
	}

	public void ClickBtnRightArrow()
	{
		TableMoveRight();
		SetTableIndex(-1);
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
	}

	public void SetTableIndex(int step)
	{
		int curTabIndex = XLDT_GameManager.getInstance()._mGameInfo.CurTabIndex;
		int totalTabNum = XLDT_GameInfo.getInstance().TotalTabNum;
		if (totalTabNum != 0)
		{
			XLDT_GameManager.getInstance()._mGameInfo.CurTabIndex = (curTabIndex + step + totalTabNum) % totalTabNum;
			sptDropdownList.ChooseTable(XLDT_GameManager.getInstance()._mGameInfo.CurTabIndex);
			sptDotsAnimCtrl.ChooseTable(XLDT_GameManager.getInstance()._mGameInfo.CurTabIndex);
		}
	}

	public void TableMoveRight()
	{
		if (XLDT_GameInfo.getInstance().TableList.Count != 0 && !XLDT_ShowUI.getInstance().m_bTableMove)
		{
			XLDT_ShowUI.getInstance().m_bTableMove = true;
			tfCenterTable.DOLocalMoveX(1100f, 0.2f).SetEase(Ease.Linear);
			tfLeftTable.DOLocalMoveX(0f, 0.2f).SetEase(Ease.Linear).OnComplete(ResetTablePosition);
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.changeTable);
		}
	}

	public void TableMoveLeft()
	{
		if (XLDT_GameInfo.getInstance().TableList.Count != 0 && !XLDT_ShowUI.getInstance().m_bTableMove)
		{
			XLDT_ShowUI.getInstance().m_bTableMove = true;
			tfCenterTable.DOLocalMoveX(-1100f, 0.2f).SetEase(Ease.Linear);
			tfRightTable.DOLocalMoveX(0f, 0.2f).SetEase(Ease.Linear).OnComplete(ResetTablePosition);
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.changeTable);
		}
	}

	private void ResetTablePosition()
	{
		tfCenterTable.localPosition = Vector3.down * 90f;
		tfLeftTable.localPosition = Vector3.left * 1100f + Vector3.down * 90f;
		tfRightTable.localPosition = Vector3.right * 1100f + Vector3.down * 90f;
		XLDT_ShowUI.getInstance().m_bTableMove = false;
		XLDT_ShowUI.getInstance().SetTableInfoVisible();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Vector2 position = eventData.position;
		posX = position.x;
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
			TableMoveRight();
			SetTableIndex(-1);
			return;
		}
		Vector2 position3 = eventData.position;
		if (position3.y < posX)
		{
			TableMoveLeft();
			SetTableIndex(1);
		}
	}

	public void HideTable()
	{
		XLDT_ShowUI.getInstance().m_bTableMove = true;
		tfCenterTable.DOLocalMoveY(120f, 0.5f);
		tfCenterTable.DOScale(0.5f, 0.5f).OnComplete(delegate
		{
			XLDT_ShowUI.getInstance().m_bTableMove = false;
			tfCenterTable.gameObject.SetActive(value: false);
			XLDT_ShowUI.getInstance().SetTableInfoVisible();
		});
	}

	public void ShowTable()
	{
		XLDT_ShowUI.getInstance().m_bTableMove = true;
		tfCenterTable.gameObject.SetActive(value: true);
		tfCenterTable.DOLocalMoveY(-90f, 0.5f);
		tfCenterTable.DOScale(1f, 0.5f).OnComplete(ResetTablePosition);
	}

	public void HideTableNow()
	{
		tfCenterTable.localPosition = Vector3.up * 120f;
		tfCenterTable.localScale = Vector3.one * 0.5f;
		MonoBehaviour.print("HideTableNow");
		tfCenterTable.gameObject.SetActive(value: false);
	}
}
