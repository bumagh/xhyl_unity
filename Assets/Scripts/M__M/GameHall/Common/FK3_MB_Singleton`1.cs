using UnityEngine;

namespace M__M.GameHall.Common
{
	public class FK3_MB_Singleton<T> : MonoBehaviour where T : FK3_MB_Singleton<T>
	{
		protected static T _instance;

		public static T GetInstance()
		{
			return _instance;
		}

		public static T Get()
		{
			return _instance;
		}

		public static T Quit()
		{
			_instance = (T)null;
			return _instance;
		}

		public static void SetInstance(T t, bool force = false)
		{
			if ((Object)_instance == (Object)null || force)
			{
				_instance = t;
			}
		}

		public static T SetInstanceByGameObject(GameObject go, bool force = false)
		{
			if ((Object)_instance == (Object)null || force)
			{
				_instance = go.GetComponent<T>();
			}
			return _instance;
		}

		public virtual void InitSingleton()
		{
		}

		public virtual void Release()
		{
		}

		private void OnApplicationQuit()
		{
			_instance = (T)null;
		}
	}
}
