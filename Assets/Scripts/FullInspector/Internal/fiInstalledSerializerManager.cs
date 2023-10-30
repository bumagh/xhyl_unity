using System;
using System.Collections.Generic;
using FullSerializer.Internal;

namespace FullInspector.Internal
{
	public static class fiInstalledSerializerManager
	{
		static fiInstalledSerializerManager()
		{
			List<Type> list = new List<Type>();
			List<Type> list2 = new List<Type>();
			fiInstalledSerializerManager.LoadedMetadata = new List<fiISerializerMetadata>();
			fiILoadedSerializers fiILoadedSerializers;
			if (fiInstalledSerializerManager.TryGetLoadedSerializerType(out fiILoadedSerializers))
			{
				fiInstalledSerializerManager._defaultMetadata = fiInstalledSerializerManager.GetProvider(fiILoadedSerializers.DefaultSerializerProvider);
				foreach (Type type in fiILoadedSerializers.AllLoadedSerializerProviders)
				{
					fiISerializerMetadata provider = fiInstalledSerializerManager.GetProvider(type);
					fiInstalledSerializerManager.LoadedMetadata.Add(provider);
					list.AddRange(provider.SerializationOptInAnnotationTypes);
					list2.AddRange(provider.SerializationOptOutAnnotationTypes);
				}
			}
			foreach (Type type2 in fiRuntimeReflectionUtility.AllSimpleTypesDerivingFrom(typeof(fiISerializerMetadata)))
			{
				fiISerializerMetadata provider2 = fiInstalledSerializerManager.GetProvider(type2);
				fiInstalledSerializerManager.LoadedMetadata.Add(provider2);
				list.AddRange(provider2.SerializationOptInAnnotationTypes);
				list2.AddRange(provider2.SerializationOptOutAnnotationTypes);
			}
			fiInstalledSerializerManager.SerializationOptInAnnotations = list.ToArray();
			fiInstalledSerializerManager.SerializationOptOutAnnotations = list2.ToArray();
		}

		private static fiISerializerMetadata GetProvider(Type type)
		{
			return (fiISerializerMetadata)Activator.CreateInstance(type);
		}

		public static bool TryGetLoadedSerializerType(out fiILoadedSerializers serializers)
		{
			string name = "FullInspector.Internal.fiLoadedSerializers";
			fsTypeCache.Reset();
			Type type = fsTypeCache.GetType(name);
			if (type == null)
			{
				serializers = null;
				return false;
			}
			serializers = (fiILoadedSerializers)Activator.CreateInstance(type);
			return true;
		}

		public static List<fiISerializerMetadata> LoadedMetadata { get; private set; }

		public static fiISerializerMetadata DefaultMetadata
		{
			get
			{
				if (fiInstalledSerializerManager._defaultMetadata == null)
				{
					throw new InvalidOperationException("Please register a default serializer. You should see a popup window on the next serialization reload.");
				}
				return fiInstalledSerializerManager._defaultMetadata;
			}
		}

		public static bool IsLoaded(Guid serializerGuid)
		{
			if (fiInstalledSerializerManager.LoadedMetadata == null)
			{
				return false;
			}
			for (int i = 0; i < fiInstalledSerializerManager.LoadedMetadata.Count; i++)
			{
				if (fiInstalledSerializerManager.LoadedMetadata[i].SerializerGuid == serializerGuid)
				{
					return true;
				}
			}
			return false;
		}

		public static bool HasDefault
		{
			get
			{
				return fiInstalledSerializerManager._defaultMetadata != null;
			}
		}

		public static Type[] SerializationOptInAnnotations { get; private set; }

		public static Type[] SerializationOptOutAnnotations { get; private set; }

		public const string GeneratedTypeName = "fiLoadedSerializers";

		private static fiISerializerMetadata _defaultMetadata;
	}
}
