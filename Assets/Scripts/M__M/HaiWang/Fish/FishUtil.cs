using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public static class FishUtil
	{
		public static int GetFishRateWeight(FishType fishType)
		{
			int result = 0;
			switch (fishType)
			{
			case FishType.Gurnard_迦魶鱼:
				result = 2;
				break;
			case FishType.Clown_小丑鱼:
				result = 3;
				break;
			case FishType.Rasbora_鲽鱼:
				result = 4;
				break;
			case FishType.Puffer_河豚:
				result = 5;
				break;
			case FishType.Grouper_狮子鱼:
				result = 6;
				break;
			case FishType.Flounder_比目鱼:
				result = 7;
				break;
			case FishType.Lobster_龙虾:
				result = 8;
				break;
			case FishType.Swordfish_旗鱼:
				result = 9;
				break;
			case FishType.Octopus_章鱼:
				result = 10;
				break;
			case FishType.Lantern_灯笼鱼:
				result = 12;
				break;
			case FishType.Tortoise_海龟:
				result = 15;
				break;
			case FishType.Sawfish_锯齿鲨:
				result = 18;
				break;
			case FishType.Mobula_蝠魟:
				result = 20;
				break;
			case FishType.GoldShark_霸王鲸:
				result = 100;
				break;
			case FishType.Shark_鲨鱼:
				result = 60;
				break;
			case FishType.KillerWhale_杀人鲸:
				result = 100;
				break;
			case FishType.Big_Clown_巨型小丑鱼:
				result = 30;
				break;
			case FishType.Big_Rasbora_巨型鲽鱼:
				result = 30;
				break;
			case FishType.Big_Puffer_巨型河豚:
				result = 30;
				break;
			case FishType.Boss_Dorgan_狂暴火龙:
				result = 250;
				break;
			case FishType.Boss_Crab_霸王蟹:
				result = 500;
				break;
			case FishType.Boss_Kraken_深海八爪鱼:
				result = 500;
				break;
			case FishType.Boss_Lantern_暗夜炬兽:
				result = 500;
				break;
			case FishType.CrabLaser_电磁蟹:
				result = 200;
				break;
			case FishType.CrabBoom_连环炸弹蟹:
				result = 200;
				break;
			case FishType.Lightning_Gurnard_闪电迦魶鱼:
				result = 2;
				break;
			case FishType.Lightning_Clown_闪电小丑鱼:
				result = 3;
				break;
			case FishType.Lightning_Rasbora_闪电鲽鱼:
				result = 4;
				break;
			case FishType.Lightning_Puffer_闪电河豚:
				result = 5;
				break;
			case FishType.Lightning_Grouper_闪电狮子鱼:
				result = 6;
				break;
			case FishType.Lightning_Flounder_闪电比目鱼:
				result = 7;
				break;
			case FishType.Lightning_Lobster_闪电龙虾:
				result = 8;
				break;
			case FishType.Lightning_Swordfish_闪电旗鱼:
				result = 9;
				break;
			case FishType.Lightning_Octopus_闪电章鱼:
				result = 10;
				break;
			default:
				UnityEngine.Debug.LogError($"GetFishRate: unhandle fishtype:[{fishType}]");
				break;
			}
			return result;
		}

		public static int GetFishRateWeight(this FishBehaviour fish)
		{
			return GetFishRateWeight(fish.type);
		}
	}
}
