using System.IO;
using UnityEngine;

namespace FullInspector.Samples.Other.DiskSerialization
{
	[AddComponentMenu("Full Inspector Samples/Other/Disk Serialized Behavior")]
	public class DiskSerializedBehavior : BaseBehavior<FullSerializerSerializer>
	{
		[InspectorComment("This is the value that will be serialized. Use the buttons below for serialization operations.")]
		public SerializedStruct Value;

		[InspectorComment("The path that the value will be serialized to")]
		[InspectorMargin(10)]
		public string Path;

		[InspectorHidePrimary]
		[ShowInInspector]
		[InspectorMargin(10)]
		private int __inspectorMethodDivider;

		[InspectorButton]
		private void DeserializeFromPath()
		{
			string content = File.ReadAllText(Path);
			Value = fiSerializationHelpers.DeserializeFromContent<SerializedStruct, FullSerializerSerializer>(content);
			UnityEngine.Debug.Log("Object state has been restored from " + Path);
		}

		[InspectorButton]
		private void SerializeToPath()
		{
			string contents = fiSerializationHelpers.SerializeToContent<SerializedStruct, FullSerializerSerializer>(Value);
			File.WriteAllText(Path, contents);
			UnityEngine.Debug.Log("Object state has been saved to " + Path);
		}

		[InspectorButton]
		private void SerializeToConsole()
		{
			string message = fiSerializationHelpers.SerializeToContent<SerializedStruct, FullSerializerSerializer>(Value);
			UnityEngine.Debug.Log(message);
		}
	}
}
