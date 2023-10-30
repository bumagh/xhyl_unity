using UIFrameWork;
using UnityEngine;

public class FK3_UserInfoMgr : FK3_BaseUIForm
{
	private void Awake()
	{
		uiType.uiFormType = FK3_UIFormTypes.Normal;
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
