using M__M.HaiWang.FishPathEditor.Core;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.Assistance
{
	public class SpawnerDesign<T>
	{
		public SpawnerData spawnerData = new SpawnerData();

		public GeneratorBase<T> generator;

		public bool IsValid => spawnerData != null && generator != null;
	}
}
