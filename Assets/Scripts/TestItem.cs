using System;

public struct TestItem
{
	public object Item;

	public Type ItemStorageType;

	public Func<object, object, bool> Comparer;
}
