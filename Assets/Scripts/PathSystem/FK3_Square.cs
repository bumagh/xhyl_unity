namespace PathSystem
{
	public class FK3_Square : FK3_CartesianEquation
	{
		public float amplitude = 1f;

		public override float Evaluate(float x)
		{
			return amplitude * x * x;
		}
	}
}
