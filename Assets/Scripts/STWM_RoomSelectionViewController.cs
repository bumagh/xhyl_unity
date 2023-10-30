using UnityEngine;

public class STWM_RoomSelectionViewController : STWM_MB_Singleton<STWM_RoomSelectionViewController>
{
	[SerializeField]
	private GameObject _goUIContainer;

	private void Awake()
	{
		if (STWM_MB_Singleton<STWM_RoomSelectionViewController>._instance == null)
		{
			STWM_MB_Singleton<STWM_RoomSelectionViewController>.SetInstance(this);
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
		STWM_Utils.TrySetActive(_goUIContainer, active: true);
		STWM_MB_Singleton<STWM_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		STWM_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
