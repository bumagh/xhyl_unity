using FullSerializer;

public class PrivateHolder
{
	[fsProperty]
	private int SerializedField;

	[fsProperty]
	private int SerializedProperty
	{
		get;
		set;
	}

	public void Setup()
	{
		SerializedField = 1;
		SerializedProperty = 2;
	}

	public override bool Equals(object obj)
	{
		PrivateHolder privateHolder = obj as PrivateHolder;
		if (privateHolder == null)
		{
			return false;
		}
		return SerializedField == privateHolder.SerializedField && SerializedProperty == privateHolder.SerializedProperty;
	}

	public override int GetHashCode()
	{
		return SerializedField.GetHashCode() + 17 * SerializedProperty.GetHashCode();
	}
}
