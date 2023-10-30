using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public class Formation<T> : BaseBehavior<FullSerializerSerializer>
	{
		public bool PlayOnStart;

		public static bool forbiddenPlayOnStart;

		protected IAgentFactory<T> _factory;

		[SerializeField]
		protected List<SubFormation<T>> _subFormations = new List<SubFormation<T>>();

		protected HashSet<AgentData<T>> _agentDatas;

		protected FormationState _state;

		protected int _spawningSubFormationCount;

		protected int _nonDeadAgentsCount;

		public FormationState state => _state;

		public event Action<Formation<T>> OnComplete;

		public event Action<Formation<T>> OnSpawningDone;

		public void ResetOnComplete()
		{
			this.OnSpawningDone = null;
		}

		public void Play()
		{
			if (_state != 0)
			{
				return;
			}
			_state = FormationState.Playing;
			if (_subFormations.Count <= 0)
			{
				UnityEngine.Debug.Log("SubFormation list is empty, please assign at least one SubFormation first!");
			}
			_spawningSubFormationCount = 0;
			_nonDeadAgentsCount = 0;
			_onPlayStart();
			for (int i = 0; i < _subFormations.Count; i++)
			{
				SubFormation<T> subFormation = _subFormations[i];
				if (!(subFormation == null))
				{
					subFormation.Play();
					_spawningSubFormationCount++;
					_nonDeadAgentsCount += subFormation.count;
				}
			}
		}

		public void Stop(bool destroyObjects = true)
		{
			if (_state == FormationState.Playing)
			{
				foreach (SubFormation<T> subFormation in _subFormations)
				{
					subFormation.Stop();
				}
				if (destroyObjects)
				{
					foreach (AgentData<T> agentData in _agentDatas)
					{
						agentData.Clear();
						agentData.agent.StopMove();
						_factory.Destroy(agentData.agent, agentData.type);
					}
					_agentDatas.Clear();
				}
				_state = FormationState.Stopped;
			}
		}

		public void RemoveObject(NavPathAgent agent)
		{
			if (agent.userData == null)
			{
				UnityEngine.Debug.Log("RemoveObject failed, the object " + agent.name + " is not managed by Formation " + base.name);
				return;
			}
			AgentData<T> agentData = agent.userData as AgentData<T>;
			agentData.Clear();
			_agentDatas.Remove(agentData);
			_factory.Destroy(agent, agentData.type);
			_nonDeadAgentsCount--;
			if (_nonDeadAgentsCount == 0)
			{
				_state = FormationState.Stopped;
			}
		}

		public int GetTotalCount()
		{
			int num = 0;
			foreach (SubFormation<T> subFormation in _subFormations)
			{
				num += subFormation.count;
			}
			return num;
		}

		protected virtual NavPathAgent _createObject(T type, object userData)
		{
			return _factory.Create(type, userData);
		}

		protected virtual void _onPlayStart()
		{
		}

		internal AgentData<T> AddObject(T type, object userData)
		{
			NavPathAgent agent = _createObject(type, userData);
			AgentData<T> agentData = new AgentData<T>(agent, type, this);
			_agentDatas.Add(agentData);
			return agentData;
		}

		protected override void Awake()
		{
			base.Awake();
			_agentDatas = new HashSet<AgentData<T>>();
			foreach (SubFormation<T> subFormation in _subFormations)
			{
				if (!(subFormation == null))
				{
					subFormation.Init(this);
					subFormation.OnSpawningDone += delegate
					{
						_spawningSubFormationCount--;
						if (_spawningSubFormationCount == 0 && this.OnSpawningDone != null)
						{
							this.OnSpawningDone(this);
						}
					};
				}
			}
		}

		protected virtual void Start()
		{
			if (PlayOnStart && !forbiddenPlayOnStart)
			{
				Play();
			}
		}

		public List<SubFormation<T>> GetSubFormations()
		{
			return _subFormations;
		}

		[InspectorName("侦测所有子阵型")]
		[InspectorButton]
		private void CheckSubFormations()
		{
			_subFormations.Clear();
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					Transform transform = (Transform)current;
					if (transform.gameObject.activeSelf)
					{
						SubFormation<T> component = transform.GetComponent<SubFormation<T>>();
						_subFormations.Add(component);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
