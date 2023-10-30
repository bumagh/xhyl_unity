using System;
using System.Reflection;

namespace FullInspector.Internal
{
    public static class fiAssemblyExtensions
    {
        public static Type[] GetTypesWithoutException(this Assembly assembly)
        {
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch
            {
                types = fiAssemblyExtensions.s_EmptyArray;
            }
            return types;
        }

        private static Type[] s_EmptyArray = Array.Empty<Type>();
    }
}