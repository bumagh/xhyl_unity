using UnityEngine;

namespace M__M.HaiWang
{
	public class Event
	{
		public ArgList args;

		public EventType Type
		{
			get;
			set;
		}

		private Event(EventType t)
		{
			Type = t;
		}

		private Event(EventType t, params object[] args)
		{
			Type = t;
			this.args = ArgList.Create(args);
		}

		public static Event Create(EventType t, params object[] args)
		{
			return new Event(t, args);
		}

		public static Event Create(EventType t)
		{
			return new Event(t);
		}

		public A Next<A>()
		{
			return args.Next<A>();
		}

		public void Rewind()
		{
			args.Rewind();
		}

		public bool Finish()
		{
			return args.Finish();
		}
	}
}
