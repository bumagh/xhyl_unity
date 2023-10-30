using UIFrameWork;
using UnityEngine;

public class HeadTitleMgr : BaseUIForm
{
	[SerializeField]
	private GameObject headTitle;

	private static HeadTitleMgr instance;

	public static HeadTitleMgr Get()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
		uiType.uiFormType = UIFormTypes.Normal;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
