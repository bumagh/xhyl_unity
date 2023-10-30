namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FK3_SpawnerBaseItem<T>
	{
		public FK3_SpawnerBase<T> spawner;

		public int index;

		public int validIndex;

		public FK3_SpawnerBaseItem()
		{
		}

		public FK3_SpawnerBaseItem(FK3_SpawnerBase<T> spawner, int index, int validIndex)
		{
			this.spawner = spawner;
			this.index = index;
			this.validIndex = validIndex;
		}
	}
}
