using UIFrameWork;

public class FK3_NoDeskMgr : FK3_BaseUIForm
{
	private void Awake()
	{
		uiType.uiFormType = FK3_UIFormTypes.Popup;
	}

	public void CloseUI()
	{
		FK3_UIRoomManager.GetInstance().CloseUI("NoDeskImage");
	}
}
