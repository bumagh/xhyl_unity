using UnityEngine;

public class CSF_DeskSelectionViewController : CSF_MB_Singleton<CSF_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		if (CSF_MB_Singleton<CSF_DeskSelectionViewController>._instance == null)
		{
			CSF_MB_Singleton<CSF_DeskSelectionViewController>.SetInstance(this);
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
		CSF_MB_Singleton<CSF_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		CSF_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
