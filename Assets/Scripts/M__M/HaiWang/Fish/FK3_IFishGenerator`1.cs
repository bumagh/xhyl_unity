namespace M__M.HaiWang.Fish
{
	public interface FK3_IFishGenerator<T>
	{
		T GetNext();

		bool HasNext();
	}
}
