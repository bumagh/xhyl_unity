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
	public class FishSpawnerBehaviour : SpawnerBehaviour<FishType>
	{
		[SerializeField]
		protected BGCurve _curve;

		public float moveSpeed = 2f;

		public float moveStartDelay;

		private CurveUsage _curveUsage;

		private List<CursorUsage> _activeCursors;

		[NotSerialized]
		[ShowInInspector]
		public List<FishBehaviour> _fishList;

		public static bool useStaticProcessor;

		public static Action<ProcessData<FishType>> fishProcessAction;

		public static Func<FishSpawnerBehaviour, bool, bool> playOnStartFunc;

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
				_activeCursors = new List<CursorUsage>();
			}
			_spawner = new SpawnerBase<FishType>();
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
			_curveUsage = new CurveUsage(_curve, id);
			_curveUsage.speed = moveSpeed;
			_curveUsage.startDelay = moveStartDelay;
			CurveUsage curveUsage = _curveUsage;
			curveUsage.onCreateUsage = (Action<CurveUsage, CursorUsage>)Delegate.Combine(curveUsage.onCreateUsage, new Action<CurveUsage, CursorUsage>(OnCreateCursorUsage));
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

		public CurveUsage GetCurveUsage()
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

		private void DoProcess_Default(ProcessData<FishType> data)
		{
			if (printLog)
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Orange("FishSpawnerBehaviour.DoProcess_Default> spawn[{0}] fish[type:{1},index:{2}]", data.info, data.value, data.index));
			}
		}

		public void DoAfterProcess(ProcessData<FishType> data)
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
				FishBehaviour fishBehaviour = data.objBehaviour as FishBehaviour;
				_fishList.Add(fishBehaviour);
				FishBehaviour fishBehaviour2 = fishBehaviour;
				fishBehaviour2.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, (Action<FishBehaviour>)delegate(FishBehaviour _fish)
				{
					_despawnCount++;
					if (!_fishList.Contains(_fish))
					{
						UnityEngine.Debug.LogError(HW2_LogHelper.Red("{0}>list not find {1}. list.count:{2} ", GetIdentity(), _fish.identity, _fishList.Count));
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

		private void OnCreateCursorUsage(CurveUsage curveUsage, CursorUsage cursorUsage)
		{
			_activeCursors.Add(cursorUsage);
			cursorUsage.FreeAction = (Action<CursorUsage>)Delegate.Remove(cursorUsage.FreeAction, new Action<CursorUsage>(OnCursorFree));
			cursorUsage.FreeAction = (Action<CursorUsage>)Delegate.Combine(cursorUsage.FreeAction, new Action<CursorUsage>(OnCursorFree));
		}

		private void OnCursorFree(CursorUsage cursorUsage)
		{
			_activeCursors.Remove(cursorUsage);
			cursorUsage.FreeAction = (Action<CursorUsage>)Delegate.Remove(cursorUsage.FreeAction, new Action<CursorUsage>(OnCursorFree));
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
			IGenerator<FishType> generator = new GeneratorBase<FishType>();
			IGenerator<FishType> generator2 = new SingleGenerator<FishType>();
			IGenerator<FishType> generator3 = new SequenceGenerator<FishType>();
			IGenerator<FishType> generator4 = new FancySequenceGenerator<FishType>();
		}

		[InspectorShowIf("isRunning")]
		[InspectorButton]
		private void LetFishesDie()
		{
			FishBehaviour[] array = _fishList.ToArray();
			FishBehaviour[] array2 = array;
			foreach (FishBehaviour fishBehaviour in array2)
			{
				if (fishBehaviour != null)
				{
					fishBehaviour.Die();
				}
				if (fishBehaviour != null)
				{
					fishBehaviour.Die();
				}
			}
		}

		[InspectorShowIf("isRunning")]
		[InspectorButton]
		private void LetFishesDyingAndDie()
		{
			FishBehaviour[] array = _fishList.ToArray();
			FishBehaviour[] array2 = array;
			foreach (FishBehaviour fishBehaviour in array2)
			{
				if (fishBehaviour != null)
				{
					fishBehaviour.Dying();
					fishBehaviour.Die();
				}
			}
		}
	}
}
