using System.Collections.Generic;

public class PrivateFieldsProvider : TestProvider<PrivateHolder>
{
	public override bool Compare(PrivateHolder before, PrivateHolder after)
	{
		return before.Equals(after);
	}

	public override IEnumerable<PrivateHolder> GetValues()
	{
		PrivateHolder holder = new PrivateHolder();
		holder.Setup();
		yield return holder;
	}
}
