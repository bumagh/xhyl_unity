using System.Collections.Generic;

public class CustomIEnumerableProviderWithoutAdd : TestProvider<MyEnumerableTypeWithoutAdd>
{
	public override bool Compare(MyEnumerableTypeWithoutAdd before, MyEnumerableTypeWithoutAdd after)
	{
		return before.A == after.A;
	}

	public override IEnumerable<MyEnumerableTypeWithoutAdd> GetValues()
	{
		yield return new MyEnumerableTypeWithoutAdd
		{
			A = -1
		};
		yield return new MyEnumerableTypeWithoutAdd
		{
			A = 0
		};
		yield return new MyEnumerableTypeWithoutAdd
		{
			A = 1
		};
	}
}
