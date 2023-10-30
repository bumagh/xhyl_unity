using PathSystem;

namespace M__M.HaiWang.Fish
{
	public class ServerRandom<T> : GeneratorBase<T>
	{
		public int randID;

		public override void Init(SubFormation<T> subFormation)
		{
			base.Init(subFormation);
		}

		public override AgentData<T> GetNext(object userData)
		{
			FishFormation fishFormation = _subFormation.formation as FishFormation;
			FishType randomType = fishFormation.GetRandomType(randID);
			return fishFormation.AddObject(randomType, userData) as AgentData<T>;
		}
	}
}
