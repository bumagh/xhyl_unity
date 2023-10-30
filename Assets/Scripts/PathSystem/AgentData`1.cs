namespace PathSystem
{
	public class AgentData<T>
	{
		public NavPathAgent agent;

		public T type;

		public Formation<T> formation;

		public AgentData(NavPathAgent agent, T type, Formation<T> formation)
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

		protected void _onComplete(NavPathAgent agent)
		{
			formation.RemoveObject(agent);
		}
	}
}
