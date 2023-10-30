using UnityEngine;
using UnityEngine.UI;

public class LRS_DeskSelectionViewController : LRS_MB_Singleton<LRS_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (LRS_MB_Singleton<LRS_DeskSelectionViewController>._instance == null)
		{
			LRS_MB_Singleton<LRS_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		LRS_Utils.TrySetActive(_goUIContainer, active: true);
		LRS_MB_Singleton<LRS_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		LRS_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
