using FullInspector.Internal;
using System;

namespace FullInspector
{
	public static class fiLogicalOperatorSupport
	{
		public static bool ComputeValue(fiLogicalOperator op, string[] memberNames, object element)
		{
			bool flag = GetInitialValue(op);
			foreach (string memberName in memberNames)
			{
				bool booleanReflectedMember = fiRuntimeReflectionUtility.GetBooleanReflectedMember(element.GetType(), element, memberName, defaultValue: true);
				flag = Combine(op, flag, booleanReflectedMember);
			}
			return flag;
		}

		private static bool GetInitialValue(fiLogicalOperator op)
		{
			switch (op)
			{
			case fiLogicalOperator.AND:
				return true;
			case fiLogicalOperator.OR:
				return false;
			default:
				throw new NotImplementedException();
			}
		}

		private static bool Combine(fiLogicalOperator op, bool a, bool b)
		{
			switch (op)
			{
			case fiLogicalOperator.AND:
				return a && b;
			case fiLogicalOperator.OR:
				return a || b;
			default:
				throw new NotImplementedException();
			}
		}
	}
}
