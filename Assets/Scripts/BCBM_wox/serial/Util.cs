using System;
using System.Reflection;

namespace BCBM_wox.serial
{
	internal class Util
	{
		public static void main(string[] args)
		{
			Type type = Type.GetType("serializer.Student");
			ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
			object obj = constructor.Invoke(null);
		}

		public static bool stringable(object o)
		{
			return o is sbyte || o is double || o is float || o is int || o is long || o is short || o is bool || o is char || o is Type || o is string;
		}

		public static bool stringable(string name)
		{
			try
			{
				Type left = (Type)BCBM_Serial.mapWOXToCSharp[name];
				if (left != null)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("Exception: " + ex.Message);
				return false;
			}
		}

		public static ConstructorInfo forceDefaultConstructor(Type cl)
		{
			return cl.GetConstructor(Type.EmptyTypes);
		}

		public static void testReflection(object testObject)
		{
			Type type = testObject.GetType();
			testReflection(type);
		}

		public static void testReflection(Type objectType)
		{
			ConstructorInfo constructor = objectType.GetConstructor(Type.EmptyTypes);
			ConstructorInfo[] constructors = objectType.GetConstructors();
			MethodInfo[] methods = objectType.GetMethods();
			object obj = constructor.Invoke(new object[0]);
			ConstructorInfo[] array = constructors;
			foreach (ConstructorInfo constructorInfo in array)
			{
			}
			MethodInfo[] array2 = methods;
			foreach (MethodInfo methodInfo in array2)
			{
			}
		}

		public static bool primitive(Type typeOb)
		{
			for (int i = 0; i < BCBM_Serial.primitives.Length; i++)
			{
				if (BCBM_Serial.primitives[i].Equals(typeOb))
				{
					return true;
				}
			}
			return false;
		}
	}
}
