using UnityEngine;

namespace M__M.HaiWang
{
	public class FishEnvManager : MonoBehaviour
	{
		private static FishEnvManager s_instance;

		public bool setWhiteOnStart;

		public static FishEnvManager Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
		}

		private void Start()
		{
			if (setWhiteOnStart)
			{
				SetWhite();
			}
		}

		public void SetWhite()
		{
			RenderSettings.ambientLight = Color.white;
		}
	}
}
