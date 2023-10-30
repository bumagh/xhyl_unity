using UnityEngine;
using UnityEngine.UI;

public class DPR_RoomSelectionViewController : DPR_MB_Singleton<DPR_RoomSelectionViewController>
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
			DPR_MB_Singleton<DPR_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			DPR_MB_Singleton<DPR_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (DPR_MB_Singleton<DPR_RoomSelectionViewController>._instance == null)
		{
			DPR_MB_Singleton<DPR_RoomSelectionViewController>.SetInstance(this);
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
		DPR_Utils.TrySetActive(_goUIContainer, active: true);
		DPR_MB_Singleton<DPR_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		DPR_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
