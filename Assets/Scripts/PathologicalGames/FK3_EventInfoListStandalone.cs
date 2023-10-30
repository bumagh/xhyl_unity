using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/TriggerEventPRO/Utilities/FK3_EventInfoList (Standalone)")]
	public class FK3_EventInfoListStandalone : MonoBehaviour
	{
		public List<FK3_EventInfoListGUIBacker> _eventInfoListGUIBacker = new List<FK3_EventInfoListGUIBacker>();

		public Dictionary<object, bool> _inspectorListItemStates = new Dictionary<object, bool>();

		public FK3_EventInfoList eventInfoList
		{
			get
			{
				FK3_EventInfoList fK3_EventInfoList = new FK3_EventInfoList();
				foreach (FK3_EventInfoListGUIBacker item in _eventInfoListGUIBacker)
				{
					fK3_EventInfoList.Add(new FK3_EventInfo
					{
						name = item.name,
						value = item.value,
						duration = item.duration
					});
				}
				return fK3_EventInfoList;
			}
			set
			{
				_eventInfoListGUIBacker.Clear();
				foreach (FK3_EventInfo item2 in value)
				{
					FK3_EventInfoListGUIBacker item = new FK3_EventInfoListGUIBacker(item2);
					_eventInfoListGUIBacker.Add(item);
				}
			}
		}
	}
}
