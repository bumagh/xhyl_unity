using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class FollowData
	{
		public Vector3 position;

		public Transform transform;

		public FollowData(Vector3 pos)
		{
			position = pos;
		}

		public FollowData(Transform trans)
		{
			transform = trans;
			position = trans.position;
		}
	}
}
