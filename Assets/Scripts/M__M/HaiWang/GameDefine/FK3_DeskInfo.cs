namespace M__M.HaiWang.GameDefine
{
	public class FK3_DeskInfo
	{
		public int id;

		public string name;

		public int minGold;

		public int minGunValue;

		public int maxGunValue;

		public int addstepGunValue;

		public int exchange;

		public int onceExchangeValue;

		public FK3_SeatInfo[] seats = new FK3_SeatInfo[4];

		public string display => string.Format(string.Empty);

		public FK3_DeskInfo(string name = "", int id = 0)
		{
			this.name = name;
			this.id = id;
			for (int i = 0; i < 4; i++)
			{
				seats[i] = new FK3_SeatInfo(i + 1, isUsed: false, string.Empty);
			}
		}

		public FK3_SeatInfo GetSeat(int id)
		{
			for (int i = 0; i < seats.Length; i++)
			{
				if (id == seats[i].id)
				{
					return seats[i];
				}
			}
			return null;
		}

		public int GetSeatUseCount()
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				if (seats[i].isUsed)
				{
					num++;
				}
			}
			return num;
		}
	}
}
