using UnityEngine;

public class LKB_DiceGameController : LKB_MB_Singleton<LKB_DiceGameController>
{
	private GameObject _goUIContainer;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		if (LKB_MB_Singleton<LKB_DiceGameController>._instance == null)
		{
			LKB_MB_Singleton<LKB_DiceGameController>.SetInstance(this);
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
		LKB_MB_Singleton<LKB_DiceGameController2>.GetInstance().Show();
	}

	public void Hide()
	{
		LKB_Utils.TrySetActive(_goUIContainer, active: false);
		LKB_MB_Singleton<LKB_DiceGameController2>.GetInstance().Hide();
	}
}
