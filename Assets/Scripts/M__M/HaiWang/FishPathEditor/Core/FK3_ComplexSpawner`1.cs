using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FK3_ComplexSpawner<T> : FK3_ISpawnerPlayable<T>, FK3_ISpawnerProcess<T>, FK3_ICheckValid
	{
		public delegate void ForEachCallback(FK3_SpawnerBaseItem<T> item);

		public List<FK3_SpawnerBase<T>> spawners = new List<FK3_SpawnerBase<T>>();

		public bool printLog;

		protected string _errMessage = string.Empty;

		public Action<FK3_ComplexSpawner<T>> onFinish;

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
				FK3_SpawnerBase<T> fK3_SpawnerBase = spawners[i];
				if (fK3_SpawnerBase == null)
				{
					if (printLog)
					{
						UnityEngine.Debug.Log(FK3_LogHelper.Red("spawners[{0}] is null", i));
					}
				}
				else if (!fK3_SpawnerBase.CheckValid())
				{
					if (printLog)
					{
						UnityEngine.Debug.Log(FK3_LogHelper.Red("spawners[{0}] is invalid. error:{1}", i, fK3_SpawnerBase.GetError()));
					}
				}
				else
				{
					num++;
					fK3_SpawnerBase.Play(playTime);
				}
			}
		}

		public virtual void SetProcess(Action<FK3_ProcessData<T>> processAction)
		{
			if (spawners == null)
			{
				return;
			}
			int count = spawners.Count;
			for (int i = 0; i < count; i++)
			{
				FK3_SpawnerBase<T> fK3_SpawnerBase = spawners[i];
				if (fK3_SpawnerBase != null && fK3_SpawnerBase.CheckValid())
				{
					fK3_SpawnerBase.SetProcess(processAction);
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
				FK3_SpawnerBase<T> fK3_SpawnerBase = spawners[i];
				if (fK3_SpawnerBase != null && fK3_SpawnerBase.CheckValid())
				{
					num++;
					fK3_SpawnerBase.ClearError();
					fK3_SpawnerBase.Stop();
					if (printLog && fK3_SpawnerBase.HasError())
					{
						UnityEngine.Debug.Log($"stop error:{fK3_SpawnerBase.GetError()}");
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
				FK3_SpawnerBase<T> fK3_SpawnerBase = spawners[i];
				if (fK3_SpawnerBase != null && fK3_SpawnerBase.CheckValid())
				{
					num++;
					action(new FK3_SpawnerBaseItem<T>(fK3_SpawnerBase, i, num));
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
				FK3_SpawnerBase<T> fK3_SpawnerBase = spawners[i];
				action(new FK3_SpawnerBaseItem<T>(fK3_SpawnerBase, i, num));
				if (fK3_SpawnerBase != null && fK3_SpawnerBase.CheckValid())
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
			foreach (FK3_SpawnerBase<T> spawner in spawners)
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
			foreach (FK3_SpawnerBase<T> spawner in spawners)
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
				FK3_SpawnerBase<T> fK3_SpawnerBase = spawners[i];
				if (fK3_SpawnerBase != null && fK3_SpawnerBase.CheckValid() && fK3_SpawnerBase.IsPlaying)
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
