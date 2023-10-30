namespace M__M.HaiWang.GameDefine
{
	public class DeskInfo
	{
		public int id;

		public string name;

		public int minGold;

		public int minGunValue;

		public int maxGunValue;

		public int addstepGunValue;

		public int exchange;

		public int onceExchangeValue;

		public SeatInfo[] seats = new SeatInfo[4];

		public string display => string.Format(string.Empty);

		public DeskInfo(string name = "", int id = 0)
		{
			this.name = name;
			this.id = id;
			for (int i = 0; i < 4; i++)
			{
				seats[i] = new SeatInfo(i + 1, isUsed: false, string.Empty);
			}
		}

		public SeatInfo GetSeat(int id)
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
