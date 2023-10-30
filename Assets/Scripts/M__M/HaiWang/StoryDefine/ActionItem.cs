using FullInspector;
using System;

namespace M__M.HaiWang.StoryDefine
{
	[Serializable]
	public class ActionItem
	{
		[InspectorRange(0f, 1f, float.NaN)]
		public float chance = 1f;
	}
}
