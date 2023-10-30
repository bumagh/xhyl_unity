using UnityEngine;

namespace M__M.HaiWang.BeyondScene
{
	public class FK3_MainCamera : MonoBehaviour
	{
		private static FK3_MainCamera s_instance;

		public static FK3_MainCamera Get()
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
