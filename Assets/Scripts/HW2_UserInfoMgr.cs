using UIFrameWork;
using UnityEngine;

public class HW2_UserInfoMgr : BaseUIForm
{
	private void Awake()
	{
		uiType.uiFormType = UIFormTypes.Normal;
	}

	private void OnEnable()
	{
		RotateControl();
	}

	private void Start()
	{
	}

	private void RotateControl()
	{
		base.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
	}

	private void Update()
	{
	}
}
