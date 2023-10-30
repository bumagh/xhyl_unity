using UnityEngine;
using UnityEngine.UI;

public class PTM_RoomSelectionViewController : PTM_MB_Singleton<PTM_RoomSelectionViewController>
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
			PTM_MB_Singleton<PTM_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			PTM_MB_Singleton<PTM_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (PTM_MB_Singleton<PTM_RoomSelectionViewController>._instance == null)
		{
			PTM_MB_Singleton<PTM_RoomSelectionViewController>.SetInstance(this);
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
		PTM_Utils.TrySetActive(_goUIContainer, active: true);
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		PTM_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
