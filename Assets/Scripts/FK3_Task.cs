using System;
using System.Collections;
using UnityEngine;

public class FK3_Task
{
	public delegate void FinishedHandler(bool manual);

	private FK3_CoroutineWrapper _coroutineWrapper;

	public bool isRunning => _coroutineWrapper.Running;

	public bool isPaused => _coroutineWrapper.Paused;

	public Coroutine coroutine => _coroutineWrapper._internalCo;

	public event FinishedHandler Finished;

	public event Action<Exception> ExceptionHandler;

	public FK3_Task(IEnumerator enumerator, bool autoStart = true)
	{
		FK3_CoroutineWrapper._Init();
		_coroutineWrapper = new FK3_CoroutineWrapper(enumerator, this);
		_coroutineWrapper.Finished += TaskFinished;
		if (autoStart)
		{
			Start();
		}
	}

	public void Start()
	{
		_coroutineWrapper.Start();
	}

	public void Stop()
	{
		_coroutineWrapper.Stop();
	}

	public void Pause()
	{
		_coroutineWrapper.Pause();
	}

	public void Unpause()
	{
		_coroutineWrapper.Unpause();
	}

	internal void OnException(Exception e)
	{
		if (this.ExceptionHandler != null)
		{
			this.ExceptionHandler(e);
		}
		else
		{
			UnityEngine.Debug.LogError(e);
		}
	}

	private void TaskFinished(bool manual)
	{
		this.Finished?.Invoke(manual);
	}
}
