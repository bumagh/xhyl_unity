using FullInspector;
using JsonFx.Json;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace M__M.HaiWang.Tests.JSONUsage
{
	public class FK3_JSONUsage_Main : BaseBehavior<FullSerializerSerializer>
	{
		protected override void Awake()
		{
			base.Awake();
		}

		private void Start()
		{
		}

		[InspectorButton]
		private void Test1()
		{
			_Test1(debug: true);
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (int i = 0; i < 100; i++)
			{
				_Test1(debug: false);
			}
			UnityEngine.Debug.Log(stopwatch.Elapsed.TotalSeconds);
		}

		[InspectorButton]
		private void Test2()
		{
			_Test2();
		}

		private void _Test1(bool debug)
		{
			FK3_A fK3_A = new FK3_A();
			fK3_A.intVar = 4;
			fK3_A.strVar = "aa";
			FK3_A a = fK3_A;
			FK3_B fK3_B = new FK3_B();
			fK3_B.x = 3;
			fK3_B.a = a;
			string text = JsonWriter.Serialize(fK3_B);
			FK3_B fK3_B2 = JsonReader.Deserialize<FK3_B>(text);
			string message = JsonWriter.Serialize(fK3_B);
			if (debug)
			{
				UnityEngine.Debug.Log(text);
			}
			if (debug)
			{
				UnityEngine.Debug.Log(message);
			}
			string value = "{\"x\":3,\"a\":{ \"intVarx\":4,\"strVarx\":\"aa\"} }";
			FK3_B value2 = JsonReader.Deserialize<FK3_B>(value);
			string message2 = JsonWriter.Serialize(value2);
			if (debug)
			{
				UnityEngine.Debug.Log(message2);
			}
		}

		private void _Test2()
		{
			bool flag = true;
			FK3_A fK3_A = new FK3_A();
			fK3_A.intVar = 4;
			fK3_A.strVar = "aa";
			FK3_A a = fK3_A;
			FK3_B fK3_B = new FK3_B();
			fK3_B.x = 3;
			fK3_B.a = a;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["key1"] = 7;
			dictionary["key2"] = "hello";
			dictionary["key3"] = fK3_B;
			string text = JsonWriter.Serialize(dictionary);
			Dictionary<string, object> value = JsonReader.Deserialize<Dictionary<string, object>>(text);
			string message = JsonWriter.Serialize(value);
			if (flag)
			{
				UnityEngine.Debug.Log(text);
			}
			if (flag)
			{
				UnityEngine.Debug.Log(message);
			}
		}
	}
}
