using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullInspector.Internal.Preserve;
using FullInspector.Serializers.FullSerializer;
using FullSerializer;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector
{
	[Preserve]
	public class FullSerializerSerializer : BaseSerializer
	{
		static FullSerializerSerializer()
		{
			FullSerializerSerializer.AddConverter<UnityObjectConverter>();
			FullSerializerSerializer.AddProcessor<SerializationCallbackReceiverObjectProcessor>();
		}

		private static fsSerializer Serializer
		{
			get
			{
				if (FullSerializerSerializer._serializer == null)
				{
					object typeFromHandle = typeof(FullSerializerSerializer);
					object obj = typeFromHandle;
					lock (obj)
					{
						FullSerializerSerializer._serializer = new fsSerializer();
						FullSerializerSerializer._serializers.Add(FullSerializerSerializer._serializer);
						FullSerializerSerializer._serializer.RemoveProcessor<fsSerializationCallbackReceiverProcessor>();
						foreach (Type type in FullSerializerSerializer._converters)
						{
							FullSerializerSerializer._serializer.AddConverter((fsConverter)Activator.CreateInstance(type));
						}
						foreach (Type type2 in FullSerializerSerializer._processors)
						{
							FullSerializerSerializer._serializer.AddProcessor((fsObjectProcessor)Activator.CreateInstance(type2));
						}
					}
				}
				return FullSerializerSerializer._serializer;
			}
		}

		public static void AddConverter<TConverter>() where TConverter : fsConverter, new()
		{
			object typeFromHandle = typeof(FullSerializerSerializer);
			object obj = typeFromHandle;
			lock (obj)
			{
				FullSerializerSerializer._converters.Add(typeof(TConverter));
				foreach (fsSerializer fsSerializer in FullSerializerSerializer._serializers)
				{
					fsSerializer.AddConverter(Activator.CreateInstance<TConverter>());
				}
			}
		}

		public static void AddProcessor<TProcessor>() where TProcessor : fsObjectProcessor, new()
		{
			object typeFromHandle = typeof(FullSerializerSerializer);
			object obj = typeFromHandle;
			lock (obj)
			{
				FullSerializerSerializer._processors.Add(typeof(TProcessor));
				foreach (fsSerializer fsSerializer in FullSerializerSerializer._serializers)
				{
					fsSerializer.AddProcessor(Activator.CreateInstance<TProcessor>());
				}
			}
		}

		public override string Serialize(MemberInfo storageType, object value, ISerializationOperator serializationOperator)
		{
			FullSerializerSerializer.Serializer.Context.Set<ISerializationOperator>(serializationOperator);
			fsData data;
			fsResult result = FullSerializerSerializer.Serializer.TrySerialize(BaseSerializer.GetStorageType(storageType), value, out data);
			if (FullSerializerSerializer.EmitFailWarning(result))
			{
				return null;
			}
			if (fiSettings.PrettyPrintSerializedJson)
			{
				return fsJsonPrinter.PrettyJson(data);
			}
			return fsJsonPrinter.CompressedJson(data);
		}

		public override object Deserialize(MemberInfo storageType, string serializedState, ISerializationOperator serializationOperator)
		{
			fsData data;
			fsResult result = fsJsonParser.Parse(serializedState, out data);
			if (FullSerializerSerializer.EmitFailWarning(result))
			{
				return null;
			}
			FullSerializerSerializer.Serializer.Context.Set<ISerializationOperator>(serializationOperator);
			object result2 = null;
			result = FullSerializerSerializer.Serializer.TryDeserialize(data, BaseSerializer.GetStorageType(storageType), ref result2);
			if (FullSerializerSerializer.EmitFailWarning(result))
			{
				return null;
			}
			return result2;
		}

		public override bool SupportsMultithreading
		{
			get
			{
				return true;
			}
		}

		private static bool EmitFailWarning(fsResult result)
		{
			if (fiSettings.EmitWarnings && result.RawMessages.Any<string>())
			{
				Debug.LogWarning(result.FormattedMessages);
			}
			return result.Failed;
		}

		[ThreadStatic]
		private static fsSerializer _serializer;

		private static readonly List<fsSerializer> _serializers = new List<fsSerializer>();

		private static readonly List<Type> _converters = new List<Type>();

		private static readonly List<Type> _processors = new List<Type>();
	}
}
