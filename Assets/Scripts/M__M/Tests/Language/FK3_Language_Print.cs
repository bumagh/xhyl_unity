using HW3L;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace M__M.Tests.Language
{
	public class FK3_Language_Print : MonoBehaviour
	{
		private void Start()
		{
			Test_PrintIntArray();
		}

		private void Update()
		{
		}

		private void Test_PrintIntArray()
		{
			UnityEngine.Debug.Log("Test_PrintIntArray");
			int[] array = new int[4]
			{
				1,
				2,
				3,
				4
			};
			UnityEngine.Debug.Log(array);
			UnityEngine.Debug.Log(array.ToString());
			List<int> list = new List<int>(array);
			UnityEngine.Debug.Log(list);
			UnityEngine.Debug.Log(string.Join(",", (from x in list
				select x.ToString()).ToArray()));
			UnityEngine.Debug.Log(string.Join(",", Array.ConvertAll(array, (int item) => item.ToString())));
			UnityEngine.Debug.Log(array.JoinStrings());
			UnityEngine.Debug.Log(list.JoinStrings());
		}
	}
}
