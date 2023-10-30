namespace M__M.HaiWang.Fish
{
	public class FK3_DeadFishData
	{
		public FK3_FishBehaviour fish;

		public int score;

		public int bulletPower;

		public int fishRate;

		public FK3_DeadFishData(FK3_FishBehaviour fish, int bulletPower, int fishRate, int score = -1)
		{
			this.fish = fish;
			this.bulletPower = bulletPower;
			this.fishRate = fishRate;
			this.score = score;
		}
	}
}
