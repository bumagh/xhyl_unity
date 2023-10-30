using System.Collections.Generic;
using System.Linq;

public class IDictionaryStringStringProvider : TestProvider<IDictionary<string, string>>
{
	public override bool Compare(IDictionary<string, string> before, IDictionary<string, string> after)
	{
		return before.Except(after).Count() == 0 && after.Except(before).Count() == 0;
	}

	public override IEnumerable<IDictionary<string, string>> GetValues()
	{
		yield return new Dictionary<string, string>
		{
			{
				string.Empty,
				null
			}
		};
	}
}
