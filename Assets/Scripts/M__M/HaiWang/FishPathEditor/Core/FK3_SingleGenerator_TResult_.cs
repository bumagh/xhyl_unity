namespace M__M.HaiWang.FishPathEditor.Core
{
	public class FK3_SingleGenerator<TResult> : FK3_GeneratorBase<TResult>
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
			FK3_SingleGenerator<TResult> fK3_SingleGenerator = new FK3_SingleGenerator<TResult>();
			fK3_SingleGenerator._default = _default;
			fK3_SingleGenerator.value = value;
			return fK3_SingleGenerator;
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
