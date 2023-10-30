namespace M__M.HaiWang.Fish
{
	public static class FishExtends
	{
		public static FishType BasicType(this FishType fishType)
		{
			switch (fishType)
			{
			case FishType.Lightning_Gurnard_闪电迦魶鱼:
				return FishType.Gurnard_迦魶鱼;
			case FishType.Lightning_Clown_闪电小丑鱼:
				return FishType.Clown_小丑鱼;
			case FishType.Lightning_Rasbora_闪电鲽鱼:
				return FishType.Rasbora_鲽鱼;
			case FishType.Lightning_Puffer_闪电河豚:
				return FishType.Puffer_河豚;
			case FishType.Lightning_Grouper_闪电狮子鱼:
				return FishType.Grouper_狮子鱼;
			case FishType.Lightning_Flounder_闪电比目鱼:
				return FishType.Flounder_比目鱼;
			case FishType.Lightning_Lobster_闪电龙虾:
				return FishType.Lobster_龙虾;
			case FishType.Lightning_Swordfish_闪电旗鱼:
				return FishType.Swordfish_旗鱼;
			case FishType.Lightning_Octopus_闪电章鱼:
				return FishType.Octopus_章鱼;
			default:
				return fishType;
			}
		}
	}
}
