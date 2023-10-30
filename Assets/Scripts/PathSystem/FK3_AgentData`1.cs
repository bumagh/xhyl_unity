namespace PathSystem
{
	public class FK3_AgentData<T>
	{
		public FK3_NavPathAgent agent;

		public T type;

		public FK3_Formation<T> formation;

		public FK3_AgentData(FK3_NavPathAgent agent, T type, FK3_Formation<T> formation)
		{
			this.agent = agent;
			this.type = type;
			this.formation = formation;
			agent.OnComplete += _onComplete;
			agent.userData = this;
		}

		public void Clear()
		{
			agent.StopMove();
			agent.OnComplete -= _onComplete;
			agent.userData = null;
		}

		protected void _onComplete(FK3_NavPathAgent agent)
		{
			formation.RemoveObject(agent);
		}
	}
}
