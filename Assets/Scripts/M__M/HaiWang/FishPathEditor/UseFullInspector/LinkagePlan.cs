using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class LinkagePlan
	{
		public class Mode
		{
		}

		public class OffsetMode : Mode
		{
			public Vector3 offset = Vector3.zero;
		}

		public class RotationMode : Mode
		{
			public Vector3 center = Vector3.zero;

			public float r;

			public float angle;
		}
	}
}
