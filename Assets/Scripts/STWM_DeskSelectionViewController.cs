using UnityEngine;

public class STWM_DeskSelectionViewController : STWM_MB_Singleton<STWM_DeskSelectionViewController>
{
	[SerializeField]
	private GameObject _goUIContainer;

	private void Awake()
	{
		if (STWM_MB_Singleton<STWM_DeskSelectionViewController>._instance == null)
		{
			STWM_MB_Singleton<STWM_DeskSelectionViewController>.SetInstance(this);
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
		STWM_MB_Singleton<STWM_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		STWM_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
