using System;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface ISpawnerProcess<T>
	{
		void SetProcess(Action<ProcessData<T>> processAction);
	}
}
