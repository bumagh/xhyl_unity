namespace BansheeGz.BGSpline.Curve
{
	public static class FieldExtensions
	{
		public static bool In(this BGCurveBaseMath.Field field, int mask)
		{
			return (field.Val() & mask) != 0;
		}

		public static int Val(this BGCurveBaseMath.Field field)
		{
			return (int)field;
		}
	}
}
