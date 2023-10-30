using UnityEngine;

namespace M__M.HaiWang.BeyondScene
{
	public class MainCamera : MonoBehaviour
	{
		private static MainCamera s_instance;

		public static MainCamera Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			if (!_CheckSingleton())
			{
			}
		}

		private bool _CheckSingleton()
		{
			bool result = true;
			if (s_instance == null)
			{
				s_instance = this;
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(base.gameObject);
			}
			return result;
		}
	}
}
