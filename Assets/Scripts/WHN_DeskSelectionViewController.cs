using UnityEngine;
using UnityEngine.UI;

public class WHN_DeskSelectionViewController : WHN_MB_Singleton<WHN_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (WHN_MB_Singleton<WHN_DeskSelectionViewController>._instance == null)
		{
			WHN_MB_Singleton<WHN_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(WHN_MB_Singleton<WHN_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(WHN_MB_Singleton<WHN_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		WHN_Utils.TrySetActive(_goUIContainer, active: true);
		WHN_MB_Singleton<WHN_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		WHN_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
