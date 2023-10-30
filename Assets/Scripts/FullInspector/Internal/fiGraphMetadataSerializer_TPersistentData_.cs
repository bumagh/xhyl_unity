using UnityEngine;

namespace FullInspector.Internal
{
	public class fiGraphMetadataSerializer<TPersistentData> : fiIGraphMetadataStorage, ISerializationCallbackReceiver where TPersistentData : IGraphMetadataItemPersistent
	{
		[SerializeField]
		private string[] _keys;

		[SerializeField]
		private TPersistentData[] _values;

		[SerializeField]
		private Object _target;

		public void RestoreData(fiUnityObjectReference target)
		{
			_target = target.Target;
			if (_keys != null && _values != null)
			{
				fiPersistentMetadata.GetMetadataFor(target).Deserialize(_keys, _values);
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (!object.ReferenceEquals(_target, null))
			{
				try
				{
					fiUnityObjectReference target = new fiUnityObjectReference(_target, tryRestore: false);
					if (fiPersistentMetadata.HasMetadata(target))
					{
						fiGraphMetadata metadataFor = fiPersistentMetadata.GetMetadataFor(target);
						if (metadataFor.ShouldSerialize())
						{
							metadataFor.Serialize(out _keys, out _values);
						}
					}
				}
				catch (MissingReferenceException arg)
				{
					fiLog.Log(typeof(fiGraphMetadataSerializer<TPersistentData>), "Caught exception {0}", arg);
				}
			}
		}
	}
}
