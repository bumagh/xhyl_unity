using System;
using System.Collections.Generic;

namespace M__M.HaiWang.StoryDefine
{
	[Serializable]
	public class FK3_StoryItem
	{
		public int id;

		public float duration;

		public List<FK3_EventItem> events;
	}
}
