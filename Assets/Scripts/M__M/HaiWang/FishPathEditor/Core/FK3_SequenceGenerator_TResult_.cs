using System.Collections.Generic;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FK3_SequenceGenerator<TResult> : FK3_GeneratorBase<TResult>
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
			FK3_SequenceGenerator<TResult> fK3_SequenceGenerator = new FK3_SequenceGenerator<TResult>();
			fK3_SequenceGenerator._default = _default;
			fK3_SequenceGenerator._index = _index;
			fK3_SequenceGenerator.list = new List<TResult>(list);
			return fK3_SequenceGenerator;
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
