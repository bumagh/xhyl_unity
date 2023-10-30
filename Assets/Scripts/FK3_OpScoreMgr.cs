using UIFrameWork;
using UnityEngine;

public class FK3_OpScoreMgr : FK3_BaseUIForm
{
	private static FK3_OpScoreMgr instance;

	public static FK3_OpScoreMgr Get()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
		uiType.uiFormType = FK3_UIFormTypes.Normal;
		base.transform.localScale = Vector3.zero;
	}

	private void Update()
	{
		if (base.transform.localScale != Vector3.zero)
		{
			base.transform.localScale = Vector3.zero;
		}
		if (base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
