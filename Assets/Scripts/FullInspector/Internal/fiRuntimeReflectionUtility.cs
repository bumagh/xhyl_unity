using FullSerializer;
using FullSerializer.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
//TODO 修改
namespace FullInspector.Internal
{
	public class fiRuntimeReflectionUtility
	{
		private static List<Assembly> _cachedRuntimeAssemblies;

		private static List<Assembly> _cachedUserDefinedEditorAssemblies;

		private static List<Assembly> _cachedAllEditorAssembles;

		private static Dictionary<Type, List<Type>> _allSimpleTypesDerivingFromCache;

		private static Dictionary<Type, List<Type>> _allSimpleCreatableTypesDerivingFromCache;

		public static object InvokeStaticMethod(Type type, string methodName, object[] parameters)
		{
			try
			{
				return type.GetFlattenedMethod(methodName).Invoke(null, parameters);
			}
			catch
			{
			}
			return null;
		}

		public static object InvokeStaticMethod(string typeName, string methodName, object[] parameters)
		{
			return InvokeStaticMethod(fsTypeCache.GetType(typeName), methodName, parameters);
		}

		public static void InvokeMethod(Type type, string methodName, object thisInstance, object[] parameters)
		{
			try
			{
				type.GetFlattenedMethod(methodName).Invoke(thisInstance, parameters);
			}
			catch
			{
			}
		}

		public static bool GetBooleanReflectedMember(Type elementType, object element, string memberName, bool defaultValue)
		{
			MemberInfo[] flattenedMember = elementType.GetFlattenedMember(memberName);
			if (flattenedMember == null || flattenedMember.Length == 0)
			{
				UnityEngine.Debug.LogError("Could not find a member with name " + memberName + " on object type " + elementType);
				return true;
			}
			MemberInfo memberInfo = flattenedMember[0];
			object obj = defaultValue;
			try
			{
				if (memberInfo is FieldInfo)
				{
					obj = ((FieldInfo)memberInfo).GetValue(element);
				}
				else if (memberInfo is PropertyInfo)
				{
					obj = ((PropertyInfo)memberInfo).GetValue(element, null);
				}
				else if (memberInfo is MethodInfo)
				{
					obj = ((MethodInfo)memberInfo).Invoke(element, null);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("When running " + memberInfo.ReflectedType + "::" + memberInfo.Name + ", caught exception " + ex);
				UnityEngine.Debug.LogException(ex);
				obj = defaultValue;
			}
			if (obj.GetType() != typeof(bool))
			{
				UnityEngine.Debug.LogError(memberInfo.ReflectedType + "::" + memberInfo.Name + " needs to return a bool to be used with [InspectorDisplayIf]");
				obj = defaultValue;
			}
			return (bool)obj;
		}

		public static T ReadField<TContext, T>(TContext context, string fieldName)
		{
			MemberInfo[] flattenedMember = typeof(TContext).GetFlattenedMember(fieldName);
			if (flattenedMember == null || flattenedMember.Length == 0)
			{
				throw new ArgumentException(typeof(TContext).CSharpName() + " does not contain a field named \"" + fieldName + "\"");
			}
			if (flattenedMember.Length > 1)
			{
				throw new ArgumentException(typeof(TContext).CSharpName() + " has more than one field named \"" + fieldName + "\"");
			}
			FieldInfo fieldInfo = flattenedMember[0] as FieldInfo;
			if (fieldInfo == null)
			{
				throw new ArgumentException(typeof(TContext).CSharpName() + "." + fieldName + " is not a field");
			}
			if (fieldInfo.FieldType != typeof(T))
			{
				throw new ArgumentException(typeof(TContext).CSharpName() + "." + fieldName + " type is not compatable with " + typeof(T).CSharpName());
			}
			return (T)fieldInfo.GetValue(context);
		}

		public static T ReadFields<TContext, T>(TContext context, params string[] fieldNames)
		{
			foreach (string memberName in fieldNames)
			{
				MemberInfo[] flattenedMember = typeof(TContext).GetFlattenedMember(memberName);
				if (flattenedMember != null && flattenedMember.Length != 0 && flattenedMember.Length <= 1)
				{
					FieldInfo fieldInfo = flattenedMember[0] as FieldInfo;
					if (!(fieldInfo == null) && !(fieldInfo.FieldType != typeof(T)))
					{
						return (T)fieldInfo.GetValue(context);
					}
				}
			}
			throw new ArgumentException("Unable to read any of the following fields " + string.Join(", ", fieldNames) + " on " + context);
		}

		public static IEnumerable<TInterface> GetAssemblyInstances<TInterface>()
		{
			return from _003C_003E__TranspIdent0 in GetUserDefinedEditorAssemblies().SelectMany(fiAssemblyExtensions.GetTypesWithoutException, (Assembly assembly, Type type) => new
				{
					assembly,
					type
				})
				where !_003C_003E__TranspIdent0.type.Resolve().IsGenericTypeDefinition
				where !_003C_003E__TranspIdent0.type.Resolve().IsAbstract
				where !_003C_003E__TranspIdent0.type.Resolve().IsInterface
				where typeof(TInterface).IsAssignableFrom(_003C_003E__TranspIdent0.type)
				where _003C_003E__TranspIdent0.type.GetDeclaredConstructor(fsPortableReflection.EmptyTypes) != null
				select (TInterface)Activator.CreateInstance(_003C_003E__TranspIdent0.type);
		}

		public static IEnumerable<Type> GetUnityObjectTypes()
		{
			return from _003C_003E__TranspIdent1 in GetRuntimeAssemblies().SelectMany(fiAssemblyExtensions.GetTypesWithoutException, (Assembly assembly, Type type) => new
				{
					assembly,
					type
				})
				where _003C_003E__TranspIdent1.type.Resolve().IsVisible
				where !_003C_003E__TranspIdent1.type.Resolve().IsGenericTypeDefinition
				where typeof(UnityEngine.Object).IsAssignableFrom(_003C_003E__TranspIdent1.type)
				select _003C_003E__TranspIdent1.type;
		}

		private static string GetName(Assembly assembly)
		{
			int num = assembly.FullName.IndexOf(",");
			if (num >= 0)
			{
				return assembly.FullName.Substring(0, num);
			}
			return assembly.FullName;
		}

		public static IEnumerable<Assembly> GetRuntimeAssemblies()
		{
			if (_cachedRuntimeAssemblies == null)
			{
				_cachedRuntimeAssemblies = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
					where !IsBannedAssembly(assembly)
					where !IsUnityEditorAssembly(assembly)
					where !GetName(assembly).Contains("-Editor")
					select assembly).ToList();
				fiLog.Blank();
				foreach (Assembly cachedRuntimeAssembly in _cachedRuntimeAssemblies)
				{
					fiLog.Log(typeof(fiRuntimeReflectionUtility), "GetRuntimeAssemblies - " + GetName(cachedRuntimeAssembly));
				}
				fiLog.Blank();
			}
			return _cachedRuntimeAssemblies;
		}

		public static IEnumerable<Assembly> GetUserDefinedEditorAssemblies()
		{
			if (_cachedUserDefinedEditorAssemblies == null)
			{
				_cachedUserDefinedEditorAssemblies = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
					where !IsBannedAssembly(assembly)
					where !IsUnityEditorAssembly(assembly)
					select assembly).ToList();
				fiLog.Blank();
				foreach (Assembly cachedUserDefinedEditorAssembly in _cachedUserDefinedEditorAssemblies)
				{
					fiLog.Log(typeof(fiRuntimeReflectionUtility), "GetUserDefinedEditorAssemblies - " + GetName(cachedUserDefinedEditorAssembly));
				}
				fiLog.Blank();
			}
			return _cachedUserDefinedEditorAssemblies;
		}

		public static IEnumerable<Assembly> GetAllEditorAssemblies()
		{
			if (_cachedAllEditorAssembles == null)
			{
				_cachedAllEditorAssembles = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
					where !IsBannedAssembly(assembly)
					select assembly).ToList();
				fiLog.Blank();
				foreach (Assembly cachedAllEditorAssemble in _cachedAllEditorAssembles)
				{
					fiLog.Log(typeof(fiRuntimeReflectionUtility), "GetAllEditorAssemblies - " + GetName(cachedAllEditorAssemble));
				}
				fiLog.Blank();
			}
			return _cachedAllEditorAssembles;
		}

		private static bool IsUnityEditorAssembly(Assembly assembly)
		{
			string[] source = new string[2]
			{
				"UnityEditor",
				"UnityEditor.UI"
			};
			return source.Contains(GetName(assembly));
		}

		private static bool IsBannedAssembly(Assembly assembly)
		{
			string[] source = new string[56]
			{
				"AssetStoreTools",
				"AssetStoreToolsExtra",
				"UnityScript",
				"UnityScript.Lang",
				"Boo.Lang.Parser",
				"Boo.Lang",
				"Boo.Lang.Compiler",
				"mscorlib",
				"System.ComponentModel.DataAnnotations",
				"System.Xml.Linq",
				"ICSharpCode.NRefactory",
				"Mono.Cecil",
				"Mono.Cecil.Mdb",
				"Unity.DataContract",
				"Unity.IvyParser",
				"Unity.Locator",
				"Unity.PackageManager",
				"Unity.SerializationLogic",
				"UnityEngine.Analytics",
				"UnityEngine.Networking",
				"UnityEngine.UI",
				"UnityEditor.Advertisements",
				"UnityEditor.Android.Extensions",
				"UnityEditor.AppleTV.Extensions",
				"UnityEditor.BB10.Extensions",
				"UnityEditor.EditorTestsRunner",
				"UnityEditor.Graphs",
				"UnityEditor.iOS.Extensions",
				"UnityEditor.iOS.Extensions.Common",
				"UnityEditor.iOS.Extensions.Xcode",
				"UnityEditor.LinuxStandalone.Extensions",
				"UnityEditor.Metro.Extensions",
				"UnityEditor.Networking",
				"UnityEditor.OSXStandalone.Extensions",
				"UnityEditor.SamsungTV.Extensions",
				"UnityEditor.Tizen.Extensions",
				"UnityEditor.TreeEditor",
				"UnityEditor.WebGL.Extensions",
				"UnityEditor.WindowsStandalone.Extensions",
				"UnityEditor.WP8.Extensions",
				"protobuf-net",
				"Newtonsoft.Json",
				"System",
				"System.Configuration",
				"System.Xml",
				"System.Core",
				"Mono.Security",
				"I18N",
				"I18N.West",
				"nunit.core",
				"nunit.core.interfaces",
				"nunit.framework",
				"NSubstitute",
				"UnityVS.VersionSpecific",
				"SyntaxTree.VisualStudio.Unity.Bridge",
				"SyntaxTree.VisualStudio.Unity.Messaging"
			};
			return source.Contains(GetName(assembly));
		}

		public static IEnumerable<Type> AllSimpleTypesDerivingFrom(Type t)
		{
			if (_allSimpleTypesDerivingFromCache == null)
			{
				_allSimpleTypesDerivingFromCache = new Dictionary<Type, List<Type>>();
			}
			List<Type> value;
			if (!_allSimpleTypesDerivingFromCache.TryGetValue(t, out value))
			{
				value = AllSimpleTypesDerivingFromInternal(t);
				_allSimpleTypesDerivingFromCache[t] = value;
			}
			return value;
		}

		private static List<Type> AllSimpleTypesDerivingFromInternal(Type baseType)
		{
			return (from _003C_003E__TranspIdent2 in GetRuntimeAssemblies().SelectMany(fiAssemblyExtensions.GetTypesWithoutException, (Assembly assembly, Type type) => new
				{
					assembly,
					type
				})
				where baseType.IsAssignableFrom(_003C_003E__TranspIdent2.type)
				where _003C_003E__TranspIdent2.type.Resolve().IsClass
				where !_003C_003E__TranspIdent2.type.Resolve().IsGenericTypeDefinition
				select _003C_003E__TranspIdent2.type).ToList();
		}

		public static IEnumerable<Type> AllSimpleCreatableTypesDerivingFrom(Type t)
		{
			if (fiRuntimeReflectionUtility._allSimpleCreatableTypesDerivingFromCache == null)
			{
				fiRuntimeReflectionUtility._allSimpleCreatableTypesDerivingFromCache = new Dictionary<Type, List<Type>>();
			}
			List<Type> list;
			if (!fiRuntimeReflectionUtility._allSimpleCreatableTypesDerivingFromCache.TryGetValue(t, out list))
			{
				list = fiRuntimeReflectionUtility.AllSimpleCreatableTypesDerivingFromInternal(t);
				fiRuntimeReflectionUtility._allSimpleCreatableTypesDerivingFromCache[t] = list;
			}
			return list;
		}

		private static List<Type> AllSimpleCreatableTypesDerivingFromInternal(Type baseType)
		{
			return (from type in fiRuntimeReflectionUtility.AllSimpleTypesDerivingFrom(baseType)
				where !type.Resolve().IsAbstract
				where !type.Resolve().IsGenericType
				where type.GetDeclaredConstructor(fsPortableReflection.EmptyTypes) != null
				select type).ToList<Type>();
		}
	}
}
