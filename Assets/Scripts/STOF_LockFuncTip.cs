using UnityEngine;
using UnityEngine.UI;

public class STOF_LockFuncTip : MonoBehaviour
{
	private Image imgLock;

	[HideInInspector]
	public Button btnClose;

	[HideInInspector]
	public Toggle togTip;

	[SerializeField]
	private Sprite[] spiLock;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
	}

	private void OnEnable()
	{
		if (PlayerPrefs.GetInt("Profile") == 1)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void Start()
	{
		imgLock = base.transform.Find("ImgLock").GetComponent<Image>();
		btnClose = base.transform.Find("BtnClose").GetComponent<Button>();
		togTip = base.transform.Find("Toggle").GetComponent<Toggle>();
		togTip.isOn = false;
		imgLock.sprite = spiLock[STOF_GameInfo.getInstance().Language];
		if (!PlayerPrefs.HasKey("profile"))
		{
			PlayerPrefs.SetInt("profile", 0);
		}
	}

	public void CloseLockTip()
	{
		int value = togTip.isOn ? 1 : 0;
		PlayerPrefs.SetInt("Profile", value);
		base.gameObject.SetActive(value: false);
	}
}
