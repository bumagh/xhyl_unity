using UnityEngine;
using UnityEngine.UI;

public class LLD_RoomSelectionViewController : LLD_MB_Singleton<LLD_RoomSelectionViewController>
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
			LLD_MB_Singleton<LLD_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			LLD_MB_Singleton<LLD_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (LLD_MB_Singleton<LLD_RoomSelectionViewController>._instance == null)
		{
			LLD_MB_Singleton<LLD_RoomSelectionViewController>.SetInstance(this);
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
		LLD_Utils.TrySetActive(_goUIContainer, active: true);
		LLD_MB_Singleton<LLD_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		LLD_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
