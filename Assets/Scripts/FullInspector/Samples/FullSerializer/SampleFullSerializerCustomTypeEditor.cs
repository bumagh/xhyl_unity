using UnityEngine;

namespace FullInspector.Samples.FullSerializer
{
	[AddComponentMenu("Full Inspector Samples/FullSerializer/Custom Type Editor")]
	public class SampleFullSerializerCustomTypeEditor : BaseBehavior<FullSerializerSerializer>
	{
		public CustomTypeEditorNonGeneric CustomTypeEditorNonGeneric;

		public CustomTypeEditorGeneric<float, string> CustomTypeEditorFloatString;

		public ICustomTypeEditorInherited ICustomTypeEditorInherited;

		public CustomTypeEditorInheritedA CustomTypeEditorInheritedA;

		public CustomTypeEditorInheritedB CustomTypeEditorInheritedB;
	}
}
