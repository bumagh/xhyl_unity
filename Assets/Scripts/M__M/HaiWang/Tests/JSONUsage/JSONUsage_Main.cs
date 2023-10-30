using FullInspector;
using JsonFx.Json;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace M__M.HaiWang.Tests.JSONUsage
{
	public class JSONUsage_Main : BaseBehavior<FullSerializerSerializer>
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
			A a = new A();
			a.intVar = 4;
			a.strVar = "aa";
			A a2 = a;
			B b = new B();
			b.x = 3;
			b.a = a2;
			string text = JsonWriter.Serialize(b);
			B b2 = JsonReader.Deserialize<B>(text);
			string message = JsonWriter.Serialize(b);
			if (debug)
			{
				UnityEngine.Debug.Log(text);
			}
			if (debug)
			{
				UnityEngine.Debug.Log(message);
			}
			string value = "{\"x\":3,\"a\":{ \"intVarx\":4,\"strVarx\":\"aa\"} }";
			B value2 = JsonReader.Deserialize<B>(value);
			string message2 = JsonWriter.Serialize(value2);
			if (debug)
			{
				UnityEngine.Debug.Log(message2);
			}
		}

		private void _Test2()
		{
			bool flag = true;
			A a = new A();
			a.intVar = 4;
			a.strVar = "aa";
			A a2 = a;
			B b = new B();
			b.x = 3;
			b.a = a2;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["key1"] = 7;
			dictionary["key2"] = "hello";
			dictionary["key3"] = b;
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
