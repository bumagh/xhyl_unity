using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	protected static T sInstance;

	protected static bool IsCreate;

	public static bool s_debugDestroy;

	public static T Instance
	{
		get
		{
			if (s_debugDestroy)
			{
				return (T)null;
			}
			CreateInstance();
			return sInstance;
		}
	}

	protected virtual void Awake()
	{
		if ((Object)sInstance == (Object)null)
		{
			sInstance = (this as T);
			IsCreate = true;
			Init();
		}
	}

	protected virtual void Init()
	{
	}

	protected virtual void OnDestroy()
	{
		sInstance = (T)null;
		IsCreate = false;
	}

	private void OnApplicationQuit()
	{
		sInstance = (T)null;
		IsCreate = false;
	}

	public static void CreateInstance()
	{
		if (IsCreate)
		{
			return;
		}
		IsCreate = true;
		T[] array = UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
		if (array.Length != 0)
		{
			if (array.Length == 1)
			{
				sInstance = array[0];
				sInstance.gameObject.name = typeof(T).Name;
				Object.DontDestroyOnLoad(sInstance.gameObject);
				return;
			}
			T[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				T val = array2[i];
				UnityEngine.Object.Destroy(val.gameObject);
			}
		}
		GameObject gameObject = new GameObject(typeof(T).Name, typeof(T));
		sInstance = gameObject.GetComponent<T>();
		Object.DontDestroyOnLoad(sInstance.gameObject);
	}

	public static void ReleaseInstance()
	{
		if ((Object)sInstance != (Object)null)
		{
			UnityEngine.Object.Destroy(sInstance.gameObject);
			sInstance = (T)null;
			IsCreate = false;
		}
	}
}
