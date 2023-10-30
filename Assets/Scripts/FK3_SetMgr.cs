using UIFrameWork;

public class FK3_SetMgr : FK3_BaseUIForm
{
	private static FK3_SetMgr instance;

	public static FK3_SetMgr Get()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
		uiType.uiFormType = FK3_UIFormTypes.Normal;
	}
}
