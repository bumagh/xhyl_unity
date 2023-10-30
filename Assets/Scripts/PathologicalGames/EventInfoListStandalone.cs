using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/Utilities/EventInfoList (Standalone)")]
	public class EventInfoListStandalone : MonoBehaviour
	{
		public List<EventInfoListGUIBacker> _eventInfoListGUIBacker = new List<EventInfoListGUIBacker>();

		public Dictionary<object, bool> _inspectorListItemStates = new Dictionary<object, bool>();

		public EventInfoList eventInfoList
		{
			get
			{
				EventInfoList eventInfoList = new EventInfoList();
				foreach (EventInfoListGUIBacker item in _eventInfoListGUIBacker)
				{
					eventInfoList.Add(new EventInfo
					{
						name = item.name,
						value = item.value,
						duration = item.duration
					});
				}
				return eventInfoList;
			}
			set
			{
				_eventInfoListGUIBacker.Clear();
				foreach (EventInfo item2 in value)
				{
					EventInfoListGUIBacker item = new EventInfoListGUIBacker(item2);
					_eventInfoListGUIBacker.Add(item);
				}
			}
		}
	}
}
