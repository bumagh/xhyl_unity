using System.Collections.Generic;

namespace PathSystem
{
	public class FK3_FixedSequence<T> : FK3_GeneratorBase<T>
	{
		public List<T> types;

		protected int _index;

		public override FK3_AgentData<T> GetNext(object userData)
		{
			return _getNext(types[_index++ % types.Count], userData);
		}

		public override void Reset()
		{
			_index = 0;
		}

		public override T[] GetTypes()
		{
			return (types == null) ? base.GetTypes() : types.ToArray();
		}
	}
}
