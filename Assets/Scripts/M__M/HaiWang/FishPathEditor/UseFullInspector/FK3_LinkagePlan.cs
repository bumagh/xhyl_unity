using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_LinkagePlan
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
