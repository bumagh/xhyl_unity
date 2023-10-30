using UnityEngine;

public class LKB_RoomSelectionViewController : LKB_MB_Singleton<LKB_RoomSelectionViewController>
{
	private GameObject _goUIContainer;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		if (LKB_MB_Singleton<LKB_RoomSelectionViewController>._instance == null)
		{
			LKB_MB_Singleton<LKB_RoomSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	public void PreInit()
	{
		if (_goUIContainer == null)
		{
			_goUIContainer = base.gameObject;
		}
	}

	public void Show()
	{
		LKB_Utils.TrySetActive(_goUIContainer, active: true);
		LKB_MB_Singleton<LKB_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		LKB_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
