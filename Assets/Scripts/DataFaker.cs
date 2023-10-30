using System.Collections.Generic;

public static class DataFaker
{
	public static void MakerFakeUser()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("id", 12345);
		dictionary.Add("username", "FakeUser");
		dictionary.Add("nickname", "FakeUser nick");
		dictionary.Add("password", "FakeUser nick");
		dictionary.Add("sex", "shemale");
		dictionary.Add("level", 10);
		dictionary.Add("gameGold", 19999);
		dictionary.Add("expeGold", 999);
		dictionary.Add("lottery", 1);
		dictionary.Add("photoId", 3);
		dictionary.Add("overflow", 0);
		dictionary.Add("type", 0);
		dictionary.Add("security", 0);
		dictionary.Add("safeBox", 1);
		dictionary.Add("promoterName", "FakeUser推广员");
		ZH2_GVars.user = User.CreateWithDic(dictionary);
	}

	public static void Init()
	{
		MakerFakeUser();
	}
}
