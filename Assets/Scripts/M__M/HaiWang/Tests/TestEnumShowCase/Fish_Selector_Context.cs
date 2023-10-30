using M__M.HaiWang.Fish;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	[Serializable]
	internal class Fish_Selector_Context
	{
		public Transform curFish;

		public List<FishType> fishes;

		public XSelector<FishType> selector;

		public Fish_Selector_Context()
		{
			fishes = new List<FishType>
			{
				FishType.Gurnard_迦魶鱼,
				FishType.Clown_小丑鱼,
				FishType.Rasbora_鲽鱼,
				FishType.Puffer_河豚,
				FishType.Grouper_狮子鱼,
				FishType.Flounder_比目鱼,
				FishType.Lobster_龙虾,
				FishType.Swordfish_旗鱼,
				FishType.Octopus_章鱼,
				FishType.Lantern_灯笼鱼,
				FishType.Tortoise_海龟,
				FishType.Sawfish_锯齿鲨,
				FishType.Mobula_蝠魟,
				FishType.GoldShark_霸王鲸,
				FishType.Shark_鲨鱼,
				FishType.KillerWhale_杀人鲸,
				FishType.Big_Clown_巨型小丑鱼,
				FishType.Big_Rasbora_巨型鲽鱼,
				FishType.Big_Puffer_巨型河豚,
				FishType.Boss_Dorgan_狂暴火龙,
				FishType.Boss_Kraken_深海八爪鱼,
				FishType.Boss_Crab_霸王蟹,
				FishType.Boss_Lantern_暗夜炬兽,
				FishType.CrabLaser_电磁蟹,
				FishType.CrabBoom_连环炸弹蟹,
				FishType.Lightning_Gurnard_闪电迦魶鱼,
				FishType.Lightning_Clown_闪电小丑鱼,
				FishType.Lightning_Rasbora_闪电鲽鱼,
				FishType.Lightning_Puffer_闪电河豚,
				FishType.Lightning_Grouper_闪电狮子鱼,
				FishType.Lightning_Flounder_闪电比目鱼,
				FishType.Lightning_Lobster_闪电龙虾,
				FishType.Lightning_Swordfish_闪电旗鱼,
				FishType.Lightning_Octopus_闪电章鱼
			};
			selector = new XSelector<FishType>(fishes);
		}
	}
}
