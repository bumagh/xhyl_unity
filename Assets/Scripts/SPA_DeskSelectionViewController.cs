using UnityEngine;
using UnityEngine.UI;

public class SPA_DeskSelectionViewController : SPA_MB_Singleton<SPA_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (SPA_MB_Singleton<SPA_DeskSelectionViewController>._instance == null)
		{
			SPA_MB_Singleton<SPA_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(SPA_MB_Singleton<SPA_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(SPA_MB_Singleton<SPA_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		SPA_Utils.TrySetActive(_goUIContainer, active: true);
		SPA_MB_Singleton<SPA_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		SPA_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
