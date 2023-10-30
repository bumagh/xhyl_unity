using FullInspector;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FormationBehaviour<T> : BaseBehavior<FullSerializerSerializer>, ISpawnerPlayable<T>, ISpawnerProcess<T>, ICheckValid
	{
		public class FormationContext<TT> : SpawnerContext
		{
			public FormationBehaviour<TT> parent;
		}

		public int id;

		protected ComplexSpawner<T> _complexSpawner;

		public bool printLog;

		public bool pringLogChild;

		protected string _errMessage = string.Empty;

		protected int _startId;

		protected bool _prepared;

		protected int _processCount;

		protected int _despawnCount;

		protected int _ignoreCount;

		protected int _expectCount;

		protected bool _spawnFinished;

		protected bool _despawnFinished;

		protected float _startTime;

		public float activeTimeInSec;

		public Action<FormationBehaviour<T>> onSpawnFinish;

		public Action<FormationBehaviour<T>> onAllSpawnDespawned;

		public Action<ProcessData<T>> processAction;

		public Action<FormationBehaviour<T>> onFormationDespawn;

		public Func<int, T, T> generatorFunc;

		public bool isRunning => Application.isPlaying;

		public bool IsPlaying => CheckValid() && _complexSpawner.IsPlaying;

		public int startId
		{
			get
			{
				return _startId;
			}
			set
			{
				_startId = value;
			}
		}

		public virtual bool CheckValid()
		{
			return !ErrorJudge(_complexSpawner == null, "_complexSpawner is null") && !ErrorJudge(!_complexSpawner.CheckValid(), "_complexSpawner is invalid. " + _complexSpawner.GetError());
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
			_errMessage = $"[{GetIdentity()}] {errMsg}";
			return result;
		}

		public virtual void SetPrintLog(bool value)
		{
			printLog = value;
			if (_complexSpawner != null)
			{
				_complexSpawner.SetPrintLog(value);
			}
		}

		public virtual void Play()
		{
			Play(0f);
		}

		public virtual void Play(float playTime)
		{
			DoReset_Formation();
			_expectCount = _complexSpawner.Count();
			int num = _complexSpawner.ValidSubCount();
			if (printLog)
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Magenta("{0}[id:{1}] Play({2}), [subs:{6},count:{3}, from:{4}, to:{5}]", GetType().Name, id, playTime, _expectCount, startId, startId + _expectCount - 1, num));
			}
			_complexSpawner.SetPrintLog(printLog);
			int spawnerIndex = startId;
			_complexSpawner.ForEachValid(delegate(SpawnerBaseItem<T> _)
			{
				_.spawner.startIndex = spawnerIndex;
				spawnerIndex += _.spawner.Count();
				_.spawner.SetPrintLog(pringLogChild);
				_.spawner.GetManager().gameObject.SetActive(value: true);
				_.spawner.SetGeneratorFunc(generatorFunc);
			});
			_startTime = Time.time;
			_complexSpawner.Play(playTime);
		}

		public virtual int Count()
		{
			if (_complexSpawner == null)
			{
				return 0;
			}
			return _complexSpawner.Count();
		}

		public virtual int ValidSubCount()
		{
			if (_complexSpawner == null)
			{
				return 0;
			}
			return _complexSpawner.ValidSubCount();
		}

		public virtual void SetProcess(Action<ProcessData<T>> processAction)
		{
			this.processAction = processAction;
			_complexSpawner.SetProcess(OnProcess);
		}

		protected virtual void OnProcess(ProcessData<T> data)
		{
			_processCount++;
			if (data.ignore)
			{
				_ignoreCount++;
			}
			if (_expectCount == _processCount)
			{
				_spawnFinished = true;
				UnityEngine.Debug.Log(HW2_LogHelper.Cyan("{0} all spawn finished", GetIdentity()));
				if (onSpawnFinish != null)
				{
					onSpawnFinish(this);
				}
			}
			if (processAction != null)
			{
				processAction(data);
			}
		}

		public virtual void Stop()
		{
			_complexSpawner.Stop();
			Finish();
		}

		public virtual void StopAndClear()
		{
		}

		public virtual string GetIdentity()
		{
			return $"Formation[id:{id}]";
		}

		public virtual void Prepare()
		{
			_prepared = true;
		}

		public virtual void Despawn()
		{
			if (onFormationDespawn != null)
			{
				onFormationDespawn(this);
			}
		}

		public virtual void Finish()
		{
		}

		public virtual void DoReset_Event()
		{
			onAllSpawnDespawned = null;
			onFormationDespawn = null;
			onSpawnFinish = null;
		}

		public virtual void DoReset_Formation()
		{
			_processCount = 0;
			_despawnCount = 0;
			_ignoreCount = 0;
			_spawnFinished = false;
			_despawnFinished = false;
		}

		protected override void Awake()
		{
			base.Awake();
			if (_complexSpawner == null)
			{
				_complexSpawner = new ComplexSpawner<T>();
			}
		}

		protected virtual void Start()
		{
		}

		[InspectorShowIf("isRunning")]
		[InspectorName("Play")]
		[InspectorButton]
		protected virtual void PlayBtn()
		{
			if (!CheckValid())
			{
				UnityEngine.Debug.Log($"{GetIdentity()}.operation[Play] cannot be executed. Error:{GetError()}");
				return;
			}
			if (IsPlaying)
			{
				UnityEngine.Debug.Log($"{GetIdentity()}.operation[Play] cannot be executed. please stop first");
				return;
			}
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: true);
			}
			UnityEngine.Debug.Log($"{GetIdentity()}.PlayBtn execute");
			Play();
		}

		[InspectorShowIf("isRunning")]
		[InspectorName("Stop")]
		[InspectorButton]
		private void StopBtn()
		{
			UnityEngine.Debug.Log($"{GetIdentity()}.StopBtn execute");
			Stop();
		}

		[InspectorButton]
		[InspectorName("Stop And Clear")]
		[InspectorShowIf("isRunning")]
		private void StopAndClearBtn()
		{
			StopAndClear();
		}
	}
}
