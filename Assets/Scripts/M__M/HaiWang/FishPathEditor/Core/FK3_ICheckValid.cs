namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface FK3_ICheckValid
	{
		bool CheckValid();

		bool HasError();

		void ClearError();

		string GetError();
	}
}
