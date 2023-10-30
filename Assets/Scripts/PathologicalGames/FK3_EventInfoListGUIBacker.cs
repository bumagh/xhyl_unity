using System;

namespace PathologicalGames
{
	[Serializable]
	public class FK3_EventInfoListGUIBacker
	{
		public string name = "<Event Name>";

		public float value;

		public float duration;

		public FK3_EventInfoListGUIBacker()
		{
		}

		public FK3_EventInfoListGUIBacker(FK3_EventInfo info)
		{
			name = info.name;
			value = info.value;
			duration = info.duration;
		}

		public FK3_EventInfo GetEventInfo()
		{
			FK3_EventInfo result = default(FK3_EventInfo);
			result.name = name;
			result.value = value;
			result.duration = duration;
			return result;
		}
	}
}
