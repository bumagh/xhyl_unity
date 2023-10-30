using FullSerializer.Internal;
using System;
using System.Reflection;

namespace FullInspector
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorTooltipAttribute : Attribute
	{
		public string Tooltip;

		public InspectorTooltipAttribute(string tooltip)
		{
			Tooltip = (tooltip ?? string.Empty);
		}

		public static string GetTooltip(MemberInfo memberInfo)
		{
			InspectorTooltipAttribute attribute = fsPortableReflection.GetAttribute<InspectorTooltipAttribute>(memberInfo);
			if (attribute == null)
			{
				return string.Empty;
			}
			return attribute.Tooltip;
		}
	}
}
