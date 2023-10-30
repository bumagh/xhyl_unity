using FullInspector;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_FormationMgr<T> : FK3_fiSimpleSingletonBehaviour<FK3_FormationMgr<T>>
	{
		[SerializeField]
		protected Dictionary<string, FK3_FormationBehaviour<T>> _map;

		private int tempNum;

		[SerializeField]
		[InspectorName("测试Id")]
		[InspectorTooltip("用于测试")]
		[InspectorOrder(9.1000003814697266)]
		protected int _testFormationId;

		[InspectorTooltip("用于测试")]
		[SerializeField]
		[InspectorOrder(9.1999998092651367)]
		[InspectorName("测试startTime")]
		protected float _startTime;

		protected int _startId;

		protected Dictionary<string, FK3_ObjectPool<FK3_FormationBehaviour<T>>> _formationPool;

		public bool repalceProcessor;

		public Action<FK3_ProcessData<T>> processAction;

		[ShowInInspector]
		protected List<FK3_FormationBehaviour<T>> playingFormations = new List<FK3_FormationBehaviour<T>>();

		protected Dictionary<int, float> lastActiveTimeMap = new Dictionary<int, float>();

		protected bool isRunning => Application.isPlaying;

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

		[InspectorName("检索Map")]
		[InspectorButton]
		private void RecoverSubSpawners()
		{
			DoRecoverSubSpawners();
		}

		private void DoRecoverSubSpawners()
		{
			if (_map == null)
			{
				_map = new Dictionary<string, FK3_FormationBehaviour<T>>();
			}
			_map.Clear();
			tempNum = 0;
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					Transform transform = (Transform)current;
					UnityEngine.Debug.LogError(transform.name);
					_map.Add(tempNum.ToString(), transform.GetComponent<FK3_FormationBehaviour<T>>());
					tempNum++;
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

		protected override void Awake()
		{
			base.Awake();
		}

		protected virtual void Start()
		{
			_formationPool = new Dictionary<string, FK3_ObjectPool<FK3_FormationBehaviour<T>>>();
			foreach (KeyValuePair<string, FK3_FormationBehaviour<T>> pair in _map)
			{
				if (!(pair.Value == null))
				{
					_formationPool[pair.Key] = new FK3_ObjectPool<FK3_FormationBehaviour<T>>(() => UnityEngine.Object.Instantiate(_map[pair.Key]), null, delegate(FK3_FormationBehaviour<T> _formation)
					{
						_formation.gameObject.SetActive(value: true);
					}, delegate(FK3_FormationBehaviour<T> _formation)
					{
						_formation.gameObject.SetActive(value: false);
					});
				}
			}
		}

		[InspectorButton]
		[InspectorShowIf("isRunning")]
		[InspectorName("测试Play")]
		protected virtual void InspectorBtn_PlayTest()
		{
			if (isRunning)
			{
				PlayFormation(new FK3_FormationPlayParam<T>
				{
					formationId = _testFormationId,
					startTime = _startTime
				});
			}
		}

		[InspectorShowIf("isRunning")]
		[InspectorName("测试Stop")]
		[InspectorButton]
		protected virtual void InspectorBtn_StopTest()
		{
			StopRunningFormations();
		}

		public virtual void StopRunningFormations(bool clear = false)
		{
			if (!isRunning)
			{
				return;
			}
			FK3_FormationBehaviour<T>[] array = playingFormations.ToArray();
			FK3_FormationBehaviour<T>[] array2 = array;
			foreach (FK3_FormationBehaviour<T> fK3_FormationBehaviour in array2)
			{
				if (!(fK3_FormationBehaviour == null))
				{
					if (clear)
					{
						fK3_FormationBehaviour.StopAndClear();
					}
					else
					{
						fK3_FormationBehaviour.Stop();
					}
				}
			}
			playingFormations.Clear();
		}

		[InspectorButton]
		[InspectorName("测试Stop & Clear")]
		[InspectorShowIf("isRunning")]
		protected virtual void InspectorBtn_StopAndClearTest()
		{
			StopRunningFormations(clear: true);
		}

		[InspectorShowIf("isRunning")]
		[InspectorButton]
		[InspectorName("测试PlayAll")]
		protected virtual void InspectorBtn_PlayAll()
		{
			if (_map != null)
			{
				lastActiveTimeMap.Clear();
				int num = 0;
				foreach (KeyValuePair<string, FK3_FormationBehaviour<T>> pair in _map)
				{
					if (!(pair.Value == null))
					{
						FK3_FormationPlayParam<T> fK3_FormationPlayParam = new FK3_FormationPlayParam<T>();
						fK3_FormationPlayParam.formationId = pair.Value.id;
						fK3_FormationPlayParam.startId = num;
						FK3_FormationPlayParam<T> fK3_FormationPlayParam2 = fK3_FormationPlayParam;
						FK3_FormationBehaviour<T> fK3_FormationBehaviour = PlayFormation(fK3_FormationPlayParam2);
						FK3_FormationBehaviour<T> fK3_FormationBehaviour2 = fK3_FormationBehaviour;
						fK3_FormationBehaviour2.onFormationDespawn = (Action<FK3_FormationBehaviour<T>>)Delegate.Combine(fK3_FormationBehaviour2.onFormationDespawn, (Action<FK3_FormationBehaviour<T>>)delegate(FK3_FormationBehaviour<T> _formation)
						{
							lastActiveTimeMap[pair.Value.id] = _formation.activeTimeInSec;
						});
						num += fK3_FormationPlayParam2.count;
					}
				}
			}
		}

		public virtual void PlayFormationById(int formationId)
		{
			PlayFormation(new FK3_FormationPlayParam<T>
			{
				formationId = formationId
			});
		}

		public virtual FK3_FormationBehaviour<T> PlayFormation(FK3_FormationPlayParam<T> param)
		{
			FK3_FormationBehaviour<T> formationById = GetFormationById(param.formationId);
			if (formationById == null)
			{
				UnityEngine.Debug.LogError($"cannot find formation[{param.formationId}]");
				throw new Exception($"cannot find formation[{param.formationId}]");
			}
			if (!formationById.CheckValid())
			{
				UnityEngine.Debug.LogError($"formation[{param.formationId}] is not valid. {formationById.GetError()}");
				throw new Exception($"formation[{param.formationId}] is not valid. {formationById.GetError()}");
			}
			formationById.transform.position = param.offset;
			if (formationById.IsPlaying)
			{
				UnityEngine.Debug.LogError($"warnning! formation[{formationById.id}] is playing. It will be stopped then be started");
				formationById.Stop();
			}
			if (repalceProcessor)
			{
				UnityEngine.Debug.LogError(FK3_LogHelper.Lightblue("{0} replace formation processor. processAction:{1}", GetIdentity(), (processAction == null) ? FK3_LogHelper.Red("null") : FK3_LogHelper.Green("ready")));
				formationById.SetProcess(processAction);
			}
			UnityEngine.Debug.LogError("========PlayFormation=======");
			formationById.startId = param.startId;
			param.count = formationById.Count();
			formationById.generatorFunc = param.generatorFunc;
			formationById.Play(param.startTime);
			FK3_FormationBehaviour<T> fK3_FormationBehaviour = formationById;
			fK3_FormationBehaviour.onAllSpawnDespawned = (Action<FK3_FormationBehaviour<T>>)Delegate.Combine(fK3_FormationBehaviour.onAllSpawnDespawned, (Action<FK3_FormationBehaviour<T>>)delegate(FK3_FormationBehaviour<T> _formation)
			{
				playingFormations.Remove(_formation);
				_formation.Despawn();
			});
			playingFormations.Remove(formationById);
			playingFormations.Add(formationById);
			return formationById;
		}

		public virtual FK3_FormationBehaviour<T> GetFormationById_old(int formationId)
		{
			FK3_FormationBehaviour<T> formationPrototypeById = GetFormationPrototypeById(formationId);
			if (formationPrototypeById == null)
			{
				return null;
			}
			FK3_FormationBehaviour<T> fK3_FormationBehaviour = UnityEngine.Object.Instantiate(formationPrototypeById);
			fK3_FormationBehaviour.SetActive();
			fK3_FormationBehaviour.Prepare();
			if (!fK3_FormationBehaviour.CheckValid())
			{
				UnityEngine.Debug.Log(fK3_FormationBehaviour.GetError());
			}
			return fK3_FormationBehaviour;
		}

		public virtual FK3_FormationBehaviour<T> GetFormationById(int formationId)
		{
			FK3_FormationBehaviour<T> fK3_FormationBehaviour = null;
			FK3_ObjectPool<FK3_FormationBehaviour<T>> pool = null;
			if (formationId >= _formationPool.Count || formationId <= 0)
			{
				formationId %= _formationPool.Count;
			}
			if (formationId <= 0)
			{
				formationId = 1;
			}
			if (_formationPool.Count <= 0)
			{
				return null;
			}
			if (_formationPool.TryGetValue(formationId.ToString(), out pool) && pool != null)
			{
				fK3_FormationBehaviour = pool.Get();
				fK3_FormationBehaviour.Prepare();
				fK3_FormationBehaviour.DoReset_Event();
				fK3_FormationBehaviour.transform.SetParent(base.transform);
				FK3_FormationBehaviour<T> fK3_FormationBehaviour2 = fK3_FormationBehaviour;
				fK3_FormationBehaviour2.onFormationDespawn = (Action<FK3_FormationBehaviour<T>>)Delegate.Combine(fK3_FormationBehaviour2.onFormationDespawn, (Action<FK3_FormationBehaviour<T>>)delegate(FK3_FormationBehaviour<T> _formation)
				{
					pool.Release(_formation);
				});
				return fK3_FormationBehaviour;
			}
			UnityEngine.Debug.LogError("此处未执行!");
			if (fK3_FormationBehaviour == null)
			{
				UnityEngine.Debug.LogError($"GetFormationById. [id:{formationId}] not found");
			}
			return fK3_FormationBehaviour;
		}

		public virtual FK3_FormationBehaviour<T> GetFormationPrototypeById(int formationId)
		{
			if (_map == null)
			{
				return null;
			}
			FK3_FormationBehaviour<T> value = null;
			_map.TryGetValue(formationId.ToString(), out value);
			return value;
		}

		public void Default_DoProcess(FK3_ProcessData<T> data)
		{
			UnityEngine.Debug.Log($"spawn [value:{data.value},index:{data.index}]");
		}

		[InspectorButton]
		[InspectorHideIf("isRunning")]
		private void EnableAllFormations()
		{
			SetAllFormationsActive(value: true);
		}

		[InspectorButton]
		[InspectorHideIf("isRunning")]
		private void DisableAllFormations()
		{
			SetAllFormationsActive(value: false);
		}

		private void SetAllFormationsActive(bool value)
		{
			if (_map != null)
			{
				foreach (KeyValuePair<string, FK3_FormationBehaviour<T>> item in _map)
				{
					if (!(item.Value == null))
					{
						item.Value.gameObject.SetActive(value);
					}
				}
			}
		}

		[InspectorButton]
		[InspectorHideIf("isRunning")]
		[InspectorName("重建FormationMap")]
		private void RebuildFormationMap()
		{
			RebuildFormationMap(onlyActive: false);
		}

		[InspectorName("重建FormationMap(active)")]
		[InspectorButton]
		[InspectorHideIf("isRunning")]
		private void RebuildFormationMap_OnlyActive()
		{
			RebuildFormationMap(onlyActive: true);
		}

		private void RebuildFormationMap(bool onlyActive = true)
		{
			_map.Clear();
			int num = 0;
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					Transform transform = (Transform)current;
					if (!transform.Equals(base.transform) && (!onlyActive || transform.gameObject.activeSelf))
					{
						FK3_FormationBehaviour<T> component = transform.GetComponent<FK3_FormationBehaviour<T>>();
						if (!(component == null))
						{
							if (_map.ContainsKey(component.id.ToString()))
							{
								UnityEngine.Debug.Log($"formation id:{component.id} repeat");
							}
							else
							{
								num++;
							}
							_map[component.id.ToString()] = component;
						}
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
			UnityEngine.Debug.Log($"new formation map size:{num}");
		}

		public virtual string GetIdentity()
		{
			return GetType().Name;
		}
	}
}
