using System;

[Serializable]
public struct sSocketData
{
	public byte[] _data;

	public eProtocalCommand _protocallType;

	public int _buffLength;

	public int _dataLength;
}
