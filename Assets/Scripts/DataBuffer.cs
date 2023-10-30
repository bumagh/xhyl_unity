using System;

[Serializable]
public class DataBuffer
{
	private int _minBuffLen;

	private byte[] _buff;

	private int _curBuffPosition;

	private int _buffLength;

	private int _dataLength;

	private ushort _protocalType;

	public DataBuffer(int _minBuffLen = 1024)
	{
		if (_minBuffLen <= 0)
		{
			this._minBuffLen = 1024;
		}
		else
		{
			this._minBuffLen = _minBuffLen;
		}
		_buff = new byte[this._minBuffLen];
	}

	public void AddBuffer(byte[] _data, int _dataLen)
	{
		if (_dataLen > _buff.Length - _curBuffPosition)
		{
			byte[] array = new byte[_curBuffPosition + _dataLen];
			Array.Copy(_buff, 0, array, 0, _curBuffPosition);
			Array.Copy(_data, 0, array, _curBuffPosition, _dataLen);
			_buff = array;
			array = null;
		}
		else
		{
			Array.Copy(_data, 0, _buff, _curBuffPosition, _dataLen);
		}
		_curBuffPosition += _dataLen;
	}

	public void UpdateDataLength()
	{
		if (_dataLength == 0 && _curBuffPosition >= Constants.HEAD_LEN)
		{
			byte[] array = new byte[Constants.HEAD_DATA_LEN];
			Array.Copy(_buff, 0, array, 0, Constants.HEAD_DATA_LEN);
			_buffLength = BitConverter.ToInt32(array, 0);
			byte[] array2 = new byte[Constants.HEAD_TYPE_LEN];
			Array.Copy(_buff, Constants.HEAD_DATA_LEN, array2, 0, Constants.HEAD_TYPE_LEN);
			_protocalType = BitConverter.ToUInt16(array2, 0);
			_dataLength = _buffLength - Constants.HEAD_LEN;
		}
	}

	public bool GetData(out sSocketData _tmpSocketData)
	{
		_tmpSocketData = default(sSocketData);
		if (_buffLength <= 0)
		{
			UpdateDataLength();
		}
		if (_buffLength > 0 && _curBuffPosition >= _buffLength)
		{
			_tmpSocketData._buffLength = _buffLength;
			_tmpSocketData._dataLength = _dataLength;
			_tmpSocketData._protocallType = (eProtocalCommand)_protocalType;
			_tmpSocketData._data = new byte[_dataLength];
			Array.Copy(_buff, Constants.HEAD_LEN, _tmpSocketData._data, 0, _dataLength);
			_curBuffPosition -= _buffLength;
			byte[] array = new byte[(_curBuffPosition >= _minBuffLen) ? _curBuffPosition : _minBuffLen];
			Array.Copy(_buff, _buffLength, array, 0, _curBuffPosition);
			_buff = array;
			_buffLength = 0;
			_dataLength = 0;
			_protocalType = 0;
			return true;
		}
		return false;
	}
}
