namespace M__M.HaiWang.Fish
{
	public static class FK3_FishExtends
	{
		public static FK3_FishType BasicType(this FK3_FishType fishType)
		{
			switch (fishType)
			{
			case FK3_FishType.Lightning_Gurnard_闪电迦魶鱼:
				return FK3_FishType.Gurnard_迦魶鱼;
			case FK3_FishType.Lightning_Clown_闪电小丑鱼:
				return FK3_FishType.Clown_小丑鱼;
			case FK3_FishType.Lightning_Rasbora_闪电鲽鱼:
				return FK3_FishType.Rasbora_鲽鱼;
			case FK3_FishType.Lightning_Puffer_闪电河豚:
				return FK3_FishType.Puffer_河豚;
			case FK3_FishType.Lightning_Grouper_闪电狮子鱼:
				return FK3_FishType.Grouper_狮子鱼;
			case FK3_FishType.Lightning_Flounder_闪电比目鱼:
				return FK3_FishType.Flounder_比目鱼;
			case FK3_FishType.Lightning_Lobster_闪电龙虾:
				return FK3_FishType.Lobster_龙虾;
			case FK3_FishType.Lightning_Swordfish_闪电旗鱼:
				return FK3_FishType.Swordfish_旗鱼;
			case FK3_FishType.Lightning_Octopus_闪电章鱼:
				return FK3_FishType.Octopus_章鱼;
			default:
				return fishType;
			}
		}
	}
}
