using FullInspector.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector
{
	public abstract class BaseObject : fiValueProxyEditor, fiIValueProxyAPI, ISerializedObject, ISerializationCallbackReceiver
	{
		[HideInInspector]
		[NotSerialized]
		[SerializeField]
		private List<Object> _objectReferences;

		[HideInInspector]
		[NotSerialized]
		[SerializeField]
		private List<string> _serializedStateKeys;

		[NotSerialized]
		[SerializeField]
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

		object fiIValueProxyAPI.Value
		{
			get
			{
				return this;
			}
			set
			{
			}
		}

		void ISerializedObject.RestoreState()
		{
			fiISerializedObjectUtility.RestoreState<FullSerializerSerializer>(this);
		}

		void ISerializedObject.SaveState()
		{
			fiISerializedObjectUtility.SaveState<FullSerializerSerializer>(this);
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			fiISerializedObjectUtility.RestoreState<FullSerializerSerializer>(this);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			fiISerializedObjectUtility.SaveState<FullSerializerSerializer>(this);
		}

		void fiIValueProxyAPI.SaveState()
		{
		}

		void fiIValueProxyAPI.LoadState()
		{
		}
	}
}
