using System.Collections.Generic;

public interface ITestProvider
{
	IEnumerable<TestItem> GetValues();
}
