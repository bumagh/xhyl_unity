using System.Collections.Generic;

namespace M__M.HaiWang.Fish
{
	public class FK3_FG_FixedSequence<T> : FK3_FishGeneratorBase<T>
	{
		public List<T> types;

		protected int _index;

		public override T GetNext()
		{
			return types[_index++ % types.Count];
		}

		public override bool HasNext()
		{
			return _index < types.Count;
		}
	}
}
