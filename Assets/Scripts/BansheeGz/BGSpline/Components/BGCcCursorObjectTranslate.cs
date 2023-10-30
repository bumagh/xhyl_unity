using BansheeGz.BGSpline.Curve;
using System;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[CcDescriptor(Description = "Translate an object to the position, the cursor provides.", Name = "Translate Object By Cursor", Image = "Assets/BansheeGz/BGCurve/Icons/Components/BGCcCursorObjectTranslate123.png")]
	[ExecuteInEditMode]
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcCursorObjectTranslate")]
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcTranslateObject")]
	public class BGCcCursorObjectTranslate : BGCcWithCursorObject
	{
		public Vector3 offset
		{
			get;
			protected set;
		}

		public event EventHandler ObjectTranslated;

		public Vector3 SetOffset(Vector3 offset, bool adjustByRotation, Vector3 upDir)
		{
			if (adjustByRotation)
			{
				BGCcMath component = curve.GetComponent<BGCcMath>();
				Vector3 toDirection = component.CalcTangentByDistanceRatio(0f);
				this.offset = Quaternion.FromToRotation(upDir, toDirection) * offset;
			}
			else
			{
				this.offset = offset;
			}
			return this.offset;
		}

		public Vector3 SetOffset(Vector3 offset)
		{
			return SetOffset(offset, adjustByRotation: false, Vector3.zero);
		}

		public Vector3 SetOffset(Vector3 offset, bool adjustByRotation)
		{
			return SetOffset(offset, adjustByRotation, Vector3.right);
		}

		public void ForceUpdate()
		{
			Update();
		}

		private void Update()
		{
			Transform objectToManipulate = base.ObjectToManipulate;
			if (objectToManipulate == null)
			{
				return;
			}
			switch (base.Curve.PointsCount)
			{
			case 0:
				return;
			case 1:
				objectToManipulate.position = base.Curve[0].PositionWorld + offset;
				return;
			}
			objectToManipulate.position = base.Cursor.CalculatePosition() + offset;
			if (this.ObjectTranslated != null)
			{
				this.ObjectTranslated(this, null);
			}
		}
	}
}
