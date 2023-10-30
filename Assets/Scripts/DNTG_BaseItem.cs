public abstract class DNTG_BaseItem
{
	protected int mId;

	protected string mName;

	protected int mRoomId;

	public int Id
	{
		get
		{
			return mId;
		}
		set
		{
			mId = value;
		}
	}

	public string Name
	{
		get
		{
			return mName;
		}
		set
		{
			mName = value;
		}
	}

	public int RoomId
	{
		get
		{
			return mRoomId;
		}
		set
		{
			mRoomId = value;
		}
	}
}
