using UnityEngine;
using UnityEngine.UI;

public class CRL_DeskSelectionViewController : CRL_MB_Singleton<CRL_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (CRL_MB_Singleton<CRL_DeskSelectionViewController>._instance == null)
		{
			CRL_MB_Singleton<CRL_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(CRL_MB_Singleton<CRL_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(CRL_MB_Singleton<CRL_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		CRL_Utils.TrySetActive(_goUIContainer, active: true);
		CRL_MB_Singleton<CRL_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		CRL_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
