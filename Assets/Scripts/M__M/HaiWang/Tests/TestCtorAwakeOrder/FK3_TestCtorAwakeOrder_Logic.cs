using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Tests.TestCtorAwakeOrder
{
	public class FK3_TestCtorAwakeOrder_Logic : MonoBehaviour
	{
		[SerializeField]
		private GameObject disabledObj;

		[SerializeField]
		private FK3_FooA fooA1;

		private FK3_FooA fooA2;

		private FK3_FooA fooA3 = new FK3_FooA("mmm");

		[SerializeField]
		private FK3_FooA fooA4 = new FK3_FooA("xyz");

		private FK3_FooB fooB1;

		private FK3_TestCtorAwakeOrder_Logic()
		{
			UnityEngine.Debug.Log("FK3_TestCtorAwakeOrder_Logic Ctor");
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
