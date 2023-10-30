namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FK3_SpawnerContext
	{
		protected FK3_SpawnerContext _parent;

		public FK3_SpawnerContext GetParent()
		{
			return _parent;
		}

		public void SetParent(FK3_SpawnerContext parent)
		{
			_parent = parent;
		}

		public bool HasParent()
		{
			return _parent != null;
		}
	}
}
