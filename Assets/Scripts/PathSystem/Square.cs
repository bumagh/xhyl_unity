namespace PathSystem
{
	public class Square : CartesianEquation
	{
		public float amplitude = 1f;

		public override float Evaluate(float x)
		{
			return amplitude * x * x;
		}
	}
}
