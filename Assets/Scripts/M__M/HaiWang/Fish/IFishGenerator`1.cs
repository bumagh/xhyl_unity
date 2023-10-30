namespace M__M.HaiWang.Fish
{
	public interface IFishGenerator<T>
	{
		T GetNext();

		bool HasNext();
	}
}
