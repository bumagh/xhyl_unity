using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface FK3_ISpawner<T> : FK3_ISpawnerPlayable<T>, FK3_ISpawnerSetup<T>, FK3_ISpawnerProcess<T>, FK3_ICheckValid
	{
		void Setup(FK3_SpawnerData data, FK3_IGenerator<T> generator, MonoBehaviour manager, Action<FK3_ProcessData<T>> processAction);
	}
}
