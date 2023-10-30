using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public class ComplexSpawner<T> : ISpawnerPlayable<T>, ISpawnerProcess<T>, ICheckValid
	{
		public delegate void ForEachCallback(SpawnerBaseItem<T> item);

		public List<SpawnerBase<T>> spawners = new List<SpawnerBase<T>>();

		public bool printLog;

		protected string _errMessage = string.Empty;

		public Action<ComplexSpawner<T>> onFinish;

		public bool isRunning => Application.isPlaying;

		public bool isValid => CheckValid();

		public bool IsPlaying => IsAnyPlaying();

		public virtual void SetPrintLog(bool value)
		{
			printLog = value;
		}

		public virtual bool CheckValid()
		{
			return !ErrorJudge(spawners == null, "spawners is null") && !ErrorJudge(spawners.Count == 0, "spawners.Count is 0");
		}

		public virtual bool HasError()
		{
			return _errMessage != null && _errMessage.Length != 0;
		}

		public virtual string GetError()
		{
			return _errMessage;
		}

		public virtual void ClearError()
		{
			_errMessage = string.Empty;
		}

		public virtual void Play()
		{
			Play(0f);
		}

		public virtual void Play(float playTime)
		{
			if (spawners == null)
			{
				return;
			}
			int count = spawners.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				SpawnerBase<T> spawnerBase = spawners[i];
				if (spawnerBase == null)
				{
					if (printLog)
					{
						UnityEngine.Debug.Log(HW2_LogHelper.Red("spawners[{0}] is null", i));
					}
				}
				else if (!spawnerBase.CheckValid())
				{
					if (printLog)
					{
						UnityEngine.Debug.Log(HW2_LogHelper.Red("spawners[{0}] is invalid. error:{1}", i, spawnerBase.GetError()));
					}
				}
				else
				{
					num++;
					spawnerBase.Play(playTime);
				}
			}
			if (printLog)
			{
				UnityEngine.Debug.Log($"ComplexSpawner.Play> spawners[{num}/{count}] play({playTime})");
			}
		}

		public virtual void SetProcess(Action<ProcessData<T>> processAction)
		{
			if (spawners == null)
			{
				return;
			}
			int count = spawners.Count;
			for (int i = 0; i < count; i++)
			{
				SpawnerBase<T> spawnerBase = spawners[i];
				if (spawnerBase != null && spawnerBase.CheckValid())
				{
					spawnerBase.SetProcess(processAction);
				}
			}
		}

		public virtual void Stop()
		{
			if (spawners == null)
			{
				return;
			}
			int count = spawners.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				SpawnerBase<T> spawnerBase = spawners[i];
				if (spawnerBase != null && spawnerBase.CheckValid())
				{
					num++;
					spawnerBase.ClearError();
					spawnerBase.Stop();
					if (printLog && spawnerBase.HasError())
					{
						UnityEngine.Debug.Log($"stop error:{spawnerBase.GetError()}");
					}
				}
			}
			if (printLog)
			{
				UnityEngine.Debug.Log($"ComplexSpawner.Stop> spawners[{num}/{count}] Stop");
			}
		}

		public virtual void ForEachValid(ForEachCallback action)
		{
			if (spawners == null || action == null)
			{
				return;
			}
			int count = spawners.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				SpawnerBase<T> spawnerBase = spawners[i];
				if (spawnerBase != null && spawnerBase.CheckValid())
				{
					num++;
					action(new SpawnerBaseItem<T>(spawnerBase, i, num));
				}
			}
		}

		public virtual void ForEach(ForEachCallback action)
		{
			if (spawners == null || action == null)
			{
				return;
			}
			int count = spawners.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				SpawnerBase<T> spawnerBase = spawners[i];
				action(new SpawnerBaseItem<T>(spawnerBase, i, num));
				if (spawnerBase != null && spawnerBase.CheckValid())
				{
					num++;
				}
			}
		}

		public virtual int Count()
		{
			if (spawners == null)
			{
				return -1;
			}
			int num = 0;
			foreach (SpawnerBase<T> spawner in spawners)
			{
				if (spawner != null && spawner.CheckValid() && spawner.Count() > 0)
				{
					num += spawner.Count();
				}
			}
			return num;
		}

		public virtual int ValidSubCount()
		{
			if (spawners == null)
			{
				return -1;
			}
			int num = 0;
			foreach (SpawnerBase<T> spawner in spawners)
			{
				if (spawner != null && spawner.CheckValid() && spawner.Count() > 0)
				{
					num++;
				}
			}
			return num;
		}

		protected virtual bool IsAnyPlaying()
		{
			if (spawners == null)
			{
				return false;
			}
			int count = spawners.Count;
			for (int i = 0; i < count; i++)
			{
				SpawnerBase<T> spawnerBase = spawners[i];
				if (spawnerBase != null && spawnerBase.CheckValid() && spawnerBase.IsPlaying)
				{
					return true;
				}
			}
			return false;
		}

		protected virtual bool ErrorJudge(bool result, string errMsg)
		{
			_errMessage = errMsg;
			return result;
		}
	}
}
