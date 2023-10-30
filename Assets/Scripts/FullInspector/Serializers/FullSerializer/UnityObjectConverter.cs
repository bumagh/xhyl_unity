using FullSerializer;
using FullSerializer.Internal;
using System;
using UnityEngine;

namespace FullInspector.Serializers.FullSerializer
{
	public class UnityObjectConverter : fsConverter
	{
		public override bool CanProcess(Type type)
		{
			return typeof(UnityEngine.Object).Resolve().IsAssignableFrom(type.Resolve());
		}

		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			UnityEngine.Object obj = (UnityEngine.Object)instance;
			ISerializationOperator serializationOperator = Serializer.Context.Get<ISerializationOperator>();
			int num = serializationOperator.StoreObjectReference(obj);
			if (num < 0)
			{
				serialized = fsData.Null;
				return fsResult.Success;
			}
			return Serializer.TrySerialize(num, out serialized);
		}

		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			ISerializationOperator serializationOperator = Serializer.Context.Get<ISerializationOperator>();
			int instance2 = 0;
			fsResult result = Serializer.TryDeserialize(data, ref instance2);
			if (result.Failed)
			{
				return result;
			}
			instance = serializationOperator.RetrieveObjectReference(instance2);
			return fsResult.Success;
		}

		public override object CreateInstance(fsData data, Type storageType)
		{
			return storageType;
		}
	}
}
