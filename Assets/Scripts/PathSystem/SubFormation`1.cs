using FullInspector;
using System;
using System.Collections;
using UnityEngine;

namespace PathSystem
{
	public class SubFormation<T> : BaseBehavior<FullSerializerSerializer>
	{
		[HideInInspector]
		public object userData;

		public float delay;

		public int loopCount = 1;

		public float speed = 2f;

		public float interval = 1.5f;

		public bool smoothRotation = true;

		[SerializeField]
		protected int _count = 6;

		protected int _remainCount;

		[SerializeField]
		protected IGenerator<T> _generator = new SingleType<T>();

		protected Formation<T> _formation;

		[SerializeField]
		protected Path _path;

		protected Coroutine _coroutine;

		public float finishDelayTime = 30f;

		public int count => _count;

		public IGenerator<T> generator => _generator;

		public Formation<T> formation => _formation;

		public Path path => _path;

		public event Action<SubFormation<T>> OnSpawningDone;

		public virtual void Init(Formation<T> formation)
		{
			_formation = formation;
			if (_generator == null)
			{
				throw new Exception("SubFormation " + base.name + "'s Generator is null!");
			}
			_generator.Init(this);
		}

		public virtual void Play()
		{
			_coroutine = StartCoroutine(_doPlaying());
		}

		public virtual void Stop()
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
				_coroutine = null;
			}
		}

		public virtual void DoReset()
		{
		}

		protected IEnumerator _doPlaying()
		{
			if (_path == null)
			{
				UnityEngine.Debug.Log("Path is empty, please assign a path first!");
				yield break;
			}
			_remainCount = _count;
			_generator.Reset();
			yield return new WaitForSeconds(delay);
			while (_remainCount > 0)
			{
				AgentData<T> agentData = _getNextAgentData();
				agentData.agent.StopMove();
				agentData.agent.speed = speed;
				agentData.agent.loopCount = loopCount;
				agentData.agent.smoothRotation = smoothRotation;
				agentData.agent.rotationOffset = 0f;
				agentData.agent.StartMove(_path);
				_remainCount--;
				yield return new WaitForSeconds(interval);
			}
			yield return new WaitForSeconds(finishDelayTime);
			if (this.OnSpawningDone != null)
			{
				this.OnSpawningDone(this);
			}
			_coroutine = null;
		}

		protected virtual AgentData<T> _getNextAgentData()
		{
			return _generator.GetNext(userData);
		}

		public virtual T[] GetTypes()
		{
			return (_generator == null) ? new T[0] : _generator.GetTypes();
		}
	}
}
