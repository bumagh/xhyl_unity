using UnityEngine;
using UnityEngine.UI;

public class SHT_DeskSelectionViewController : SHT_MB_Singleton<SHT_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (SHT_MB_Singleton<SHT_DeskSelectionViewController>._instance == null)
		{
			SHT_MB_Singleton<SHT_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(SHT_MB_Singleton<SHT_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(SHT_MB_Singleton<SHT_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		SHT_Utils.TrySetActive(_goUIContainer, active: true);
		SHT_MB_Singleton<SHT_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		SHT_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
