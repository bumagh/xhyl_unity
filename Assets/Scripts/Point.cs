using System;

public struct Point
{
	private int x;

	private int y;

	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Point))
		{
			return false;
		}
		Point point = (Point)obj;
		return (x == point.x) & (y == point.y);
	}

	public override int GetHashCode()
	{
		return ShiftAndWrap(x.GetHashCode(), 2) ^ y.GetHashCode();
	}

	private int ShiftAndWrap(int value, int positions)
	{
		positions &= 0x1F;
		uint num = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
		uint num2 = num >> 32 - positions;
		return BitConverter.ToInt32(BitConverter.GetBytes((num << positions) | num2), 0);
	}
}
