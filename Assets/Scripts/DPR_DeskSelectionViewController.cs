using UnityEngine;
using UnityEngine.UI;

public class DPR_DeskSelectionViewController : DPR_MB_Singleton<DPR_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (DPR_MB_Singleton<DPR_DeskSelectionViewController>._instance == null)
		{
			DPR_MB_Singleton<DPR_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(DPR_MB_Singleton<DPR_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(DPR_MB_Singleton<DPR_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		DPR_Utils.TrySetActive(_goUIContainer, active: true);
		DPR_MB_Singleton<DPR_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		DPR_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
