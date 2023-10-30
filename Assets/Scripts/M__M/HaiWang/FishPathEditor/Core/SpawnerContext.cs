namespace M__M.HaiWang.FishPathEditor.Core
{
	public class SpawnerContext
	{
		protected SpawnerContext _parent;

		public SpawnerContext GetParent()
		{
			return _parent;
		}

		public void SetParent(SpawnerContext parent)
		{
			_parent = parent;
		}

		public bool HasParent()
		{
			return _parent != null;
		}
	}
}
