using System.Collections.Generic;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public class SequenceGenerator<TResult> : GeneratorBase<TResult>
	{
		public List<TResult> list;

		private int _index;

		public override TResult GetCurrent()
		{
			return list[_index];
		}

		public override TResult GetNext()
		{
			_index++;
			if (_index > list.Count - 1)
			{
				_index = 0;
			}
			return list[_index];
		}

		public override void Reset()
		{
			base.Reset();
			_index = 0;
		}

		public override bool CheckValid()
		{
			return list != null && list.Count > 0;
		}

		public override object Clone()
		{
			SequenceGenerator<TResult> sequenceGenerator = new SequenceGenerator<TResult>();
			sequenceGenerator._default = _default;
			sequenceGenerator._index = _index;
			sequenceGenerator.list = new List<TResult>(list);
			return sequenceGenerator;
		}

		public override TResult[] GetEnums()
		{
			if (list == null)
			{
				return new TResult[0];
			}
			return list.ToArray();
		}
	}
}
