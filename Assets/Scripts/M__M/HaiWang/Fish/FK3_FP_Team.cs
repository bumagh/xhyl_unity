namespace M__M.HaiWang.Fish
{
	public class FK3_FP_Team : FK3_FishPathBase
	{
		public int pathId;

		public int teamId;

		public FK3_FP_Team()
		{
			type = FK3_FishPathType.Team;
		}
	}
}
