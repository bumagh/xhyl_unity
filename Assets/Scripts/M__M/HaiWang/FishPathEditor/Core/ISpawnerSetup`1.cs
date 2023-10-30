using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface ISpawnerSetup<T>
	{
		void Setup(SpawnerData data, IGenerator<T> generator, MonoBehaviour manager);
	}
}
