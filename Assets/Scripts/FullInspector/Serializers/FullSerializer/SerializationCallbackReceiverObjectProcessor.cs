using FullSerializer;
using FullSerializer.Internal;
using System;
using UnityEngine;

namespace FullInspector.Serializers.FullSerializer
{
	public class SerializationCallbackReceiverObjectProcessor : fsObjectProcessor
	{
		public override bool CanProcess(Type type)
		{
			return !typeof(UnityEngine.Object).Resolve().IsAssignableFrom(type.Resolve()) && typeof(ISerializationCallbackReceiver).Resolve().IsAssignableFrom(type.Resolve()) && !typeof(BaseObject).Resolve().IsAssignableFrom(type.Resolve());
		}

		public override void OnBeforeSerialize(Type storageType, object instance)
		{
			((ISerializationCallbackReceiver)instance)?.OnBeforeSerialize();
		}

		public override void OnAfterSerialize(Type storageType, object instance, ref fsData data)
		{
		}

		public override void OnBeforeDeserialize(Type storageType, ref fsData data)
		{
		}

		public override void OnAfterDeserialize(Type storageType, object instance)
		{
			((ISerializationCallbackReceiver)instance)?.OnAfterDeserialize();
		}
	}
}
