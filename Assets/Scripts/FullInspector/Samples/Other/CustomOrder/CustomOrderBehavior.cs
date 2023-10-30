using UnityEngine;

namespace FullInspector.Samples.Other.CustomOrder
{
	[AddComponentMenu("Full Inspector Samples/Other/Custom Order")]
	public class CustomOrderBehavior : BaseBehavior<FullSerializerSerializer>
	{
		[InspectorOrder(2.0)]
		public int Two;

		[InspectorOrder(2.2)]
		public Struct TwoPtTwo;

		[InspectorOrder(1.0)]
		public int One;

		[InspectorOrder(-10.0)]
		public int NegativeTen;

		public int NonOrdered;

		[InspectorOrder(2.1)]
		public int TwoPt1
		{
			get;
			set;
		}

		[InspectorOrder(3.0)]
		public int Three
		{
			get;
			private set;
		}
	}
}
