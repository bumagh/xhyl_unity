using UnityEngine;

namespace PathologicalGames
{
	public struct MessageData_TargetableOnHit
	{
		public EventInfoList eventInfoList;

		public Target target;

		public Collider collider;

		public MessageData_TargetableOnHit(EventInfoList eventInfoList, Target target)
		{
			this.eventInfoList = eventInfoList;
			this.target = target;
			collider = null;
		}

		public MessageData_TargetableOnHit(EventInfoList eventInfoList, Target target, Collider other)
		{
			this.eventInfoList = eventInfoList;
			this.target = target;
			collider = other;
		}
	}
}
