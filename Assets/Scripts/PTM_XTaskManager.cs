using System.Collections;
using UnityEngine;

internal class PTM_XTaskManager : MonoBehaviour
{
	public class TaskState
	{
		public delegate void FinishedHandler(bool manual);

		private IEnumerator coroutine;

		private bool running;

		private bool paused;

		private bool stopped;

		public Coroutine executeCoroutine;

		public bool Running => running;

		public bool Paused => paused;

		public event FinishedHandler Finished;

		public TaskState(IEnumerator c)
		{
			coroutine = c;
		}

		public void Pause()
		{
			UnityEngine.Debug.LogError("暂停");
		}

		public void Unpause()
		{
			UnityEngine.Debug.LogError("取消暂停");
		}

		public Coroutine Run()
		{
			running = true;
			executeCoroutine = singleton.StartCoroutine(CallWrapper());
			return executeCoroutine;
		}

		public void Stop(bool doNothing = false)
		{
			UnityEngine.Debug.Log($"Stop> doNothing: {doNothing}, executeCoroutine: {executeCoroutine == null}");
			stopped = true;
			running = false;
			if (doNothing)
			{
				try
				{
					if (executeCoroutine != null)
					{
						singleton.StopCoroutine(executeCoroutine);
					}
					executeCoroutine = null;
				}
				catch
				{
				}
				UnityEngine.Debug.Log("doNothing success");
			}
			else
			{
				this.Finished?.Invoke(stopped);
			}
		}

		private IEnumerator CallWrapper()
		{
			yield return null;
			IEnumerator e = coroutine;
			while (running)
			{
				if (paused)
				{
					yield return null;
				}
				else if (e != null && e.MoveNext())
				{
					yield return e.Current;
				}
				else
				{
					running = false;
				}
			}
			executeCoroutine = null;
			FinishedHandler handler = this.Finished;
			if (handler != null && !stopped)
			{
				handler(manual: false);
			}
		}
	}

	private static PTM_XTaskManager singleton;

	public static TaskState CreateTask(IEnumerator coroutine)
	{
		if (singleton == null)
		{
			GameObject gameObject = new GameObject("XTaskManager");
			singleton = gameObject.AddComponent<PTM_XTaskManager>();
		}
		return new TaskState(coroutine);
	}

	private void OnDestroy()
	{
		singleton = null;
	}
}
