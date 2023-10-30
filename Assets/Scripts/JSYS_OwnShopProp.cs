public class JSYS_OwnShopProp
{
	public int Id;

	public long RemainTime;

	public bool IsShow;

	public JSYS_OwnShopProp(int id, long remainTime, bool ishow)
	{
		Id = id;
		RemainTime = remainTime;
		IsShow = ishow;
	}
}
