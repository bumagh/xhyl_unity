using System;
using System.Collections.Generic;

namespace M__M.HaiWang.StoryDefine
{
	[Serializable]
	public class StoryItem
	{
		public int id;

		public float duration;

		public List<EventItem> events;
	}
}
