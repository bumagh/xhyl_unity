using FullInspector;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FormationMgr<T> : fiSimpleSingletonBehaviour<FormationMgr<T>>
	{
		[SerializeField]
		protected Dictionary<string, FormationBehaviour<T>> _map;

		private int tempNum;

		[SerializeField]
		[InspectorOrder(9.1000003814697266)]
		[InspectorName("测试Id")]
		[InspectorTooltip("用于测试")]
		protected int _testFormationId;

		[InspectorTooltip("用于测试")]
		[InspectorOrder(9.1999998092651367)]
		[InspectorName("测试startTime")]
		[SerializeField]
		protected float _startTime;

		protected int _startId;

		protected Dictionary<string, ObjectPool<FormationBehaviour<T>>> _formationPool;

		public bool repalceProcessor;

		public Action<ProcessData<T>> processAction;

		[ShowInInspector]
		protected List<FormationBehaviour<T>> playingFormations = new List<FormationBehaviour<T>>();

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

		[InspectorButton]
		[InspectorName("检索Map")]
		private void RecoverSubSpawners()
		{
			DoRecoverSubSpawners();
		}

		private void DoRecoverSubSpawners()
		{
			if (_map == null)
			{
				_map = new Dictionary<string, FormationBehaviour<T>>();
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
					_map.Add(tempNum.ToString(), transform.GetComponent<FormationBehaviour<T>>());
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
			_formationPool = new Dictionary<string, ObjectPool<FormationBehaviour<T>>>();
			foreach (KeyValuePair<string, FormationBehaviour<T>> pair in _map)
			{
				if (!(pair.Value == null))
				{
					_formationPool[pair.Key] = new ObjectPool<FormationBehaviour<T>>(() => UnityEngine.Object.Instantiate(_map[pair.Key]), null, delegate(FormationBehaviour<T> _formation)
					{
						_formation.gameObject.SetActive(value: true);
					}, delegate(FormationBehaviour<T> _formation)
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
				PlayFormation(new FormationPlayParam<T>
				{
					formationId = _testFormationId,
					startTime = _startTime
				});
			}
		}

		[InspectorName("测试Stop")]
		[InspectorShowIf("isRunning")]
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
			FormationBehaviour<T>[] array = playingFormations.ToArray();
			FormationBehaviour<T>[] array2 = array;
			foreach (FormationBehaviour<T> formationBehaviour in array2)
			{
				if (!(formationBehaviour == null))
				{
					if (clear)
					{
						formationBehaviour.StopAndClear();
					}
					else
					{
						formationBehaviour.Stop();
					}
				}
			}
			playingFormations.Clear();
		}

		[InspectorName("测试Stop & Clear")]
		[InspectorButton]
		[InspectorShowIf("isRunning")]
		protected virtual void InspectorBtn_StopAndClearTest()
		{
			StopRunningFormations(clear: true);
		}

		[InspectorName("测试PlayAll")]
		[InspectorShowIf("isRunning")]
		[InspectorButton]
		protected virtual void InspectorBtn_PlayAll()
		{
			if (_map != null)
			{
				lastActiveTimeMap.Clear();
				int num = 0;
				foreach (KeyValuePair<string, FormationBehaviour<T>> pair in _map)
				{
					if (!(pair.Value == null))
					{
						FormationPlayParam<T> formationPlayParam = new FormationPlayParam<T>();
						formationPlayParam.formationId = pair.Value.id;
						formationPlayParam.startId = num;
						FormationPlayParam<T> formationPlayParam2 = formationPlayParam;
						FormationBehaviour<T> formationBehaviour = PlayFormation(formationPlayParam2);
						FormationBehaviour<T> formationBehaviour2 = formationBehaviour;
						formationBehaviour2.onFormationDespawn = (Action<FormationBehaviour<T>>)Delegate.Combine(formationBehaviour2.onFormationDespawn, (Action<FormationBehaviour<T>>)delegate(FormationBehaviour<T> _formation)
						{
							lastActiveTimeMap[pair.Value.id] = _formation.activeTimeInSec;
						});
						num += formationPlayParam2.count;
					}
				}
			}
		}

		public virtual void PlayFormationById(int formationId)
		{
			PlayFormation(new FormationPlayParam<T>
			{
				formationId = formationId
			});
		}

		public virtual FormationBehaviour<T> PlayFormation(FormationPlayParam<T> param)
		{
			FormationBehaviour<T> formationById = GetFormationById(param.formationId);
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
				UnityEngine.Debug.LogError(HW2_LogHelper.Lightblue("{0} replace formation processor. processAction:{1}", GetIdentity(), (processAction == null) ? HW2_LogHelper.Red("null") : HW2_LogHelper.Green("ready")));
				formationById.SetProcess(processAction);
			}
			UnityEngine.Debug.LogError("========PlayFormation=======");
			formationById.startId = param.startId;
			param.count = formationById.Count();
			formationById.generatorFunc = param.generatorFunc;
			formationById.Play(param.startTime);
			FormationBehaviour<T> formationBehaviour = formationById;
			formationBehaviour.onAllSpawnDespawned = (Action<FormationBehaviour<T>>)Delegate.Combine(formationBehaviour.onAllSpawnDespawned, (Action<FormationBehaviour<T>>)delegate(FormationBehaviour<T> _formation)
			{
				playingFormations.Remove(_formation);
				_formation.Despawn();
			});
			playingFormations.Remove(formationById);
			playingFormations.Add(formationById);
			return formationById;
		}

		public virtual FormationBehaviour<T> GetFormationById_old(int formationId)
		{
			FormationBehaviour<T> formationPrototypeById = GetFormationPrototypeById(formationId);
			if (formationPrototypeById == null)
			{
				return null;
			}
			FormationBehaviour<T> formationBehaviour = UnityEngine.Object.Instantiate(formationPrototypeById);
			formationBehaviour.SetActive();
			formationBehaviour.Prepare();
			if (!formationBehaviour.CheckValid())
			{
				UnityEngine.Debug.Log(formationBehaviour.GetError());
			}
			return formationBehaviour;
		}

		public virtual FormationBehaviour<T> GetFormationById(int formationId)
		{
			FormationBehaviour<T> formationBehaviour = null;
			ObjectPool<FormationBehaviour<T>> pool = null;
			UnityEngine.Debug.LogError("formationId0: " + formationId);
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
			UnityEngine.Debug.LogError("formationId1: " + formationId);
			if (_formationPool.TryGetValue(formationId.ToString(), out pool) && pool != null)
			{
				formationBehaviour = pool.Get();
				formationBehaviour.Prepare();
				formationBehaviour.DoReset_Event();
				formationBehaviour.transform.SetParent(base.transform);
				FormationBehaviour<T> formationBehaviour2 = formationBehaviour;
				formationBehaviour2.onFormationDespawn = (Action<FormationBehaviour<T>>)Delegate.Combine(formationBehaviour2.onFormationDespawn, (Action<FormationBehaviour<T>>)delegate(FormationBehaviour<T> _formation)
				{
					pool.Release(_formation);
				});
				return formationBehaviour;
			}
			UnityEngine.Debug.LogError("此处未执行!");
			if (formationBehaviour == null)
			{
				UnityEngine.Debug.LogError($"GetFormationById. [id:{formationId}] not found");
			}
			return formationBehaviour;
		}

		public virtual FormationBehaviour<T> GetFormationPrototypeById(int formationId)
		{
			if (_map == null)
			{
				return null;
			}
			FormationBehaviour<T> value = null;
			_map.TryGetValue(formationId.ToString(), out value);
			return value;
		}

		public void Default_DoProcess(ProcessData<T> data)
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
				foreach (KeyValuePair<string, FormationBehaviour<T>> item in _map)
				{
					if (!(item.Value == null))
					{
						item.Value.gameObject.SetActive(value);
					}
				}
			}
		}

		[InspectorName("重建FormationMap")]
		[InspectorHideIf("isRunning")]
		[InspectorButton]
		private void RebuildFormationMap()
		{
			RebuildFormationMap(onlyActive: false);
		}

		[InspectorHideIf("isRunning")]
		[InspectorButton]
		[InspectorName("重建FormationMap(active)")]
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
						FormationBehaviour<T> component = transform.GetComponent<FormationBehaviour<T>>();
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
