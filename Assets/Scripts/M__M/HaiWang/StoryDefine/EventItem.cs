using System;
using System.Collections.Generic;

namespace M__M.HaiWang.StoryDefine
{
	[Serializable]
	public class EventItem
	{
		public int id;

		public float delay;

		public List<FishItem> fishList;

		public List<GroupItem> groupList;

		public List<FormationItem> formationList;
	}
}
