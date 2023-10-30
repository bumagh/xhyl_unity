using System.Collections.Generic;
using System.Linq;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FancySequenceGenerator<TResult> : GeneratorBase<TResult>
	{
		public List<FancySequenceData<TResult>> list;

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
			FancySequenceGenerator<TResult> fancySequenceGenerator = new FancySequenceGenerator<TResult>();
			fancySequenceGenerator._default = _default;
			fancySequenceGenerator._index = _index;
			fancySequenceGenerator._count = _count;
			if (list != null)
			{
				fancySequenceGenerator.list = (from _ in list
					select _?.Clone()).ToList();
			}
			return fancySequenceGenerator;
		}

		public override TResult[] GetEnums()
		{
			if (this.list == null)
			{
				return new TResult[0];
			}
			List<TResult> list = new List<TResult>();
			foreach (FancySequenceData<TResult> item in this.list)
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
