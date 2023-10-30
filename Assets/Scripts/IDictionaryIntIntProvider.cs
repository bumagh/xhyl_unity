using System.Collections.Generic;
using System.Linq;

public class IDictionaryIntIntProvider : TestProvider<IDictionary<int, int>>
{
	public override bool Compare(IDictionary<int, int> before, IDictionary<int, int> after)
	{
		return before.Except(after).Count() == 0 && after.Except(before).Count() == 0;
	}

	public override IEnumerable<IDictionary<int, int>> GetValues()
	{
		yield return new Dictionary<int, int>();
		yield return new Dictionary<int, int>
		{
			{
				1,
				1
			},
			{
				2,
				0
			},
			{
				3,
				32
			}
		};
	}
}
