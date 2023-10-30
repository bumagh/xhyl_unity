using UnityEngine;
using UnityEngine.UI;

public class MSE_RoomSelectionViewController : MSE_MB_Singleton<MSE_RoomSelectionViewController>
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
			MSE_MB_Singleton<MSE_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			MSE_MB_Singleton<MSE_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (MSE_MB_Singleton<MSE_RoomSelectionViewController>._instance == null)
		{
			MSE_MB_Singleton<MSE_RoomSelectionViewController>.SetInstance(this);
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
		MSE_Utils.TrySetActive(_goUIContainer, active: true);
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		MSE_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
