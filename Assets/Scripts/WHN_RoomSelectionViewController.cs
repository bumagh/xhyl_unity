using UnityEngine;
using UnityEngine.UI;

public class WHN_RoomSelectionViewController : WHN_MB_Singleton<WHN_RoomSelectionViewController>
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
			WHN_MB_Singleton<WHN_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			WHN_MB_Singleton<WHN_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (WHN_MB_Singleton<WHN_RoomSelectionViewController>._instance == null)
		{
			WHN_MB_Singleton<WHN_RoomSelectionViewController>.SetInstance(this);
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
		WHN_Utils.TrySetActive(_goUIContainer, active: true);
		WHN_MB_Singleton<WHN_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		WHN_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
