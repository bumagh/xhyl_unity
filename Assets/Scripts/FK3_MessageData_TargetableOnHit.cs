using UnityEngine;

namespace PathologicalGames
{
	public struct FK3_MessageData_TargetableOnHit
	{
		public FK3_EventInfoList eventInfoList;

		public FK3_Target target;

		public Collider collider;

		public FK3_MessageData_TargetableOnHit(FK3_EventInfoList eventInfoList, FK3_Target target)
		{
			this.eventInfoList = eventInfoList;
			this.target = target;
			collider = null;
		}

		public FK3_MessageData_TargetableOnHit(FK3_EventInfoList eventInfoList, FK3_Target target, Collider other)
		{
			this.eventInfoList = eventInfoList;
			this.target = target;
			collider = other;
		}
	}
}
