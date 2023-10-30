using UnityEngine;
using UnityEngine.UI;

public class PHG_DeskSelectionViewController : PHG_MB_Singleton<PHG_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (PHG_MB_Singleton<PHG_DeskSelectionViewController>._instance == null)
		{
			PHG_MB_Singleton<PHG_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(PHG_MB_Singleton<PHG_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(PHG_MB_Singleton<PHG_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		PHG_Utils.TrySetActive(_goUIContainer, active: true);
		PHG_MB_Singleton<PHG_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		PHG_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
