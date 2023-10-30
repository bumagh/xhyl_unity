using System.Collections;
using UnityEngine;

public class PTM_XTask
{
	public delegate void FinishedHandler(bool manual);

	public FinishedHandler Finished;

	private PTM_XTaskManager.TaskState task;

	private Coroutine _co;

	public bool Running => task.Running;

	public bool Paused => task.Paused;

	public PTM_XTask(IEnumerator c, bool autoRun = true)
	{
		task = PTM_XTaskManager.CreateTask(c);
		task.Finished += TaskFinished;
		if (autoRun)
		{
			Run();
		}
	}

	public void Run()
	{
		task.Run();
	}

	public void Stop(bool doNothing = false)
	{
		task.Stop(doNothing);
	}

	public void Pause()
	{
		UnityEngine.Debug.LogError("暂停");
	}

	public void Unpause()
	{
		UnityEngine.Debug.LogError("取消暂停");
	}

	private void TaskFinished(bool manual)
	{
		Finished?.Invoke(manual);
	}
}
