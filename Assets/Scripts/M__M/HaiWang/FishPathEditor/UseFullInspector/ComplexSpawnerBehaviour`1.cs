using FullInspector;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class ComplexSpawnerBehaviour<T> : BaseBehavior<FullSerializerSerializer>, ISpawnerPlayable<T>, ISpawnerProcess<T>, ICheckValid
	{
		[SerializeField]
		protected List<ISpawner<T>> _spawners = new List<ISpawner<T>>();

		protected string _errMessage = string.Empty;

		protected bool isRunning => Application.isPlaying;

		public bool isValid => CheckValid();

		public bool IsPlaying => IsAnyPlaying();

		public virtual bool CheckValid()
		{
			return !ErrorJudge(_spawners == null, "_spawners is null") && !ErrorJudge(_spawners.Count == 0, "_spawners.Count is 0");
		}

		public virtual void Play()
		{
			Play(0f);
		}

		public virtual void Play(float playTime)
		{
			if (_spawners == null)
			{
				return;
			}
			int count = _spawners.Count;
			for (int i = 0; i < count; i++)
			{
				ISpawner<T> spawner = _spawners[i];
				if (spawner != null && spawner.CheckValid())
				{
					spawner.Play(playTime);
				}
			}
		}

		public virtual void SetProcess(Action<ProcessData<T>> processAction)
		{
			if (_spawners == null)
			{
				return;
			}
			int count = _spawners.Count;
			for (int i = 0; i < count; i++)
			{
				ISpawner<T> spawner = _spawners[i];
				if (spawner != null && spawner.CheckValid())
				{
					spawner.SetProcess(processAction);
				}
			}
		}

		public virtual void Stop()
		{
			if (_spawners == null)
			{
				return;
			}
			int count = _spawners.Count;
			for (int i = 0; i < count; i++)
			{
				ISpawner<T> spawner = _spawners[i];
				if (spawner != null && spawner.CheckValid())
				{
					spawner.Stop();
				}
			}
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

		protected bool ErrorJudge(bool result, string errMsg)
		{
			_errMessage = errMsg;
			return result;
		}

		protected bool IsAnyPlaying()
		{
			if (_spawners == null)
			{
				return false;
			}
			int count = _spawners.Count;
			for (int i = 0; i < count; i++)
			{
				ISpawner<T> spawner = _spawners[i];
				if (spawner != null && spawner.CheckValid() && spawner.IsPlaying)
				{
					return true;
				}
			}
			return false;
		}
	}
}
