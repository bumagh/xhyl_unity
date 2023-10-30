using System;

namespace BansheeGz.BGSpline.Curve
{
	public static class BGReflectionAdapter
	{
		public static object[] GetCustomAttributes(Type type, Type attributeType, bool inherit)
		{
			return type.GetCustomAttributes(attributeType, inherit);
		}

		public static bool IsAbstract(Type type)
		{
			return type.IsAbstract;
		}

		public static bool IsClass(Type type)
		{
			return type.IsClass;
		}

		public static bool IsSubclassOf(Type type, Type typeToCheck)
		{
			return type.IsSubclassOf(typeToCheck);
		}

		public static bool IsValueType(Type type)
		{
			return type.IsValueType;
		}
	}
}
