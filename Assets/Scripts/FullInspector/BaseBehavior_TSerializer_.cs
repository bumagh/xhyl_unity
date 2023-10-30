using FullInspector.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector
{
	public abstract class BaseBehavior<TSerializer> : CommonBaseBehavior, ISerializedObject, ISerializationCallbackReceiver where TSerializer : BaseSerializer
	{
		[HideInInspector]
		[SerializeField]
		private string _sharedStateGuid;

		[SerializeField]
		[NotSerialized]
		[HideInInspector]
		private List<UnityEngine.Object> _objectReferences;

		[SerializeField]
		[NotSerialized]
		[HideInInspector]
		private List<string> _serializedStateKeys;

		[SerializeField]
		[NotSerialized]
		[HideInInspector]
		private List<string> _serializedStateValues;

		string ISerializedObject.SharedStateGuid => _sharedStateGuid;

		List<UnityEngine.Object> ISerializedObject.SerializedObjectReferences
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

		public bool SharedStatePrefabInstantiation
		{
			get
			{
				return !string.IsNullOrEmpty(_sharedStateGuid);
			}
			set
			{
				if (Application.isPlaying && value)
				{
					_sharedStateGuid = Guid.NewGuid().ToString();
				}
				else
				{
					_sharedStateGuid = string.Empty;
				}
			}
		}

		static BaseBehavior()
		{
			BehaviorTypeToSerializerTypeMap.Register(typeof(BaseBehavior<TSerializer>), typeof(TSerializer));
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
			fiISerializedObjectUtility.SaveState<TSerializer>(this);
		}

		[ContextMenu("Restore Saved State")]
		public void RestoreState()
		{
			fiISerializedObjectUtility.RestoreState<TSerializer>(this);
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			((ISerializedObject)this).IsRestored = false;
			fiSerializationManager.OnUnityObjectDeserialize<TSerializer>(this);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			fiSerializationManager.OnUnityObjectSerialize<TSerializer>(this);
		}
	}
}
