using PathSystem;

namespace M__M.HaiWang.Fish
{
	public class RotationAdjuster
	{
		public int eventIndex;

		public float rotationOffset;

		public void Init(FishSubFormation subFormation)
		{
			subFormation.OnNewAgentCreated += delegate(NavPathAgent agent)
			{
				agent.AddEventListener(eventIndex, delegate(int index, NavPathAgent navPathAgent)
				{
					navPathAgent.rotationOffset = rotationOffset;
				});
			};
		}
	}
}
