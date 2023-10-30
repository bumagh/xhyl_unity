namespace M__M.HaiWang.FSM
{
	public abstract class State<O>
	{
		public O Owner;

		public FSM<O> Fsm;

		public virtual void OnEnter(ArgList args)
		{
		}

		public virtual void OnExit()
		{
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void OnEvent(Event evt)
		{
		}
	}
}
