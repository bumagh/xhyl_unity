namespace PathologicalGames
{
	public struct FK3_MessageData_TargetTrackerEvent
	{
		public FK3_TargetTracker targetTracker;

		public FK3_Target target;

		public FK3_TargetList targets;

		public FK3_MessageData_TargetTrackerEvent(FK3_TargetTracker targetTracker, FK3_Target target)
		{
			this.targetTracker = targetTracker;
			this.target = target;
			targets = new FK3_TargetList();
		}

		public FK3_MessageData_TargetTrackerEvent(FK3_TargetTracker targetTracker, FK3_TargetList targets)
		{
			this.targetTracker = targetTracker;
			target = FK3_Target.Null;
			this.targets = targets;
		}
	}
}
