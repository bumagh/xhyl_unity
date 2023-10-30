using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FK3_SpawnerBase<T> : FK3_ISpawner<T>, FK3_ISpawnerPlayable<T>, FK3_ISpawnerSetup<T>, FK3_ISpawnerProcess<T>, FK3_ICheckValid
	{
		[Serializable]
		public class RuntimeData
		{
			public float virtualTimer;

			public float realTimer;

			public int count;

			public bool isPlaying;

			public Coroutine coPlay;

			public int processCount;

			private float _startTime;

			private float _beginTime;

			private float _stopTime;

			public float Elapsed => isPlaying ? (Time.time - _startTime) : _stopTime;

			public float BeginTime => _beginTime;

			public void Run(float playBeginTime)
			{
				isPlaying = true;
				_startTime = Time.time;
				_beginTime = playBeginTime;
			}

			public void Reset()
			{
				virtualTimer = 0f;
				count = 0;
				isPlaying = false;
				coPlay = null;
				_startTime = 0f;
				processCount = 0;
			}

			public void Finish()
			{
				_stopTime = Elapsed;
				isPlaying = false;
			}
		}

		protected FK3_SpawnerData _data;

		protected FK3_IGenerator<T> _generator;

		protected MonoBehaviour _manager;

		protected Action<FK3_ProcessData<T>> _processAction;

		protected Action<FK3_ProcessData<T>> _afterProcessAction;

		protected Action<FK3_ProcessData<T>> _ignoreAction;

		protected Func<int, T, T> _generatorFunc;

		protected bool _printLog;

		public Action<FK3_SpawnerBase<T>> OnFinish;

		public string id = string.Empty;

		public int startIndex;

		public MonoBehaviour container;

		protected Dictionary<string, object> _fields = new Dictionary<string, object>();

		public FK3_SpawnerContext context = new FK3_SpawnerContext();

		private RuntimeData _runtime = new RuntimeData();

		protected FK3_PreciseDelay _delayTool = new FK3_PreciseDelay();

		protected string _errMessage = string.Empty;

		private float _lastMarkTime = -1f;

		public Dictionary<string, object> fields => _fields;

		public virtual bool IsValid => CheckValid();

		public virtual bool IsPlaying => _runtime.isPlaying;

		public virtual float Elapsed => _runtime.Elapsed;

		public virtual void Play(float playTime)
		{
			if (_runtime.isPlaying)
			{
				if (_printLog)
				{
					UnityEngine.Debug.Log($"{GetIdentity()} is still playing, will stop first.");
				}
				DoStop();
			}
			_runtime.coPlay = _manager.StartCoroutine(IE_Play(playTime));
		}

		public virtual void Play()
		{
			Play(0f);
		}

		public virtual void Stop()
		{
			if (_runtime.isPlaying)
			{
				DoStop();
			}
		}

		public virtual void Setup(FK3_SpawnerData data, FK3_IGenerator<T> generator, MonoBehaviour manager, Action<FK3_ProcessData<T>> processAction)
		{
			_data = data;
			_generator = generator;
			_manager = manager;
			SetProcess(processAction);
		}

		public virtual void Setup(FK3_SpawnerData data, FK3_IGenerator<T> generator, MonoBehaviour manager)
		{
			_data = data;
			_generator = generator;
			_manager = manager;
		}

		public virtual void SetProcess(Action<FK3_ProcessData<T>> processAction)
		{
			if (_printLog)
			{
				UnityEngine.Debug.Log(string.Format("{0}.SetProcess> processAction:{1}", GetIdentity(), (processAction == null) ? FK3_LogHelper.Red("null") : FK3_LogHelper.Green("ready")));
			}
			_processAction = processAction;
		}

		public virtual void SetAfterProcess(Action<FK3_ProcessData<T>> afterProcessAction)
		{
			if (_printLog)
			{
				UnityEngine.Debug.Log(string.Format("{0}.SetAfterProcess> afterProcessAction:{1}", GetIdentity(), (afterProcessAction == null) ? FK3_LogHelper.Red("null") : FK3_LogHelper.Green("ready")));
			}
			_afterProcessAction = afterProcessAction;
		}

		public virtual void SetGeneratorFunc(Func<int, T, T> generatorFunc)
		{
			_generatorFunc = generatorFunc;
		}

		public virtual RuntimeData Debug_GetRuntimeData()
		{
			return _runtime;
		}

		public virtual bool CheckValid()
		{
			return !ErrorJudge(_data == null, "_data is null") && !ErrorJudge(_generator == null, "_generator is null") && !ErrorJudge(_manager == null, "_manager is null");
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

		public virtual string GetIdentity()
		{
			return $"{GetType().Name}[id:{id}]";
		}

		public virtual bool IsProcessActionReady()
		{
			return _processAction != null;
		}

		public virtual void SetPrintLog(bool value)
		{
			_printLog = value;
		}

		public virtual bool GetPrintLog()
		{
			return _printLog;
		}

		public virtual MonoBehaviour GetManager()
		{
			return _manager;
		}

		public virtual TT GetManager<TT>() where TT : MonoBehaviour
		{
			return GetManager() as TT;
		}

		public virtual int Count()
		{
			if (_data == null)
			{
				return -1;
			}
			return _data.count;
		}

		public virtual int RuntimeCount()
		{
			if (_runtime == null)
			{
				return -1;
			}
			return _runtime.count;
		}

		private void DoStop()
		{
			if (_runtime.coPlay != null)
			{
				_manager.StopCoroutine(_runtime.coPlay);
			}
			_runtime.Reset();
			if (_generator != null)
			{
				_generator.Reset();
			}
		}

		private bool ErrorJudge(bool result, string errMsg)
		{
			_errMessage = errMsg;
			return result;
		}

		private IEnumerator IE_Play(float playTime)
		{
			if (_printLog)
			{
				UnityEngine.Debug.Log(string.Format("{0} ie_play({1}). _processAction:{2}", GetIdentity(), playTime, (_processAction == null) ? FK3_LogHelper.Red("null") : FK3_LogHelper.Green("ready")));
			}
			RuntimeData _ = _runtime;
			_delayTool.Clear();
			FK3_PreciseDelay delayTool = _delayTool;
			delayTool.onPreDelay = (FK3_PreciseDelay.PreDelayAction)Delegate.Combine(delayTool.onPreDelay, (FK3_PreciseDelay.PreDelayAction)delegate(float expectedDelay, float adjustedDelay)
			{
				_.realTimer += adjustedDelay;
			});
			FK3_PreciseDelay delayTool2 = _delayTool;
			delayTool2.onDelayDone = (FK3_PreciseDelay.AfterDelayAction)Delegate.Combine(delayTool2.onDelayDone, (FK3_PreciseDelay.AfterDelayAction)delegate
			{
				_.realTimer = _.Elapsed;
			});
			_delayTool.SetMode(FK3_PreciseDelay.AdjustMode.Nagative);
			_.Reset();
			_.Run(playTime);
			_lastMarkTime = -1f;
			if (playTime < _data.startDelay)
			{
				float remainDelay = _data.startDelay - playTime;
				yield return _delayTool.WaitDelay(remainDelay);
				_delayTool.Done();
			}
			_.virtualTimer += _data.startDelay;
			while (_.count < _data.count)
			{
				bool willProcess = true;
				float delay = _data.interval;
				T rawResult = _generator.GetNext();
				T result = rawResult;
				if (_generatorFunc != null)
				{
					result = _generatorFunc(_.count, rawResult);
				}
				_.count++;
				bool isLastOne = _.count == _data.count;
				if (playTime > _.virtualTimer)
				{
					willProcess = false;
					float num = playTime - _.virtualTimer;
					delay = ((!(num > _data.interval)) ? Mathf.Min(delay, _data.interval - num) : 0f);
				}
				if (willProcess)
				{
					_.processCount++;
					FK3_ProcessData<T> obj = FK3_ProcessData<T>.Create(result, _.count - 1, this, id);
					if (_printLog)
					{
						UnityEngine.Debug.Log(string.Format("{0} will process. index:{1},doAction:{2}", GetIdentity(), _.count - 1, (_processAction == null) ? FK3_LogHelper.Red("no") : FK3_LogHelper.Green("yes")));
					}
					if (_processAction != null)
					{
						_processAction(obj);
					}
					if (_afterProcessAction != null)
					{
						_afterProcessAction(obj);
					}
				}
				else
				{
					FK3_ProcessData<T> obj2 = FK3_ProcessData<T>.CreateIgnore(result, _.count - 1, this, id);
					if (_processAction != null)
					{
						_processAction(obj2);
					}
					if (_afterProcessAction != null)
					{
						_afterProcessAction(obj2);
					}
				}
				if (!isLastOne && delay > 0f)
				{
					yield return _delayTool.WaitDelay(delay);
					_delayTool.Done();
					CheckPoint(_.count, _delayTool.AdjustedDelay, delay, _data.interval);
				}
				if (!isLastOne)
				{
					_.virtualTimer += _data.interval;
				}
			}
			if (_data.finishDelay > 0f && playTime < _data.totalDuration)
			{
				float remainDelay2 = Mathf.Min(_data.finishDelay, _data.totalDuration - playTime);
				yield return _delayTool.WaitDelay(remainDelay2);
				_delayTool.Done();
				_.virtualTimer += remainDelay2;
			}
			if (_printLog)
			{
				UnityEngine.Debug.Log($"{GetIdentity()} finish. processCount:{_.processCount}");
			}
			if (OnFinish != null)
			{
				OnFinish(this);
			}
			_.Finish();
		}

		private void CheckPoint(int index, float adjustedDelay, float delay, float interval)
		{
			if (_lastMarkTime < 0f)
			{
				_lastMarkTime = Time.time;
				return;
			}
			float num = Time.time - _lastMarkTime;
			float num2 = num / delay;
			_lastMarkTime = Time.time;
			if (interval > 0.4f && !(num2 > 1.2f) && !(num2 < 0.7f))
			{
			}
		}
	}
}
