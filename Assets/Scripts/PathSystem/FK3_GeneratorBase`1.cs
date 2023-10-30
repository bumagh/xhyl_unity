namespace PathSystem
{
	public class FK3_GeneratorBase<T> : FK3_IGenerator<T>
	{
		protected FK3_SubFormation<T> _subFormation;

		public virtual void Init(FK3_SubFormation<T> formation)
		{
			_subFormation = formation;
		}

		public virtual FK3_AgentData<T> GetNext(object userData)
		{
			return _getNext(default(T), userData);
		}

		public virtual void Reset()
		{
		}

		public virtual T[] GetTypes()
		{
			return new T[0];
		}

		protected FK3_AgentData<T> _getNext(T type, object userData)
		{
			return _subFormation.formation.AddObject(type, userData);
		}
	}
}
