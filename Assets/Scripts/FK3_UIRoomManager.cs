using UIFrameWork;

public class FK3_UIRoomManager : FK3_UIManager
{
	private static FK3_UIManager instance;

	public static FK3_UIManager GetInstance()
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
