using System;
using System.Collections;

public class MyEnumerableTypeWithoutAdd : IEnumerable
{
	public int A;

	public IEnumerator GetEnumerator()
	{
		throw new NotImplementedException();
	}
}
