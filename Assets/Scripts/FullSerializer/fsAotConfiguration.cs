using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullSerializer
{
	[CreateAssetMenu(menuName = "Full Serializer AOT Configuration")]
	public class fsAotConfiguration : ScriptableObject
	{
		public enum AotState
		{
			Default,
			Enabled,
			Disabled
		}

		[Serializable]
		public struct Entry
		{
			public AotState State;

			public string FullTypeName;

			public Entry(Type type)
			{
				FullTypeName = type.FullName;
				State = AotState.Default;
			}

			public Entry(Type type, AotState state)
			{
				FullTypeName = type.FullName;
				State = state;
			}
		}

		public List<Entry> aotTypes = new List<Entry>();

		public string outputDirectory = "Assets/AotModels";

		public bool TryFindEntry(Type type, out Entry result)
		{
			string fullName = type.FullName;
			foreach (Entry aotType in aotTypes)
			{
				Entry current = aotType;
				if (current.FullTypeName == fullName)
				{
					result = current;
					return true;
				}
			}
			result = default(Entry);
			return false;
		}

		public void UpdateOrAddEntry(Entry entry)
		{
			for (int i = 0; i < aotTypes.Count; i++)
			{
				Entry entry2 = aotTypes[i];
				if (entry2.FullTypeName == entry.FullTypeName)
				{
					aotTypes[i] = entry;
					return;
				}
			}
			aotTypes.Add(entry);
		}
	}
}
