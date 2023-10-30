using System;

namespace M__M.HaiWang.FishPathEditor.Core
{
	[Serializable]
	public class GeneratorBase<TResult> : IGenerator<TResult>, ICloneable, IGenerator
	{
		protected TResult _default;

		object IGenerator.GetCurrent()
		{
			return GetCurrent();
		}

		object IGenerator.GetNext()
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
			GeneratorBase<TResult> generatorBase = new GeneratorBase<TResult>();
			generatorBase._default = _default;
			return generatorBase;
		}
	}
}
