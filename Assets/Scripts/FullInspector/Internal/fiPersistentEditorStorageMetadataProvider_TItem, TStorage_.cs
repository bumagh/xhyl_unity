using System;

namespace FullInspector.Internal
{
	public abstract class fiPersistentEditorStorageMetadataProvider<TItem, TStorage> : fiIPersistentMetadataProvider where TItem : new()where TStorage : fiIGraphMetadataStorage, new()
	{
		public Type MetadataType => typeof(TItem);

		public void RestoreData(fiUnityObjectReference target)
		{
			fiPersistentEditorStorage.Read<TStorage>(target).RestoreData(target);
		}

		public void Reset(fiUnityObjectReference target)
		{
			fiPersistentEditorStorage.Reset<TStorage>(target);
		}
	}
}
