using FullInspector;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_SpawnerBehaviour<T> : FK3_fiSimpleSingletonBehaviour<FK3_SpawnerBehaviour<T>>, FK3_ISpawner<T>, FK3_ICheckValid, FK3_ISpawnerPlayable<T>, FK3_ISpawnerSetup<T>, FK3_ISpawnerProcess<T>
	{
		public class SpawnerBehaviourContext<TT> : FK3_SpawnerContext
		{
			public FK3_SpawnerBehaviour<TT> spawnerBehaviour;

			public SpawnerBehaviourContext(FK3_SpawnerBehaviour<TT> spawnerBehaviour)
			{
				this.spawnerBehaviour = spawnerBehaviour;
			}
		}

		public string id = string.Empty;

		[InspectorDisabled]
		public string contextId = string.Empty;

		public bool playOnStart = true;

		[SerializeField]
		protected FK3_SpawnerData _spawnerData;

		[SerializeField]
		protected FK3_IGenerator<T> _generator;

		protected FK3_SpawnerBase<T> _spawner;

		protected MonoBehaviour _manager;

		public bool printLog;

		protected string _errMessage = string.Empty;

		protected Action<FK3_ProcessData<T>> _processAction;

		public bool IsValid => CheckValid();

		public bool IsPlaying => _spawner != null && _spawner.IsPlaying;

		public virtual bool Inject(FK3_SpawnerBase<T> spawner, FK3_SpawnerData data, FK3_IGenerator<T> generator, Action<FK3_ProcessData<T>> processAction)
		{
			_spawner = spawner;
			_spawnerData = data;
			_generator = generator;
			_processAction = processAction;
			_spawner.Setup(data, generator, this, processAction);
			return CheckValid();
		}

		public virtual bool Inject(FK3_SpawnerBase<T> spawner, Action<FK3_ProcessData<T>> processAction)
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
				UnityEngine.Debug.Log(FK3_LogHelper.Magenta("{0} Play({1})", GetIdentity(), playTime));
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
				UnityEngine.Debug.Log(FK3_LogHelper.Magenta("{0} Stop", GetIdentity()));
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

		public void SetSpawnerData(FK3_SpawnerData data)
		{
			_spawnerData = data;
		}

		public void SetGenerator(FK3_IGenerator<T> generator)
		{
			_generator = generator;
		}

		public void SetManager(MonoBehaviour manager)
		{
			_manager = manager;
		}

		public virtual void SetProcess(Action<FK3_ProcessData<T>> processAction)
		{
			_processAction = processAction;
			_spawner.SetProcess(processAction);
		}

		public virtual void SetAfterProcess(Action<FK3_ProcessData<T>> afterProcessAction)
		{
			_spawner.SetAfterProcess(afterProcessAction);
		}

		public virtual Action<FK3_ProcessData<T>> GetProcess()
		{
			return _processAction;
		}

		public void SetupSpawner()
		{
			_spawner.Setup(_spawnerData, _generator, _manager, _processAction);
		}

		public virtual void Setup(FK3_SpawnerData data, FK3_IGenerator<T> generator, MonoBehaviour manager, Action<FK3_ProcessData<T>> processAction)
		{
			_spawnerData = data;
			_generator = generator;
			_manager = manager;
			_processAction = processAction;
			_spawner.Setup(data, generator, manager, processAction);
		}

		public virtual void Setup(FK3_SpawnerData data, FK3_IGenerator<T> generator, MonoBehaviour manager)
		{
			_spawner.Setup(data, generator, manager);
		}

		public FK3_SpawnerBase<T> GetSpawner()
		{
			return _spawner;
		}

		public FK3_IGenerator<T> GetGenerator()
		{
			return _generator;
		}

		public FK3_SpawnerData GetSpawnerData()
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
