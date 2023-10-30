namespace FullInspector.Samples.Other.CustomOrder
{
	public struct Struct
	{
		[InspectorOrder(0.0)]
		[InspectorComment("Structs/classes also get their own order groups")]
		[InspectorHidePrimary]
		[ShowInInspector]
		private int __inspectorComment;

		[InspectorOrder(2.0)]
		public int Two;

		[InspectorOrder(1.0)]
		public int One;
	}
}
