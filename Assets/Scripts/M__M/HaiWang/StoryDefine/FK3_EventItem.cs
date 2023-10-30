using System;
using System.Collections.Generic;

namespace M__M.HaiWang.StoryDefine
{
	[Serializable]
	public class FK3_EventItem
	{
		public int id;

		public float delay;

		public List<FK3_FishItem> fishList;

		public List<FK3_GroupItem> groupList;

		public List<FK3_FormationItem> formationList;
	}
}
