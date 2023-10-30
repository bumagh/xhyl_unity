namespace PathSystem
{
	public class GeneratorBase<T> : IGenerator<T>
	{
		protected SubFormation<T> _subFormation;

		public virtual void Init(SubFormation<T> formation)
		{
			_subFormation = formation;
		}

		public virtual AgentData<T> GetNext(object userData)
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

		protected AgentData<T> _getNext(T type, object userData)
		{
			return _subFormation.formation.AddObject(type, userData);
		}
	}
}
