public class HW2_Singleton<T> where T : class, new()
{
	private static object _syncobj = new object();

	public static volatile T _Instance = (T)null;

	protected HW2_Singleton()
	{
		_onInit();
	}

	public static T Get()
	{
		if (_Instance == null)
		{
			object syncobj = _syncobj;
			lock (syncobj)
			{
				if (_Instance == null)
				{
					_Instance = new T();
				}
			}
		}
		return _Instance;
	}

	protected virtual void _onInit()
	{
	}

	public static void OnApplicationQuit()
	{
		_Instance = (T)null;
	}
}
