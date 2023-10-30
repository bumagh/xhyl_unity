using PathSystem;
using System;
using System.Collections.Generic;

namespace M__M.HaiWang.Fish
{
	public class FishSubFormation : SubFormation<FishType>
	{
		public List<RotationAdjuster> rotationAdjusters;

		protected int _startID;

		public event Action<NavPathAgent> OnNewAgentCreated;

		public override void Init(Formation<FishType> formation)
		{
			base.Init(formation);
			if (rotationAdjusters != null)
			{
				foreach (RotationAdjuster rotationAdjuster in rotationAdjusters)
				{
					rotationAdjuster.Init(this);
				}
			}
		}

		public void SetStartID(int startID)
		{
			_startID = startID;
		}

		protected override AgentData<FishType> _getNextAgentData()
		{
			AgentData<FishType> next = _generator.GetNext(_startID++);
			if (this.OnNewAgentCreated != null)
			{
				this.OnNewAgentCreated(next.agent);
			}
			return next;
		}
	}
}
