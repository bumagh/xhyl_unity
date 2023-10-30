using BansheeGz.BGSpline.Curve;
using FullInspector;
using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	[ExecuteInEditMode]
	public class FK3_FishSpawnerBehaviour : FK3_SpawnerBehaviour<FK3_FishType>
	{
		[SerializeField]
		protected BGCurve _curve;

		public float moveSpeed = 2f;

		public float moveStartDelay;

		private FK3_CurveUsage _curveUsage;

		private List<FK3_CursorUsage> _activeCursors;

		[ShowInInspector]
		[NotSerialized]
		public List<FK3_FishBehaviour> _fishList;

		public static bool useStaticProcessor;

		public static Action<FK3_ProcessData<FK3_FishType>> fishProcessAction;

		public static Func<FK3_FishSpawnerBehaviour, bool, bool> playOnStartFunc;

		protected bool prepared;

		private int _spawnCount;

		private int _ignoreCount;

		private int _despawnCount;

		public bool isRunning => Application.isPlaying;

		protected override void Awake()
		{
			base.Awake();
			if (_activeCursors == null)
			{
				_activeCursors = new List<FK3_CursorUsage>();
			}
			_spawner = new FK3_SpawnerBase<FK3_FishType>();
			_spawner.container = this;
			GainCurve();
			if (CheckValid())
			{
				_spawner.SetPrintLog(printLog);
				_spawner.Setup(_spawnerData, _generator, this);
				Prepare_CurveUsage();
			}
		}

		protected override bool CanPlayOnStart()
		{
			if (playOnStartFunc == null)
			{
				return playOnStart;
			}
			return playOnStartFunc(this, playOnStart);
		}

		protected override void Start()
		{
			if (Application.isPlaying && CheckValid())
			{
				if (!prepared)
				{
					Prepare();
				}
				base.Start();
			}
		}

		public override void Prepare()
		{
			base.Prepare();
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: true);
			}
			if (_processAction == null)
			{
				if (useStaticProcessor)
				{
					_processAction = fishProcessAction;
					if (printLog)
					{
						UnityEngine.Debug.Log(string.Format("{0}.Start> useStaticProcess. processor:{1}", GetIdentity(), "fishProcessAction"));
					}
				}
				else
				{
					_processAction = DoProcess_Default;
					if (printLog)
					{
						UnityEngine.Debug.Log($"{GetIdentity()}.Start> useDefaultProcess.");
					}
				}
			}
			_spawner.SetProcess(_processAction);
			Update_CurveUsage();
			prepared = true;
		}

		public void Prepare_CurveUsage()
		{
			_curveUsage = new FK3_CurveUsage(_curve, id);
			_curveUsage.speed = moveSpeed;
			_curveUsage.startDelay = moveStartDelay;
			FK3_CurveUsage curveUsage = _curveUsage;
			curveUsage.onCreateUsage = (Action<FK3_CurveUsage, FK3_CursorUsage>)Delegate.Combine(curveUsage.onCreateUsage, new Action<FK3_CurveUsage, FK3_CursorUsage>(OnCreateCursorUsage));
			_curveUsage.disableOnNotUsed = false;
			_curveUsage.Prepare();
		}

		public void Update_CurveUsage()
		{
			if (_curveUsage == null)
			{
				UnityEngine.Debug.LogError("======curveUsage 为空====== " + base.name);
				return;
			}
			_curveUsage.speed = moveSpeed;
			_curveUsage.startDelay = moveStartDelay;
		}

		public FK3_CurveUsage GetCurveUsage()
		{
			if (_curveUsage == null)
			{
				Prepare_CurveUsage();
			}
			return _curveUsage;
		}

		public override bool CheckValid()
		{
			return !ErrorJudge(_curve == null, "_curve is null") && base.CheckValid();
		}

		private void DoProcess_Default(FK3_ProcessData<FK3_FishType> data)
		{
			if (printLog)
			{
				UnityEngine.Debug.Log(FK3_LogHelper.Orange("FK3_FishSpawnerBehaviour.DoProcess_Default> spawn[{0}] fish[type:{1},index:{2}]", data.info, data.value, data.index));
			}
		}

		public void DoAfterProcess(FK3_ProcessData<FK3_FishType> data)
		{
			if (data.index == 0)
			{
				_spawnCount = 0;
				_ignoreCount = 0;
				_despawnCount = 0;
			}
			_spawnCount++;
			if (data.ignore)
			{
				_ignoreCount++;
			}
			else if (!(data.objBehaviour == null))
			{
				FK3_FishBehaviour fK3_FishBehaviour = data.objBehaviour as FK3_FishBehaviour;
				_fishList.Add(fK3_FishBehaviour);
				FK3_FishBehaviour fK3_FishBehaviour2 = fK3_FishBehaviour;
				fK3_FishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate(FK3_FishBehaviour _fish)
				{
					_despawnCount++;
					if (!_fishList.Contains(_fish))
					{
						UnityEngine.Debug.LogError(FK3_LogHelper.Red("{0}>list not find {1}. list.count:{2} ", GetIdentity(), _fish.identity, _fishList.Count));
					}
					_fishList.Remove(_fish);
					int num = Count();
					if (data.index == num - 1)
					{
						_spawnCount = 0;
						_ignoreCount = 0;
						_despawnCount = 0;
					}
				});
			}
		}

		private void OnCreateCursorUsage(FK3_CurveUsage curveUsage, FK3_CursorUsage cursorUsage)
		{
			_activeCursors.Add(cursorUsage);
			cursorUsage.FreeAction = (Action<FK3_CursorUsage>)Delegate.Remove(cursorUsage.FreeAction, new Action<FK3_CursorUsage>(OnCursorFree));
			cursorUsage.FreeAction = (Action<FK3_CursorUsage>)Delegate.Combine(cursorUsage.FreeAction, new Action<FK3_CursorUsage>(OnCursorFree));
		}

		private void OnCursorFree(FK3_CursorUsage cursorUsage)
		{
			_activeCursors.Remove(cursorUsage);
			cursorUsage.FreeAction = (Action<FK3_CursorUsage>)Delegate.Remove(cursorUsage.FreeAction, new Action<FK3_CursorUsage>(OnCursorFree));
		}

		public void GainCurve(bool logError = false)
		{
			if (_curve == null)
			{
				_curve = GetComponent<BGCurve>();
			}
			if (_curve == null && logError)
			{
				UnityEngine.Debug.LogError(string.Format("GainCurve failed", GetIdentity()));
			}
		}

		public BGCurve GetCurve()
		{
			return _curve;
		}

		private void AOTSupportHelp()
		{
			FK3_IGenerator<FK3_FishType> fK3_IGenerator = new FK3_GeneratorBase<FK3_FishType>();
			FK3_IGenerator<FK3_FishType> fK3_IGenerator2 = new FK3_SingleGenerator<FK3_FishType>();
			FK3_IGenerator<FK3_FishType> fK3_IGenerator3 = new FK3_SequenceGenerator<FK3_FishType>();
			FK3_IGenerator<FK3_FishType> fK3_IGenerator4 = new FK3_FancySequenceGenerator<FK3_FishType>();
		}

		[InspectorButton]
		[InspectorShowIf("isRunning")]
		private void LetFishesDie()
		{
			FK3_FishBehaviour[] array = _fishList.ToArray();
			FK3_FishBehaviour[] array2 = array;
			foreach (FK3_FishBehaviour fK3_FishBehaviour in array2)
			{
				if (fK3_FishBehaviour != null)
				{
					fK3_FishBehaviour.Die();
				}
				if (fK3_FishBehaviour != null)
				{
					fK3_FishBehaviour.Die();
				}
			}
		}

		[InspectorButton]
		[InspectorShowIf("isRunning")]
		private void LetFishesDyingAndDie()
		{
			FK3_FishBehaviour[] array = _fishList.ToArray();
			FK3_FishBehaviour[] array2 = array;
			foreach (FK3_FishBehaviour fK3_FishBehaviour in array2)
			{
				if (fK3_FishBehaviour != null)
				{
					fK3_FishBehaviour.Dying();
					fK3_FishBehaviour.Die();
				}
			}
		}
	}
}
