using FullInspector.Internal;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace FullInspector
{
	public abstract class BaseNetworkBehavior : NetworkBehaviour, ISerializedObject, ISerializationCallbackReceiver
	{
		[SerializeField]
		[NotSerialized]
		[HideInInspector]
		private List<Object> _objectReferences;

		[SerializeField]
		[NotSerialized]
		[HideInInspector]
		private List<string> _serializedStateKeys;

		[SerializeField]
		[NotSerialized]
		[HideInInspector]
		private List<string> _serializedStateValues;

		string ISerializedObject.SharedStateGuid => string.Empty;

		List<Object> ISerializedObject.SerializedObjectReferences
		{
			get
			{
				return _objectReferences;
			}
			set
			{
				_objectReferences = value;
			}
		}

		List<string> ISerializedObject.SerializedStateKeys
		{
			get
			{
				return _serializedStateKeys;
			}
			set
			{
				_serializedStateKeys = value;
			}
		}

		List<string> ISerializedObject.SerializedStateValues
		{
			get
			{
				return _serializedStateValues;
			}
			set
			{
				_serializedStateValues = value;
			}
		}

		bool ISerializedObject.IsRestored
		{
			get;
			set;
		}

		static BaseNetworkBehavior()
		{
			BehaviorTypeToSerializerTypeMap.Register(typeof(BaseBehavior<FullSerializerSerializer>), typeof(FullSerializerSerializer));
		}

		protected virtual void Awake()
		{
			fiSerializationManager.OnUnityObjectAwake(this);
		}

		protected virtual void OnValidate()
		{
			if (!Application.isPlaying && !((ISerializedObject)this).IsRestored)
			{
				RestoreState();
			}
		}

		[ContextMenu("Save Current State")]
		public void SaveState()
		{
			fiISerializedObjectUtility.SaveState<FullSerializerSerializer>(this);
		}

		[ContextMenu("Restore Saved State")]
		public void RestoreState()
		{
			fiISerializedObjectUtility.RestoreState<FullSerializerSerializer>(this);
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			((ISerializedObject)this).IsRestored = false;
			fiSerializationManager.OnUnityObjectDeserialize<FullSerializerSerializer>(this);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			fiSerializationManager.OnUnityObjectSerialize<FullSerializerSerializer>(this);
		}

		private void UNetVersion()
		{
		}

		public override bool OnSerialize(NetworkWriter writer, bool forceAll)
		{
			bool result = default(bool);
			return result;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
		}
	}
}
