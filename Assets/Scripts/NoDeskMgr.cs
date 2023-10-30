using UIFrameWork;

public class NoDeskMgr : BaseUIForm
{
	private void Awake()
	{
		uiType.uiFormType = UIFormTypes.Popup;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void CloseUI()
	{
		UIRoomManager.GetInstance().CloseUI("NoDeskImage");
	}
}
