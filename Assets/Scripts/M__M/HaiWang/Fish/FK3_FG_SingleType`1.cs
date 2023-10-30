namespace M__M.HaiWang.Fish
{
	public class FK3_FG_SingleType<T> : FK3_FishGeneratorBase<T>
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
