using System;

namespace M__M.HaiWang.Fish
{
	public class FK3_FishGeneratorBase<T> : FK3_IFishGenerator<T>
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
