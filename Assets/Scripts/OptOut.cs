using FullSerializer;

[fsObject(MemberSerialization = fsMemberSerialization.OptOut)]
public class OptOut
{
	public int publicField;

	private int privateField;

	[fsIgnore]
	private int ignoredField;

	public int publicAutoProperty
	{
		get;
		set;
	}

	public int publicManualProperty
	{
		get
		{
			return publicField;
		}
		set
		{
			publicField = value;
		}
	}

	private int privateAutoProperty
	{
		get;
		set;
	}

	[fsIgnore]
	private int ignoredAutoProperty
	{
		get;
		set;
	}

	public OptOut()
	{
	}

	public OptOut(int publicField, int publicAutoProperty, int publicManualProperty, int privateField, int privateAutoProperty, int ignoredField, int ignoredAutoProperty)
	{
		this.publicField = publicField;
		this.publicAutoProperty = publicAutoProperty;
		this.publicManualProperty = publicManualProperty;
		this.privateField = privateField;
		this.privateAutoProperty = privateAutoProperty;
		this.ignoredField = ignoredField;
		this.ignoredAutoProperty = ignoredAutoProperty;
	}

	public int GetPrivateField()
	{
		return privateField;
	}

	public int GetPrivateAutoProperty()
	{
		return privateAutoProperty;
	}

	public int GetIgnoredField()
	{
		return ignoredField;
	}

	public int GetIgnoredAutoProperty()
	{
		return ignoredAutoProperty;
	}
}
