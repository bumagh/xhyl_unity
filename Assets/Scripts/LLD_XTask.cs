using System.Collections;
using UnityEngine;

public class LLD_XTask
{
	public delegate void FinishedHandler(bool manual);

	public FinishedHandler Finished;

	private LLD_XTaskManager.TaskState task;

	private Coroutine _co;

	public bool Running => task.Running;

	public bool Paused => task.Paused;

	public LLD_XTask(IEnumerator c, bool autoRun = true)
	{
		task = LLD_XTaskManager.CreateTask(c);
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
		task.Pause();
	}

	public void Unpause()
	{
		task.Unpause();
	}

	private void TaskFinished(bool manual)
	{
		Finished?.Invoke(manual);
	}
}
