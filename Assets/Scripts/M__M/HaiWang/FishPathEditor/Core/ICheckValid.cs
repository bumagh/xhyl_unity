namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface ICheckValid
	{
		bool CheckValid();

		bool HasError();

		void ClearError();

		string GetError();
	}
}
