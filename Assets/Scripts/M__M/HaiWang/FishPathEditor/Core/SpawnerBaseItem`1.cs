namespace M__M.HaiWang.FishPathEditor.Core
{
	public class SpawnerBaseItem<T>
	{
		public SpawnerBase<T> spawner;

		public int index;

		public int validIndex;

		public SpawnerBaseItem()
		{
		}

		public SpawnerBaseItem(SpawnerBase<T> spawner, int index, int validIndex)
		{
			this.spawner = spawner;
			this.index = index;
			this.validIndex = validIndex;
		}
	}
}
