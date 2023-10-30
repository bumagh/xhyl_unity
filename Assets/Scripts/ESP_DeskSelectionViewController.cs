using UnityEngine;
using UnityEngine.UI;

public class ESP_DeskSelectionViewController : ESP_MB_Singleton<ESP_DeskSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnLeft;

	private Button btnRight;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnLeft = base.transform.Find("DeskInfo/Arrows/BtnLeft").GetComponent<Button>();
		btnRight = base.transform.Find("DeskInfo/Arrows/BtnRight").GetComponent<Button>();
		if (ESP_MB_Singleton<ESP_DeskSelectionViewController>._instance == null)
		{
			ESP_MB_Singleton<ESP_DeskSelectionViewController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		btnLeft.onClick.AddListener(ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().OnBtnLeftDesk_Click);
		btnRight.onClick.AddListener(ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().OnBtnRightDesk_Click);
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
		ESP_Utils.TrySetActive(_goUIContainer, active: true);
		ESP_MB_Singleton<ESP_GameManager>.GetInstance().ChangeView("DeskSelectionView");
	}

	public void Hide()
	{
		ESP_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
