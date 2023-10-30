using System;
using UnityEngine.Events;

namespace PathSystem
{
	[Serializable]
	public class FK3_PointEvent : UnityEvent<int, FK3_NavPathAgent>
	{
	}
}
