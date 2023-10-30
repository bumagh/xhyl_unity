using System.Collections.Generic;

namespace PathologicalGames
{
	public class EventInfoList : List<EventInfo>
	{
		public EventInfoList()
		{
		}

		public EventInfoList(EventInfoList eventInfoList)
			: base((IEnumerable<EventInfo>)eventInfoList)
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

		public EventInfoList CopyWithHitTime()
		{
			EventInfoList eventInfoList = new EventInfoList();
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EventInfo current = enumerator.Current;
					eventInfoList.Add(current);
				}
				return eventInfoList;
			}
		}
	}
}
