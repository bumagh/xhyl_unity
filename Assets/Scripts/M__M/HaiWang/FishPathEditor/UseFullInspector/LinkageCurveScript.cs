using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	[ExecuteInEditMode]
	public class LinkageCurveScript : BaseBehavior<FullSerializerSerializer>
	{
		[SerializeField]
		private CopyWorkshop workshop;

		protected override void Awake()
		{
			base.Awake();
			Init();
		}

		private void Init()
		{
			if (workshop == null)
			{
				workshop = new CopyWorkshop();
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
			LinkageCurveData linkageCurveData = new LinkageCurveData();
			linkageCurveData.root = base.transform;
			linkageCurveData.node = bGCurve.transform;
			linkageCurveData.curve = bGCurve;
			linkageCurveData.master = null;
			linkageCurveData.isMaster = true;
			linkageCurveData.nodeProvider = CreateBGCurve;
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

		private void CreateByOffsetModeWorkshop(OffsetModeWorkshop workshop)
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
