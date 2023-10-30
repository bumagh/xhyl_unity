using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface ISpawner<T> : ISpawnerPlayable<T>, ISpawnerSetup<T>, ISpawnerProcess<T>, ICheckValid
	{
		void Setup(SpawnerData data, IGenerator<T> generator, MonoBehaviour manager, Action<ProcessData<T>> processAction);
	}
}
