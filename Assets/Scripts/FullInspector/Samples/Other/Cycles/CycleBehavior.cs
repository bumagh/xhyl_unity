using UnityEngine;

namespace FullInspector.Samples.Other.Cycles
{
	[AddComponentMenu("Full Inspector Samples/Other/Cycles")]
	public class CycleBehavior : BaseBehavior<FullSerializerSerializer>
	{
		[InspectorHidePrimary]
		[ShowInInspector]
		[InspectorComment(CommentType.Info, "CycleRoot points to an object graph that internally contains cycles. Full Inspector automatically detects this and displays <cycle> when an inspected object will lead to an infinite loop")]
		private int _comment;

		public CycleType CycleRoot;

		public void Reset()
		{
			CycleRoot = new CycleType
			{
				PrefixValue = 1,
				PostfixValue = 2
			};
			CycleRoot.CycleReference = new CycleType
			{
				PrefixValue = 3,
				PostfixValue = 4
			};
			CycleRoot.CycleReference.CycleReference = CycleRoot;
		}
	}
}
