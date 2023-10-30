using UnityEngine;
using UnityEngine.UI;

public class LRS_RoomSelectionViewController : LRS_MB_Singleton<LRS_RoomSelectionViewController>
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
			LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (LRS_MB_Singleton<LRS_RoomSelectionViewController>._instance == null)
		{
			LRS_MB_Singleton<LRS_RoomSelectionViewController>.SetInstance(this);
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
		LRS_Utils.TrySetActive(_goUIContainer, active: true);
		LRS_MB_Singleton<LRS_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		LRS_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
