using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/UnityConstraints/Constraint- World To 2D Camera")]
	public class WorldTo2DCameraConstraint : TransformConstraint
	{
		public enum OFFSET_MODE
		{
			WorldSpace,
			ViewportSpace
		}

		public enum OFFSCREEN_MODE
		{
			Constrain,
			Limit,
			DoNothing
		}

		public bool vertical = true;

		public Camera targetCamera;

		public Camera orthoCamera;

		public Vector3 offset;

		public OFFSET_MODE offsetMode;

		public OFFSCREEN_MODE offScreenMode;

		public Vector2 offscreenThreasholdW = new Vector2(0f, 1f);

		public Vector2 offscreenThreasholdH = new Vector2(0f, 1f);

		protected override void Awake()
		{
			base.Awake();
			Camera[] array = Object.FindObjectsOfType(typeof(Camera)) as Camera[];
			Camera[] array2 = array;
			foreach (Camera camera in array2)
			{
				if (camera.orthographic && (camera.cullingMask & (1 << base.gameObject.layer)) > 0)
				{
					orthoCamera = camera;
					break;
				}
			}
			if (!(base.target != null))
			{
				return;
			}
			Camera[] array3 = array;
			int num = 0;
			Camera camera2;
			while (true)
			{
				if (num < array3.Length)
				{
					camera2 = array3[num];
					if ((camera2.cullingMask & (1 << base.target.gameObject.layer)) > 0)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			targetCamera = camera2;
		}

		protected override void OnConstraintUpdate()
		{
			bool constrainPosition = base.constrainPosition;
			base.constrainPosition = false;
			base.OnConstraintUpdate();
			base.constrainPosition = constrainPosition;
			if (!base.constrainPosition)
			{
				return;
			}
			base.pos = base.target.position;
			if (offsetMode == OFFSET_MODE.WorldSpace)
			{
				base.pos.x = base.pos.x + offset.x;
				base.pos.y = base.pos.y + offset.y;
			}
			base.pos = targetCamera.WorldToViewportPoint(base.pos);
			if (offsetMode == OFFSET_MODE.ViewportSpace)
			{
				base.pos.x = base.pos.x + offset.x;
				base.pos.y = base.pos.y + offset.y;
			}
			switch (offScreenMode)
			{
			case OFFSCREEN_MODE.Limit:
				base.pos.x = Mathf.Clamp(base.pos.x, offscreenThreasholdW.x, offscreenThreasholdW.y);
				base.pos.y = Mathf.Clamp(base.pos.y, offscreenThreasholdH.x, offscreenThreasholdH.y);
				break;
			case OFFSCREEN_MODE.DoNothing:
				if (base.pos.z <= 0f || base.pos.x <= offscreenThreasholdW.x || base.pos.x >= offscreenThreasholdW.y || base.pos.y <= offscreenThreasholdH.x || base.pos.y >= offscreenThreasholdH.y)
				{
					return;
				}
				break;
			}
			base.pos = orthoCamera.ViewportToWorldPoint(base.pos);
			base.pos.z = offset.z;
			if (!outputPosX)
			{
				ref Vector3 pos = ref base.pos;
				Vector3 position = base.position;
				pos.x = position.x;
			}
			if (!outputPosY)
			{
				ref Vector3 pos2 = ref base.pos;
				Vector3 position2 = base.position;
				pos2.y = position2.y;
			}
			if (!outputPosZ)
			{
				ref Vector3 pos3 = ref base.pos;
				Vector3 position3 = base.position;
				pos3.z = position3.z;
			}
			base.transform.position = base.pos;
		}
	}
}
