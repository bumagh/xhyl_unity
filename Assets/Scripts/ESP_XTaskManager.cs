using System.Collections;
using UnityEngine;

internal class ESP_XTaskManager : MonoBehaviour
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
			paused = true;
		}

		public void Unpause()
		{
			paused = false;
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

	private static ESP_XTaskManager singleton;

	public static TaskState CreateTask(IEnumerator coroutine)
	{
		if (singleton == null)
		{
			GameObject gameObject = new GameObject("XTaskManager");
			singleton = gameObject.AddComponent<ESP_XTaskManager>();
		}
		return new TaskState(coroutine);
	}

	private void OnDestroy()
	{
		singleton = null;
	}
}
