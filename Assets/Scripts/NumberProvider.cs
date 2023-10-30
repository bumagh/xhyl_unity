using System.Collections.Generic;

public class NumberProvider : TestProvider<object>
{
	public override bool Compare(object before, object after)
	{
		return EqualityComparer<object>.Default.Equals(before, after);
	}

	public override IEnumerable<object> GetValues()
	{
		yield return -2.5f;
		yield return float.MaxValue;
		yield return float.MinValue;
		yield return -2.5;
		yield return 0.2m;
		yield return (byte)2;
		yield return (byte)0;
		yield return byte.MaxValue;
		yield return (sbyte)2;
		yield return sbyte.MinValue;
		yield return sbyte.MaxValue;
		yield return (ushort)2;
		yield return (ushort)0;
		yield return ushort.MaxValue;
		yield return (short)4;
		yield return short.MinValue;
		yield return short.MaxValue;
		yield return 5u;
		yield return 0u;
		yield return uint.MaxValue;
		yield return 6;
		yield return int.MinValue;
		yield return int.MaxValue;
		yield return 7uL;
		yield return 0uL;
		yield return 8L;
		yield return long.MinValue;
		yield return long.MaxValue;
	}
}
