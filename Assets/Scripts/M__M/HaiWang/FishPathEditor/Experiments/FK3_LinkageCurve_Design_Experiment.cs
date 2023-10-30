using BansheeGz.BGSpline.Curve;
using FullInspector;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Experiments
{
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public class FK3_LinkageCurve_Design_Experiment : BaseBehavior<FullSerializerSerializer>
	{
		private BGCurve _masterCurve;

		private BGCurve _slaveCurve;

		private bool m_autoSync;

		private bool _alwaysShowCurve = true;

		[SerializeField]
		private BGCurve masterCurve
		{
			get
			{
				return _masterCurve;
			}
			set
			{
				if (!(_masterCurve == value))
				{
					_masterCurve = value;
					CurveRelatedChanged("masterCurve");
				}
			}
		}

		[SerializeField]
		private BGCurve slaveCurve
		{
			get
			{
				return _slaveCurve;
			}
			set
			{
				if (!(_slaveCurve == value))
				{
					_slaveCurve = value;
					CurveRelatedChanged("slaveCurve");
				}
			}
		}

		[ShowInInspector]
		private bool alwaysShowCurve
		{
			get
			{
				return _alwaysShowCurve;
			}
			set
			{
				_alwaysShowCurve = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			CurveRelatedChanged("awake");
		}

		private void Start()
		{
		}

		[InspectorName("自动同步")]
		[InspectorButton]
		[InspectorHideIf("m_autoSync")]
		private void AutoSync()
		{
			m_autoSync = true;
			CurveRelatedChanged("m_autoSync");
		}

		[InspectorName("取消自动同步")]
		[InspectorShowIf("m_autoSync")]
		[InspectorButton]
		private void CancelAuto()
		{
			m_autoSync = false;
			CurveRelatedChanged("m_autoSync");
		}

		[InspectorButton]
		[InspectorName("手动同步")]
		[InspectorHideIf("m_autoSync")]
		private void ForceSync()
		{
			if (CheckCanSync())
			{
				DoSync();
			}
			else
			{
				UnityEngine.Debug.Log("ForceSync failed. sth not ready.");
			}
		}

		private void CurveRelatedChanged(string fieldName)
		{
			UnityEngine.Debug.Log($"{fieldName} changed:");
			if (_masterCurve != null && _slaveCurve != null && m_autoSync)
			{
				_masterCurve.Changed -= MasterCurve_Changed;
				_masterCurve.Changed += MasterCurve_Changed;
				DoSync();
			}
			else if ((bool)_masterCurve)
			{
				_masterCurve.Changed -= MasterCurve_Changed;
			}
		}

		private void MasterCurve_Changed(object sender, BGCurveChangedArgs e)
		{
			if (_slaveCurve != null)
			{
				DoSync();
			}
		}

		private bool CheckCanSync()
		{
			return _slaveCurve != null && _masterCurve != null;
		}

		private void DoSync()
		{
			FK3_BGCurveUtils.CopyBGCurveBasic(_slaveCurve, _masterCurve);
			FK3_BGCurveUtils.CopyBGCurvePoints(_slaveCurve, _masterCurve);
		}
	}
}
