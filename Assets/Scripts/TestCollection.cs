using System.Collections.ObjectModel;

public class TestCollection : KeyedCollection<TestEnum, TestClass>
{
	protected override TestEnum GetKeyForItem(TestClass item)
	{
		return item?.TestEnum ?? TestEnum.Null;
	}

	public bool TryGetValue(TestEnum key, out TestClass item)
	{
		if (base.Dictionary == null)
		{
			item = null;
			return false;
		}
		return base.Dictionary.TryGetValue(key, out item);
	}

	public new bool Contains(TestClass item)
	{
		return Contains(GetKeyForItem(item));
	}
}
