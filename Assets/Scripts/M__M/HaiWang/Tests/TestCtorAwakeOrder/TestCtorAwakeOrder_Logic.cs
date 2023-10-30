using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Tests.TestCtorAwakeOrder
{
	public class TestCtorAwakeOrder_Logic : MonoBehaviour
	{
		[SerializeField]
		private GameObject disabledObj;

		[SerializeField]
		private FooA fooA1;

		private FooA fooA2;

		private FooA fooA3 = new FooA("mmm");

		[SerializeField]
		private FooA fooA4 = new FooA("xyz");

		private FooB fooB1;

		private TestCtorAwakeOrder_Logic()
		{
			UnityEngine.Debug.Log("TestCtorAwakeOrder_Logic Ctor");
		}

		private void Awake()
		{
			Test_1();
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void Test_1()
		{
			StartCoroutine(IE_Test_1());
		}

		private IEnumerator IE_Test_1()
		{
			UnityEngine.Debug.Log("IE_Test_1");
			disabledObj.SetActive(value: true);
			yield break;
		}
	}
}
