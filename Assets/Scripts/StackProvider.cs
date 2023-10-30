using System.Collections.Generic;
using System.Linq;

public class StackProvider : TestProvider<Stack<int>>
{
	public override bool Compare(Stack<int> before, Stack<int> after)
	{
		return before.Except(after).Count() == 0 && after.Except(before).Count() == 0;
	}

	public override IEnumerable<Stack<int>> GetValues()
	{
		yield return new Stack<int>();
		Stack<int> s2 = new Stack<int>();
		s2.Push(1);
		yield return s2;
		s2 = new Stack<int>();
		s2.Push(1);
		s2.Push(5);
		s2.Push(3);
		yield return s2;
	}
}
