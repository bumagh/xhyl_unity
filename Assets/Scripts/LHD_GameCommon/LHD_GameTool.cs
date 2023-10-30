using UnityEngine;

namespace LHD_GameCommon
{
	internal class LHD_GameTool
	{
		public static float GetDeltaTime()
		{
			float num = Time.deltaTime;
			if (num > 1f / LHD_Parameter.G_DestFPS)
			{
				num = 1f / LHD_Parameter.G_DestFPS;
			}
			return num;
		}
	}
}
