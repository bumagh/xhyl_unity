using UIFrameWork;

public class SetMgr : BaseUIForm
{
	private static SetMgr instance;

	public static SetMgr Get()
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
