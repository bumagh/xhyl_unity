using System;
using UnityEngine;

namespace M__M.HaiWang.Tests.TestCtorAwakeOrder
{
	[Serializable]
	public class FK3_FooA
	{
		public string name = "xxx";

		public FK3_FooA(string name = "xxx")
		{
			UnityEngine.Debug.Log($"FooA[{name}] Ctor");
			if (name == "xxx")
			{
				name = "yyy";
			}
		}
	}
}
