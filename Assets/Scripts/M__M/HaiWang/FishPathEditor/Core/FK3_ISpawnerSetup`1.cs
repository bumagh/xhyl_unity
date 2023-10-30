using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public interface FK3_ISpawnerSetup<T>
	{
		void Setup(FK3_SpawnerData data, FK3_IGenerator<T> generator, MonoBehaviour manager);
	}
}
