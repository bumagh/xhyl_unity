using System;

namespace PathologicalGames
{
	[Serializable]
	public class EventInfoListGUIBacker
	{
		public string name = "<Event Name>";

		public float value;

		public float duration;

		public EventInfoListGUIBacker()
		{
		}

		public EventInfoListGUIBacker(EventInfo info)
		{
			name = info.name;
			value = info.value;
			duration = info.duration;
		}

		public EventInfo GetEventInfo()
		{
			EventInfo result = default(EventInfo);
			result.name = name;
			result.value = value;
			result.duration = duration;
			return result;
		}
	}
}
