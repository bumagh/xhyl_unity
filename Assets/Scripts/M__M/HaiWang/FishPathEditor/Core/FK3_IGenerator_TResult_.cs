namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface FK3_IGenerator<TResult> : FK3_IGenerator
	{
		new TResult GetCurrent();

		new TResult GetNext();

		TResult[] GetEnums();
	}
	public interface FK3_IGenerator
	{
		object GetCurrent();

		object GetNext();

		void Reset();

		bool CheckValid();
	}
}
