using FullInspector.Internal;
using FullSerializer;
using System.Linq;
using UnityEngine;

namespace FullInspector
{
	public static class fiPersistentMetadata
	{
		private static readonly fiIPersistentMetadataProvider[] s_providers;

		private static fiArrayDictionary<fiUnityObjectReference, fiGraphMetadata> s_metadata;

		static fiPersistentMetadata()
		{
			s_metadata = new fiArrayDictionary<fiUnityObjectReference, fiGraphMetadata>();
			s_providers = fiRuntimeReflectionUtility.GetAssemblyInstances<fiIPersistentMetadataProvider>().ToArray();
			for (int i = 0; i < s_providers.Length; i++)
			{
				fiLog.Log(typeof(fiPersistentMetadata), "Using provider {0} to support metadata of type {1}", s_providers[i].GetType().CSharpName(), s_providers[i].MetadataType.CSharpName());
			}
		}

		public static bool HasMetadata(fiUnityObjectReference target)
		{
			return s_metadata.ContainsKey(target);
		}

		public static fiGraphMetadata GetMetadataFor(Object target)
		{
			return GetMetadataFor(new fiUnityObjectReference(target, tryRestore: false));
		}

		public static fiGraphMetadata GetMetadataFor(fiUnityObjectReference target)
		{
			if (!s_metadata.TryGetValue(target, out fiGraphMetadata value))
			{
				value = new fiGraphMetadata(target);
				s_metadata[target] = value;
				for (int i = 0; i < s_providers.Length; i++)
				{
					s_providers[i].RestoreData(target);
				}
			}
			return value;
		}

		public static void Reset(fiUnityObjectReference target)
		{
			if (s_metadata.ContainsKey(target))
			{
				s_metadata.Remove(target);
				fiLateBindings.EditorApplication.InvokeOnEditorThread(delegate
				{
					for (int i = 0; i < s_providers.Length; i++)
					{
						s_providers[i].Reset(target);
					}
				});
			}
		}
	}
}
