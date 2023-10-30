namespace M__M.HaiWang.FK3_FSM
{
	public abstract class FK3_State<O>
	{
		public O Owner;

		public FK3_FSM<O> Fsm;

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
