public class XLDT_BetItem
{
	public float fPower;

	public int nCount;

	public int nMy;

	public int nTotal;

	public void SetValue(float power, int count, int my, int total)
	{
		fPower = power;
		nCount = count;
		nMy = my;
		nTotal = total;
	}

	public void Reset()
	{
		fPower = 0f;
		nCount = 0;
		nMy = 0;
		nTotal = 0;
	}
}
