using UnityEngine;
using UnityEngine.UI;

public class USW_DeskSelectionViewController : USW_MB_Singleton<USW_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (USW_MB_Singleton<USW_DeskSelectionViewController>._instance == null)
		{
			USW_MB_Singleton<USW_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(USW_MB_Singleton<USW_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(USW_MB_Singleton<USW_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		USW_Utils.TrySetActive(_goUIContainer, active: true);
		USW_MB_Singleton<USW_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		USW_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
