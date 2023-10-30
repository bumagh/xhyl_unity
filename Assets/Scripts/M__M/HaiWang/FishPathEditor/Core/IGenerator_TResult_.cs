namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface IGenerator<TResult> : IGenerator
	{
		new TResult GetCurrent();

		new TResult GetNext();

		TResult[] GetEnums();
	}
	public interface IGenerator
	{
		object GetCurrent();

		object GetNext();

		void Reset();

		bool CheckValid();
	}
}
