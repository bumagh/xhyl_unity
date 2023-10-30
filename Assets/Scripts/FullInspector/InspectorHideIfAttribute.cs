using System;

namespace FullInspector
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorHideIfAttribute : Attribute
	{
		public string[] ConditionalMemberNames;

		public fiLogicalOperator Operator;

		public string ConditionalMemberName
		{
			set
			{
				ConditionalMemberNames = new string[1]
				{
					value
				};
			}
		}

		public InspectorHideIfAttribute(string conditionalMemberName)
		{
			Operator = fiLogicalOperator.AND;
			ConditionalMemberName = conditionalMemberName;
		}

		public InspectorHideIfAttribute(fiLogicalOperator op, params string[] conditionalMemberNames)
		{
			Operator = op;
			ConditionalMemberNames = conditionalMemberNames;
		}
	}
}
