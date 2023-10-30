using UnityEngine;
using UnityEngine.UI;

public class MSE_DeskSelectionViewController : MSE_MB_Singleton<MSE_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (MSE_MB_Singleton<MSE_DeskSelectionViewController>._instance == null)
		{
			MSE_MB_Singleton<MSE_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(MSE_MB_Singleton<MSE_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(MSE_MB_Singleton<MSE_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		MSE_Utils.TrySetActive(_goUIContainer, active: true);
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		MSE_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
