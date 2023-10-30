using M__M.HaiWang.Fish;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishSpawn
{
	[CreateAssetMenu(menuName = "FishSpawn/FishSpawnConfig", fileName = "FishSpawnConfigx")]
	public class FishSpawnConfig : ScriptableObject
	{
		public float defaultFishSpeed = 2f;

		public static float staticDefaultFishSpeed = 2f;

		public List<FishTypeSpeedItem> constSpeeds;

		public float GetConstSpeed(FishType fishType)
		{
			foreach (FishTypeSpeedItem constSpeed in constSpeeds)
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
