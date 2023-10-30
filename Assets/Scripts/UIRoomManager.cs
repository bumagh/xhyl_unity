using UIFrameWork;

public class UIRoomManager : UIManager
{
	private static UIManager instance;

	public static UIManager GetInstance()
	{
		return instance;
	}

	public static void OnQuit()
	{
		instance = null;
	}

	public override void Awake()
	{
		base.Awake();
		instance = this;
	}
}
