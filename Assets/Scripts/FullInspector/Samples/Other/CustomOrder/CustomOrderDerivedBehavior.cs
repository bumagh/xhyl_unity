using UnityEngine;

namespace FullInspector.Samples.Other.CustomOrder
{
	[AddComponentMenu("Full Inspector Samples/Other/Custom Order Derived")]
	public class CustomOrderDerivedBehavior : CustomOrderBehavior
	{
		[InspectorOrder(2.0)]
		public int DerivedTwo;

		[InspectorOrder(1.0)]
		public int DerivedOne;

		[InspectorOrder(-10.0)]
		public int DerivedNegativeTen;

		public int DerivedNonOrdered;

		[InspectorOrder(2.1)]
		public int DerivedTwoPt1
		{
			get;
			set;
		}

		[InspectorOrder(3.0)]
		public int DerivedThree
		{
			get;
			private set;
		}
	}
}
