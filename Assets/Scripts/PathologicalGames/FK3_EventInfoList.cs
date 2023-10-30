using System.Collections.Generic;

namespace PathologicalGames
{
	public class FK3_EventInfoList : List<FK3_EventInfo>
	{
		public FK3_EventInfoList()
		{
		}

		public FK3_EventInfoList(FK3_EventInfoList eventInfoList)
			: base((IEnumerable<FK3_EventInfo>)eventInfoList)
		{
		}

		public override string ToString()
		{
			string[] array = new string[base.Count];
			int num = 0;
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					array[num] = enumerator.Current.ToString();
					num++;
				}
			}
			return string.Join(", ", array);
		}

		public FK3_EventInfoList CopyWithHitTime()
		{
			FK3_EventInfoList fK3_EventInfoList = new FK3_EventInfoList();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FK3_EventInfo current = enumerator.Current;
					fK3_EventInfoList.Add(current);
				}
				return fK3_EventInfoList;
			}
		}
	}
}
