using System.Collections.Generic;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	public class FK3_XSelector<T>
	{
		public List<T> list;

		private int index;

		private const int MaxTry = 100;

		public FK3_XSelector(List<T> list)
		{
			this.list = list;
			index = 0;
		}

		public T GetPrev()
		{
			T val = default(T);
			int num = 100;
			T val2;
			do
			{
				index = (--index + list.Count) % list.Count;
				val2 = list[index];
			}
			while (val2 == null && --num > 0);
			return val2;
		}

		public T GetNext()
		{
			index = ++index % list.Count;
			return list[index];
		}

		public T GetCurrent()
		{
			index %= list.Count;
			return list[index];
		}

		public T GetAt(int index)
		{
			this.index = index;
			return list[index];
		}
	}
}
