using UnityEngine;

namespace BCBM_GameCommon
{
	internal class BCBM_GameTool
	{
		public static float GetDeltaTime()
		{
			float num = Time.deltaTime;
			if (num > 1f / BCBM_Parameter.G_DestFPS)
			{
				num = 1f / BCBM_Parameter.G_DestFPS;
			}
			return num;
		}
	}
}
