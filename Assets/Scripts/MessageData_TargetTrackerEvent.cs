namespace PathologicalGames
{
	public struct MessageData_TargetTrackerEvent
	{
		public TargetTracker targetTracker;

		public Target target;

		public TargetList targets;

		public MessageData_TargetTrackerEvent(TargetTracker targetTracker, Target target)
		{
			this.targetTracker = targetTracker;
			this.target = target;
			targets = new TargetList();
		}

		public MessageData_TargetTrackerEvent(TargetTracker targetTracker, TargetList targets)
		{
			this.targetTracker = targetTracker;
			target = Target.Null;
			this.targets = targets;
		}
	}
}
