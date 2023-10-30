using FullInspector.Internal;
using System;

namespace FullInspector
{
	public interface fiIPersistentMetadataProvider
	{
		Type MetadataType
		{
			get;
		}

		void RestoreData(fiUnityObjectReference target);

		void Reset(fiUnityObjectReference target);
	}
}
