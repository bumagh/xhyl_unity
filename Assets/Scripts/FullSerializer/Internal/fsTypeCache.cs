using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FullSerializer.Internal
{
	public static class fsTypeCache
	{
		private static Dictionary<string, Type> _cachedTypes;

		private static Dictionary<string, Assembly> _assembliesByName;

		private static List<Assembly> _assembliesByIndex;

		[CompilerGenerated]
		private static AssemblyLoadEventHandler f_mgcache0;

		static fsTypeCache()
		{
			_cachedTypes = new Dictionary<string, Type>();
			object typeFromHandle = typeof(fsTypeCache);
			lock (typeFromHandle)
			{
				_assembliesByName = new Dictionary<string, Assembly>();
				_assembliesByIndex = new List<Assembly>();
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly in assemblies)
				{
					_assembliesByName[assembly.FullName] = assembly;
					_assembliesByIndex.Add(assembly);
				}
				_cachedTypes = new Dictionary<string, Type>();
				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.AssemblyLoad += OnAssemblyLoaded;
			}
		}

		private static void OnAssemblyLoaded(object sender, AssemblyLoadEventArgs args)
		{
			object typeFromHandle = typeof(fsTypeCache);
			lock (typeFromHandle)
			{
				_assembliesByName[args.LoadedAssembly.FullName] = args.LoadedAssembly;
				_assembliesByIndex.Add(args.LoadedAssembly);
				_cachedTypes = new Dictionary<string, Type>();
			}
		}

		private static bool TryDirectTypeLookup(string assemblyName, string typeName, out Type type)
		{
			if (assemblyName != null && _assembliesByName.TryGetValue(assemblyName, out Assembly value))
			{
				type = value.GetType(typeName, throwOnError: false);
				return type != null;
			}
			type = null;
			return false;
		}

		private static bool TryIndirectTypeLookup(string typeName, out Type type)
		{
			for (int i = 0; i < _assembliesByIndex.Count; i++)
			{
				Assembly assembly = _assembliesByIndex[i];
				type = assembly.GetType(typeName);
				if (type != null)
				{
					return true;
				}
			}
			for (int j = 0; j < _assembliesByIndex.Count; j++)
			{
				Assembly assembly2 = _assembliesByIndex[j];
				Type[] types = assembly2.GetTypes();
				foreach (Type type2 in types)
				{
					if (type2.FullName == typeName)
					{
						type = type2;
						return true;
					}
				}
			}
			type = null;
			return false;
		}

		public static void Reset()
		{
			_cachedTypes = new Dictionary<string, Type>();
		}

		public static Type GetType(string name)
		{
			return GetType(name, null);
		}

		public static Type GetType(string name, string assemblyHint)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object typeFromHandle = typeof(fsTypeCache);
			lock (typeFromHandle)
			{
				if (!_cachedTypes.TryGetValue(name, out Type value))
				{
					if (TryDirectTypeLookup(assemblyHint, name, out value) || !TryIndirectTypeLookup(name, out value))
					{
					}
					_cachedTypes[name] = value;
				}
				return value;
			}
		}
	}
}
