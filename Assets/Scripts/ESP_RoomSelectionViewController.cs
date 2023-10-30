using UnityEngine;
using UnityEngine.UI;

public class ESP_RoomSelectionViewController : ESP_MB_Singleton<ESP_RoomSelectionViewController>
{
	private GameObject _goUIContainer;

	private Button btnRoom0;

	private Button btnRoom1;

	private void Awake()
	{
		_goUIContainer = base.gameObject;
		btnRoom0 = base.transform.Find("BtnRoom0").GetComponent<Button>();
		btnRoom1 = base.transform.Find("BtnRoom1").GetComponent<Button>();
		btnRoom0.onClick.AddListener(delegate
		{
			ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (ESP_MB_Singleton<ESP_RoomSelectionViewController>._instance == null)
		{
			ESP_MB_Singleton<ESP_RoomSelectionViewController>.SetInstance(this);
			PreInit();
		}
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
		ESP_MB_Singleton<ESP_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		ESP_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
