using System;
using UnityEngine;

namespace M__M.HaiWang.Tests.TestCtorAwakeOrder
{
	[Serializable]
	public class FooB
	{
		public string name = "xxx";

		public FooB(string name = "xxx")
		{
			UnityEngine.Debug.Log($"FooB[{name}] Ctor");
			if (name == "xxx")
			{
				name = "yyy";
			}
		}
	}
}
