using UIFrameWork;

public class UIIngameManager : UIManager
{
	private static UIManager instance;

	public static UIManager GetInstance()
	{
		return instance;
	}

	public override void Awake()
	{
		base.Awake();
		instance = this;
	}
}
