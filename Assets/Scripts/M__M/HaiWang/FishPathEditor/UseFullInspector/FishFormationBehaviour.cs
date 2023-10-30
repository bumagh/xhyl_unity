using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FullInspector;
using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.Core;
using M__M.HaiWang.Misc;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FishFormationBehaviour : FormationBehaviour<FishType>
	{
		protected override void Awake()
		{
			base.Awake();
			this.SetPrintLog(true);
		}

		protected override void Start()
		{
			base.Start();
			if (this._spawners == null)
			{
				return;
			}
			if (!this._prepared)
			{
				this.Prepare();
			}
		}

		public override void StopAndClear()
		{
			FishBehaviour[] array = this._fishList.ToArray();
			foreach (FishBehaviour fishBehaviour in array)
			{
				if (!(fishBehaviour == null))
				{
					fishBehaviour.Die();
				}
			}
			this._fishList.Clear();
			this.Stop();
		}

		[InspectorHideIf("isRunning")]
		[InspectorButton]
		[InspectorName("检索子生成器")]
		private void RecoverSubSpawners()
		{
			this.DoRecoverSubSpawners(false);
		}

		[InspectorButton]
		[InspectorName("检索子生成器(仅active)")]
		[InspectorHideIf("isRunning")]
		private void RecoverSubSpawners_OnlyActive()
		{
			this.DoRecoverSubSpawners(true);
		}

		private void DoRecoverSubSpawners(bool onlyActive = false)
		{
			if (this._spawners == null)
			{
				this._spawners = new List<FishSpawnerBehaviour>();
			}
			this._spawners.Clear();
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (!(transform == base.transform) && (!onlyActive || transform.gameObject.activeInHierarchy))
					{
						FishSpawnerBehaviour[] componentsInChildren = transform.GetComponentsInChildren<FishSpawnerBehaviour>();
						this._spawners.AddRange(componentsInChildren);
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
			Debug.Log(string.Format("{0}.RecoverSubSpawners done. spawners.count:{1}", this.GetIdentity(), this._spawners.Count));
		}

		public new virtual int Count()
		{
			if (this._spawners == null)
			{
				return -1;
			}
			int num = 0;
			foreach (FishSpawnerBehaviour fishSpawnerBehaviour in this._spawners)
			{
				if (!(fishSpawnerBehaviour == null))
				{
					num += fishSpawnerBehaviour.Count();
				}
			}
			return num;
		}

		[InspectorButton]
		private void PrintCount()
		{
			Debug.Log(string.Format("{0}> count:{1}", this.GetIdentity(), this.Count()));
		}

		[InspectorButton]
		private void PrintDetailCount()
		{
			if (this._spawners == null)
			{
				Debug.Log(string.Format("{0}>_spawners is null", this.GetIdentity()));
				return;
			}
			int num = 0;
			foreach (FishSpawnerBehaviour fishSpawnerBehaviour in this._spawners)
			{
				if (!(fishSpawnerBehaviour == null))
				{
					int num2 = fishSpawnerBehaviour.Count();
					num += num2;
					Debug.Log(string.Format("{0}.count:{1}", fishSpawnerBehaviour.GetIdentity(), num2));
				}
			}
		}

		public override string GetIdentity()
		{
			return string.Format("FishFormation[id:{0}]", this.id);
		}

		[InspectorButton]
		private void PrintSpawnersInfo()
		{
			this.ForeachSpawner(delegate(FishSpawnerBehaviour _)
			{
				IGenerator<FishType> generator = _.GetGenerator();
				if (generator == null)
				{
					return;
				}
				Debug.Log(string.Format("{0}>[{1}]", _.GetIdentity(), generator.GetEnums().JoinStrings(",")));
			});
		}

		public void ForeachSpawner(Action<FishSpawnerBehaviour> action)
		{
			if (this._spawners == null)
			{
				return;
			}
			this._spawners.ForEach(action);
		}

		public override void Prepare()
		{
			base.Prepare();
			this.PrepareSpawners();
			if (FishSpawnerBehaviour.fishProcessAction != null)
			{
				this.processAction = FishSpawnerBehaviour.fishProcessAction;
			}
		}

		private void PrepareSpawners()
		{
			this._spawners.ForEach(delegate(FishSpawnerBehaviour _)
			{
				_.contextId = this.id.ToString();
				_.Prepare();
				_.SetSpawnerContext();
				_.SetProcess(new Action<ProcessData<FishType>>(this.OnProcess));
			});
			this._complexSpawner.spawners.Clear();
			this._complexSpawner.spawners.AddRange(from _ in this._spawners
			select _.GetSpawner());
		}

		protected override void OnProcess(ProcessData<FishType> data)
		{
			base.OnProcess(data);
			if (data.objBehaviour != null)
			{
				FishBehaviour fishBehaviour = data.objBehaviour as FishBehaviour;
				if (fishBehaviour != null)
				{
					this._fishList.Add(fishBehaviour);
					FishBehaviour fishBehaviour2 = fishBehaviour;
					fishBehaviour2.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, new Action<FishBehaviour>(delegate(FishBehaviour _fish)
					{
						this._despawnCount++;
						this._fishList.Remove(_fish);
						if (this._ignoreCount + this._despawnCount == this._expectCount)
						{
							this._despawnFinished = true;
							this.activeTimeInSec = Time.time - this._startTime;
							Debug.Log(HW2_LogHelper.Cyan("{0} all despawn finished. activeTime:{1}, ingore:{2}, despawn:{3}, expect:{4}, process:{5}", new object[]
							{
								this.GetIdentity(),
								this.activeTimeInSec,
								this._ignoreCount,
								this._despawnCount,
								this._expectCount,
								this._processCount
							}));
							if (this.onAllSpawnDespawned != null)
							{
								this.onAllSpawnDespawned(this);
							}
						}
					}));
				}
			}
		}

		[InspectorHideIf("isValid")]
		[InspectorComment("无效，数据缺失", Type = CommentType.Error)]
		protected int _invalidComment;

		[SerializeField]
		protected List<FishSpawnerBehaviour> _spawners;

		[ShowInInspector]
		protected List<FishBehaviour> _fishList = new List<FishBehaviour>();
	}
}
