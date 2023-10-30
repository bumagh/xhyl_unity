namespace M__M.HaiWang.GameDefine
{
	public class FK3_HW2_UserInfo
	{
		public int id;

		public string username;

		public string nickname;

		public string sex;

		public int level;

		public int gameGold;

		public int expeGold;

		public int photoId;

		public int overflow;

		public int usertype;

		public string promoterName;

		public int gameScore;

		public int shutupStatus;

		public void CopyTo(FK3_HW2_UserInfo user)
		{
			user.id = id;
			user.username = username;
			user.nickname = nickname;
			user.sex = sex;
			user.level = level;
			user.gameGold = gameGold;
			user.expeGold = expeGold;
			user.photoId = photoId;
			user.overflow = overflow;
			user.usertype = usertype;
			user.promoterName = promoterName;
			user.gameScore = gameScore;
			user.shutupStatus = shutupStatus;
		}
	}
}
