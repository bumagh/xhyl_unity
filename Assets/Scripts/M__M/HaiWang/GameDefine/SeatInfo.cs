namespace M__M.HaiWang.GameDefine
{
	public class SeatInfo
	{
		public int id;

		public string username;

		public int iconId;

		public bool isUsed;

		public int level;

		public int sex;

		public SeatInfo(int id, bool isUsed = false, string name = "", int icon = 0, int level = 0, int sex = -1)
		{
			this.id = id;
			this.isUsed = isUsed;
			username = name;
			iconId = icon;
			this.level = level;
			this.sex = sex;
		}
	}
}
