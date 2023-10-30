using FullInspector;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.StoryDefine
{
	[CreateAssetMenu]
	public class GameStoryTimelineData : BaseScriptableObject<FullSerializerSerializer>
	{
		public string description = string.Empty;

		public List<StoryItem> storys;
	}
}
