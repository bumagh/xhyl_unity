using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T inst;

	public static T Instance
	{
		get
		{
			if ((Object)inst == (Object)null)
			{
				inst = UnityEngine.Object.FindObjectOfType<T>();
			}
			return inst;
		}
	}

	private void Awake()
	{
		inst = (this as T);
		Init();
	}

	protected virtual void Init()
	{
	}
}
