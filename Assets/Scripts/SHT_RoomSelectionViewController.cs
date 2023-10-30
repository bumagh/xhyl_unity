using UnityEngine;
using UnityEngine.UI;

public class SHT_RoomSelectionViewController : SHT_MB_Singleton<SHT_RoomSelectionViewController>
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
			SHT_MB_Singleton<SHT_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			SHT_MB_Singleton<SHT_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (SHT_MB_Singleton<SHT_RoomSelectionViewController>._instance == null)
		{
			SHT_MB_Singleton<SHT_RoomSelectionViewController>.SetInstance(this);
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
		SHT_Utils.TrySetActive(_goUIContainer, active: true);
		SHT_MB_Singleton<SHT_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		SHT_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
