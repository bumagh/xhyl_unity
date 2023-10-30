using UIFrameWork;

public class OpScoreMgr : BaseUIForm
{
	private static OpScoreMgr instance;

	public static OpScoreMgr Get()
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
