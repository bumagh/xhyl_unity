using System.Collections.Generic;
using System.Linq;

public class IDictionaryStringIntProvider : TestProvider<IDictionary<string, int>>
{
	public override bool Compare(IDictionary<string, int> before, IDictionary<string, int> after)
	{
		return before.Except(after).Count() == 0 && after.Except(before).Count() == 0;
	}

	public override IEnumerable<IDictionary<string, int>> GetValues()
	{
		yield return new Dictionary<string, int>
		{
			{
				"ok",
				3
			},
			{
				string.Empty,
				2
			}
		};
	}
}
