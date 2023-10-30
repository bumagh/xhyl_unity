namespace PathSystem
{
	public class Circle : PolarEquation
	{
		public float radius = 3f;

		public override float Evaluate(float theta)
		{
			return radius;
		}
	}
}
