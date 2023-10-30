namespace FullInspector.Samples.Button
{
	public class ButtonContainerType
	{
		[InspectorOrder(1.0)]
		public int Member1;

		[InspectorOrder(3.0)]
		public int Member2;

		[InspectorButton]
		[InspectorOrder(2.0)]
		[InspectorName("A custom name")]
		public void Method(int val)
		{
		}
	}
}
