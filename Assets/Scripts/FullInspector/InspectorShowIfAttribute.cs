using System;

namespace FullInspector
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorShowIfAttribute : Attribute
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

		public InspectorShowIfAttribute(string conditionalMemberName)
		{
			Operator = fiLogicalOperator.AND;
			ConditionalMemberName = conditionalMemberName;
		}

		public InspectorShowIfAttribute(fiLogicalOperator op, params string[] conditionalMemberNames)
		{
			Operator = op;
			ConditionalMemberNames = conditionalMemberNames;
		}
	}
}
