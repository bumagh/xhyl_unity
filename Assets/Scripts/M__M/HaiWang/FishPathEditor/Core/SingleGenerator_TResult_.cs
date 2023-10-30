namespace M__M.HaiWang.FishPathEditor.Core
{
	public class SingleGenerator<TResult> : GeneratorBase<TResult>
	{
		public TResult value;

		public override TResult GetCurrent()
		{
			return value;
		}

		public override TResult GetNext()
		{
			return value;
		}

		public override object Clone()
		{
			SingleGenerator<TResult> singleGenerator = new SingleGenerator<TResult>();
			singleGenerator._default = _default;
			singleGenerator.value = value;
			return singleGenerator;
		}

		public override TResult[] GetEnums()
		{
			return new TResult[1]
			{
				value
			};
		}
	}
}
