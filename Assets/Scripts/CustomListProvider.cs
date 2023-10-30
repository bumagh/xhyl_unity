using System.Collections.Generic;

public class CustomListProvider : TestProvider<IListType>
{
	public override bool Compare(IListType before, IListType after)
	{
		if (before.Count != after.Count)
		{
			return false;
		}
		for (int i = 0; i < before.Count; i++)
		{
			if (before[i] != after[i])
			{
				return false;
			}
		}
		return true;
	}

	public override IEnumerable<IListType> GetValues()
	{
		yield return new MyList();
		yield return new MyList
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7
		};
	}
}
