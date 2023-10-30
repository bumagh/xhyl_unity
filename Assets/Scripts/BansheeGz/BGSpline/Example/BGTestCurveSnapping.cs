using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	public class BGTestCurveSnapping : MonoBehaviour
	{
		public BGCurve Curve;

		public float XChange = 5f;

		public float YChange = 0.5f;

		private bool goingUp = true;

		private Vector3 from;

		private Vector3 to;

		private float initialY;

		private void Start()
		{
			float x = 0f - XChange;
			Vector3 position = base.transform.position;
			float y = position.y;
			Vector3 position2 = base.transform.position;
			from = new Vector3(x, y, position2.z);
			float xChange = XChange;
			Vector3 position3 = base.transform.position;
			float y2 = position3.y;
			Vector3 position4 = base.transform.position;
			to = new Vector3(xChange, y2, position4.z);
			Vector3 position5 = base.transform.position;
			initialY = position5.y;
		}

		private void Update()
		{
			Vector3 position = base.transform.position;
			Vector3 b = Vector3.up * Time.deltaTime * 2f;
			position = ((!goingUp) ? (position - b) : (position + b));
			if (position.y > initialY + YChange)
			{
				goingUp = false;
			}
			else if (position.y < initialY - YChange)
			{
				goingUp = true;
			}
			Vector3 position2 = Vector3.MoveTowards(position, to, Time.deltaTime * 2f);
			if ((double)Mathf.Abs(position2.x - to.x) < 0.1 && (double)Mathf.Abs(position2.z - to.z) < 0.1)
			{
				Vector3 vector = to;
				to = from;
				from = vector;
			}
			base.transform.position = position2;
			if (Curve.SnapMonitoring)
			{
				return;
			}
			switch (Curve.SnapType)
			{
			case BGCurve.SnapTypeEnum.Points:
				Curve.ApplySnapping();
				break;
			case BGCurve.SnapTypeEnum.Curve:
				if (!Curve.ApplySnapping())
				{
					Curve.GetComponent<BGCcMath>().Recalculate();
				}
				break;
			}
		}
	}
}
