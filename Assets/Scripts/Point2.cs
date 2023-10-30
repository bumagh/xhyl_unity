using System;

public class Point2
{
	public int[] ConStr(string personId)
	{
		if (personId.Length <= 16)
		{
			personId = personId.ToString().PadRight(16, '0');
			char[] array = personId.ToCharArray();
			int[] array2 = new int[8];
			int num = 0;
			Point1 point = new Point1();
			for (int i = 0; i < array.Length; i += 2)
			{
				string character = array[i].ToString();
				int x = point.Acs(character);
				string character2 = array[i + 1].ToString();
				int y = point.Acs(character2);
				int num2 = array2[num] = new Point(x, y).GetHashCode() % 10;
				num++;
			}
			return array2;
		}
		if (personId.Length <= 20)
		{
			personId = personId.ToString().PadRight(20, '0');
			char[] array3 = personId.ToCharArray();
			int[] array4 = new int[personId.Length / 2];
			int num3 = 0;
			Point1 point2 = new Point1();
			for (int j = 0; j < array3.Length; j += 2)
			{
				string character3 = array3[j].ToString();
				int x2 = point2.Acs(character3);
				string character4 = array3[j + 1].ToString();
				int y2 = point2.Acs(character4);
				int num4 = array4[num3] = new Point(x2, y2).GetHashCode() % 10;
				num3++;
			}
			return array4;
		}
		if (personId.Length <= 40)
		{
			personId = personId.ToString().PadRight(40, '0');
			char[] array5 = personId.ToCharArray();
			int[] array6 = new int[personId.Length / 2];
			int num5 = 0;
			Point1 point3 = new Point1();
			for (int k = 0; k < array5.Length; k += 2)
			{
				string character5 = array5[k].ToString();
				int x3 = point3.Acs(character5);
				string character6 = array5[k + 1].ToString();
				int y3 = point3.Acs(character6);
				int num6 = array6[num5] = new Point(x3, y3).GetHashCode() % 10;
				num5++;
			}
			return array6;
		}
		throw new Exception("PersonId is not valid");
	}
}
