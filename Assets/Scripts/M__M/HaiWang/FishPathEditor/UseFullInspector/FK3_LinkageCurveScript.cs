using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	[ExecuteInEditMode]
	public class FK3_LinkageCurveScript : BaseBehavior<FullSerializerSerializer>
	{
		[SerializeField]
		private FK3_CopyWorkshop workshop;

		protected override void Awake()
		{
			base.Awake();
			Init();
		}

		private void Init()
		{
			if (workshop == null)
			{
				workshop = new FK3_CopyWorkshop();
			}
			if (workshop.root == null)
			{
				workshop.root = base.transform;
			}
		}

		private void Start()
		{
		}

		[InspectorButton]
		private void AddMaster()
		{
			BGCurve bGCurve = CreateBGCurve();
			bGCurve.gameObject.name = "master";
			if (workshop != null)
			{
				workshop.master = bGCurve.transform;
			}
			FK3_LinkageCurveData fK3_LinkageCurveData = new FK3_LinkageCurveData();
			fK3_LinkageCurveData.root = base.transform;
			fK3_LinkageCurveData.node = bGCurve.transform;
			fK3_LinkageCurveData.curve = bGCurve;
			fK3_LinkageCurveData.master = null;
			fK3_LinkageCurveData.isMaster = true;
			fK3_LinkageCurveData.nodeProvider = CreateBGCurve;
		}

		[InspectorButton]
		private void Reset()
		{
			Init();
			if (workshop != null && workshop.master == null && base.transform.childCount > 0)
			{
				Transform child = base.transform.GetChild(0);
			}
		}

		private void CreateByOffsetModeWorkshop(FK3_OffsetModeWorkshop workshop)
		{
		}

		private BGCurve CreateBGCurve()
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = Vector3.zero;
			BGCurve bGCurve = gameObject.AddComponent<BGCurve>();
			bGCurve.Mode2D = BGCurve.Mode2DEnum.XY;
			BGCcMath bGCcMath = gameObject.AddComponent<BGCcMath>();
			return bGCurve;
		}
	}
}
