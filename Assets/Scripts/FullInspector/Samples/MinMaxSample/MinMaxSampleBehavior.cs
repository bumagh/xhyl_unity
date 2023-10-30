using UnityEngine;

namespace FullInspector.Samples.MinMaxSample
{
	[AddComponentMenu("Full Inspector Samples/Other/MinMax")]
	public class MinMaxSampleBehavior : BaseBehavior<FullSerializerSerializer>
	{
		public MinMax<float> FloatMinMax;

		public MinMax<int> IntMinMax;

		protected void Reset()
		{
			FloatMinMax.MinLimit = 0f;
			FloatMinMax.MaxLimit = 100f;
			FloatMinMax.ResetMin();
			IntMinMax.MinLimit = 33;
			IntMinMax.MaxLimit = 88;
			IntMinMax.ResetMin();
		}
	}
}
