using UnityEngine;

public class STWM_DiceGameController : STWM_MB_Singleton<STWM_DiceGameController>
{
	[SerializeField]
	private GameObject _goUIContainer;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		if (STWM_MB_Singleton<STWM_DiceGameController>._instance == null)
		{
			STWM_MB_Singleton<STWM_DiceGameController>.SetInstance(this);
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
		STWM_MB_Singleton<STWM_DiceGameController2>.GetInstance().Show();
	}

	public void Hide()
	{
		STWM_Utils.TrySetActive(_goUIContainer, active: false);
		STWM_MB_Singleton<STWM_DiceGameController2>.GetInstance().Hide();
	}
}
