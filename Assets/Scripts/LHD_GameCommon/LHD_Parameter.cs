using UnityEngine;

namespace LHD_GameCommon
{
	internal class LHD_Parameter
	{
		public static LHD_Parameter G_Parameter = null;

		public static bool G_Test = false;

		public static string G_Version = "1.0.7";

		public static int S_SvrID = 12;

		public static float G_fGameMusicVolume = 1f;

		public static bool G_IsSoundOn = true;

		public static float G_DestFPS = 20f;

		public static string[] G_AnimalPrefabNameArray = new string[4]
		{
			"Animal/Rabbit",
			"Animal/Monkey",
			"Animal/Panda",
			"Animal/Lion"
		};

		public static int G_nAnimalNumber = 24;

		public static Vector3 G_AnimalMinScale = new Vector3(1f, 1f, 1f);

		public static Vector3 G_AnimalMaxScale = new Vector3(1.33f, 1.33f, 1.33f);

		public static Vector3 G_AnimalMaxScale2 = new Vector3(1.5f, 1.5f, 1.5f);

		public static float G_fAnimalBounceTime = 1.6f;

		public static float G_fCameraDelayTime = 2.6f;

		public static float G_fCenterAnimalHeight = 0.5f;

		public static float G_fNoTouchNoBetMaxTime = 600f;

		public static LHD_Parameter GetSingleton()
		{
			if (G_Parameter == null)
			{
				G_Parameter = new LHD_Parameter();
				return G_Parameter;
			}
			return G_Parameter;
		}

		public string GetSvrIP()
		{
			string empty = string.Empty;
			return LHD_GameInfo.getInstance().IP;
		}
	}
}
