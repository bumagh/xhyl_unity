using System;
using System.Collections;
using UnityEngine;

namespace FullInspector.Tests
{
	public abstract class fiBaseEditorTest
	{
		public event Action OnCleanup;

		public abstract IEnumerable ExecuteTest(MonoBehaviour target);

		public void Cleanup()
		{
			if (this.OnCleanup != null)
			{
				this.OnCleanup();
			}
		}

		public virtual bool WantsEvent(EventType eventType)
		{
			return eventType == EventType.Repaint;
		}
	}
}
