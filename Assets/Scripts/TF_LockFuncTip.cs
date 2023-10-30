using UnityEngine;
using UnityEngine.UI;

public class TF_LockFuncTip : MonoBehaviour
{
	private Image imgLock;

	[HideInInspector]
	public Button btnClose;

	[HideInInspector]
	public Toggle togTip;

	[SerializeField]
	private Sprite[] spiLock;

	private void Start()
	{
		imgLock = base.transform.Find("ImgLock").GetComponent<Image>();
		btnClose = base.transform.Find("BtnClose").GetComponent<Button>();
		togTip = base.transform.Find("Toggle").GetComponent<Toggle>();
		togTip.isOn = false;
		imgLock.sprite = spiLock[TF_GameInfo.getInstance().Language];
		if (!PlayerPrefs.HasKey("profile"))
		{
			PlayerPrefs.SetInt("profile", 0);
		}
	}

	public void CloseLockTip()
	{
		int num = (!togTip.isOn) ? 1 : 0;
		PlayerPrefs.SetInt("Profile", 0);
		base.gameObject.SetActive(value: false);
	}
}
