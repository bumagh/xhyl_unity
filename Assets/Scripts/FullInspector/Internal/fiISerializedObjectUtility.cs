using FullSerializer;
using FullSerializer.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.Internal
{
	public static class fiISerializedObjectUtility
	{
		private static Dictionary<string, ISerializedObject> _skipSerializationQueue = new Dictionary<string, ISerializedObject>();

		private static void SkipCloningValues(ISerializedObject obj)
		{
			lock (_skipSerializationQueue)
			{
				if (!_skipSerializationQueue.ContainsKey(obj.SharedStateGuid))
				{
					_skipSerializationQueue[obj.SharedStateGuid] = obj;
				}
			}
		}

		private static bool TryToCopyValues(ISerializedObject newInstance)
		{
			if (string.IsNullOrEmpty(newInstance.SharedStateGuid))
			{
				return false;
			}
			ISerializedObject value = null;
			lock (_skipSerializationQueue)
			{
				if (!_skipSerializationQueue.TryGetValue(newInstance.SharedStateGuid, out value))
				{
					return false;
				}
				_skipSerializationQueue.Remove(newInstance.SharedStateGuid);
			}
			if (object.ReferenceEquals(newInstance, value))
			{
				return true;
			}
			InspectedType inspectedType = InspectedType.Get(value.GetType());
			for (int i = 0; i < value.SerializedStateKeys.Count; i++)
			{
				InspectedProperty inspectedProperty = inspectedType.GetPropertyByName(value.SerializedStateKeys[i]) ?? inspectedType.GetPropertyByFormerlySerializedName(value.SerializedStateKeys[i]);
				inspectedProperty.Write(newInstance, inspectedProperty.Read(value));
			}
			return true;
		}

		private static bool SaveStateForProperty(ISerializedObject obj, InspectedProperty property, BaseSerializer serializer, ISerializationOperator serializationOperator, out string serializedValue, ref bool success)
		{
			object obj2 = property.Read(obj);
			try
			{
				if (obj2 == null)
				{
					serializedValue = null;
				}
				else
				{
					serializedValue = serializer.Serialize(property.MemberInfo, obj2, serializationOperator);
				}
				return true;
			}
			catch (Exception ex)
			{
				success = false;
				serializedValue = null;
				UnityEngine.Debug.LogError($"{obj} 中 {property.Name} 的 {obj2} 序列化属性时捕获异常 {ex}");
				return false;
			}
		}

		public static bool SaveState<TSerializer>(ISerializedObject obj) where TSerializer : BaseSerializer
		{
			fiLog.Log(typeof(fiISerializedObjectUtility), "Serializing object of type {0}", obj.GetType());
			bool success = true;
			ISerializationCallbacks serializationCallbacks = obj as ISerializationCallbacks;
			serializationCallbacks?.OnBeforeSerialize();
			if (!string.IsNullOrEmpty(obj.SharedStateGuid))
			{
				SkipCloningValues(obj);
				serializationCallbacks?.OnAfterSerialize();
				return true;
			}
			TSerializer serializer = fiSingletons.Get<TSerializer>();
			ListSerializationOperator listSerializationOperator = fiSingletons.Get<ListSerializationOperator>();
			listSerializationOperator.SerializedObjects = new List<UnityEngine.Object>();
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			if (fiUtility.IsEditor || obj.SerializedStateKeys == null || obj.SerializedStateKeys.Count == 0)
			{
				List<InspectedProperty> properties = InspectedType.Get(obj.GetType()).GetProperties(InspectedMemberFilters.FullInspectorSerializedProperties);
				for (int i = 0; i < properties.Count; i++)
				{
					InspectedProperty inspectedProperty = properties[i];
					if (SaveStateForProperty(obj, inspectedProperty, serializer, listSerializationOperator, out string serializedValue, ref success))
					{
						list.Add(inspectedProperty.Name);
						list2.Add(serializedValue);
					}
				}
			}
			else
			{
				InspectedType inspectedType = InspectedType.Get(obj.GetType());
				for (int j = 0; j < obj.SerializedStateKeys.Count; j++)
				{
					InspectedProperty inspectedProperty2 = inspectedType.GetPropertyByName(obj.SerializedStateKeys[j]) ?? inspectedType.GetPropertyByFormerlySerializedName(obj.SerializedStateKeys[j]);
					if (inspectedProperty2 != null && SaveStateForProperty(obj, inspectedProperty2, serializer, listSerializationOperator, out string serializedValue2, ref success))
					{
						list.Add(inspectedProperty2.Name);
						list2.Add(serializedValue2);
					}
				}
			}
			bool flag = false;
			if (AreListsDifferent(obj.SerializedStateKeys, list))
			{
				obj.SerializedStateKeys = list;
				flag = true;
			}
			if (AreListsDifferent(obj.SerializedStateValues, list2))
			{
				obj.SerializedStateValues = list2;
				flag = true;
			}
			if (AreListsDifferent(obj.SerializedObjectReferences, listSerializationOperator.SerializedObjects))
			{
				obj.SerializedObjectReferences = listSerializationOperator.SerializedObjects;
				flag = true;
			}
			if (flag && fiUtility.IsEditor)
			{
				fiLateBindings.EditorApplication.InvokeOnEditorThread(delegate
				{
					UnityEngine.Object @object = (UnityEngine.Object)obj;
					if (@object != null)
					{
						fiLateBindings.EditorUtility.SetDirty(@object);
					}
				});
			}
			serializationCallbacks?.OnAfterSerialize();
			return success;
		}

		private static bool AreListsDifferent(IList<string> a, IList<string> b)
		{
			if (a == null)
			{
				return true;
			}
			if (a.Count != b.Count)
			{
				return true;
			}
			int count = a.Count;
			for (int i = 0; i < count; i++)
			{
				if (a[i] != b[i])
				{
					return true;
				}
			}
			return false;
		}

		private static bool AreListsDifferent(IList<UnityEngine.Object> a, IList<UnityEngine.Object> b)
		{
			if (a == null)
			{
				return true;
			}
			if (a.Count != b.Count)
			{
				return true;
			}
			int count = a.Count;
			for (int i = 0; i < count; i++)
			{
				if (!object.ReferenceEquals(a[i], b[i]))
				{
					return true;
				}
			}
			return false;
		}

		public static bool RestoreState<TSerializer>(ISerializedObject obj) where TSerializer : BaseSerializer
		{
			fiLog.Log(typeof(fiISerializedObjectUtility), "Deserializing object of type {0}", obj.GetType());
			ISerializationCallbacks serializationCallbacks = obj as ISerializationCallbacks;
			try
			{
				serializationCallbacks?.OnBeforeDeserialize();
				if (!string.IsNullOrEmpty(obj.SharedStateGuid))
				{
					if (obj.IsRestored)
					{
						return true;
					}
					if (TryToCopyValues(obj))
					{
						fiLog.Log(typeof(fiISerializedObjectUtility), "-- note: Used fast path when deserializing object of type {0}", obj.GetType());
						obj.IsRestored = true;
						serializationCallbacks?.OnAfterDeserialize();
						return true;
					}
					UnityEngine.Debug.LogError("Shared state deserialization failed for object of type " + obj.GetType().CSharpName(), obj as UnityEngine.Object);
				}
				if (obj.SerializedStateKeys == null)
				{
					obj.SerializedStateKeys = new List<string>();
				}
				if (obj.SerializedStateValues == null)
				{
					obj.SerializedStateValues = new List<string>();
				}
				if (obj.SerializedObjectReferences == null)
				{
					obj.SerializedObjectReferences = new List<UnityEngine.Object>();
				}
				if (obj.SerializedStateKeys.Count != obj.SerializedStateValues.Count && fiSettings.EmitWarnings)
				{
					UnityEngine.Debug.LogWarning("Serialized key count does not equal value count; possible data corruption / bad manual edit?", obj as UnityEngine.Object);
				}
				if (obj.SerializedStateKeys.Count == 0)
				{
					if (fiSettings.AutomaticReferenceInstantation)
					{
						InstantiateReferences(obj, null);
					}
					obj.IsRestored = true;
					return true;
				}
				TSerializer val = fiSingletons.Get<TSerializer>();
				ListSerializationOperator listSerializationOperator = fiSingletons.Get<ListSerializationOperator>();
				listSerializationOperator.SerializedObjects = obj.SerializedObjectReferences;
				InspectedType inspectedType = InspectedType.Get(obj.GetType());
				bool result = true;
				for (int i = 0; i < obj.SerializedStateKeys.Count; i++)
				{
					string text = obj.SerializedStateKeys[i];
					string text2 = obj.SerializedStateValues[i];
					InspectedProperty inspectedProperty = inspectedType.GetPropertyByName(text) ?? inspectedType.GetPropertyByFormerlySerializedName(text);
					if (inspectedProperty == null)
					{
						if (fiSettings.EmitWarnings)
						{
							UnityEngine.Debug.LogWarning("Unable to find serialized property with name=" + text + " on type " + obj.GetType(), obj as UnityEngine.Object);
						}
					}
					else
					{
						object value = null;
						if (!string.IsNullOrEmpty(text2))
						{
							try
							{
								value = val.Deserialize(inspectedProperty.MemberInfo, text2, listSerializationOperator);
							}
							catch (Exception ex)
							{
								result = false;
								UnityEngine.Debug.LogError("Exception caught when deserializing property <" + text + "> with type <" + obj.GetType() + ">\n" + ex, obj as UnityEngine.Object);
							}
						}
						try
						{
							inspectedProperty.Write(obj, value);
						}
						catch (Exception message)
						{
							result = false;
							if (fiSettings.EmitWarnings)
							{
								UnityEngine.Debug.LogWarning("Caught exception when updating property value; see next message for the exception", obj as UnityEngine.Object);
								UnityEngine.Debug.LogError(message);
							}
						}
					}
				}
				obj.IsRestored = true;
				return result;
			}
			finally
			{
				serializationCallbacks?.OnAfterDeserialize();
			}
		}

		private static void InstantiateReferences(object obj, InspectedType metadata)
		{
			if (metadata == null)
			{
				metadata = InspectedType.Get(obj.GetType());
			}
			if (metadata.IsCollection)
			{
				return;
			}
			List<InspectedProperty> properties = metadata.GetProperties(InspectedMemberFilters.InspectableMembers);
			for (int i = 0; i < properties.Count; i++)
			{
				InspectedProperty inspectedProperty = properties[i];
				if (!inspectedProperty.StorageType.Resolve().IsClass || inspectedProperty.StorageType.Resolve().IsAbstract)
				{
					continue;
				}
				object obj2 = inspectedProperty.Read(obj);
				if (obj2 == null)
				{
					InspectedType inspectedType = InspectedType.Get(inspectedProperty.StorageType);
					if (inspectedType.HasDefaultConstructor)
					{
						object obj3 = inspectedType.CreateInstance();
						inspectedProperty.Write(obj, obj3);
						InstantiateReferences(obj3, inspectedType);
					}
				}
			}
		}
	}
}
