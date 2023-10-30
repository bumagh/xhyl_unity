using System.Collections.Generic;

public class KeyedCollectionProvider : TestProvider<TestCollection>
{
	public override bool Compare(TestCollection before, TestCollection after)
	{
		if (before.Count != after.Count)
		{
			return false;
		}
		for (int i = 0; i < before.Count; i++)
		{
			if (before[i].TestEnum != after[i].TestEnum)
			{
				return false;
			}
		}
		return true;
	}

	public override IEnumerable<TestCollection> GetValues()
	{
		yield return new TestCollection();
		yield return new TestCollection
		{
			new TestClass
			{
				TestEnum = TestEnum.Null
			}
		};
		yield return new TestCollection
		{
			new TestClass
			{
				TestEnum = TestEnum.Null
			},
			new TestClass
			{
				TestEnum = TestEnum.Value1
			}
		};
		yield return new TestCollection
		{
			new TestClass
			{
				TestEnum = TestEnum.Null
			},
			new TestClass
			{
				TestEnum = TestEnum.Value1
			},
			new TestClass
			{
				TestEnum = TestEnum.Value2
			}
		};
		yield return new TestCollection
		{
			new TestClass
			{
				TestEnum = TestEnum.Null
			},
			new TestClass
			{
				TestEnum = TestEnum.Value1
			},
			new TestClass
			{
				TestEnum = TestEnum.Value2
			},
			new TestClass
			{
				TestEnum = TestEnum.Value3
			}
		};
	}
}
