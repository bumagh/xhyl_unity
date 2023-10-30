using System.Collections.Generic;

namespace PathologicalGames
{
	public class TargetList : List<Target>
	{
		public TargetList()
		{
		}

		public TargetList(TargetList targetList)
			: base((IEnumerable<Target>)targetList)
		{
		}

		public TargetList(Area area)
			: base((IEnumerable<Target>)area)
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
					Target current = enumerator.Current;
					if (!(current.transform == null))
					{
						array[num] = current.transform.name;
						num++;
					}
				}
			}
			return string.Join(", ", array);
		}
	}
}
