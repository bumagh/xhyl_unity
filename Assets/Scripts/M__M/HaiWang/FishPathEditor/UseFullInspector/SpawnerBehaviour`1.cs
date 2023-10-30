using FullInspector;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class SpawnerBehaviour<T> : fiSimpleSingletonBehaviour<SpawnerBehaviour<T>>, ISpawner<T>, ICheckValid, ISpawnerPlayable<T>, ISpawnerSetup<T>, ISpawnerProcess<T>
	{
		public class SpawnerBehaviourContext<TT> : SpawnerContext
		{
			public SpawnerBehaviour<TT> spawnerBehaviour;

			public SpawnerBehaviourContext(SpawnerBehaviour<TT> spawnerBehaviour)
			{
				this.spawnerBehaviour = spawnerBehaviour;
			}
		}

		public string id = string.Empty;

		[InspectorDisabled]
		public string contextId = string.Empty;

		public bool playOnStart = true;

		[SerializeField]
		protected SpawnerData _spawnerData;

		[SerializeField]
		protected IGenerator<T> _generator;

		protected SpawnerBase<T> _spawner;

		protected MonoBehaviour _manager;

		public bool printLog;

		protected string _errMessage = string.Empty;

		protected Action<ProcessData<T>> _processAction;

		public bool IsValid => CheckValid();

		public bool IsPlaying => _spawner != null && _spawner.IsPlaying;

		public virtual bool Inject(SpawnerBase<T> spawner, SpawnerData data, IGenerator<T> generator, Action<ProcessData<T>> processAction)
		{
			_spawner = spawner;
			_spawnerData = data;
			_generator = generator;
			_processAction = processAction;
			_spawner.Setup(data, generator, this, processAction);
			return CheckValid();
		}

		public virtual bool Inject(SpawnerBase<T> spawner, Action<ProcessData<T>> processAction)
		{
			_spawner = spawner;
			_processAction = processAction;
			if (!CheckValid())
			{
				UnityEngine.Debug.LogError(string.Empty);
				return false;
			}
			_spawner.Setup(_spawnerData, _generator, this, processAction);
			return true;
		}

		protected override void Awake()
		{
			base.Awake();
		}

		protected virtual bool CanPlayOnStart()
		{
			return playOnStart;
		}

		protected virtual void Start()
		{
			if (CanPlayOnStart())
			{
				if (!IsValid)
				{
					UnityEngine.Debug.LogError($"{GetIdentity()} is not ready! Please check!");
				}
				else
				{
					Play();
				}
			}
		}

		public virtual void Prepare()
		{
			if (_spawner != null)
			{
				_spawner.id = $"{contextId}_{id}";
			}
		}

		public virtual void Play(float playTime)
		{
			Prepare();
			if (printLog)
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Magenta("{0} Play({1})", GetIdentity(), playTime));
			}
			_spawner.Play(playTime);
		}

		public virtual void Play()
		{
			Play(0f);
		}

		public virtual void Stop()
		{
			_spawner.Stop();
			if (printLog)
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Magenta("{0} Stop", GetIdentity()));
			}
		}

		public virtual bool CheckValid()
		{
			return !ErrorJudge(_spawnerData == null, "_spawnerData is null") && !ErrorJudge(_generator == null, "_generator is null") && !ErrorJudge(_spawner == null, "_spawner is null");
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

		protected virtual bool ErrorJudge(bool result, string errMsg)
		{
			_errMessage = $"[{GetIdentity()}] {errMsg}";
			return result;
		}

		public void SetSpawnerData(SpawnerData data)
		{
			_spawnerData = data;
		}

		public void SetGenerator(IGenerator<T> generator)
		{
			_generator = generator;
		}

		public void SetManager(MonoBehaviour manager)
		{
			_manager = manager;
		}

		public virtual void SetProcess(Action<ProcessData<T>> processAction)
		{
			_processAction = processAction;
			_spawner.SetProcess(processAction);
		}

		public virtual void SetAfterProcess(Action<ProcessData<T>> afterProcessAction)
		{
			_spawner.SetAfterProcess(afterProcessAction);
		}

		public virtual Action<ProcessData<T>> GetProcess()
		{
			return _processAction;
		}

		public void SetupSpawner()
		{
			_spawner.Setup(_spawnerData, _generator, _manager, _processAction);
		}

		public virtual void Setup(SpawnerData data, IGenerator<T> generator, MonoBehaviour manager, Action<ProcessData<T>> processAction)
		{
			_spawnerData = data;
			_generator = generator;
			_manager = manager;
			_processAction = processAction;
			_spawner.Setup(data, generator, manager, processAction);
		}

		public virtual void Setup(SpawnerData data, IGenerator<T> generator, MonoBehaviour manager)
		{
			_spawner.Setup(data, generator, manager);
		}

		public SpawnerBase<T> GetSpawner()
		{
			return _spawner;
		}

		public IGenerator<T> GetGenerator()
		{
			return _generator;
		}

		public SpawnerData GetSpawnerData()
		{
			return _spawnerData;
		}

		public virtual string GetIdentity()
		{
			string arg = (contextId == null) ? string.Empty : contextId;
			return $"{GetType().Name}[id:{arg}_{id}]";
		}

		public virtual void SetSpawnerContext()
		{
			_spawner.context.SetParent(new SpawnerBehaviourContext<T>(this));
		}

		public virtual int Count()
		{
			if (_spawnerData == null)
			{
				return -1;
			}
			return _spawnerData.count;
		}
	}
}
