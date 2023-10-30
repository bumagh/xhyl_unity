namespace M__M.HaiWang.Fish
{
	public class DeadFishData
	{
		public FishBehaviour fish;

		public int score;

		public int bulletPower;

		public int fishRate;

		public DeadFishData(FishBehaviour fish, int bulletPower, int fishRate, int score = -1)
		{
			this.fish = fish;
			this.bulletPower = bulletPower;
			this.fishRate = fishRate;
			this.score = score;
		}
	}
}
