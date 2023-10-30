using System;
using System.Text;

public class Point1
{
	public int Acs(string character)
	{
		if (character.Length <= 1)
		{
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			return aSCIIEncoding.GetBytes(character)[0];
		}
		throw new Exception("Character is not valid");
	}
}
