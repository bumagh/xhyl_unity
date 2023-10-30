namespace M__M.HaiWang.Fish
{
	public static class FK3_FishUtil
	{
		public static int GetFishRateWeight(FK3_FishType fishType)
		{
			int num = 0;
			switch (fishType)
			{
			case FK3_FishType.Gurnard_迦魶鱼:
				return 2;
			case FK3_FishType.Clown_小丑鱼:
				return 3;
			case FK3_FishType.Rasbora_鲽鱼:
				return 4;
			case FK3_FishType.Puffer_河豚:
				return 5;
			case FK3_FishType.Grouper_狮子鱼:
				return 6;
			case FK3_FishType.Flounder_比目鱼:
				return 7;
			case FK3_FishType.Lobster_龙虾:
				return 8;
			case FK3_FishType.Swordfish_旗鱼:
				return 9;
			case FK3_FishType.Octopus_章鱼:
				return 10;
			case FK3_FishType.Lantern_灯笼鱼:
				return 12;
			case FK3_FishType.Tortoise_海龟:
				return 15;
			case FK3_FishType.Sawfish_锯齿鲨:
				return 18;
			case FK3_FishType.Mobula_蝠魟:
				return 20;
			case FK3_FishType.GoldShark_霸王鲸:
				return 100;
			case FK3_FishType.Shark_鲨鱼:
				return 60;
			case FK3_FishType.KillerWhale_杀人鲸:
				return 100;
			case FK3_FishType.Big_Clown_巨型小丑鱼:
				return 30;
			case FK3_FishType.Big_Rasbora_巨型鲽鱼:
				return 30;
			case FK3_FishType.Big_Puffer_巨型河豚:
				return 30;
			case FK3_FishType.Boss_Dorgan_狂暴火龙:
				return 250;
			case FK3_FishType.Boss_Dorgan_冰封暴龙:
				return 250;
			case FK3_FishType.Boss_Crab_霸王蟹:
				return 500;
			case FK3_FishType.Boss_Kraken_深海八爪鱼:
				return 500;
			case FK3_FishType.Boss_Lantern_暗夜炬兽:
				return 500;
			case FK3_FishType.CrabLaser_电磁蟹:
			case FK3_FishType.CrabBoom_连环炸弹蟹:
			case FK3_FishType.CrabDrill_钻头蟹:
			case FK3_FishType.CrabStorm_暴风蟹:
				return 200;
			case FK3_FishType.Lightning_Gurnard_闪电迦魶鱼:
				return 2;
			case FK3_FishType.Lightning_Clown_闪电小丑鱼:
				return 3;
			case FK3_FishType.Lightning_Rasbora_闪电鲽鱼:
				return 4;
			case FK3_FishType.Lightning_Puffer_闪电河豚:
				return 5;
			case FK3_FishType.Lightning_Grouper_闪电狮子鱼:
				return 6;
			case FK3_FishType.Lightning_Flounder_闪电比目鱼:
				return 7;
			case FK3_FishType.Lightning_Lobster_闪电龙虾:
				return 8;
			case FK3_FishType.Lightning_Swordfish_闪电旗鱼:
				return 9;
			case FK3_FishType.Lightning_Octopus_闪电章鱼:
				return 10;
			default:
				return 10;
			}
		}

		public static int GetFishRateWeight(this FK3_FishBehaviour fish)
		{
			return GetFishRateWeight(fish.type);
		}
	}
}
