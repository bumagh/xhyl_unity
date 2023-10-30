using M__M.HaiWang.Fish;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	[Serializable]
	internal class FK3_Fish_Selector_Context
	{
		public Transform curFish;

		public List<FK3_FishType> fishes;

		public FK3_XSelector<FK3_FishType> selector;

		public FK3_Fish_Selector_Context()
		{
			fishes = new List<FK3_FishType>
			{
				FK3_FishType.Gurnard_迦魶鱼,
				FK3_FishType.Clown_小丑鱼,
				FK3_FishType.Rasbora_鲽鱼,
				FK3_FishType.Puffer_河豚,
				FK3_FishType.Grouper_狮子鱼,
				FK3_FishType.Flounder_比目鱼,
				FK3_FishType.Lobster_龙虾,
				FK3_FishType.Swordfish_旗鱼,
				FK3_FishType.Octopus_章鱼,
				FK3_FishType.Lantern_灯笼鱼,
				FK3_FishType.Tortoise_海龟,
				FK3_FishType.Sawfish_锯齿鲨,
				FK3_FishType.Mobula_蝠魟,
				FK3_FishType.GoldShark_霸王鲸,
				FK3_FishType.Shark_鲨鱼,
				FK3_FishType.KillerWhale_杀人鲸,
				FK3_FishType.Big_Clown_巨型小丑鱼,
				FK3_FishType.Big_Rasbora_巨型鲽鱼,
				FK3_FishType.Big_Puffer_巨型河豚,
				FK3_FishType.Boss_Dorgan_狂暴火龙,
				FK3_FishType.Boss_Kraken_深海八爪鱼,
				FK3_FishType.Boss_Crab_霸王蟹,
				FK3_FishType.Boss_Crocodil_史前巨鳄,
				FK3_FishType.Boss_Lantern_暗夜炬兽,
				FK3_FishType.CrabLaser_电磁蟹,
				FK3_FishType.CrabBoom_连环炸弹蟹,
				FK3_FishType.Lightning_Gurnard_闪电迦魶鱼,
				FK3_FishType.Lightning_Clown_闪电小丑鱼,
				FK3_FishType.Lightning_Rasbora_闪电鲽鱼,
				FK3_FishType.Lightning_Puffer_闪电河豚,
				FK3_FishType.Lightning_Grouper_闪电狮子鱼,
				FK3_FishType.Lightning_Flounder_闪电比目鱼,
				FK3_FishType.Lightning_Lobster_闪电龙虾,
				FK3_FishType.Lightning_Swordfish_闪电旗鱼,
				FK3_FishType.Lightning_Octopus_闪电章鱼
			};
			selector = new FK3_XSelector<FK3_FishType>(fishes);
		}
	}
}
