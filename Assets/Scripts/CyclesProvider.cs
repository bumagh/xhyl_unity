using System.Collections.Generic;

public class CyclesProvider : ITestProvider
{
	public interface ICycle
	{
		int A
		{
			get;
			set;
		}

		ICycle Cycle
		{
			get;
			set;
		}

		int B
		{
			get;
			set;
		}
	}

	public class CycleDerivedA : ICycle
	{
		public int A
		{
			get;
			set;
		}

		public ICycle Cycle
		{
			get;
			set;
		}

		public int B
		{
			get;
			set;
		}
	}

	public class CycleDerivedB : ICycle
	{
		public int A
		{
			get;
			set;
		}

		public ICycle Cycle
		{
			get;
			set;
		}

		public int B
		{
			get;
			set;
		}
	}

	public class Cyclic
	{
	}

	public IEnumerable<TestItem> GetValues()
	{
		CycleDerivedA simpleCycle = new CycleDerivedA
		{
			A = 1,
			B = 2
		};
		simpleCycle.Cycle = simpleCycle;
		yield return new TestItem
		{
			Item = simpleCycle,
			ItemStorageType = simpleCycle.GetType(),
			Comparer = delegate(object a, object b)
			{
				CycleDerivedA cycleDerivedA = (CycleDerivedA)b;
				return cycleDerivedA.A == 1 && cycleDerivedA.B == 2 && object.ReferenceEquals(cycleDerivedA.Cycle, cycleDerivedA);
			}
		};
		ICycle simpleInheritCycle = new CycleDerivedA
		{
			A = 1,
			B = 2
		};
		simpleInheritCycle.Cycle = simpleInheritCycle;
		yield return new TestItem
		{
			Item = new ValueHolder<ICycle>(simpleInheritCycle),
			ItemStorageType = typeof(ValueHolder<ICycle>),
			Comparer = delegate(object a, object b)
			{
				ValueHolder<ICycle> valueHolder2 = (ValueHolder<ICycle>)b;
				return valueHolder2.Value.GetType() == typeof(CycleDerivedA) && valueHolder2.Value.A == 1 && valueHolder2.Value.B == 2 && object.ReferenceEquals(valueHolder2.Value.Cycle, valueHolder2.Value);
			}
		};
		ICycle complexInheritCycle = new CycleDerivedA
		{
			A = 1,
			B = 2
		};
		complexInheritCycle.Cycle = new CycleDerivedB
		{
			A = 3,
			B = 4
		};
		complexInheritCycle.Cycle.Cycle = complexInheritCycle;
		yield return new TestItem
		{
			Item = new ValueHolder<ICycle>(complexInheritCycle),
			ItemStorageType = typeof(ValueHolder<ICycle>),
			Comparer = delegate(object a, object b)
			{
				ValueHolder<ICycle> valueHolder = (ValueHolder<ICycle>)b;
				return valueHolder.Value.GetType() == typeof(CycleDerivedA) && valueHolder.Value.Cycle.GetType() == typeof(CycleDerivedB) && valueHolder.Value.A == 1 && valueHolder.Value.B == 2 && valueHolder.Value.Cycle.A == 3 && valueHolder.Value.Cycle.B == 4 && object.ReferenceEquals(valueHolder.Value.Cycle.Cycle, valueHolder.Value);
			}
		};
		Cyclic a4 = new Cyclic();
		Cyclic a3 = new Cyclic();
		Cyclic a2 = new Cyclic();
		yield return new TestItem
		{
			Item = new List<object>
			{
				a4,
				a3,
				a2,
				a3,
				a2,
				a4,
				a4,
				a4,
				a4
			},
			ItemStorageType = typeof(List<object>),
			Comparer = delegate(object a, object b)
			{
				List<object> list = (List<object>)b;
				object obj = list[0];
				object obj2 = list[1];
				object obj3 = list[2];
				return list[3] == obj2 && list[4] == obj3 && list[5] == obj && list[6] == obj && list[7] == obj && list[8] == obj;
			}
		};
	}
}
