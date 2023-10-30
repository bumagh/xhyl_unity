using UnityEngine;

namespace PathologicalGames
{
	public struct EventInfo
	{
		public string name;

		public float value;

		public float duration;

		public float hitTime;

		public float deltaDurationTime => Mathf.Max(hitTime + duration - Time.time, 0f);

		public EventInfo(EventInfo eventInfo)
		{
			name = eventInfo.name;
			value = eventInfo.value;
			duration = eventInfo.duration;
			hitTime = eventInfo.hitTime;
		}

		public override string ToString()
		{
			return $"(name '{name}', value {value}, duration {duration}, hitTime {hitTime}, deltaDurationTime {deltaDurationTime})";
		}
	}
}
