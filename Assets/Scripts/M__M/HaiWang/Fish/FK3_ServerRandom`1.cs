using PathSystem;

namespace M__M.HaiWang.Fish
{
	public class FK3_ServerRandom<T> : FK3_GeneratorBase<T>
	{
		public int randID;

		public override void Init(FK3_SubFormation<T> subFormation)
		{
			base.Init(subFormation);
		}

		public override FK3_AgentData<T> GetNext(object userData)
		{
			FK3_FishFormation fK3_FishFormation = _subFormation.formation as FK3_FishFormation;
			FK3_FishType randomType = fK3_FishFormation.GetRandomType(randID);
			return fK3_FishFormation.AddObject(randomType, userData) as FK3_AgentData<T>;
		}
	}
}
