using PathSystem;
using System;
using System.Collections.Generic;

namespace M__M.HaiWang.Fish
{
	public class FK3_FishSubFormation : FK3_SubFormation<FK3_FishType>
	{
		public List<FK3_RotationAdjuster> rotationAdjusters;

		protected int _startID;

		public event Action<FK3_NavPathAgent> OnNewAgentCreated;

		public override void Init(FK3_Formation<FK3_FishType> formation)
		{
			base.Init(formation);
			if (rotationAdjusters != null)
			{
				foreach (FK3_RotationAdjuster rotationAdjuster in rotationAdjusters)
				{
					rotationAdjuster.Init(this);
				}
			}
		}

		public void SetStartID(int startID)
		{
			_startID = startID;
		}

		protected override FK3_AgentData<FK3_FishType> _getNextAgentData()
		{
			FK3_AgentData<FK3_FishType> next = _generator.GetNext(_startID++);
			if (this.OnNewAgentCreated != null)
			{
				this.OnNewAgentCreated(next.agent);
			}
			return next;
		}
	}
}
