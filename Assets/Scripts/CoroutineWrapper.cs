using System;
using System.Collections;
using UnityEngine;

public class CoroutineWrapper
{
	public class _Helper : MonoBehaviour
	{
	}

	public delegate void FinishedHandler(bool manual);

	private static _Helper _CoroutineHelper;

	private IEnumerator coroutine;

	internal Coroutine _internalCo;

	private bool running;

	private bool paused;

	private bool stopped;

	private Task _task;

	public bool Running => running;

	public bool Paused => paused;

	public event FinishedHandler Finished;

	public CoroutineWrapper(IEnumerator c, Task task)
	{
		coroutine = c;
		_task = task;
	}

	internal static void _Init()
	{
		if (_CoroutineHelper == null)
		{
			GameObject gameObject = new GameObject("_CoroutineHelper");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			HW2_GVars.dontDestroyOnLoadList.Add(gameObject);
			_CoroutineHelper = gameObject.AddComponent<_Helper>();
		}
	}

	public void Pause()
	{
		paused = true;
	}

	public void Unpause()
	{
		paused = false;
	}

	public void Start()
	{
		running = true;
		_internalCo = _CoroutineHelper.StartCoroutine(CallWrapper());
	}

	public void Stop()
	{
		stopped = true;
		running = false;
		if (_internalCo != null)
		{
			_CoroutineHelper.StopCoroutine(_internalCo);
			_internalCo = null;
		}
	}

	private IEnumerator CallWrapper()
	{
		yield return null;
		IEnumerator enumerator = coroutine;
		while (running)
		{
			if (paused)
			{
				yield return null;
			}
			else if (enumerator != null)
			{
				bool yieldNextSucess = false;
				try
				{
					yieldNextSucess = enumerator.MoveNext();
				}
				catch (Exception e)
				{
					_task.OnException(e);
				}
				if (yieldNextSucess)
				{
					yield return enumerator.Current;
				}
				else
				{
					running = false;
				}
			}
			else
			{
				running = false;
			}
		}
		this.Finished?.Invoke(stopped);
		_internalCo = null;
	}
}
