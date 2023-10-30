using FullSerializer;
using System;
using UnityEngine;

namespace FullInspector.Serializers.FullSerializer
{
	public class FullSerializerMetadata : fiISerializerMetadata
	{
		public Guid SerializerGuid => new Guid("bc898177-6ff4-423f-91bb-589bc83d8fde");

		public Type SerializerType => typeof(FullSerializerSerializer);

		public Type[] SerializationOptInAnnotationTypes => new Type[2]
		{
			typeof(SerializeField),
			typeof(fsPropertyAttribute)
		};

		public Type[] SerializationOptOutAnnotationTypes => new Type[1]
		{
			typeof(fsIgnoreAttribute)
		};
	}
}
