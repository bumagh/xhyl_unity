using FullInspector;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.StoryDefine
{
	[CreateAssetMenu]
	public class FK3_GameStoryTimelineData : BaseScriptableObject<FullSerializerSerializer>
	{
		public string description = string.Empty;

		public List<FK3_StoryItem> storys;
	}
}
