using UIFrameWork;
using UnityEngine;

public class FK3_HeadTitleMgr : FK3_BaseUIForm
{
	[SerializeField]
	private GameObject headTitle;

	private static FK3_HeadTitleMgr instance;

	public static FK3_HeadTitleMgr Get()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
		uiType.uiFormType = FK3_UIFormTypes.Normal;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
