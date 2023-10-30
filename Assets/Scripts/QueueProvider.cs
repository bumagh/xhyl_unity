using System.Collections.Generic;
using System.Linq;

public class QueueProvider : TestProvider<Queue<int>>
{
	public override bool Compare(Queue<int> before, Queue<int> after)
	{
		return before.Except(after).Count() == 0 && after.Except(before).Count() == 0;
	}

	public override IEnumerable<Queue<int>> GetValues()
	{
		yield return new Queue<int>();
		Queue<int> q2 = new Queue<int>();
		q2.Enqueue(1);
		yield return q2;
		q2 = new Queue<int>();
		q2.Enqueue(1);
		q2.Enqueue(5);
		q2.Enqueue(3);
		yield return q2;
	}
}
