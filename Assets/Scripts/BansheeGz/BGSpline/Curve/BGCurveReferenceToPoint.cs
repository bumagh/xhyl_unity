using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	public class BGCurveReferenceToPoint : MonoBehaviour
	{
		[SerializeField]
		private BGCurvePointComponent pointComponent;

		[SerializeField]
		private BGCurvePointGO pointGo;

		public BGCurvePointI Point
		{
			get
			{
				return (BGCurvePointI)((!(pointGo != null)) ? ((object)pointComponent) : ((object)pointGo));
			}
			set
			{
				if (value == null)
				{
					pointGo = null;
					pointComponent = null;
				}
				else if (value is BGCurvePointGO)
				{
					pointGo = (BGCurvePointGO)value;
					pointComponent = null;
				}
				else if (value is BGCurvePointComponent)
				{
					pointComponent = (BGCurvePointComponent)value;
					pointGo = null;
				}
				else
				{
					pointGo = null;
					pointComponent = null;
				}
			}
		}

		public static BGCurveReferenceToPoint GetReferenceToPoint(BGCurvePointI point)
		{
			if (point.PointTransform == null)
			{
				return null;
			}
			BGCurveReferenceToPoint[] components = point.PointTransform.GetComponents<BGCurveReferenceToPoint>();
			if (components.Length == 0)
			{
				return null;
			}
			int num = components.Length;
			for (int i = 0; i < num; i++)
			{
				BGCurveReferenceToPoint bGCurveReferenceToPoint = components[i];
				if (bGCurveReferenceToPoint.Point == point)
				{
					return bGCurveReferenceToPoint;
				}
			}
			return null;
		}
	}
}
