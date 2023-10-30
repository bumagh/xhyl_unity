using System.Collections.Generic;
using System.Linq;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FK3_FancySequenceGenerator<TResult> : FK3_GeneratorBase<TResult>
	{
		public List<FK3_FancySequenceData<TResult>> list;

		private int _index;

		private int _count;

		public override TResult GetCurrent()
		{
			return list[_index].value;
		}

		public override TResult GetNext()
		{
			_count++;
			if (_count > list[_index].count)
			{
				_index++;
				_count = 1;
				if (_index > list.Count - 1)
				{
					_index = 0;
				}
			}
			return list[_index].value;
		}

		public override void Reset()
		{
			base.Reset();
			_index = 0;
			_count = 0;
		}

		public override bool CheckValid()
		{
			return list != null && list.Count > 0;
		}

		public override object Clone()
		{
			FK3_FancySequenceGenerator<TResult> fK3_FancySequenceGenerator = new FK3_FancySequenceGenerator<TResult>();
			fK3_FancySequenceGenerator._default = _default;
			fK3_FancySequenceGenerator._index = _index;
			fK3_FancySequenceGenerator._count = _count;
			if (list != null)
			{
				fK3_FancySequenceGenerator.list = (from _ in list
					select _?.Clone()).ToList();
			}
			return fK3_FancySequenceGenerator;
		}

		public override TResult[] GetEnums()
		{
			if (this.list == null)
			{
				return new TResult[0];
			}
			List<TResult> list = new List<TResult>();
			foreach (FK3_FancySequenceData<TResult> item in this.list)
			{
				if (item != null)
				{
					list.AddRange(item.GetEnums());
				}
			}
			return list.ToArray();
		}
	}
}
