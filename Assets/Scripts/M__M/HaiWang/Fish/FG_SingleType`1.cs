namespace M__M.HaiWang.Fish
{
	public class FG_SingleType<T> : FishGeneratorBase<T>
	{
		public T type;

		public int count;

		protected int _index;

		public override T GetNext()
		{
			_index++;
			return type;
		}

		public override bool HasNext()
		{
			return _index < count;
		}
	}
}
