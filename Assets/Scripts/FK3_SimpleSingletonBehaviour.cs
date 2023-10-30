using UnityEngine;

public class FK3_SimpleSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
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

	private void Awake()
	{
		s_instance = (this as T);
	}
}
