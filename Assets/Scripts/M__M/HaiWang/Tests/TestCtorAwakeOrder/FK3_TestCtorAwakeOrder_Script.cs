using UnityEngine;

namespace M__M.HaiWang.Tests.TestCtorAwakeOrder
{
	public class FK3_TestCtorAwakeOrder_Script : MonoBehaviour
	{
		private FK3_TestCtorAwakeOrder_Script()
		{
			UnityEngine.Debug.Log($"FK3_TestCtorAwakeOrder_Script Ctor");
		}

		private void Awake()
		{
			UnityEngine.Debug.Log($"obj[{base.gameObject.name}] Awake");
		}

		private void Start()
		{
			UnityEngine.Debug.Log($"obj[{base.gameObject.name}] Start");
		}

		private void Update()
		{
		}
	}
}
