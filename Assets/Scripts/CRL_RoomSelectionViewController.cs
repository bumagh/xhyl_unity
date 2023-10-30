using UnityEngine;
using UnityEngine.UI;

public class CRL_RoomSelectionViewController : CRL_MB_Singleton<CRL_RoomSelectionViewController>
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
			CRL_MB_Singleton<CRL_LobbyViewController>.GetInstance().OnBtnRoom_Click(1);
		});
		btnRoom1.onClick.AddListener(delegate
		{
			CRL_MB_Singleton<CRL_LobbyViewController>.GetInstance().OnBtnRoom_Click(2);
		});
		if (CRL_MB_Singleton<CRL_RoomSelectionViewController>._instance == null)
		{
			CRL_MB_Singleton<CRL_RoomSelectionViewController>.SetInstance(this);
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
		CRL_Utils.TrySetActive(_goUIContainer, active: true);
		CRL_MB_Singleton<CRL_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	public void Hide()
	{
		CRL_Utils.TrySetActive(_goUIContainer, active: false);
	}
}
