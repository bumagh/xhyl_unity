using FullInspector.Internal;
using FullSerializer.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FullInspector
{
	public static class InspectedMemberFilters
	{
		private class AllFilter : IInspectedMemberFilter
		{
			public bool IsInterested(InspectedProperty property)
			{
				return true;
			}

			public bool IsInterested(InspectedMethod method)
			{
				return true;
			}
		}

		private class FullInspectorSerializedPropertiesFilter : IInspectedMemberFilter
		{
			public bool IsInterested(InspectedProperty property)
			{
				return property.CanWrite && InspectedType.IsSerializedByFullInspector(property) && !InspectedType.IsSerializedByUnity(property);
			}

			public bool IsInterested(InspectedMethod method)
			{
				return false;
			}
		}

		private class InspectableMembersFilter : IInspectedMemberFilter
		{
			public bool IsInterested(InspectedProperty property)
			{
				return IsPropertyTypeInspectable(property) && ShouldDisplayProperty(property);
			}

			public bool IsInterested(InspectedMethod method)
			{
				return method.Method.IsDefined(typeof(InspectorButtonAttribute), inherit: true);
			}
		}

		private class StaticInspectableMembersFilter : IInspectedMemberFilter
		{
			public bool IsInterested(InspectedProperty property)
			{
				return property.IsStatic && IsPropertyTypeInspectable(property);
			}

			public bool IsInterested(InspectedMethod method)
			{
				return method.Method.IsDefined(typeof(InspectorButtonAttribute), inherit: true);
			}
		}

		private class ButtonMembersFilter : IInspectedMemberFilter
		{
			public bool IsInterested(InspectedProperty property)
			{
				return false;
			}

			public bool IsInterested(InspectedMethod method)
			{
				return method.Method.IsDefined(typeof(InspectorButtonAttribute), inherit: true);
			}
		}

		public class TkControlMembersFilter : IInspectedMemberFilter
		{
			public bool IsInterested(InspectedProperty property)
			{
				return typeof(IEnumerable<tkIControl>).IsAssignableFrom(property.StorageType) || typeof(tkIControl).IsAssignableFrom(property.StorageType);
			}

			public bool IsInterested(InspectedMethod method)
			{
				return false;
			}
		}

		public static IInspectedMemberFilter All = new AllFilter();

		public static IInspectedMemberFilter FullInspectorSerializedProperties = new FullInspectorSerializedPropertiesFilter();

		public static IInspectedMemberFilter InspectableMembers = new InspectableMembersFilter();

		public static IInspectedMemberFilter StaticInspectableMembers = new StaticInspectableMembersFilter();

		public static IInspectedMemberFilter ButtonMembers = new ButtonMembersFilter();

		public static TkControlMembersFilter TkControlMembers = new TkControlMembersFilter();

		private static bool ShouldDisplayProperty(InspectedProperty property)
		{
			MemberInfo memberInfo = property.MemberInfo;
			if (memberInfo.IsDefined(typeof(ShowInInspectorAttribute), inherit: true))
			{
				return true;
			}
			if (memberInfo.IsDefined(typeof(HideInInspector), inherit: true) || memberInfo.IsDefined(typeof(NotSerializedAttribute), inherit: true) || fiInstalledSerializerManager.SerializationOptOutAnnotations.Any((Type t) => memberInfo.IsDefined(t, inherit: true)))
			{
				return false;
			}
			if (!property.IsStatic && fiInstalledSerializerManager.SerializationOptInAnnotations.Any((Type t) => memberInfo.IsDefined(t, inherit: true)))
			{
				return true;
			}
			if (property.MemberInfo is PropertyInfo && fiSettings.InspectorRequireShowInInspector)
			{
				return false;
			}
			return typeof(BaseObject).Resolve().IsAssignableFrom(property.StorageType.Resolve()) || InspectedType.IsSerializedByFullInspector(property) || InspectedType.IsSerializedByUnity(property);
		}

		private static bool IsPropertyTypeInspectable(InspectedProperty property)
		{
			if (typeof(Delegate).IsAssignableFrom(property.StorageType))
			{
				return false;
			}
			if (property.MemberInfo is FieldInfo)
			{
				if (property.MemberInfo.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false))
				{
					return false;
				}
			}
			else if (property.MemberInfo is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)property.MemberInfo;
				if (!propertyInfo.CanRead)
				{
					return false;
				}
				string @namespace = propertyInfo.DeclaringType.Namespace;
				if (@namespace != null && (@namespace.StartsWith("UnityEngine") || @namespace.StartsWith("UnityEditor")) && !propertyInfo.CanWrite)
				{
					return false;
				}
				if (propertyInfo.Name.EndsWith("Item"))
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length > 0)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
