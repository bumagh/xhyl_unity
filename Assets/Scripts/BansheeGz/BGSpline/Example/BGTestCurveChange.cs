using UnityEngine;

namespace BansheeGz.BGSpline.Example
{
	public class BGTestCurveChange : MonoBehaviour
	{
		private const float RotationSpeed = 40f;

		private const float ScaleUpperLimit = 1.25f;

		private const float ScaleLowerLimit = 0.5f;

		private Vector3 scaleSpeed = Vector3.one * 0.1f;

		private void Update()
		{
			base.transform.RotateAround(base.transform.position, Vector3.up, 40f * Time.deltaTime);
			Vector3 a = base.transform.localScale;
			bool flag = a.x > 1.25f;
			bool flag2 = a.x < 0.5f;
			if (flag || flag2)
			{
				scaleSpeed = -scaleSpeed;
				a = ((!flag) ? new Vector3(0.5f, 0.5f, 0.5f) : new Vector3(1.25f, 1.25f, 1.25f));
			}
			base.transform.localScale = a + scaleSpeed * Time.deltaTime;
		}
	}
}
