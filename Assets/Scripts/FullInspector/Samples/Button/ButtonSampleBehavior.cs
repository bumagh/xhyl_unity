using UnityEngine;

namespace FullInspector.Samples.Button
{
	[AddComponentMenu("Full Inspector Samples/Other/Buttons")]
	public class ButtonSampleBehavior : BaseBehavior<FullSerializerSerializer>
	{
		public ButtonContainerType ImplementedType;

		[InspectorButton]
		public static void PublicStaticButton()
		{
			UnityEngine.Debug.Log("PublicStaticButton");
		}

		[InspectorButton]
		private static void PrivateStaticButton()
		{
			UnityEngine.Debug.Log("PrivateStaticButton");
		}

		[InspectorButton]
		private void PrivateButton()
		{
			UnityEngine.Debug.Log("PrivateButton " + this);
		}

		[InspectorButton]
		public void PublicButton()
		{
			UnityEngine.Debug.Log("PublicButton " + this);
		}
	}
}
