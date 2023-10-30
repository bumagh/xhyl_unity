using UnityEngine;

namespace FullInspector.Samples
{
	[AddComponentMenu("Full Inspector Samples/Other/Categories")]
	public class Categories : BaseBehavior<FullSerializerSerializer>
	{
		[InspectorCategory("Category A")]
		public int a0;

		[InspectorCategory("Category A")]
		public int a1;

		[InspectorCategory("Category A")]
		public int a2;

		[InspectorCategory("Category A")]
		public int a3;

		[InspectorCategory("Category A")]
		public int a4;

		[InspectorCategory("Category B")]
		public int b0;

		[InspectorCategory("Category B")]
		public int b1;

		[InspectorCategory("Category B")]
		public int b2;

		[InspectorCategory("Category B")]
		public int b3;

		[InspectorCategory("Category B")]
		public int b4;

		[InspectorCategory("Category A")]
		[InspectorCategory("Category B")]
		public int common0;

		[InspectorCategory("Category A")]
		[InspectorCategory("Category B")]
		public int common1;

		public int notShown;
	}
}
