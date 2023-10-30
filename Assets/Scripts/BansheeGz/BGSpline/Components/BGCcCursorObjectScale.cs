using BansheeGz.BGSpline.Curve;
using System;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcCursorObjectScale")]
	[CcDescriptor(Description = "Scale the object, according to cursor position. Scale values are taken from curve's field values.", Name = "Scale Object By Cursor", Icon = "BGCcCursorObjectScale123")]
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcScaleObject")]
	[ExecuteInEditMode]
	public class BGCcCursorObjectScale : BGCcWithCursorObject
	{
		[SerializeField]
		[Tooltip("Field to store the scale value at points. It should be a Vector3 field.")]
		private BGCurvePointField scaleField;

		public BGCurvePointField ScaleField
		{
			get
			{
				return scaleField;
			}
			set
			{
				ParamChanged(ref scaleField, value);
			}
		}

		public override string Error => ChoseMessage(base.Error, () => (!(scaleField == null)) ? null : "Scale field is not defined.");

		public event EventHandler ObjectScaled;

		private void Update()
		{
			if (base.ObjectToManipulate == null || scaleField == null)
			{
				return;
			}
			switch (base.Curve.PointsCount)
			{
			case 0:
				return;
			case 1:
				base.ObjectToManipulate.localScale = base.Curve[0].GetVector3(scaleField.FieldName);
				return;
			}
			Vector3 localScale = LerpVector(scaleField.FieldName);
			if (!float.IsNaN(localScale.x) && !float.IsNaN(localScale.y) && !float.IsNaN(localScale.z))
			{
				base.ObjectToManipulate.localScale = localScale;
				if (this.ObjectScaled != null)
				{
					this.ObjectScaled(this, null);
				}
			}
		}
	}
}
