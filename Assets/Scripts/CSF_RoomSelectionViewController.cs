using UnityEngine;

public class CSF_RoomSelectionViewController : CSF_MB_Singleton<CSF_RoomSelectionViewController>
{
	private GameObject _goUIContainer;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		if (CSF_MB_Singleton<CSF_RoomSelectionViewController>._instance == null)
		{
			CSF_MB_Singleton<CSF_RoomSelectionViewController>.SetInstance(this);
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
		CSF_Utils.TrySetActive(_goUIContainer, active: true);
		CSF_MB_Singleton<CSF_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		CSF_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
