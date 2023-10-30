using System;
using UnityEngine;

namespace M__M.HaiWang.Tests.TestCtorAwakeOrder
{
	[Serializable]
	public class FK3_FooB
	{
		public string name = "xxx";

		public FK3_FooB(string name = "xxx")
		{
			UnityEngine.Debug.Log($"FooB[{name}] Ctor");
			if (name == "xxx")
			{
				name = "yyy";
			}
		}
	}
}
