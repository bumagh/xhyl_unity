using FullInspector;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_ComplexSpawnerBehaviour<T> : BaseBehavior<FullSerializerSerializer>, FK3_ISpawnerPlayable<T>, FK3_ISpawnerProcess<T>, FK3_ICheckValid
	{
		[SerializeField]
		protected List<FK3_ISpawner<T>> _spawners = new List<FK3_ISpawner<T>>();

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
				FK3_ISpawner<T> fK3_ISpawner = _spawners[i];
				if (fK3_ISpawner != null && fK3_ISpawner.CheckValid())
				{
					fK3_ISpawner.Play(playTime);
				}
			}
		}

		public virtual void SetProcess(Action<FK3_ProcessData<T>> processAction)
		{
			if (_spawners == null)
			{
				return;
			}
			int count = _spawners.Count;
			for (int i = 0; i < count; i++)
			{
				FK3_ISpawner<T> fK3_ISpawner = _spawners[i];
				if (fK3_ISpawner != null && fK3_ISpawner.CheckValid())
				{
					fK3_ISpawner.SetProcess(processAction);
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
				FK3_ISpawner<T> fK3_ISpawner = _spawners[i];
				if (fK3_ISpawner != null && fK3_ISpawner.CheckValid())
				{
					fK3_ISpawner.Stop();
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
				FK3_ISpawner<T> fK3_ISpawner = _spawners[i];
				if (fK3_ISpawner != null && fK3_ISpawner.CheckValid() && fK3_ISpawner.IsPlaying)
				{
					return true;
				}
			}
			return false;
		}
	}
}
