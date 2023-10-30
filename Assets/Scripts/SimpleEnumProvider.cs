using System.Collections.Generic;

public class SimpleEnumProvider : TestProvider<SimpleEnum>
{
	public override bool Compare(SimpleEnum before, SimpleEnum after)
	{
		return before == after;
	}

	public override IEnumerable<SimpleEnum> GetValues()
	{
		yield return SimpleEnum.A;
		yield return SimpleEnum.C;
		yield return SimpleEnum.D;
	}
}
