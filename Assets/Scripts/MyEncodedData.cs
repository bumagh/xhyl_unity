public class MyEncodedData
{
	public string value;

	private MyEncodedData()
	{
	}

	public static MyEncodedData Make(string value)
	{
		MyEncodedData myEncodedData = new MyEncodedData();
		myEncodedData.value = value;
		return myEncodedData;
	}
}
