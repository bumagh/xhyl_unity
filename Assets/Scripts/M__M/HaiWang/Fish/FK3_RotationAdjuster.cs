using PathSystem;

namespace M__M.HaiWang.Fish
{
	public class FK3_RotationAdjuster
	{
		public int eventIndex;

		public float rotationOffset;

		public void Init(FK3_FishSubFormation subFormation)
		{
			subFormation.OnNewAgentCreated += delegate(FK3_NavPathAgent agent)
			{
				agent.AddEventListener(eventIndex, delegate(int index, FK3_NavPathAgent navPathAgent)
				{
					navPathAgent.rotationOffset = rotationOffset;
				});
			};
		}
	}
}
