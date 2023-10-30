using M__M.HaiWang.FishPathEditor.Core;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.Assistance
{
	public class FK3_SpawnerDesign<T>
	{
		public FK3_SpawnerData spawnerData = new FK3_SpawnerData();

		public FK3_GeneratorBase<T> generator;

		public bool IsValid => spawnerData != null && generator != null;
	}
}
