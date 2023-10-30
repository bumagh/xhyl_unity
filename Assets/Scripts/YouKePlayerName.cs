public class YouKePlayerName
{
	public string mSerials = string.Empty;

	public string GetYouKeName(string namestr)
	{
		Point2 point = new Point2();
		int[] array = point.ConStr(namestr);
		string text = string.Empty;
		int[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			int num = array2[i];
			text += num.ToString();
		}
		for (int j = 0; j < 3; j++)
		{
			if (text.Length >= 20)
			{
				Point2 point2 = new Point2();
				int[] array3 = point2.ConStr(text);
				text = string.Empty;
				int[] array4 = array3;
				for (int k = 0; k < array4.Length; k++)
				{
					int num2 = array4[k];
					text += num2.ToString();
				}
			}
		}
		mSerials = text;
		return "游客" + text;
	}
}
