using UIFrameWork;

public class FK3_UIIngameManager : FK3_UIManager
{
	private static FK3_UIManager instance;

	public static FK3_UIManager GetInstance()
	{
		return instance;
	}

	public override void Awake()
	{
		base.Awake();
		instance = this;
	}
}
