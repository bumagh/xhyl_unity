using System;
using System.Collections.Generic;

namespace FullInspector.Internal
{
	public static class fiSingletons
	{
		private static Dictionary<Type, object> _instances = new Dictionary<Type, object>();

		public static T Get<T>()
		{
			return (T)Get(typeof(T));
		}

		public static object Get(Type type)
		{
			if (!_instances.TryGetValue(type, out object value))
			{
				value = Activator.CreateInstance(type);
				_instances[type] = value;
			}
			return value;
		}
	}
}
