using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
	public class FK3_Formation<T> : BaseBehavior<FullSerializerSerializer>
	{
		public bool PlayOnStart;

		public static bool forbiddenPlayOnStart;

		protected FK3_IAgentFactory<T> _factory;

		[SerializeField]
		protected List<FK3_SubFormation<T>> _subFormations = new List<FK3_SubFormation<T>>();

		protected HashSet<FK3_AgentData<T>> _agentDatas;

		protected FK3_FormationState _state;

		protected int _spawningSubFormationCount;

		protected int _nonDeadAgentsCount;

		public FK3_FormationState state => _state;

		public event Action<FK3_Formation<T>> OnComplete;

		public event Action<FK3_Formation<T>> OnSpawningDone;

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
			_state = FK3_FormationState.Playing;
			if (_subFormations.Count <= 0)
			{
				UnityEngine.Debug.Log("SubFormation list is empty, please assign at least one SubFormation first!");
			}
			_spawningSubFormationCount = 0;
			_nonDeadAgentsCount = 0;
			_onPlayStart();
			for (int i = 0; i < _subFormations.Count; i++)
			{
				FK3_SubFormation<T> fK3_SubFormation = _subFormations[i];
				if (!(fK3_SubFormation == null))
				{
					fK3_SubFormation.Play();
					_spawningSubFormationCount++;
					_nonDeadAgentsCount += fK3_SubFormation.count;
				}
			}
		}

		public void Stop(bool destroyObjects = true)
		{
			if (_state == FK3_FormationState.Playing)
			{
				foreach (FK3_SubFormation<T> subFormation in _subFormations)
				{
					subFormation.Stop();
				}
				if (destroyObjects)
				{
					foreach (FK3_AgentData<T> agentData in _agentDatas)
					{
						agentData.Clear();
						agentData.agent.StopMove();
						_factory.Destroy(agentData.agent, agentData.type);
					}
					_agentDatas.Clear();
				}
				_state = FK3_FormationState.Stopped;
			}
		}

		public void RemoveObject(FK3_NavPathAgent agent)
		{
			if (agent.userData == null)
			{
				UnityEngine.Debug.Log("RemoveObject failed, the object " + agent.name + " is not managed by Formation " + base.name);
				return;
			}
			FK3_AgentData<T> fK3_AgentData = agent.userData as FK3_AgentData<T>;
			fK3_AgentData.Clear();
			_agentDatas.Remove(fK3_AgentData);
			_factory.Destroy(agent, fK3_AgentData.type);
			_nonDeadAgentsCount--;
			if (_nonDeadAgentsCount == 0)
			{
				_state = FK3_FormationState.Stopped;
			}
		}

		public int GetTotalCount()
		{
			int num = 0;
			foreach (FK3_SubFormation<T> subFormation in _subFormations)
			{
				num += subFormation.count;
			}
			return num;
		}

		protected virtual FK3_NavPathAgent _createObject(T type, object userData)
		{
			return _factory.Create(type, userData);
		}

		protected virtual void _onPlayStart()
		{
		}

		internal FK3_AgentData<T> AddObject(T type, object userData)
		{
			FK3_NavPathAgent agent = _createObject(type, userData);
			FK3_AgentData<T> fK3_AgentData = new FK3_AgentData<T>(agent, type, this);
			_agentDatas.Add(fK3_AgentData);
			return fK3_AgentData;
		}

		protected override void Awake()
		{
			base.Awake();
			_agentDatas = new HashSet<FK3_AgentData<T>>();
			foreach (FK3_SubFormation<T> subFormation in _subFormations)
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

		public List<FK3_SubFormation<T>> GetSubFormations()
		{
			return _subFormations;
		}

		[InspectorButton]
		[InspectorName("侦测所有子阵型")]
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
						FK3_SubFormation<T> component = transform.GetComponent<FK3_SubFormation<T>>();
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
