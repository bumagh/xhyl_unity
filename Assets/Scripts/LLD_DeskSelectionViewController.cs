using UnityEngine;
using UnityEngine.UI;

public class LLD_DeskSelectionViewController : LLD_MB_Singleton<LLD_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (LLD_MB_Singleton<LLD_DeskSelectionViewController>._instance == null)
		{
			LLD_MB_Singleton<LLD_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(LLD_MB_Singleton<LLD_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(LLD_MB_Singleton<LLD_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		LLD_Utils.TrySetActive(_goUIContainer, active: true);
		LLD_MB_Singleton<LLD_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		LLD_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
