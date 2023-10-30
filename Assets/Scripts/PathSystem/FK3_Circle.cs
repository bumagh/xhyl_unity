namespace PathSystem
{
	public class FK3_Circle : FK3_PolarEquation
	{
		public float radius = 3f;

		public override float Evaluate(float theta)
		{
			return radius;
		}
	}
}
