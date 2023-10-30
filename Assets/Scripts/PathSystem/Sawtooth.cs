namespace PathSystem
{
	public class Sawtooth : CartesianEquation
	{
		public float amplitude = 1f;

		public float frequency = 1f;

		public override float Evaluate(float x)
		{
			return amplitude * (x % frequency);
		}
	}
}
