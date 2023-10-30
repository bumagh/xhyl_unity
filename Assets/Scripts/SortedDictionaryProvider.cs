using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SortedDictionaryProvider : TestProvider<IDictionary>
{
	public enum Enum
	{
		A,
		B,
		C,
		D,
		E,
		F
	}

	[Flags]
	public enum FlagsEnum
	{
		FlagA = 0x1,
		FlagB = 0x2,
		FlagC = 0x4,
		FlagD = 0x8,
		FlagE = 0x10,
		FlagF = 0x20
	}

	public override bool Compare(IDictionary before, IDictionary after)
	{
		if (before is SortedDictionary<double, float>)
		{
			return CompareDicts<SortedDictionary<double, float>, double, float>(before, after);
		}
		if (before is SortedList<int, string>)
		{
			return CompareDicts<SortedList<int, string>, int, string>(before, after);
		}
		if (before is SortedDictionary<int, int>)
		{
			return CompareDicts<SortedDictionary<int, int>, int, int>(before, after);
		}
		if (before is SortedList<string, float>)
		{
			return CompareDicts<SortedList<string, float>, string, float>(before, after);
		}
		if (before is SortedDictionary<Enum, int>)
		{
			return CompareDicts<SortedDictionary<Enum, int>, Enum, int>(before, after);
		}
		if (before is SortedDictionary<FlagsEnum, int>)
		{
			return CompareDicts<SortedDictionary<FlagsEnum, int>, FlagsEnum, int>(before, after);
		}
		throw new Exception("unknown type");
	}

	private static bool CompareDicts<TDict, TKey, TValue>(object a, object b) where TDict : IDictionary<TKey, TValue>
	{
		TDict val = (TDict)a;
		TDict val2 = (TDict)b;
		return val.Except(val2).Count() == 0 && val2.Except(val).Count() == 0;
	}

	public override IEnumerable<IDictionary> GetValues()
	{
		yield return new SortedDictionary<double, float>();
		yield return new SortedList<int, string>
		{
			{
				0,
				string.Empty
			},
			{
				1,
				null
			}
		};
		yield return new SortedDictionary<int, int>
		{
			{
				0,
				0
			},
			{
				1,
				1
			},
			{
				-1,
				-1
			}
		};
		yield return new SortedList<string, float>
		{
			{
				"ok",
				1f
			},
			{
				"yes",
				2f
			},
			{
				string.Empty,
				3f
			}
		};
		yield return new SortedDictionary<Enum, int>();
		yield return new SortedDictionary<Enum, int>
		{
			{
				Enum.A,
				3
			}
		};
		yield return new SortedDictionary<Enum, int>
		{
			{
				Enum.A,
				1
			},
			{
				Enum.B,
				2
			},
			{
				Enum.C,
				3
			}
		};
		yield return new SortedDictionary<FlagsEnum, int>();
		yield return new SortedDictionary<FlagsEnum, int>
		{
			{
				FlagsEnum.FlagA,
				3
			}
		};
		yield return new SortedDictionary<FlagsEnum, int>
		{
			{
				FlagsEnum.FlagA | FlagsEnum.FlagB,
				1
			},
			{
				FlagsEnum.FlagC,
				2
			},
			{
				FlagsEnum.FlagD | FlagsEnum.FlagE | FlagsEnum.FlagF,
				3
			}
		};
	}
}
