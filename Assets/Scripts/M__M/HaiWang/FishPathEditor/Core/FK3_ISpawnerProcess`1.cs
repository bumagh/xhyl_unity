using System;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface FK3_ISpawnerProcess<T>
	{
		void SetProcess(Action<FK3_ProcessData<T>> processAction);
	}
}
