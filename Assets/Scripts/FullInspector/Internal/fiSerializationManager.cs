using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FullInspector.Internal
{
	public static class fiSerializationManager
	{
		private class DeferredSerialization
		{
			private static readonly TimeSpan DELAY = TimeSpan.FromSeconds(0.5);

			private UnityEngine.Object _tracking;

			private DateTime _startTime;

			public void RequestSerialization(UnityEngine.Object tracking)
			{
				if (_tracking == tracking || (Application.isPlaying && fiLateBindings.PrefabUtility.IsPrefab(tracking)))
				{
					return;
				}
				_startTime = DateTime.Now;
				if (_tracking != tracking)
				{
					if (_tracking != null)
					{
						SerializeObject(typeof(DeferredSerialization), _tracking);
					}
					else
					{
						fiLateBindings.EditorApplication.AddUpdateFunc(Update);
					}
					_tracking = tracking;
				}
			}

			private void Update()
			{
				if ((!fiLateBindings.EditorApplication.isPlaying && fiLateBindings.EditorApplication.isCompilingOrChangingToPlayMode) || DateTime.Now - _startTime > DELAY)
				{
					SerializeObject(typeof(DeferredSerialization), _tracking);
					_tracking = null;
					fiLateBindings.EditorApplication.RemUpdateFunc(Update);
				}
			}
		}

		private static DeferredSerialization s_inspectedObjectSerialization;

		[NonSerialized]
		public static bool DisableAutomaticSerialization;

		private static readonly List<ISerializedObject> s_pendingDeserializations;

		private static readonly List<ISerializedObject> s_pendingSerializations;

		private static readonly Dictionary<ISerializedObject, fiSerializedObjectSnapshot> s_snapshots;

		public static HashSet<UnityEngine.Object> DirtyForceSerialize;

		private static ISerializedObject[] s_cachedSelection;

		private static HashSet<ISerializedObject> s_seen;

		[CompilerGenerated]
		private static Action _003C_003Ef__mg_0024cache0;

		static fiSerializationManager()
		{
			s_inspectedObjectSerialization = new DeferredSerialization();
			DisableAutomaticSerialization = false;
			s_pendingDeserializations = new List<ISerializedObject>();
			s_pendingSerializations = new List<ISerializedObject>();
			s_snapshots = new Dictionary<ISerializedObject, fiSerializedObjectSnapshot>();
			DirtyForceSerialize = new HashSet<UnityEngine.Object>();
			s_cachedSelection = new ISerializedObject[0];
			s_seen = new HashSet<ISerializedObject>();
			if (fiUtility.IsEditor)
			{
				fiLateBindings.EditorApplication.AddUpdateFunc(OnEditorUpdate);
			}
		}

		private static bool SupportsMultithreading<TSerializer>() where TSerializer : BaseSerializer
		{
			int result;
			if (!fiSettings.ForceDisableMultithreadedSerialization && !fiUtility.IsUnity4)
			{
				TSerializer val = fiSingletons.Get<TSerializer>();
				result = (val.SupportsMultithreading ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}

		public static void OnUnityObjectAwake(ISerializedObject obj)
		{
			if (!obj.IsRestored)
			{
				DoDeserialize(obj);
			}
		}

		public static void OnUnityObjectDeserialize<TSerializer>(ISerializedObject obj) where TSerializer : BaseSerializer
		{
			if (SupportsMultithreading<TSerializer>())
			{
				DoDeserialize(obj);
			}
			else if (fiUtility.IsEditor)
			{
				lock (s_pendingDeserializations)
				{
					s_pendingDeserializations.Add(obj);
				}
			}
		}

		public static void OnUnityObjectSerialize<TSerializer>(ISerializedObject obj) where TSerializer : BaseSerializer
		{
			if (SupportsMultithreading<TSerializer>())
			{
				DoSerialize(obj);
			}
			else if (fiUtility.IsEditor)
			{
				lock (s_pendingSerializations)
				{
					s_pendingSerializations.Add(obj);
				}
			}
		}

		private static void OnEditorUpdate()
		{
			s_cachedSelection = fiLateBindings.Selection.activeSelection.OfType<ISerializedObject>().ToArray();
			if (Application.isPlaying)
			{
				if (s_pendingDeserializations.Count > 0 || s_pendingSerializations.Count > 0 || s_snapshots.Count > 0)
				{
					s_pendingDeserializations.Clear();
					s_pendingSerializations.Clear();
					s_snapshots.Clear();
				}
				return;
			}
			for (int i = 0; i < s_cachedSelection.Length; i++)
			{
				if (!s_cachedSelection[i].IsRestored)
				{
					DoDeserialize(s_cachedSelection[i]);
				}
			}
			while (s_pendingDeserializations.Count > 0)
			{
				ISerializedObject serializedObject;
				lock (s_pendingDeserializations)
				{
					serializedObject = s_pendingDeserializations[s_pendingDeserializations.Count - 1];
					s_pendingDeserializations.RemoveAt(s_pendingDeserializations.Count - 1);
				}
				if (!(serializedObject is UnityEngine.Object) || !((UnityEngine.Object)serializedObject == null))
				{
					DoDeserialize(serializedObject);
				}
			}
			while (s_pendingSerializations.Count > 0)
			{
				ISerializedObject serializedObject2;
				lock (s_pendingSerializations)
				{
					serializedObject2 = s_pendingSerializations[s_pendingSerializations.Count - 1];
					s_pendingSerializations.RemoveAt(s_pendingSerializations.Count - 1);
				}
				if (!(serializedObject2 is UnityEngine.Object) || !((UnityEngine.Object)serializedObject2 == null))
				{
					DoSerialize(serializedObject2);
				}
			}
		}

		private static void DoDeserialize(ISerializedObject obj)
		{
			obj.RestoreState();
		}

		private static void DoSerialize(ISerializedObject obj)
		{
			if (DisableAutomaticSerialization)
			{
				return;
			}
			bool flag = obj is UnityEngine.Object && DirtyForceSerialize.Contains((UnityEngine.Object)obj);
			if (flag)
			{
				DirtyForceSerialize.Remove((UnityEngine.Object)obj);
			}
			if (obj is UnityEngine.Object && (UnityEngine.Object)obj == null)
			{
				return;
			}
			if (!flag && obj is UnityEngine.Object)
			{
				UnityEngine.Object @object = (UnityEngine.Object)obj;
				for (int i = 0; i < s_cachedSelection.Length; i++)
				{
					if (object.ReferenceEquals(@object, s_cachedSelection[i]))
					{
						s_inspectedObjectSerialization.RequestSerialization(@object);
						return;
					}
				}
			}
			HandleReset(obj);
			obj.SaveState();
		}

		private static void HandleReset(ISerializedObject obj)
		{
			if (!s_seen.Add(obj) && IsNullOrEmpty(obj.SerializedObjectReferences) && IsNullOrEmpty(obj.SerializedStateKeys) && IsNullOrEmpty(obj.SerializedStateValues))
			{
				fiLog.Log(typeof(fiSerializationManager), "Reseting object of type {0}", obj.GetType());
				obj.SaveState();
				for (int i = 0; i < obj.SerializedStateValues.Count; i++)
				{
					obj.SerializedStateValues[i] = null;
				}
				obj.RestoreState();
				fiRuntimeReflectionUtility.InvokeMethod(obj.GetType(), "Reset", obj, null);
				obj.SaveState();
			}
		}

		private static bool IsNullOrEmpty<T>(IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		public static void SerializeObject(Type logContext, object obj)
		{
			if (obj is GameObject)
			{
				ISerializedObject[] components = ((GameObject)obj).GetComponents<ISerializedObject>();
				foreach (ISerializedObject obj2 in components)
				{
					Serialize(logContext, obj2);
				}
			}
			else
			{
				Serialize(logContext, obj);
			}
		}

		private static void Serialize(Type logContext, object obj)
		{
			if (obj is ISerializedObject)
			{
				fiLog.Log(logContext, "Serializing object of type {0}", obj.GetType());
				((ISerializedObject)obj).SaveState();
			}
		}
	}
}
