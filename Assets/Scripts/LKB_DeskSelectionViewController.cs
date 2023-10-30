using UnityEngine;

public class LKB_DeskSelectionViewController : LKB_MB_Singleton<LKB_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		if (LKB_MB_Singleton<LKB_DeskSelectionViewController>._instance == null)
		{
			LKB_MB_Singleton<LKB_DeskSelectionViewController>.SetInstance(this);
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
		LKB_MB_Singleton<LKB_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		LKB_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
