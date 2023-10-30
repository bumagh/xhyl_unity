public class Singleton<T> where T : new()
{
	protected static T sInstance;

	protected static bool IsCreate;

	public static T Instance
	{
		get
		{
			if (!IsCreate)
			{
				CreateInstance();
			}
			return sInstance;
		}
	}

	public static void CreateInstance()
	{
		if (!IsCreate)
		{
			IsCreate = true;
			sInstance = new T();
		}
	}

	public static void ReleaseInstance()
	{
		sInstance = default(T);
		IsCreate = false;
	}
}
