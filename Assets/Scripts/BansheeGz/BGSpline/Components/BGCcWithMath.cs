using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	[RequireComponent(typeof(BGCcMath))]
	public abstract class BGCcWithMath : BGCc
	{
		private BGCcMath math;

		public BGCcMath Math
		{
			get
			{
				if (math == null)
				{
					math = GetComponent<BGCcMath>();
				}
				return math;
			}
			set
			{
				if (!(value == null))
				{
					math = value;
					SetParent(value);
				}
			}
		}

		public override string Error => (!(Math == null)) ? null : "Math is null";
	}
}
