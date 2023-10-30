using System.Collections.Generic;

public abstract class TestProvider<T> : ITestProvider
{
	public abstract bool Compare(T before, T after);

	public abstract IEnumerable<T> GetValues();

	IEnumerable<TestItem> ITestProvider.GetValues()
	{
		foreach (T value in GetValues())
		{
			yield return new TestItem
			{
				Item = value,
				ItemStorageType = typeof(T),
				Comparer = ((object a, object b) => Compare((T)a, (T)b))
			};
		}
	}
}
