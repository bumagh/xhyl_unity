using System;

namespace M__M.HaiWang.FishPathEditor.Core
{
	[Serializable]
	public class FK3_GeneratorBase<TResult> : FK3_IGenerator<TResult>, ICloneable, FK3_IGenerator
	{
		protected TResult _default;

		object FK3_IGenerator.GetCurrent()
		{
			return GetCurrent();
		}

		object FK3_IGenerator.GetNext()
		{
			return GetNext();
		}

		public virtual TResult GetCurrent()
		{
			return _default;
		}

		public virtual TResult GetNext()
		{
			return _default;
		}

		public virtual void Reset()
		{
		}

		public virtual bool CheckValid()
		{
			return true;
		}

		public virtual TResult[] GetEnums()
		{
			return new TResult[0];
		}

		public virtual object Clone()
		{
			FK3_GeneratorBase<TResult> fK3_GeneratorBase = new FK3_GeneratorBase<TResult>();
			fK3_GeneratorBase._default = _default;
			return fK3_GeneratorBase;
		}
	}
}
