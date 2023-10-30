using System.Collections.Generic;

public class CustomIEnumerableProviderWithAdd : TestProvider<MyEnumerableTypeWithAdd>
{
	public override bool Compare(MyEnumerableTypeWithAdd before, MyEnumerableTypeWithAdd after)
	{
		if (before.items.Count != after.items.Count)
		{
			return false;
		}
		for (int i = 0; i < before.items.Count; i++)
		{
			if (before.items[i] != after.items[i])
			{
				return false;
			}
		}
		return true;
	}

	public override IEnumerable<MyEnumerableTypeWithAdd> GetValues()
	{
		yield return new MyEnumerableTypeWithAdd();
		yield return new MyEnumerableTypeWithAdd
		{
			1
		};
		yield return new MyEnumerableTypeWithAdd
		{
			1,
			2,
			3
		};
	}
}
