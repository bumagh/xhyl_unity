using UnityEngine;
using UnityEngine.UI;

public class PHG_RoomSelectionViewController : PHG_MB_Singleton<PHG_RoomSelectionViewController>
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
			PHG_MB_Singleton<PHG_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			PHG_MB_Singleton<PHG_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (PHG_MB_Singleton<PHG_RoomSelectionViewController>._instance == null)
		{
			PHG_MB_Singleton<PHG_RoomSelectionViewController>.SetInstance(this);
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
		PHG_Utils.TrySetActive(_goUIContainer, active: true);
		PHG_MB_Singleton<PHG_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		PHG_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
