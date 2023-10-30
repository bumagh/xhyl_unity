using BansheeGz.BGSpline.Curve;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public static class BGCurveUtils
	{
		public static void CopyBGCurveBasic(BGCurve target, BGCurve source)
		{
			target.Closed = source.Closed;
			target.PointsMode = source.PointsMode;
			target.Mode2D = source.Mode2D;
			target.SnapType = source.SnapType;
			target.SnapAxis = source.SnapAxis;
			target.SnapDistance = source.SnapDistance;
			target.SnapTriggerInteraction = source.SnapTriggerInteraction;
			target.SnapToBackFaces = source.SnapToBackFaces;
			target.SnapLayerMask = source.SnapLayerMask;
			target.ForceChangedEventMode = source.ForceChangedEventMode;
			target.SupressEvents = source.SupressEvents;
			target.UseEventsArgs = source.UseEventsArgs;
			target.EventMode = source.EventMode;
			target.ImmediateChangeEvents = source.ImmediateChangeEvents;
		}

		public static void CopyBGCurvePoints(BGCurve target, BGCurve source, bool useWorldCoordinates = false)
		{
			target.Clear();
			BGCurvePointI[] points = source.Points;
			foreach (BGCurvePointI bGCurvePointI in points)
			{
				target.AddPoint(new BGCurvePoint(target, useWorldCoordinates ? bGCurvePointI.PositionWorld : bGCurvePointI.PositionLocal, bGCurvePointI.ControlType, bGCurvePointI.ControlFirstLocal, bGCurvePointI.ControlSecondLocal, useWorldCoordinates));
			}
		}
	}
}
