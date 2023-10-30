using System;

namespace FullInspector
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorSkipInheritanceAttribute : Attribute, IInspectorAttributeOrder
	{
		double IInspectorAttributeOrder.Order => double.MinValue;
	}
}
