using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	public class BGTestCurveRuntimeCustomFields : MonoBehaviour
	{
		private const string SpeedFieldName = "speed";

		private const string DelayFieldName = "delay";

		private const float Width = 0.02f;

		public Transform ObjectToMove;

		public Material LineRendererMaterial;

		private void Start()
		{
			BGCcCursorObjectTranslate bGCcCursorObjectTranslate = base.gameObject.AddComponent<BGCcCursorObjectTranslate>();
			bGCcCursorObjectTranslate.ObjectToManipulate = ObjectToMove;
			BGCcCursorChangeLinear bGCcCursorChangeLinear = base.gameObject.AddComponent<BGCcCursorChangeLinear>();
			base.gameObject.AddComponent<BGCcVisualizationLineRenderer>();
			LineRenderer component = base.gameObject.GetComponent<LineRenderer>();
			component.sharedMaterial = LineRendererMaterial;
			float num3 = component.startWidth = (component.endWidth = 0.02f);
			BGCurve curve = bGCcCursorChangeLinear.Curve;
			curve.Closed = true;
			curve.Mode2D = BGCurve.Mode2DEnum.XY;
			curve.PointsMode = BGCurve.PointsModeEnum.GameObjectsTransform;
			curve.AddPoint(new BGCurvePoint(curve, new Vector2(-5f, 0f)));
			curve.AddPoint(new BGCurvePoint(curve, new Vector2(0f, 5f), BGCurvePoint.ControlTypeEnum.BezierSymmetrical, new Vector2(-5f, 0f), new Vector2(5f, 0f)));
			curve.AddPoint(new BGCurvePoint(curve, new Vector2(5f, 0f)));
			bGCcCursorChangeLinear.SpeedField = NewFloatField(bGCcCursorChangeLinear, "speed", 5f, 10f, 15f);
			bGCcCursorChangeLinear.DelayField = NewFloatField(bGCcCursorChangeLinear, "delay", 3f, 1f, 2f);
		}

		private static BGCurvePointField NewFloatField(BGCcCursorChangeLinear changeCursor, string fieldName, params float[] values)
		{
			BGCurve curve = changeCursor.Curve;
			BGCurvePointField result = curve.AddField(fieldName, BGCurvePointField.TypeEnum.Float);
			for (int i = 0; i < values.Length; i++)
			{
				curve[i].SetFloat(fieldName, values[i]);
			}
			return result;
		}
	}
}
