using M__M.HaiWang.Fish;
using M__M.HaiWang.FishSpawn;
using System.Collections.Generic;
using UnityEngine;

namespace HW3L
{
	[CreateAssetMenu(menuName = "FishSpawn/FK3_FishSpawnConfig", fileName = "FishSpawnConfigx")]
	public class FK3_FishSpawnConfig : ScriptableObject
	{
		public float defaultFishSpeed = 2f;

		public static float staticDefaultFishSpeed = 2f;

		public List<FK3_FishTypeSpeedItem> constSpeeds;

		public float GetConstSpeed(FK3_FishType fishType)
		{
			foreach (FK3_FishTypeSpeedItem constSpeed in constSpeeds)
			{
				if (constSpeed != null && constSpeed.fishType == fishType)
				{
					return constSpeed.speed;
				}
			}
			return defaultFishSpeed;
		}
	}
}
