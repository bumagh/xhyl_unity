namespace FullInspector.Samples.MinMaxSample
{
	public struct MinMax<TElement>
	{
		public TElement Min;

		public TElement Max;

		public TElement MinLimit;

		public TElement MaxLimit;

		public void ResetMin()
		{
			Min = MinLimit;
			Max = MinLimit;
		}
	}
}
