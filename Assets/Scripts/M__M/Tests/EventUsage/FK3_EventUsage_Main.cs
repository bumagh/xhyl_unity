using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace M__M.Tests.EventUsage
{
	public class FK3_EventUsage_Main : MonoBehaviour
	{
		private Action Event_D;

		private UnityEvent UnityEvent_X;

		private UnityEvent UnityEvent_Y;

		private event Action Event_A;

		private event Action Event_B;

		private event Action Event_C;

		private void Awake()
		{
		}

		private void Start()
		{
			StartCoroutine(IE_UnityActionEvent_Test());
		}

		private void Update()
		{
		}

		private void HandleEvent_A()
		{
			UnityEngine.Debug.Log("HandleEvent_A");
		}

		private void HandleEvent_B()
		{
			UnityEngine.Debug.Log("HandleEvent_B");
		}

		private void HandleEvent_C()
		{
			UnityEngine.Debug.Log("HandleEvent_C");
		}

		private void HandleEvent_X()
		{
			UnityEngine.Debug.Log("HandleEvent_X");
		}

		private void HandleEvent_Y()
		{
			UnityEngine.Debug.Log("HandleEvent_Y");
		}

		private void Register_Event()
		{
		}

		private void UnRegister_Event()
		{
		}

		private IEnumerator IE_SystemActionEvent_Test()
		{
			UnityEngine.Debug.Log("IE_SystemActionEvent_Test");
			Event_A += HandleEvent_A;
			if (this.Event_A != null)
			{
				this.Event_A();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			Event_B += HandleEvent_B;
			Event_B += HandleEvent_B;
			if (this.Event_B != null)
			{
				this.Event_B();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			Event_B -= HandleEvent_B;
			if (this.Event_B != null)
			{
				this.Event_B();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			Event_B -= HandleEvent_B;
			if (this.Event_B != null)
			{
				this.Event_B();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			Event_B -= HandleEvent_B;
			if (this.Event_B != null)
			{
				this.Event_B();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			Event_C += delegate
			{
				UnityEngine.Debug.Log("HandleEvent_Cc");
			};
			if (this.Event_C != null)
			{
				this.Event_C();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			this.Event_C = null;
			if (this.Event_C != null)
			{
				this.Event_C();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			Event_D = (Action)Delegate.Combine(Event_D, (Action)delegate
			{
				UnityEngine.Debug.Log("HandleEvent_Dd");
			});
			if (Event_D != null)
			{
				Event_D();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			Event_D = null;
			if (Event_D != null)
			{
				Event_D();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			yield break;
		}

		private IEnumerator IE_UnityActionEvent_Test()
		{
			UnityEngine.Debug.Log("IE_UnityActionEvent_Test");
			UnityEvent_X = new UnityEvent();
			UnityEvent_X.AddListener(HandleEvent_X);
			UnityEvent_X.AddListener(HandleEvent_X);
			if (UnityEvent_X != null)
			{
				UnityEvent_X.Invoke();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			UnityEvent_X.RemoveListener(HandleEvent_X);
			if (UnityEvent_X != null)
			{
				UnityEvent_X.Invoke();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			UnityEvent_X.AddListener(delegate
			{
				UnityEngine.Debug.Log("lambda HandleEvent_Xx");
			});
			if (UnityEvent_X != null)
			{
				UnityEvent_X.Invoke();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			UnityEvent_X.RemoveAllListeners();
			if (UnityEvent_X != null)
			{
				UnityEvent_X.Invoke();
			}
			UnityEngine.Debug.Log("-".Repeat(20));
			yield break;
		}
	}
}
