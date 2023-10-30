using UnityEngine;
using UnityEngine.UI;

public class PTM_DeskSelectionViewController : PTM_MB_Singleton<PTM_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (PTM_MB_Singleton<PTM_DeskSelectionViewController>._instance == null)
		{
			PTM_MB_Singleton<PTM_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(PTM_MB_Singleton<PTM_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(PTM_MB_Singleton<PTM_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		PTM_Utils.TrySetActive(_goUIContainer, active: true);
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		PTM_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
