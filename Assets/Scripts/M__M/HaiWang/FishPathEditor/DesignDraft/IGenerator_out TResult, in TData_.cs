namespace M__M.HaiWang.FishPathEditor.DesignDraft
{
	internal interface IGenerator<out TResult, in TData>
	{
		TResult GetCurrent();

		TResult GetNext(TData data);
	}
	internal interface IGenerator<T>
	{
		T GetCurrent();

		T GetNext();
	}
}
