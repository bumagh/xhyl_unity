using System;

public struct TestObject
{
	public Func<object, object, bool> EqualityComparer;

	public Type StorageType;

	public object Original;

	public string Serialized;

	public object Deserialized;
}
