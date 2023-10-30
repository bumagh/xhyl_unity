using System;
using UnityEngine.Events;

namespace PathSystem
{
	[Serializable]
	public class PointEvent : UnityEvent<int, NavPathAgent>
	{
	}
}
