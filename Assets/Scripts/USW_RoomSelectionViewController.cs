using UnityEngine;
using UnityEngine.UI;

public class USW_RoomSelectionViewController : USW_MB_Singleton<USW_RoomSelectionViewController>
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
			USW_MB_Singleton<USW_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			USW_MB_Singleton<USW_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (USW_MB_Singleton<USW_RoomSelectionViewController>._instance == null)
		{
			USW_MB_Singleton<USW_RoomSelectionViewController>.SetInstance(this);
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
		USW_Utils.TrySetActive(_goUIContainer, active: true);
		USW_MB_Singleton<USW_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		USW_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
