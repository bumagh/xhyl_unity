using FullInspector;

public class fiSimpleSingletonBehaviour<T> : BaseBehavior<FullSerializerSerializer> where T : BaseBehavior<FullSerializerSerializer>
{
	protected static T s_instance;

	public static T Instance => s_instance;

	public static T Get()
	{
		return s_instance;
	}

	public static T GetInstance()
	{
		return s_instance;
	}

	protected override void Awake()
	{
		base.Awake();
		s_instance = (this as T);
	}
}
