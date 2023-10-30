using M__M.HaiWang.Misc;
using PathSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	[ExecuteInEditMode]
	public class FishFormationMgr : MonoBehaviour
	{
		[SerializeField]
		private Dictionary<int, FishFormation> _formationMap;

		private static FishFormationMgr s_instance;

		private Dictionary<int, ObjectPool<FishFormation>> _formationPool;

		public static FishFormationMgr Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
			_formationMap = new Dictionary<int, FishFormation>();
			int childCount = base.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				FishFormation component = child.GetComponent<FishFormation>();
				if (!(component == null))
				{
					if (_formationMap.ContainsKey(component.id))
					{
						UnityEngine.Debug.LogWarning("Formation ID " + component.id + " duplicated!");
					}
					else
					{
						_formationMap.Add(component.id, component);
					}
				}
			}
			UnityEngine.Debug.Log($"formation total count: {childCount}");
			UnityEngine.Debug.Log($"all formations: [{_formationMap.Keys.JoinStrings()}]");
		}

		private void Start()
		{
			_formationPool = new Dictionary<int, ObjectPool<FishFormation>>();
			foreach (KeyValuePair<int, FishFormation> pair in _formationMap)
			{
				_formationMap[pair.Key].PlayOnStart = false;
				_formationMap[pair.Key].gameObject.SetActive(value: false);
				_formationPool[pair.Key] = new ObjectPool<FishFormation>(() => UnityEngine.Object.Instantiate(_formationMap[pair.Key]), null, delegate(FishFormation _fishFormation)
				{
					_fishFormation.gameObject.SetActive(value: true);
				}, delegate(FishFormation _fishFormation)
				{
					_fishFormation.gameObject.SetActive(value: false);
				});
			}
		}

		private void Update()
		{
		}

		public FishFormation GetFormationById2(int id)
		{
			FishFormation ret = null;
			ObjectPool<FishFormation> pool = null;
			if (_formationPool.TryGetValue(id, out pool) && pool != null)
			{
				ret = pool.Get();
				Stopwatch watch = new Stopwatch();
				watch.Start();
				Action<Formation<FishType>> value = delegate
				{
					watch.Stop();
					UnityEngine.Debug.Log(string.Format("formation[{0}] complete, run time:{1}s", id, watch.Elapsed.TotalSeconds.ToString("f2")));
					ret.ResetOnComplete();
					pool.Release(ret);
				};
				ret.OnSpawningDone += value;
				ret.transform.SetParent(base.transform);
				return ret;
			}
			if (ret == null)
			{
				UnityEngine.Debug.LogError($"GetFormationById. [id:{id}] not found");
			}
			else
			{
				ret.ResetFormation();
			}
			return ret;
		}

		public FishFormation GetFormationById(int id)
		{
			FishFormation value = null;
			if (_formationMap.TryGetValue(id, out value))
			{
				value.gameObject.SetActive(value: true);
				return value;
			}
			UnityEngine.Debug.Log($"GetFormationById. [id:{id}] not found");
			return null;
		}
	}
}
