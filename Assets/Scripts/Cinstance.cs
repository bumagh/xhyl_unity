public class Cinstance<T> where T : new()
{
	private static T instance = default(T);

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new T();
			}
			return instance;
		}
	}

	public static void Clearself()
	{
		instance = default(T);
	}
}
