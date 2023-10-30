using System.Collections.Generic;

namespace PathSystem
{
	public class FixedSequence<T> : GeneratorBase<T>
	{
		public List<T> types;

		protected int _index;

		public override AgentData<T> GetNext(object userData)
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
