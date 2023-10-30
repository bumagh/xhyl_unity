namespace M__M.HaiWang.FishPathEditor.DesignDraft
{
	internal interface FK3_IGenerator<out TResult, in TData>
	{
		TResult GetCurrent();

		TResult GetNext(TData data);
	}
	internal interface FK3_IGenerator<T>
	{
		T GetCurrent();

		T GetNext();
	}
}
