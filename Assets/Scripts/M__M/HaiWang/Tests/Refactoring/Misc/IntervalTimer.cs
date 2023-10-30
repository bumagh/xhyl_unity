using FullInspector;
using System;

namespace M__M.HaiWang.Tests.Refactoring.Misc
{
	[Serializable]
	public class IntervalTimer
	{
		public float interval;

		[NotSerialized]
		public int count;

		[NotSerialized]
		public float totalTimer;

		[NotSerialized]
		public float timer;

		[NotSerialized]
		public bool isFirstTime = true;

		public IntervalTimer(float interval, float timer = 0f, bool isFirstTime = true)
		{
			this.interval = interval;
			this.timer = timer;
			this.isFirstTime = isFirstTime;
			count = 0;
			totalTimer = 0f;
		}

		public void DoOnce()
		{
			isFirstTime = false;
			timer = 0f;
		}

		public void Reset()
		{
			count = 0;
			isFirstTime = true;
			totalTimer = 0f;
			timer = 0f;
		}

		public void AddTime(float deltaTime)
		{
			timer += deltaTime;
		}

		public bool CanDo()
		{
			return isFirstTime || timer >= interval;
		}
	}
}
