using UnityEngine;

public class CSF_DiceGameController : CSF_MB_Singleton<CSF_DiceGameController>
{
	private GameObject _goUIContainer;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		if (CSF_MB_Singleton<CSF_DiceGameController>._instance == null)
		{
			CSF_MB_Singleton<CSF_DiceGameController>.SetInstance(this);
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
		CSF_MB_Singleton<CSF_DiceGameController2>.GetInstance().Show();
	}

	public void Hide()
	{
		CSF_Utils.TrySetActive(_goUIContainer, active: false);
		CSF_MB_Singleton<CSF_DiceGameController2>.GetInstance().Hide();
	}
}
