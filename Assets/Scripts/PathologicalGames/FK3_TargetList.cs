using System.Collections.Generic;

namespace PathologicalGames
{
	public class FK3_TargetList : List<FK3_Target>
	{
		public FK3_TargetList()
		{
		}

		public FK3_TargetList(FK3_TargetList targetList)
			: base((IEnumerable<FK3_Target>)targetList)
		{
		}

		public FK3_TargetList(FK3_Area area)
			: base((IEnumerable<FK3_Target>)area)
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
					FK3_Target current = enumerator.Current;
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
