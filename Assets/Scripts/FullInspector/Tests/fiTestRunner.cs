using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.Tests
{
	public class fiTestRunner : MonoBehaviour
	{
		public class RunningTest
		{
			public fiBaseEditorTest Test;

			public IEnumerator Progress;
		}

		public List<RunningTest> RunningTests = new List<RunningTest>();
	}
}
