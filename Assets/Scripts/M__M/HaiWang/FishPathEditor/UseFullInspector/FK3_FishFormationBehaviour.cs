using FullInspector;
using HW3L;
using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
public class FK3_FishFormationBehaviour : FK3_FormationBehaviour<FK3_FishType>
{
	[InspectorHideIf("isValid")]
	[InspectorComment("无效，数据缺失", Type = CommentType.Error)]
	protected int _invalidComment;

	[SerializeField]
	protected List<FK3_FishSpawnerBehaviour> _spawners;

	[ShowInInspector]
	protected List<FK3_FishBehaviour> _fishList = new List<FK3_FishBehaviour>();

	protected override void Awake()
	{
		base.Awake();
		SetPrintLog(true);
	}

	protected override void Start()
	{
		base.Start();
		if (_spawners != null && !_prepared)
		{
			Prepare();
		}
	}

	public override void StopAndClear()
	{
		FK3_FishBehaviour[] array = _fishList.ToArray();
		FK3_FishBehaviour[] array2 = array;
		foreach (FK3_FishBehaviour fK3_FishBehaviour in array2)
		{
			if (!(fK3_FishBehaviour == null))
			{
				fK3_FishBehaviour.Die();
			}
		}
		_fishList.Clear();
		Stop();
	}

	[InspectorHideIf("isRunning")]
	[InspectorButton]
	[InspectorName("检索子生成器")]
	private void RecoverSubSpawners()
	{
		DoRecoverSubSpawners(false);
	}

	[InspectorButton]
	[InspectorName("检索子生成器(仅active)")]
	[InspectorHideIf("isRunning")]
	private void RecoverSubSpawners_OnlyActive()
	{
		DoRecoverSubSpawners(true);
	}

	private void DoRecoverSubSpawners(bool onlyActive = false)
	{
		if (_spawners == null)
		{
			_spawners = new List<FK3_FishSpawnerBehaviour>();
		}
		_spawners.Clear();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform = (Transform)current;
				if (!(transform == base.transform) && (!onlyActive || transform.gameObject.activeInHierarchy))
				{
					FK3_FishSpawnerBehaviour[] componentsInChildren = transform.GetComponentsInChildren<FK3_FishSpawnerBehaviour>();
					_spawners.AddRange(componentsInChildren);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		Debug.Log(string.Format("{0}.RecoverSubSpawners done. spawners.count:{1}", GetIdentity(), _spawners.Count));
	}

	public new virtual int Count()
	{
		if (_spawners == null)
		{
			return -1;
		}
		int num = 0;
		foreach (FK3_FishSpawnerBehaviour spawner in _spawners)
		{
			if (!(spawner == null))
			{
				num += spawner.Count();
			}
		}
		return num;
	}

	[InspectorButton]
	private void PrintCount()
	{
		Debug.Log(string.Format("{0}> count:{1}", GetIdentity(), Count()));
	}

	[InspectorButton]
	private void PrintDetailCount()
	{
		if (_spawners == null)
		{
			Debug.Log(string.Format("{0}>_spawners is null", GetIdentity()));
			return;
		}
		int num = 0;
		foreach (FK3_FishSpawnerBehaviour spawner in _spawners)
		{
			if (!(spawner == null))
			{
				int num2 = spawner.Count();
				num += num2;
				Debug.Log(string.Format("{0}.count:{1}", spawner.GetIdentity(), num2));
			}
		}
	}

	public override string GetIdentity()
	{
		return string.Format("FishFormation[id:{0}]", id);
	}

	[InspectorButton]
	private void PrintSpawnersInfo()
	{
		ForeachSpawner(delegate(FK3_FishSpawnerBehaviour _)
		{
			FK3_IGenerator<FK3_FishType> generator = _.GetGenerator();
			if (generator != null)
			{
				Debug.Log(string.Format("{0}>[{1}]", _.GetIdentity(), FK3_ExtendMethod.JoinStrings(generator.GetEnums())));
			}
		});
	}

	public void ForeachSpawner(Action<FK3_FishSpawnerBehaviour> action)
	{
		if (_spawners != null)
		{
			_spawners.ForEach(action);
		}
	}

	public override void Prepare()
	{
		base.Prepare();
		PrepareSpawners();
		if (FK3_FishSpawnerBehaviour.fishProcessAction != null)
		{
			processAction = FK3_FishSpawnerBehaviour.fishProcessAction;
		}
	}

	private void PrepareSpawners()
	{
		_spawners.ForEach(delegate(FK3_FishSpawnerBehaviour _)
		{
			_.contextId = id.ToString();
			_.Prepare();
			_.SetSpawnerContext();
			_.SetProcess(OnProcess);
		});
		_complexSpawner.spawners.Clear();
		_complexSpawner.spawners.AddRange(Enumerable.Select(_spawners, delegate(FK3_FishSpawnerBehaviour _)
		{
			return _.GetSpawner();
		}));
	}

	protected override void OnProcess(FK3_ProcessData<FK3_FishType> data)
	{
		base.OnProcess(data);
		if (!(data.objBehaviour != null))
		{
			return;
		}
		FK3_FishBehaviour fK3_FishBehaviour = data.objBehaviour as FK3_FishBehaviour;
		if (!(fK3_FishBehaviour != null))
		{
			return;
		}
		_fishList.Add(fK3_FishBehaviour);
		FK3_FishBehaviour fK3_FishBehaviour2 = fK3_FishBehaviour;
		fK3_FishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate(FK3_FishBehaviour _fish)
		{
			_despawnCount++;
			_fishList.Remove(_fish);
			if (_ignoreCount + _despawnCount == _expectCount)
			{
				_despawnFinished = true;
				activeTimeInSec = Time.time - _startTime;
				Debug.Log(FK3_LogHelper.Cyan("{0} all despawn finished. activeTime:{1}, ingore:{2}, despawn:{3}, expect:{4}, process:{5}", GetIdentity(), activeTimeInSec, _ignoreCount, _despawnCount, _expectCount, _processCount));
				if (onAllSpawnDespawned != null)
				{
					onAllSpawnDespawned(this);
				}
			}
		});
	}
}

}
