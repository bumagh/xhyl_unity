using UnityEngine;

namespace FullInspector.Samples.FullSerializer
{
	[AddComponentMenu("Full Inspector Samples/FullSerializer/Generics")]
	public class SampleFullSerializerGenerics : BaseBehavior<FullSerializerSerializer>
	{
		public GenericHolder<int> GenericInt;

		public GenericHolder<int, string> GenericIntString;

		public GenericHolder<GenericHolder<int>> GenericGenericInt;
	}
}
