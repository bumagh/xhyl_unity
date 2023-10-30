using System.Collections.Generic;

public class EncodedDataProvider : TestProvider<MyEncodedData>
{
	public override bool Compare(MyEncodedData before, MyEncodedData after)
	{
		return before.value == after.value;
	}

	public override IEnumerable<MyEncodedData> GetValues()
	{
		yield return MyEncodedData.Make("P:\\UnityProjects");
		yield return MyEncodedData.Make("P:\\\\UnityProjects");
	}
}
