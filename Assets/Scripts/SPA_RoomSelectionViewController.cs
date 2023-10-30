using UnityEngine;
using UnityEngine.UI;

public class SPA_RoomSelectionViewController : SPA_MB_Singleton<SPA_RoomSelectionViewController>
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
			SPA_MB_Singleton<SPA_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			SPA_MB_Singleton<SPA_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (SPA_MB_Singleton<SPA_RoomSelectionViewController>._instance == null)
		{
			SPA_MB_Singleton<SPA_RoomSelectionViewController>.SetInstance(this);
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
		SPA_Utils.TrySetActive(_goUIContainer, active: true);
		SPA_MB_Singleton<SPA_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		SPA_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
