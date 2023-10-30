using System;

namespace M__M.HaiWang.Fish
{
	public class FishGeneratorBase<T> : IFishGenerator<T>
	{
		public virtual T GetNext()
		{
			throw new NotImplementedException();
		}

		public virtual bool HasNext()
		{
			throw new NotImplementedException();
		}
	}
}
