using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class FK3_FollowData
	{
		public Vector3 position;

		public Transform transform;

		public FK3_FollowData(Vector3 pos)
		{
			position = pos;
		}

		public FK3_FollowData(Transform trans)
		{
			transform = trans;
			position = trans.position;
		}
	}
}
