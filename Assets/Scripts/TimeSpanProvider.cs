using System;
using System.Collections.Generic;

public class TimeSpanProvider : TestProvider<TimeSpan>
{
	public override bool Compare(TimeSpan original, TimeSpan deserialized)
	{
		return original == deserialized;
	}

	public override IEnumerable<TimeSpan> GetValues()
	{
		yield return TimeSpan.MaxValue;
		yield return TimeSpan.MinValue;
		yield return default(TimeSpan);
		yield return default(TimeSpan).Add(TimeSpan.FromDays(3.0)).Add(TimeSpan.FromHours(2.0)).Add(TimeSpan.FromMinutes(33.0))
			.Add(TimeSpan.FromSeconds(35.0))
			.Add(TimeSpan.FromMilliseconds(35.0))
			.Add(TimeSpan.FromTicks(250L));
	}
}
