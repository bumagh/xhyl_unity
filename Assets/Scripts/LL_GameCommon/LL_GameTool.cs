using UnityEngine;

namespace LL_GameCommon
{
	internal class LL_GameTool
	{
		public static float GetDeltaTime()
		{
			float num = Time.deltaTime;
			if (num > 1f / LL_Parameter.G_DestFPS)
			{
				num = 1f / LL_Parameter.G_DestFPS;
			}
			return num;
		}
	}
}
