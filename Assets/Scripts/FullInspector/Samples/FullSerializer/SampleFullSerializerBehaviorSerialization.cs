using System;
using UnityEngine;

namespace FullInspector.Samples.FullSerializer
{
	[AddComponentMenu("Full Inspector Samples/FullSerializer/Behavior Serialization")]
	public class SampleFullSerializerBehaviorSerialization : BaseBehavior<FullSerializerSerializer>
	{
		public struct NotSerializableByUnity
		{
			public int Value;
		}

		public NotSerializableByUnity Serialized0;

		[SerializeField]
		public NotSerializableByUnity Serialized1;

		[ShowInInspector]
		[SerializeField]
		protected internal NotSerializableByUnity Serialized2;

		[ShowInInspector]
		[SerializeField]
		internal NotSerializableByUnity Serialized3;

		[ShowInInspector]
		[SerializeField]
		protected NotSerializableByUnity Serialized4;

		[SerializeField]
		[ShowInInspector]
		private NotSerializableByUnity Serialized5;

		[NonSerialized]
		public NotSerializableByUnity NotSerialized0;

		[ShowInInspector]
		protected internal NotSerializableByUnity NotSerialized1;

		[ShowInInspector]
		internal NotSerializableByUnity NotSerialized2;

		[ShowInInspector]
		protected NotSerializableByUnity NotSerialized3;

		[ShowInInspector]
		private NotSerializableByUnity NotSerialized4;

		public NotSerializableByUnity Serialized6
		{
			get;
			set;
		}

		[SerializeField]
		public NotSerializableByUnity Serialized7
		{
			get;
			set;
		}

		[NotSerialized]
		public NotSerializableByUnity NotSerialized5
		{
			get;
			set;
		}

		[ShowInInspector]
		private NotSerializableByUnity NotSerialized6
		{
			get;
			set;
		}
	}
}
